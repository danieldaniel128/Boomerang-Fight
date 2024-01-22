using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviourPun
{
    [SerializeField] private float _currentHP;
    [SerializeField] private float _maxHP;
    public float CurrentHP { get { return _currentHP; } private set { _currentHP = value; OnValueChanged?.Invoke(); } }
    public float MaxHP { get { return _maxHP; } private set { _maxHP = value; } }
    public bool IsDead { get; private set; }

    public UnityEvent OnValueChanged;
    public UnityEvent OnDeath;


    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Revive()
    {
        CurrentHP = MaxHP;
        IsDead = false;
    }

    public void SetHealth(float health)
    {
        MaxHP = health;
        CurrentHP = health;
    }
    //private void OnDisable()
    //{
    //    OnDeath?.Invoke();
    //}
}
