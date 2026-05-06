using UnityEngine;
using UnityEngine.XR;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin;
    public Transform rightHand;
    public CharacterController characterController;

    [Header("Settings")]
    public float climbSpeedMultiplier = 1.0f;

    private bool rightHandCanClimb;
    private bool isClimbing;

    private Vector3 lastRightHandPosition;
    private InputDevice rightHandDevice;

    void Start()
    {
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        if (!rightHandDevice.isValid)
        {
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        bool gripPressed = false;
        rightHandDevice.TryGetFeatureValue(CommonUsages.gripButton, out gripPressed);

        if (rightHandCanClimb && gripPressed && !isClimbing)
        {
            StartClimb();
        }

        if (isClimbing && gripPressed)
        {
            ContinueClimb();
        }

        if (isClimbing && !gripPressed)
        {
            StopClimb();
        }
    }

    void StartClimb()
    {
        isClimbing = true;
        lastRightHandPosition = rightHand.position;
    }

    void ContinueClimb()
    {
        Vector3 handDelta = rightHand.position - lastRightHandPosition;

        // Hand nach unten = Körper nach oben
        Vector3 playerMove = -handDelta * climbSpeedMultiplier;

        if (characterController != null)
        {
            characterController.Move(playerMove);
        }
        else
        {
            xrOrigin.position += playerMove;
        }

        lastRightHandPosition = rightHand.position;
    }

    void StopClimb()
    {
        isClimbing = false;
    }

    public void SetRightHandCanClimb(bool value)
    {
        rightHandCanClimb = value;

        if (!value)
        {
            isClimbing = false;
        }
    }
}