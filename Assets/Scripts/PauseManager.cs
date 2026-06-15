using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;
using XRInputDevice = UnityEngine.XR.InputDevice;
using XRInputButtonReader = UnityEngine.XR.Interaction.Toolkit.Inputs.Readers.XRInputButtonReader;

public class PauseManager : MonoBehaviour
{
    private const string MainSceneName = "MainScene";
    private const string MenuSceneName = "MainMenu_Scene";
    private const string PlayerLayerName = "Player";
    private const string UiLayerName = "UI";

    [Header("Scene References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject pauseMenu;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask gameplayMask = ~0;
    [SerializeField] private LayerMask pauseMask;
    [SerializeField] private LayerMask unpauseBlockerMask = ~0;

    [Header("Pause Visuals")]
    [SerializeField] private Color pauseBackgroundColor = new Color(0.03f, 0.03f, 0.03f, 1f);
    [SerializeField, Min(0.5f)] private float pauseMenuDistance = 2f;

    [Header("Overlap Safety")]
    [SerializeField] private float overlapPadding = 0.0025f;

    [Header("Resume Countdown")]
    [SerializeField, Min(0)] private int resumeCountdownSeconds = 3;

    private bool isPaused;
    private bool previousControllerPausePressed;
    private int playerLayer = -1;
    private int uiLayer = -1;
    private Transform playerRoot;

    private XRInputDevice leftHandDevice;
    private XRInputDevice rightHandDevice;
    private InputAction rightPrimaryPauseAction;

    private CameraClearFlags cachedCameraClearFlags;
    private Color cachedCameraBackground;

    private readonly List<Behaviour> gameplayBehaviours = new List<Behaviour>();
    private readonly List<Collider> handColliders = new List<Collider>();
    private readonly List<GameObject> pauseInteractionObjects = new List<GameObject>();
    private readonly List<XRRayInteractor> pauseRayInteractors = new List<XRRayInteractor>();

    private bool[] cachedBehaviourStates = new bool[0];
    private bool[] cachedColliderTriggerStates = new bool[0];
    private bool[] cachedPauseInteractionStates = new bool[0];
    private bool[] cachedPauseRayUiStates = new bool[0];
    private bool[] cachedPauseRayEnabledStates = new bool[0];
    private bool[] cachedPauseRayBlockUiOnSelectionStates = new bool[0];
    private XRInputButtonReader[] cachedPauseRayUiPressInputs = new XRInputButtonReader[0];

    private Text hintText;
    private Coroutine resumeCountdownRoutine;
    private EventSystem eventSystem;
    private XRUIInputModule xrUiInputModule;
    private InputSystemUIInputModule inputSystemUiInputModule;
    private bool cachedXrUiInputModuleState;
    private bool cachedInputSystemUiInputModuleState;

    private static readonly Collider[] overlapBuffer = new Collider[32];

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsurePauseManagerInMainScene()
    {
        if (SceneManager.GetActiveScene().name != MainSceneName)
            return;

        if (FindFirstObjectByType<PauseManager>() != null)
            return;

        GameObject host = GameObject.Find("GameManager");
        if (host == null)
            host = new GameObject("GameManager");

        host.AddComponent<PauseManager>();
    }

    private void Awake()
    {
        EnsurePauseInputAction();
        ResolveReferences();
        ConfigureLayersAndMasks();
        EnsurePauseMenuExists();
        CollectGameplayBehaviours();
        CollectHandColliders();
        CollectPauseInteractionObjects();

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        EnsurePauseInputAction();
        rightPrimaryPauseAction?.Enable();
    }

    private void Update()
    {
        if (isPaused)
            EnforcePauseInteractionState();

        if (!DidRequestPauseToggle())
            return;

        if (isPaused && IsHoveringPauseMenuUI())
            return;

        if (isPaused)
            TryResume();
        else
            Pause();
    }

    private void OnDisable()
    {
        rightPrimaryPauseAction?.Disable();

        if (isPaused)
            ForceResumeState();
    }

    private void ResolveReferences()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
        {
            GameObject cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject != null)
                mainCamera = cameraObject.GetComponent<Camera>();
        }

        if (mainCamera != null)
        {
            cachedCameraClearFlags = mainCamera.clearFlags;
            cachedCameraBackground = mainCamera.backgroundColor;
        }
    }

