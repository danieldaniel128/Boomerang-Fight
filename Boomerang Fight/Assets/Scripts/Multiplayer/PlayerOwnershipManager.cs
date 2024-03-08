using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerOwnershipManager : MonoBehaviourPunCallbacks
{
    public static PlayerOwnershipManager Instance { get; private set; }
    public Dictionary<int, bool> PlayerOwnershipMap { get; private set; } = new Dictionary<int, bool>();

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("player entered");
        // Update ownership status when a new player enters the room
        UpdatePlayerOwnership(newPlayer.ActorNumber);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        // Remove ownership status when a player leaves the room
        PlayerOwnershipMap.Remove(otherPlayer.ActorNumber);
    }

    public void UpdatePlayerOwnership(int playerId)
    {
        PlayerOwnershipMap[playerId] = PhotonNetwork.LocalPlayer.ActorNumber == playerId;
        //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        //Debug.Log("/////////////////////////");
        //foreach (var item in PlayerOwnershipMap)
        //{
        //    Debug.Log(item.Key);
        //}
        //Debug.Log("/////////////////////////");
    }
}
