using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishDetection : MonoBehaviour
{
    private bool triggered = false;
    public string nextSceneName = "MainScene"; // TODO: change hard coded value

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            GameManager.Instance.StopRun();
            HighscoreManager.SaveScore(GameManager.Instance.timer);
        }
    }
}
