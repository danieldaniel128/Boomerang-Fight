using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class OnlinePlayerManager : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [SerializeField] Health _playerHealth;
    public Action OnPlayerDeath;
    private void Start()
    {
        _playerHealth.OnDeath.AddListener(PlayerDeathEvent);
    }
    public void PlayerDeathEvent()
    {
        OnPlayerDeath?.Invoke();
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if(photonView.IsMine)
            OnlineGameManager.Instance.SetMyPlayerController(this);
        OnlineGameManager.Instance.RegisterPlayerController(this);
    }
}
