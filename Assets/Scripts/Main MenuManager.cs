using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
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
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
