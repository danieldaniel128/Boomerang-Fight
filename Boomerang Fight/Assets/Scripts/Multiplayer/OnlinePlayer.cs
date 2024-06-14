using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviourPun
{
    [SerializeField] InGameUIManager gameUIManager;
    [SerializeField] PlayerController playerController;
    int _id;
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

}
