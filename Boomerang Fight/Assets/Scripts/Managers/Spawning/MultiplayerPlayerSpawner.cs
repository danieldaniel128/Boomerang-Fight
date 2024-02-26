using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPlayerSpawner : MonoBehaviourPun
{
    [SerializeField] SpawnPoint[] _spawnPoints;
    int index;
    int _myPlayerIndex;
    Player _myPlayer;
    const string PLAYER_RESOURCE_NAME = "Player";

    
    void Start()
    {
        //GetMyPlayer();
        SpawnPlayers();
    }
    
    void SpawnPlayers()
    {
        if(PhotonNetwork.IsMasterClient)
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                photonView.RPC(nameof(InstantiationPlayerRPC), player, index);
                index++;
            }
    }
    
    

    [PunRPC]
    void InstantiationPlayerRPC(int index)
    {
        GameObject playerGameobject = PhotonNetwork.Instantiate(PLAYER_RESOURCE_NAME, _spawnPoints[index].SpawnPosition, Quaternion.identity, 0);
        _myPlayerIndex = index;
    }


    
}   
