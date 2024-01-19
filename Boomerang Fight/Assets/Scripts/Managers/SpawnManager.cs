using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SpawnManager : MonoBehaviourPun
{
    // Start is called before the first frame update
    [SerializeField] List<Transform> _spawnPoints;
    Dictionary<Transform, bool> _spawnPointsDictionary = new Dictionary<Transform, bool>();
    const string PLAYER_RESOURCE_PREFAB = "Player";
    void Start()
    {
        SetSpawnPointsPoolToAllPlayers();

        // Only the master client should call the RPC
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(UpdateSpawnPointsPoolToAllPlayers), RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateSpawnPointsPoolToAllPlayers()
    {
        Transform spawnPoint = _spawnPointsDictionary.FirstOrDefault(c => !c.Value).Key;
        if (spawnPoint != null)
        {
            _spawnPointsDictionary[spawnPoint] = true;
            SpawnPlayer(spawnPoint);
        }
    }

    void SpawnPlayer(Transform spawnPoint)
    {
        PhotonNetwork.Instantiate(PLAYER_RESOURCE_PREFAB, spawnPoint.position, spawnPoint.rotation, 0);
    }

    void SetSpawnPointsPoolToAllPlayers()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            _spawnPointsDictionary.Add(spawnPoint, false);
        }
    }

    //void SetSpawnPointsToPlayers()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        return;
    //    //get all players in the current room
    //    List<Player> players = PhotonNetwork.PlayerList.ToList();
    //    foreach (var player in players)
    //    {
    //        //create hashtable for player properties
    //        Hashtable playerCustomProps = new Hashtable();
    //        //add properties to created hashtable
    //        playerCustomProps.Add(SPAWN_POINT_KEY, Random.Range(0, _spawnPoints.Length));
    //        //set the properties of the player
    //        player.SetCustomProperties(playerCustomProps);
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        
    }

}
