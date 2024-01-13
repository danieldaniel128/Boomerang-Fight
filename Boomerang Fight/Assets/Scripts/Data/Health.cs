using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _currentHP;
    public float CurrentHP { get { return _currentHP; } private set { _currentHP = value; OnValueChanged?.Invoke(); } }

    [SerializeField] private float _maxHP;
    public float MaxHP { get { return _maxHP; } private set { _maxHP = value; } }

    public UnityEvent OnValueChanged;
    public UnityEvent OnDeath;

    bool _isDead;

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Revive()
    {
        CurrentHP = MaxHP;
        _isDead = false;
    }

    public void SetHealth(float health)
    {
        MaxHP = health;
        CurrentHP = health;
    }
    private void OnDisable()
    {
        OnDeath?.Invoke();
    }
}
