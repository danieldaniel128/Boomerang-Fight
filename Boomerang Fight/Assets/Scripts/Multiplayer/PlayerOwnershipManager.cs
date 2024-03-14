using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerOwnershipManager : MonoBehaviourPunCallbacks
{
    public static PlayerOwnershipManager Instance { get; private set; }
    [SerializeField] bool _isMyPlayer = false;
    public bool IsMyPlayer { get => _isMyPlayer; private set { _isMyPlayer = value; } }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public override void OnJoinedRoom()
    {
        IsMyPlayer = true;
    }


}
