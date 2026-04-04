using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _currentHealth;
    private bool _isAlive = true;

    public Action OnDeath;
    public Action OnHealthChanged;

    public bool IsAlive => _isAlive;
    public float CurrentHealth => _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_isAlive)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke();
            if (_currentHealth <= 0)
            {
                _isAlive = false;
                OnDeath?.Invoke();
            }
        }
    }
}
