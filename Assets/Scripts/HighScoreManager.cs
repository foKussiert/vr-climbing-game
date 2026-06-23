using System.Collections.Generic;
using UnityEngine;

public static class HighscoreManager
{
    private const int MaxScores = 10;

    public static void SaveScore(float time)
    {
        List<float> scores = LoadScores();

        scores.Add(time);

        scores.Sort();

        if (scores.Count > MaxScores)
        {
            scores.RemoveRange(MaxScores, scores.Count - MaxScores);
        }

        PlayerPrefs.SetInt("ScoreCount", scores.Count);

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetFloat($"Score_{i}", scores[i]);
        }

        PlayerPrefs.Save();
    }

    public static List<float> LoadScores()
    {
        List<float> scores = new();

        int count = PlayerPrefs.GetInt("ScoreCount", 0);

        for (int i = 0; i < count; i++)
        {
            scores.Add(PlayerPrefs.GetFloat($"Score_{i}"));
        }

        return scores;
    }
}