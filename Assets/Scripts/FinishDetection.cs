using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishDetection : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(LoadLevel("MainScene")); // TODO: change hard coded value
        }
    }

    private IEnumerator LoadLevel(String sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
