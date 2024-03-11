using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviourPun
{
    [SerializeField] private float _currentHP;
    [SerializeField] private float _maxHP;
    [SerializeField] UpdateHPBarMaterial _updateHPBarMaterial;
    public float CurrentHP
    {
        get { return _currentHP; }
        private set
        {
            _currentHP = value;
            _updateHPBarMaterial.UpdateOnHealthChangedEvent(new OnPlayerHealthChangedEvent { newHealth = _currentHP, maxHealth = _maxHP });
            //if (photonView.IsMine)
            //EventBus<OnPlayerHealthChangedEvent>.Raise(new OnPlayerHealthChangedEvent { newHealth = _currentHP, maxHealth = _maxHP });
        }
    }
    public float MaxHP { get { return _maxHP; } private set { _maxHP = value; } }
    public bool IsDead { get; private set; }
    //public UnityEvent<float,float> OnValueChanged;
    public UnityEvent OnDeath;

    [ContextMenu("Take Damage Test Local")]
    public void TakeDamageTest()
    {
        // Update health for remote players
        CurrentHP -= 5;
        if (CurrentHP <= 0)
        {
            IsDead = true;
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
       
        photonView.RPC(nameof(SyncHealth), RpcTarget.All, CurrentHP - damage);
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
            gameObject.SetActive(false);
        }
    }

    public void Revive()
    {
        //Call the RPC to notify other players about the revival
        photonView.RPC(nameof(SyncRevive), RpcTarget.All);
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
