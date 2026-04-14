using TMPro;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentScore;
    [SerializeField] private TMP_Text _maxScore;

    public void SetStats()
    {
        _currentScore.text = ScoreHandler.Instance.GetCurrentScore().ToString();
        _maxScore.text = ScoreHandler.Instance.GetMaxScore().ToString();
    }
}
