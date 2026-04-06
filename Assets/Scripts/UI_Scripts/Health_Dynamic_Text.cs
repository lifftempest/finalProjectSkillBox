using TMPro;
using UnityEngine;

public class Health_Dynamic_Text : MonoBehaviour
{
    [SerializeField] private HealthComponent _playerHealth;
    [SerializeField] private TMP_Text _healthText;

    private void Awake()
    {
        Invoke("InitializeVar", 0.2f);

        _playerHealth.OnHealthChanged += ChangeHealthText;
    }

    private void OnDestroy()
    {
        _playerHealth.OnHealthChanged -= ChangeHealthText;
    }

    private void InitializeVar()
    {
        _healthText.text = _playerHealth.CurrentHealth.ToString();
    }

    private void ChangeHealthText(float hpValue)
    {
        _healthText.text = hpValue.ToString();
    }
}