    private void EnsurePauseInputAction()
    {
        if (rightPrimaryPauseAction != null)
            return;

        rightPrimaryPauseAction = new InputAction(
            name: "PauseRightPrimary",
            type: InputActionType.Button
        );

        rightPrimaryPauseAction.AddBinding("<XRController>{RightHand}/primaryButton");
        rightPrimaryPauseAction.AddBinding("<OculusTouchController>{RightHand}/primaryButton");
    }

    private void ConfigureLayersAndMasks()
    {
        playerLayer = LayerMask.NameToLayer(PlayerLayerName);
        uiLayer = LayerMask.NameToLayer(UiLayerName);

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerRoot = playerObject.transform;

            if (playerLayer >= 0)
                SetLayerRecursively(playerObject, playerLayer);
        }

        if (mainCamera != null)
            gameplayMask = mainCamera.cullingMask;

        int pauseMaskValue = 0;
        if (playerLayer >= 0)
            pauseMaskValue |= 1 << playerLayer;
        if (uiLayer >= 0)
            pauseMaskValue |= 1 << uiLayer;

        if (pauseMaskValue == 0)
            pauseMask = gameplayMask;
        else
            pauseMask = pauseMaskValue;

        int blockers = gameplayMask.value;
        if (playerLayer >= 0)
            blockers &= ~(1 << playerLayer);
        if (uiLayer >= 0)
            blockers &= ~(1 << uiLayer);
        unpauseBlockerMask = blockers;
    }

    private void EnsurePauseMenuExists()
    {
        if (pauseMenu == null)
            pauseMenu = BuildRuntimePauseMenu();

        if (pauseMenu == null)
            return;

        if (uiLayer >= 0)
            SetLayerRecursively(pauseMenu, uiLayer);
    }

    private GameObject BuildRuntimePauseMenu()
    {
        GameObject canvasObject = new GameObject("PauseMenuCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = mainCamera;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        canvasObject.AddComponent<TrackedDeviceGraphicRaycaster>();

        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1100f, 760f);
        canvasRect.localScale = Vector3.one * 0.0014f;

        GameObject panelObject = new GameObject("Panel", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup));
        panelObject.transform.SetParent(canvasObject.transform, false);

        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage = panelObject.GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.92f);
        panelImage.raycastTarget = false;

        VerticalLayoutGroup layout = panelObject.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(50, 50, 50, 50);
        layout.spacing = 18f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        CreateLabel(panelObject.transform, "PAUSE", 70, FontStyle.Bold, Color.white);
        hintText = CreateLabel(panelObject.transform, " ", 26, FontStyle.Normal, new Color(1f, 0.4f, 0.4f));

        CreateMenuButton(panelObject.transform, "Resume", TryResume);
        CreateMenuButton(panelObject.transform, "Restart", RestartScene);
        CreateMenuButton(panelObject.transform, "Main Menu", QuitToMenu);

        return canvasObject;
    }

    private Text CreateLabel(Transform parent, string content, int size, FontStyle fontStyle, Color color)
    {
        GameObject labelObject = new GameObject(content, typeof(RectTransform), typeof(LayoutElement), typeof(Text));
        labelObject.transform.SetParent(parent, false);

        LayoutElement layout = labelObject.GetComponent<LayoutElement>();
        layout.preferredHeight = 90f;

        Text text = labelObject.GetComponent<Text>();
        text.text = content;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = size;
        text.fontStyle = fontStyle;
        text.color = color;
        text.font = GetDefaultFont();
        text.raycastTarget = false;

        return text;
    }

    private void CreateMenuButton(Transform parent, string label, UnityEngine.Events.UnityAction callback)
    {
        GameObject buttonObject = new GameObject(label + "Button", typeof(RectTransform), typeof(LayoutElement), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        LayoutElement layout = buttonObject.GetComponent<LayoutElement>();
        layout.preferredHeight = 95f;

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        Button button = buttonObject.GetComponent<Button>();
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
        button.onClick.AddListener(callback);
        buttonObject.AddComponent<PauseMenuButtonOutline>();

        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObject.GetComponent<Text>();
        text.text = label;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 34;
        text.fontStyle = FontStyle.Bold;
        text.color = Color.white;
        text.font = GetDefaultFont();
        text.raycastTarget = false;
    }

    private static Font GetDefaultFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        return font;
    }

    private void CollectGameplayBehaviours()
    {
        gameplayBehaviours.Clear();

        Climbing climbing = FindFirstObjectByType<Climbing>();
        AddUniqueBehaviour(climbing);

        IsClimable[] climableScripts = FindObjectsByType<IsClimable>(FindObjectsSortMode.None);
        foreach (IsClimable climable in climableScripts)
            AddUniqueBehaviour(climable);

        GameObject locomotionRoot = FindSceneObjectByName("Locomotion");
        if (locomotionRoot != null)
        {
            Behaviour[] locomotionBehaviours = locomotionRoot.GetComponentsInChildren<Behaviour>(true);
            foreach (Behaviour behaviour in locomotionBehaviours)
                AddUniqueBehaviour(behaviour);
        }

        cachedBehaviourStates = new bool[gameplayBehaviours.Count];
    }

    private void CollectHandColliders()
    {
        handColliders.Clear();

        IsClimable[] climableScripts = FindObjectsByType<IsClimable>(FindObjectsSortMode.None);
        foreach (IsClimable climable in climableScripts)
        {
            Collider[] colliders = climable.GetComponentsInChildren<Collider>(true);
            foreach (Collider collider in colliders)
                AddUniqueCollider(collider);
        }

        cachedColliderTriggerStates = new bool[handColliders.Count];
    }

    private void CollectPauseInteractionObjects()
    {
        pauseInteractionObjects.Clear();
        pauseRayInteractors.Clear();

        AddPauseInteractionObject(FindSceneObjectByName("Left Ray Interactor"));
        AddPauseInteractionObject(FindSceneObjectByName("Right Ray Interactor"));

        if (pauseInteractionObjects.Count == 0)
        {
            XRRayInteractor[] allRayInteractors = FindObjectsByType<XRRayInteractor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (XRRayInteractor rayInteractor in allRayInteractors)
                AddPauseInteractionObject(rayInteractor.gameObject);
        }

        for (int i = 0; i < pauseInteractionObjects.Count; i++)
        {
            XRRayInteractor rayInteractor = pauseInteractionObjects[i].GetComponent<XRRayInteractor>();
            AddPauseRayInteractor(rayInteractor);
        }

        cachedPauseInteractionStates = new bool[pauseInteractionObjects.Count];
        cachedPauseRayUiStates = new bool[pauseRayInteractors.Count];
        cachedPauseRayEnabledStates = new bool[pauseRayInteractors.Count];
        cachedPauseRayBlockUiOnSelectionStates = new bool[pauseRayInteractors.Count];
        cachedPauseRayUiPressInputs = new XRInputButtonReader[pauseRayInteractors.Count];
    }

    private void AddPauseInteractionObject(GameObject gameObject)
    {
        if (gameObject == null || pauseInteractionObjects.Contains(gameObject))
            return;

        pauseInteractionObjects.Add(gameObject);
    }

    private void AddPauseRayInteractor(XRRayInteractor rayInteractor)
    {
        if (rayInteractor == null || pauseRayInteractors.Contains(rayInteractor))
            return;

        pauseRayInteractors.Add(rayInteractor);
    }

    private static GameObject FindSceneObjectByName(string objectName)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform candidate in allTransforms)
        {
            if (candidate == null)
                continue;

            if (!candidate.gameObject.scene.IsValid())
                continue;

            if (candidate.name != objectName)
                continue;

            return candidate.gameObject;
        }

        return null;
    }

    private void AddUniqueBehaviour(Behaviour behaviour)
    {
        if (behaviour == null || behaviour == this || gameplayBehaviours.Contains(behaviour))
            return;

        gameplayBehaviours.Add(behaviour);
    }

    private void AddUniqueCollider(Collider collider)
    {
        if (collider == null || handColliders.Contains(collider))
            return;

        handColliders.Add(collider);
    }

    private bool DidRequestPauseToggle()
    {
        bool actionPressed = rightPrimaryPauseAction != null && rightPrimaryPauseAction.WasPressedThisFrame();
        if (actionPressed)
        {
            previousControllerPausePressed = true;
            return true;
        }

        bool controllerPressedNow = IsControllerPausePressed();
        bool controllerEdge = controllerPressedNow && !previousControllerPausePressed;
        previousControllerPausePressed = controllerPressedNow;

        return controllerEdge;
    }

    private bool IsControllerPausePressed()
    {
        EnsureInputDevices();

        if (!rightHandDevice.isValid)
            return false;

        bool rightPrimaryPressed = false;
        rightHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out rightPrimaryPressed);
        return rightPrimaryPressed;
    }

    private void EnsureInputDevices()
    {
        if (!leftHandDevice.isValid)
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (!rightHandDevice.isValid)
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    public void TogglePause()
    {
        if (isPaused)
            TryResume();
        else
            Pause();
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (mainCamera != null)
        {
            mainCamera.cullingMask = pauseMask;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = pauseBackgroundColor;
        }

        SetGameplaySystemsPaused(true);
        SetHandCollidersToTriggerMode(true);
        SetPauseInteractionObjects(true);

        if (pauseMenu != null)
        {
            AlignPauseMenuToCamera();
            pauseMenu.SetActive(true);
        }

        SetHintMessage(" ");
    }

    private void TryResume()
    {
        if (resumeCountdownRoutine != null)
            return;

        if (HandsOverlapBlockingGeometry())
        {
            SetHintMessage("Haende aus Wand/Felsen bewegen, um fortzusetzen.");
            return;
        }

        resumeCountdownRoutine = StartCoroutine(ResumeAfterCountdown());
    }

    private IEnumerator ResumeAfterCountdown()
    {
        int seconds = Mathf.Max(0, resumeCountdownSeconds);
        if (seconds == 0)
        {
            resumeCountdownRoutine = null;
            ForceResumeState();
            yield break;
        }

        for (int remaining = seconds; remaining > 0; remaining--)
        {
            SetHintMessage($"Fortsetzen in {remaining}...");
            yield return new WaitForSecondsRealtime(1f);

            if (!isPaused)
            {
                resumeCountdownRoutine = null;
                yield break;
            }

            if (HandsOverlapBlockingGeometry())
            {
                SetHintMessage("Haende aus Wand/Felsen bewegen, um fortzusetzen.");
                resumeCountdownRoutine = null;
                yield break;
            }
        }

        resumeCountdownRoutine = null;
        ForceResumeState();
    }

    private void ForceResumeState()
    {
        if (resumeCountdownRoutine != null)
        {
            StopCoroutine(resumeCountdownRoutine);
            resumeCountdownRoutine = null;
        }

        isPaused = false;
        Time.timeScale = 1f;

        SetHandCollidersToTriggerMode(false);
        SetGameplaySystemsPaused(false);
        SetPauseInteractionObjects(false);

        if (mainCamera != null)
        {
            mainCamera.cullingMask = gameplayMask;
            mainCamera.clearFlags = cachedCameraClearFlags;
            mainCamera.backgroundColor = cachedCameraBackground;
        }

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        SetHintMessage(" ");
    }

    private void SetGameplaySystemsPaused(bool paused)
    {
        for (int i = 0; i < gameplayBehaviours.Count; i++)
        {
            Behaviour behaviour = gameplayBehaviours[i];
            if (behaviour == null)
                continue;

            if (paused)
            {
                cachedBehaviourStates[i] = behaviour.enabled;
                behaviour.enabled = false;
            }
            else
            {
                behaviour.enabled = cachedBehaviourStates[i];
            }
        }
    }

    private void SetHandCollidersToTriggerMode(bool paused)
    {
        for (int i = 0; i < handColliders.Count; i++)
        {
            Collider collider = handColliders[i];
            if (collider == null)
                continue;

            if (paused)
            {
                cachedColliderTriggerStates[i] = collider.isTrigger;
                collider.isTrigger = true;
            }
            else
            {
                collider.isTrigger = cachedColliderTriggerStates[i];
            }
        }
    }

    private void SetPauseInteractionObjects(bool paused)
    {
        for (int i = 0; i < pauseInteractionObjects.Count; i++)
        {
            GameObject interactionObject = pauseInteractionObjects[i];
            if (interactionObject == null)
                continue;

            if (paused)
            {
                cachedPauseInteractionStates[i] = interactionObject.activeSelf;
                interactionObject.SetActive(true);
            }
            else
            {
                interactionObject.SetActive(cachedPauseInteractionStates[i]);
            }
        }

        EnsureUiInputModules();

        for (int i = 0; i < pauseRayInteractors.Count; i++)
        {
            XRRayInteractor rayInteractor = pauseRayInteractors[i];
            if (rayInteractor == null)
                continue;

            if (paused)
            {
                cachedPauseRayUiStates[i] = rayInteractor.enableUIInteraction;
                cachedPauseRayEnabledStates[i] = rayInteractor.enabled;
                cachedPauseRayBlockUiOnSelectionStates[i] = rayInteractor.blockUIOnInteractableSelection;
                cachedPauseRayUiPressInputs[i] = rayInteractor.uiPressInput;

                rayInteractor.enabled = true;
                rayInteractor.enableUIInteraction = true;
                rayInteractor.blockUIOnInteractableSelection = false;
                rayInteractor.uiPressInput = rayInteractor.activateInput;
            }
            else
            {
                rayInteractor.enabled = cachedPauseRayEnabledStates[i];
                rayInteractor.enableUIInteraction = cachedPauseRayUiStates[i];
                rayInteractor.blockUIOnInteractableSelection = cachedPauseRayBlockUiOnSelectionStates[i];
                rayInteractor.uiPressInput = cachedPauseRayUiPressInputs[i];
            }
        }

        if (paused)
        {
            if (xrUiInputModule != null)
            {
                cachedXrUiInputModuleState = xrUiInputModule.enabled;
                xrUiInputModule.enabled = true;
            }

            if (inputSystemUiInputModule != null)
            {
                cachedInputSystemUiInputModuleState = inputSystemUiInputModule.enabled;
                inputSystemUiInputModule.enabled = false;
            }

            RegisterPauseRayInteractorsWithUiModule();
            eventSystem?.SetSelectedGameObject(null);
        }
        else
        {
            if (xrUiInputModule != null)
                xrUiInputModule.enabled = cachedXrUiInputModuleState;

            if (inputSystemUiInputModule != null)
                inputSystemUiInputModule.enabled = cachedInputSystemUiInputModuleState;
        }
    }

    private void EnsureUiInputModules()
    {
        if (eventSystem == null)
            eventSystem = FindFirstObjectByType<EventSystem>();

        if (eventSystem == null)
            return;

        if (xrUiInputModule == null)
            xrUiInputModule = eventSystem.GetComponent<XRUIInputModule>();

        if (xrUiInputModule == null)
            xrUiInputModule = eventSystem.gameObject.AddComponent<XRUIInputModule>();

        if (inputSystemUiInputModule == null)
            inputSystemUiInputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
    }

    private void EnforcePauseInteractionState()
    {
        for (int i = 0; i < pauseInteractionObjects.Count; i++)
        {
            GameObject interactionObject = pauseInteractionObjects[i];
            if (interactionObject != null && !interactionObject.activeSelf)
                interactionObject.SetActive(true);
        }

        for (int i = 0; i < pauseRayInteractors.Count; i++)
        {
            XRRayInteractor rayInteractor = pauseRayInteractors[i];
            if (rayInteractor == null)
                continue;

            if (!rayInteractor.enabled)
                rayInteractor.enabled = true;
            if (!rayInteractor.enableUIInteraction)
                rayInteractor.enableUIInteraction = true;
            if (rayInteractor.blockUIOnInteractableSelection)
                rayInteractor.blockUIOnInteractableSelection = false;

            rayInteractor.uiPressInput = rayInteractor.activateInput;
        }

        RegisterPauseRayInteractorsWithUiModule();

        if (xrUiInputModule != null && !xrUiInputModule.enabled)
            xrUiInputModule.enabled = true;
        if (inputSystemUiInputModule != null && inputSystemUiInputModule.enabled)
            inputSystemUiInputModule.enabled = false;
    }

    private void RegisterPauseRayInteractorsWithUiModule()
    {
        if (xrUiInputModule == null)
            return;

        for (int i = 0; i < pauseRayInteractors.Count; i++)
        {
            XRRayInteractor rayInteractor = pauseRayInteractors[i];
            if (rayInteractor == null)
                continue;

            xrUiInputModule.RegisterInteractor(rayInteractor);
        }
    }

    private bool IsHoveringPauseMenuUI()
    {
        if (pauseMenu == null)
            return false;

        Transform pauseMenuTransform = pauseMenu.transform;

        for (int i = 0; i < pauseRayInteractors.Count; i++)
        {
            XRRayInteractor rayInteractor = pauseRayInteractors[i];
            if (rayInteractor == null || !rayInteractor.enabled || !rayInteractor.enableUIInteraction)
                continue;

            if (!rayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult raycastResult))
                continue;

            GameObject hitObject = raycastResult.gameObject;
            if (hitObject != null && hitObject.transform.IsChildOf(pauseMenuTransform))
                return true;
        }

        return false;
    }

    private bool HandsOverlapBlockingGeometry()
    {
        int blockers = unpauseBlockerMask.value;
        if (blockers == 0)
            return false;

        foreach (Collider handCollider in handColliders)
        {
            if (handCollider == null || !handCollider.enabled)
                continue;

            int overlapCount = GetOverlapCount(handCollider, blockers);
            for (int i = 0; i < overlapCount; i++)
            {
                Collider overlap = overlapBuffer[i];
                if (overlap == null || overlap == handCollider)
                    continue;

                if (playerRoot != null && overlap.transform.IsChildOf(playerRoot))
                    continue;

                return true;
            }
        }

        return false;
    }

    private int GetOverlapCount(Collider source, int blockers)
    {
        if (source is BoxCollider box)
            return OverlapBox(box, blockers);
        if (source is SphereCollider sphere)
            return OverlapSphere(sphere, blockers);
        if (source is CapsuleCollider capsule)
            return OverlapCapsule(capsule, blockers);

        Bounds bounds = source.bounds;
        Vector3 extents = bounds.extents + Vector3.one * overlapPadding;
        return Physics.OverlapBoxNonAlloc(
            bounds.center,
            extents,
            overlapBuffer,
            Quaternion.identity,
            blockers,
            QueryTriggerInteraction.Collide
        );
    }

    private int OverlapBox(BoxCollider collider, int blockers)
    {
        Vector3 scaledSize = Vector3.Scale(collider.size, Abs(collider.transform.lossyScale));
        Vector3 halfExtents = (scaledSize * 0.5f) + Vector3.one * overlapPadding;
        Vector3 center = collider.transform.TransformPoint(collider.center);

        return Physics.OverlapBoxNonAlloc(
            center,
            halfExtents,
            overlapBuffer,
            collider.transform.rotation,
            blockers,
            QueryTriggerInteraction.Collide
        );
    }

    private int OverlapSphere(SphereCollider collider, int blockers)
    {
        Vector3 lossy = Abs(collider.transform.lossyScale);
        float maxScale = Mathf.Max(lossy.x, Mathf.Max(lossy.y, lossy.z));
        float radius = (collider.radius * maxScale) + overlapPadding;
        Vector3 center = collider.transform.TransformPoint(collider.center);

        return Physics.OverlapSphereNonAlloc(
            center,
            radius,
            overlapBuffer,
            blockers,
            QueryTriggerInteraction.Collide
        );
    }

    private int OverlapCapsule(CapsuleCollider collider, int blockers)
    {
        Vector3 lossy = Abs(collider.transform.lossyScale);
        int direction = collider.direction;

        float radiusScale = direction == 0
            ? Mathf.Max(lossy.y, lossy.z)
            : direction == 1
                ? Mathf.Max(lossy.x, lossy.z)
                : Mathf.Max(lossy.x, lossy.y);

        float radius = (collider.radius * radiusScale) + overlapPadding;
        float heightScale = direction == 0 ? lossy.x : direction == 1 ? lossy.y : lossy.z;
        float height = Mathf.Max(collider.height * heightScale, radius * 2f);

        Vector3 axis = direction == 0 ? collider.transform.right : direction == 1 ? collider.transform.up : collider.transform.forward;
        Vector3 center = collider.transform.TransformPoint(collider.center);
        float pointOffset = (height * 0.5f) - radius;

        Vector3 pointA = center + axis * pointOffset;
        Vector3 pointB = center - axis * pointOffset;

        return Physics.OverlapCapsuleNonAlloc(
            pointA,
            pointB,
            radius,
            overlapBuffer,
            blockers,
            QueryTriggerInteraction.Collide
        );
    }

    private static Vector3 Abs(Vector3 value)
    {
        return new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
    }

    private void AlignPauseMenuToCamera()
    {
        if (pauseMenu == null || mainCamera == null)
            return;

        Transform cameraTransform = mainCamera.transform;
        Vector3 flatForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;

        if (flatForward.sqrMagnitude < 0.0001f)
            flatForward = cameraTransform.forward.normalized;

        pauseMenu.transform.position = cameraTransform.position + flatForward * pauseMenuDistance;
        pauseMenu.transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
    }

    private void SetHintMessage(string message)
    {
        if (hintText != null)
            hintText.text = message;
    }

    public void ResumeFromButton()
    {
        TryResume();
    }

    public void RestartScene()
    {
        ForceResumeState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        ForceResumeState();
        SceneManager.LoadScene(MenuSceneName);
    }

    private static void SetLayerRecursively(GameObject root, int layer)
    {
        if (root == null)
            return;

        root.layer = layer;
        Transform transform = root.transform;
        for (int i = 0; i < transform.childCount; i++)
            SetLayerRecursively(transform.GetChild(i).gameObject, layer);
    }
}
