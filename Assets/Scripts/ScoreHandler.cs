using System;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public Action<int> OnScoreChanged;

    private int _currentScore;
    private int _overallScore;

    public static ScoreHandler Instance { get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _currentScore = 0;
        
        Invoke("Print", 1f);
    }

    private void Print()
    {
        _overallScore = GetOverallScore();
        print($"Максимальное количество очков: {_overallScore}");
    }

    public int GetOverallScore()
    {
        return FindAnyObjectByType<ScoreComponent>().GetScore();
    }

    public void AddScore(int scoreValue)
    {
        _currentScore += scoreValue;
        print("Добавлены очки");
        OnScoreChanged?.Invoke(_currentScore);
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }
}
