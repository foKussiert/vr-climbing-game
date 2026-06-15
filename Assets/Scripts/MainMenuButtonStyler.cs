using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MainMenuButtonStyler
{
    private const string MainMenuSceneName = "MainMenu_Scene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void ApplyMainMenuButtonStyling()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name != MainMenuSceneName)
            return;

        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button button in buttons)
        {
            if (button == null || button.gameObject.scene != activeScene)
                continue;

            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.None;
            button.navigation = navigation;

            if (button.GetComponent<PauseMenuButtonOutline>() == null)
                button.gameObject.AddComponent<PauseMenuButtonOutline>();

            Text[] legacyTexts = button.GetComponentsInChildren<Text>(true);
            foreach (Text legacyText in legacyTexts)
                legacyText.raycastTarget = false;

            TMP_Text[] tmpTexts = button.GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text tmpText in tmpTexts)
                tmpText.raycastTarget = false;
        }
    }
}
