using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class OnlinePlayerManager : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public static OnlinePlayerManager Instance;
    [SerializeField] Health _playerHealth;
    public Action OnPlayerDeath;
    private void Awake()
    {
        //Singleton.
        if (!photonView.IsMine)
            this.enabled = false;
        else
            Instance = this;
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
