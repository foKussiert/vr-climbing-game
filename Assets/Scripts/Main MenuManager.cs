using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public GameObject startMenuCanvas;
    public GameObject loadingScreenCanvas;

    public void StartGame(string sceneName) 
    {
        StartCoroutine(LoadLevelRoutine(sceneName)); 
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    private IEnumerator LoadLevelRoutine(string sceneName) 
    {
        startMenuCanvas.SetActive(false);
        loadingScreenCanvas.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) 
        {
            yield return null;
        }
    }
}