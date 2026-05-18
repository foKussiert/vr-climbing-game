using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public GameObject startMenuCanvas;
    public GameObject loadingScreenCanvas;

    public void StartGame() 
    {
        // 1 ist hier der index deiner Game_Scene in den Build Settings
        StartCoroutine(LoadLevelRoutine(1)); 
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    private IEnumerator LoadLevelRoutine(int sceneIndex) 
    {
        startMenuCanvas.SetActive(false);
        loadingScreenCanvas.SetActive(true); // Das schwebende Loading-Schild zeigen

        // Asynchrones Laden der Game_Scene
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        while (!op.isDone) 
        {
            yield return null;
        }
    }
}