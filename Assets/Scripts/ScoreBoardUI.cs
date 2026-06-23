using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreboardUI : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        List<float> scores = HighscoreManager.LoadScores();

        scoreText.text = "BESTZEITEN\n\n";

        for (int i = 0; i < scores.Count; i++)
        {
            scoreText.text += $"{i + 1}. {FormatTime(scores[i])}\n";
        }


    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}
