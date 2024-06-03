using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviourPun
{
    [SerializeField] private float _currentHP;
    [SerializeField] private float _maxHP;
    [SerializeField] private int _livesCount = 3;
    [SerializeField] UnityEvent<float, float> OnHealthChangedEvent;
    [SerializeField] UnityEvent<int> OnLivesCountChangedEvent;
    [SerializeField] UnityEvent OnLivesCountZero;
    [SerializeField] GameObject _healthBarObject;
    bool _isInvincible;
    public int LivesCount
    {
        get { return _livesCount; }
        private set
        {
            _livesCount = value;
            OnLivesCountChangedEvent?.Invoke(_livesCount);
        }
    }
    public float CurrentHP
    {
        get { return _currentHP; }
        private set
        {
            _currentHP = value;
            OnHealthChangedEvent?.Invoke(_currentHP, MaxHP);
        }
    }
    public float MaxHP { get { return _maxHP; } private set { _maxHP = value; } }
    public bool IsDead { get; private set; }
    //public UnityEvent<float,float> OnValueChanged;
    public UnityEvent OnDeath;

    private void Start()
    {
        OnLivesCountChangedEvent?.Invoke(LivesCount);
    }

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
        //if (!photonView.IsMine)
        //    CameraManager.Instance.CameraShakeRef.ShakeCamera();

        photonView.RPC(nameof(MasterUpdateHealth), RpcTarget.MasterClient, CurrentHP - damage);
        
    }
    public void KillPlayer()
    {
        photonView.RPC(nameof(ExecutePlayer), RpcTarget.MasterClient);
    }
    [PunRPC]
    void ExecutePlayer()
    {
        photonView.RPC(nameof(FinishPlayerLives), RpcTarget.All);
    }
    [PunRPC]
    void FinishPlayerLives()
    {
        LivesCount=0;
        IsDead = true;
        if (photonView.IsMine)  // Only call RemovePlayer if this is the local player
        {
            OnLivesCountZero?.Invoke();
            RemovePlayer();
        }
        _healthBarObject.SetActive(false);
    }
    [PunRPC]
    private void MasterUpdateHealth(float newHealth)
    {
        photonView.RPC(nameof(SyncHealth), RpcTarget.All, newHealth);
    }
    private void RemovePlayer()
    {
        StartCoroutine(OnlineGameManager.LeaveGameCoroutine());
    }
    [PunRPC]
    private void SyncHealth(float newHealth)
    {
        // Update health for remote players
        CurrentHP = newHealth;
        if(photonView.IsMine)
        {
            #if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
            #endif
            CameraManager.Instance.CameraShakeRef.ShakeCamera();
        }
        if (newHealth <= 0)
        {
            CallOnDeath();
            OnDeath?.Invoke();
        }
        StartCoroutine(InvincibleFromHitCoroutine());
    }
    public void CallOnDeath()
    {
        LivesCount--;
        if(LivesCount <= 0)
        {
            IsDead = true;
            if (photonView.IsMine)  // Only call RemovePlayer if this is the local player
            {
                OnLivesCountZero?.Invoke();
                RemovePlayer();
            }
            gameObject.GetComponent<PlayerController>().PlayerBody.SetActive(false);
            _healthBarObject.SetActive(false);
            return;

        }
        SyncRevive();
    }
    IEnumerator InvincibleFromHitCoroutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(2f);
        _isInvincible = false;
    }
    public void Revive()
    {
        //Call the RPC to notify other players about the revival
        photonView.RPC(nameof(SyncRevive), RpcTarget.All);
        //send to master to revive player with actorID
    }

    [PunRPC]
    private void SyncRevive()
    {
        // Handle revival for remote players
        CurrentHP = MaxHP;
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
