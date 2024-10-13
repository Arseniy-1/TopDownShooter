using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealthPoint;

    public event Action<float, float> HealthChanged;
    public event Action Died;

    public void Awake()
    {
        _currentHealthPoint = _maxHealth;
    }

    public void Heal(float amount)
    {
        if (amount <= 0)
            return;

        _currentHealthPoint = Mathf.Clamp(_currentHealthPoint + amount, 0, _maxHealth);
        Debug.Log("HealIteration");

        HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
    }

    public float TakeDamage(float amount)
    {
        if (amount <= 0)
            return 0;

        _currentHealthPoint = Mathf.Clamp(_currentHealthPoint - amount, 0, _maxHealth);

        if (_currentHealthPoint == 0)
            Died?.Invoke();

        HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);

        if (_currentHealthPoint < amount)
            return _currentHealthPoint;
        else
            return amount;
    }
}
