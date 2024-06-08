using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviourPun
{
    int _id;
    public int ID => _id;
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
