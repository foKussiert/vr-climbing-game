using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timer;
    public bool running;

    public TextMeshProUGUI timerText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
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
