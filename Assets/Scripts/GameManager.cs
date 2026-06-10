using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timer;
    public bool running;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countdownText;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        countdownText.text = "Ready";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "Set";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(5f);
        countdownText.text = "";

        StartRun();
    }

    void Update()
    {
        if (!running) return;

        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void StartRun()
    {
        // Debug.Log("RUN STARTED");
        timer = 0f;
        running = true;
    }

    public void StopRun()
    {
        // Debug.Log("RUN STOPPED: " + timer);
        running = false;
    }
}
