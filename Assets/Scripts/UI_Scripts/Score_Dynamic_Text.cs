using TMPro;
using UnityEngine;

public class Score_Dynamic_Text : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;

    private void Awake()
    {
        Invoke("SetStartScoreValue", 0.3f);
    }

    private void SetStartScoreValue()
    {
        _scoreText.text = ScoreHandler.Instance.GetCurrentScore().ToString();
        ScoreHandler.Instance.OnScoreChanged += UpdateScoreText;
    }

    private void UpdateScoreText(int scoreValue)
    {
        _scoreText.text = scoreValue.ToString();
    }
}
