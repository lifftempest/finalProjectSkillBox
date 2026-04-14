using System.Collections;
using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField] private int _scoreValue;

    public int ScoreValue => _scoreValue;
    private static int OverallScoreValue { get; set; }
    private WaitForSeconds _initHold = new(0.3f);

    private void Awake()
    {
        StartCoroutine(InitializeOveralScore());
    }

    private IEnumerator InitializeOveralScore()
    {
        OverallScoreValue = 0;
        yield return _initHold;
        OverallScoreValue += ScoreValue;
    }

    public int GetScore()
    {
        return OverallScoreValue;
    }

    /// <summary>
    /// !!! Только в случае если очки определяются не в ScoreComponent
    /// </summary>
    /// <param name="value"></param>
    public void SetScoreValue(int value)
    {
        _scoreValue = value;
    }
}
