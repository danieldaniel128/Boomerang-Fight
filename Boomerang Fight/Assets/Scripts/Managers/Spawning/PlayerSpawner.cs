using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] SpawnPoint[] _spawnPoints;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        //_spawnManager = new SpawnManager(_spawnPoints);
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                photonView.RPC(nameof(InstantiationPlayer), player, index);
                index++;
            }
        }
    }

    [PunRPC]
    void InstantiationPlayer(int index)
    {
        PhotonNetwork.Instantiate("Player", _spawnPoints[index].SpawnPosition, Quaternion.identity, 0);
    }
}
