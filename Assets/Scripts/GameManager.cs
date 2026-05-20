using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timer;
    public bool running;

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
    }

    public void StartRun()
    {
        Debug.Log("RUN STARTED");

        timer = 0f;
        running = true;
    }

    public void StopRun()
    {
        Debug.Log("RUN STOPPED: " + timer);
        running = false;
    }
}
