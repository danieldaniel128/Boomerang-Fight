using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPlayerSpawner : MonoBehaviourPun
{
    [SerializeField] SpawnPoint[] _spawnPoints;
    [SerializeField] int _playerLayerIndex = 3; // Change to your desired player layer index
    [SerializeField] int _enemyLayerIndex = 7;  // Change to your desired enemy layer index
    int index;
    Player _myPlayer;
    const string PLAYER_RESOURCE_NAME = "Player";
    // Start is called before the first frame update
    void Start()
    {
        GetMyPlayer();
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        if(PhotonNetwork.IsMasterClient)
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                photonView.RPC(nameof(InstantiationPlayer), player, index);
                index++;
            }
    }
    void GetMyPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal)
            {
                _myPlayer = player;
                return;
            }
        }
    }

    [PunRPC]
    void InstantiationPlayer(int index)
    {
        GameObject playerGameobject = PhotonNetwork.Instantiate(PLAYER_RESOURCE_NAME, _spawnPoints[index].SpawnPosition, Quaternion.identity, 0);
        //Identify the local player
        //Set the layer of the local player to PlayerLayer
        //playerGameobject.layer = _playerLayerIndex;
    }

}
