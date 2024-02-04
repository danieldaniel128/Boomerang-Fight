using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviourPun
{
    [SerializeField] private float _currentHP;
    [SerializeField] private float _maxHP;
    public float CurrentHP { get { return _currentHP; } private set { _currentHP = value; OnValueChanged?.Invoke(_currentHP, _maxHP); } }
    public float MaxHP { get { return _maxHP; } private set { _maxHP = value; } }
    public bool IsDead { get; private set; }

    public UnityEvent<float,float> OnValueChanged;
    public UnityEvent OnDeath;


    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine)
            return;

        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Debug.Log("took damage" + damage);
            IsDead = true;
            OnDeath?.Invoke();
        }

        // Call the RPC to notify other players
        photonView.RPC("SyncHealth", RpcTarget.Others, CurrentHP);
    }

    [PunRPC]
    private void SyncHealth(float newHealth)
    {
        // Update health for remote players
        CurrentHP = newHealth;

        if (newHealth <= 0)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Revive()
    {
        CurrentHP = MaxHP;
        IsDead = false;

        // Call the RPC to notify other players about the revival
        photonView.RPC("SyncRevive", RpcTarget.Others);
    }

    [PunRPC]
    private void SyncRevive()
    {
        // Handle revival for remote players
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
