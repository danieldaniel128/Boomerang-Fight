using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnlinePlayer : MonoBehaviourPun
{
    int _id;

    [Header("Components")]
    [SerializeField] InGameUIManager gameUIManager;
    [SerializeField] PlayerController playerController;
    [SerializeField] Health health;

    [Header("Invincibility")]
    public UnityEvent OnInvincibilityEnabled;
    public UnityEvent OnInvincibilityDisabled;
    bool _isInvincible = false;
    Coroutine _invincibilityCoroutine;

    private bool IsInvincible
    {
        get { return _isInvincible; }
        set
        {
            _isInvincible = value;
            if (value)
                OnInvincibilityEnabled.Invoke();
            else
                OnInvincibilityDisabled.Invoke();
        }
    }

    public int ID => _id;
    public InGameUIManager GameUIManager => gameUIManager;
    public PlayerController PlayerControllerRef => playerController;
    private void Start()
    {
        Initialize();
        TempLocalGameManager.Instance.AddPlayerCharacter(gameObject);
    }
    public void Initialize()
    {
        _id = photonView.OwnerActorNr;
    }

    public void TryTakeDamage(float damage)
    {
        if (IsInvincible)
            return;

        health.TakeDamage(damage);
    }

    public void Invincibility(float duration)
    {
        if(_invincibilityCoroutine != null)
            StopCoroutine( _invincibilityCoroutine);
        print("invincibility activated");
        _invincibilityCoroutine = StartCoroutine(InvincibleFromHitCoroutine(duration));
    }

    IEnumerator InvincibleFromHitCoroutine(float time)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(time);
        IsInvincible = false;
    }
}
