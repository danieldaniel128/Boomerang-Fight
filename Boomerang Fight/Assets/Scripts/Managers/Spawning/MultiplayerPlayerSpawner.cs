using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class MultiplayerPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] SpawnPoint[] _spawnPoints;
    Player _myPlayer;
    const string PLAYER_RESOURCE_NAME = "Player";

    private Dictionary<int, bool> spawnPointsHash = new Dictionary<int, bool>();
    //dictionary of actor number and player game object? player controller?
    private Dictionary<int, Object> actorToCharacterHash = new();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeSpawnPoints();
            InitialSpawnPlayers();
        }
    }

    void InitializeSpawnPoints()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            spawnPoint.Available = true;
            spawnPointsHash.Add(spawnPoint.ID, spawnPoint.Available);
        }
        UpdateRoomProperties();
    }

    void InitialSpawnPlayers()
    {
        //ok to use index here because its the first time they spawn
        int index = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            photonView.RPC(nameof(InstantiationPlayerRPC), player, index);
            index++;
        }
    }

    [PunRPC]
    void InstantiationPlayerRPC(int index)
    {
        GameObject playerGameobject = PhotonNetwork.Instantiate(PLAYER_RESOURCE_NAME, _spawnPoints[index].SpawnPosition, Quaternion.identity, 0);
    }

    public void SetSpawnPointAvailability(int id, bool availability)
    {
        if (!spawnPointsHash.ContainsKey(id))
            return;

        if (availability)
        {
            spawnPointsHash[id] = availability;
            UpdateRoomProperties();
        }
    }

    //public int GetAvailableSpawnPoint()
    //{
    //    foreach (var point in spawnPointsHash)
    //    {
    //        if (point.Value)
    //        {
    //            return point.Key;
    //        }
    //    }
    //    print("no available spawn point");
    //    //refresh available spawn points.
    //    RefreshSpawnPoints();
    //    return spawnPointsHash.;
    //}

    void Respawn()
    {

    }

    void RefreshSpawnPoints()
    {
        //set all spawn points availability to true.
        foreach(var point in spawnPointsHash)
            spawnPointsHash[point.Key] = true;
        
        UpdateRoomProperties();
    }

    void UpdateRoomProperties()
    {
        //get current properties
        Hashtable currentProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        //update or add spawn points
        currentProperties["SpawnPoints"] = spawnPointsHash;

        //update room properties :D
        PhotonNetwork.CurrentRoom.SetCustomProperties(currentProperties);
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.TryGetValue("SpawnPoints", out object spawnPointsObject))
        {
            spawnPointsHash = spawnPointsObject as Dictionary<int, bool>;
            UpdateLocalSpawnPoints();
        }
    }
    private void UpdateLocalSpawnPoints()
    {
        foreach (var point in _spawnPoints)
        {
            if (spawnPointsHash.TryGetValue(point.ID, out bool available))
                point.Available = available;
        }
    }
}
