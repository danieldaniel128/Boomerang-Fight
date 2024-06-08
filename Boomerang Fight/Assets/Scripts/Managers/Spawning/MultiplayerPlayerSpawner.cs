using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class MultiplayerPlayerSpawner : MonoBehaviourPunCallbacks
{
    public static MultiplayerPlayerSpawner Instance { get; private set; }

    [SerializeField] SpawnPoint[] _spawnPoints;
    Player _myPlayer;
    const string PLAYER_RESOURCE_NAME = "Player";
    const string SPAWN_POINTS_KEY = "SpawnPoints";
    const string PLAYER_CHARACTER_OBJECT_KEY = "PlayerCharacterObject";

    private Dictionary<int, bool> spawnPointsHash = new Dictionary<int, bool>();
    /// <summary>
    /// dictionary of int index and int type
    /// </summary>
    private Dictionary<int, int> actorToCharacterHash = new();


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitializeSpawnPoints();
            InitialSpawnPlayers();
        }
    }

    /// <summary>
    /// happens once at the start, adds all spawn points to room properties
    /// </summary>
    void InitializeSpawnPoints()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            spawnPoint.Available = true;
            spawnPointsHash.Add(spawnPoint.ID, spawnPoint.Available);
        }
        UpdateRoomSpawnProperties();
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
    /// <summary>
    /// Instantiate a player and update actor to character hash
    /// </summary>
    /// <param name="index"></param>
    [PunRPC]
    void InstantiationPlayerRPC(int index)
    {
        //photon instantiate player object
        GameObject playerGameobject = PhotonNetwork.Instantiate(PLAYER_RESOURCE_NAME, _spawnPoints[index].SpawnPosition, Quaternion.identity, 0);
    }

    /// <summary>
    /// set specific spawn point to unavailable or available and updates everyone.
    /// </summary>
    /// <param name="id">SpawnPoint ID</param>
    /// <param name="availability">available or unavailable bool</param>
    public void SetSpawnPointAvailability(int id, bool availability)
    {
        if (!spawnPointsHash.ContainsKey(id))
            return;

        if (availability)
        {
            spawnPointsHash[id] = availability;
            UpdateRoomSpawnProperties();
        }
    }

    //private int GetAvailableSpawnPoint()
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


    [PunRPC]
    public void Respawn(int id)
    {
        GameObject go = TempLocalGameManager.Instance.GetPlayerCharacterBasedOnID(id);
        //get random available spawn point
        int randSpawnID = 2;

        //move transform to spawn point
        go.transform.position = _spawnPoints[randSpawnID].transform.position;
        //TODO reset player here. (boomerang, health, ui...)
    }

    /// <summary>
    /// when spawn points are all unavailable, makes them all available again.
    /// </summary>
    void RefreshSpawnPoints()
    {
        //set all spawn points availability to true.
        foreach(var point in spawnPointsHash)
            spawnPointsHash[point.Key] = true;
        
        UpdateRoomSpawnProperties();
    }

    void UpdateRoomSpawnProperties()
    {
        //get current properties
        Hashtable currentProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        //update or add properties
        currentProperties[SPAWN_POINTS_KEY] = spawnPointsHash;

        //update room properties
        //PhotonNetwork.CurrentRoom.SetCustomProperties(currentProperties);
    }
    void UpdateRoomActorToCharacterProperties()
    {
        //get current properties
        Hashtable currentProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        //update or add properties
        currentProperties[PLAYER_CHARACTER_OBJECT_KEY] = actorToCharacterHash;

        //update room properties
        PhotonNetwork.CurrentRoom.SetCustomProperties(currentProperties);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.TryGetValue(SPAWN_POINTS_KEY, out object spawnPointsObject))
        {
            spawnPointsHash = spawnPointsObject as Dictionary<int, bool>;
            UpdateLocalSpawnPoints();
        }
        //if(propertiesThatChanged.TryGetValue(PLAYER_CHARACTER_OBJECT_KEY, out object actorToCharacterObject))
        //{
        //    actorToCharacterHash = actorToCharacterObject as Dictionary<int, Object>;
        //    DebugActorToPlayerHash();
        //}
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
