using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _currentHealth;
    private bool _isAlive = true;

    public Action OnDeath;
    public Action<float> OnHealthChanged;

    public bool IsAlive => _isAlive;
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;  

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_isAlive)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
            {
                _isAlive = false;
                OnDeath?.Invoke();
            }
        }
    }

    public void Heal()
    {
        if (_currentHealth != _maxHealth)
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }
}
