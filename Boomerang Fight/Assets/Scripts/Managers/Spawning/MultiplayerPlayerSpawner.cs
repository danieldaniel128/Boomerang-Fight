using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System;
using Random = UnityEngine.Random;

public class MultiplayerPlayerSpawner : MonoBehaviourPunCallbacks
{
    public static MultiplayerPlayerSpawner Instance { get; private set; }

    [SerializeField] SpawnPoint[] _spawnPoints;
    Player _myPlayer;
    const string PLAYER_RESOURCE_NAME = "Player";
    const string SPAWN_POINTS_KEY = "SpawnPoints";

    private Dictionary<int, bool> spawnPointsHash = new Dictionary<int, bool>();
    public Action<int> OnRespawn;

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
        //UpdateRoomSpawnProperties();
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

        spawnPointsHash[id] = availability;
        //UpdateRoomSpawnProperties();
    }

    private int GetAvailableSpawnPoint()
    {
        foreach (var point in spawnPointsHash)
        {
            if (point.Value) // if available
            {
                return point.Key;
            }
        }
        print("no available spawn point");
        //refresh available spawn points and get random spawn point
        RefreshSpawnPoints();
        int randSpawnID = 0;
        return randSpawnID;
    }

    public void TryRespawn(int id)
    {
        photonView.RPC(nameof(MasterGetSpawnPoint), RpcTarget.MasterClient, id);
    }

    [PunRPC]
    public void Respawn(int playerID, int spawnID)
    {
        OnlinePlayer deadPlayer = TempLocalGameManager.Instance.GetOnlinePlayer(playerID);

        //move transform to spawn point
        deadPlayer.gameObject.transform.position = _spawnPoints[spawnID].transform.position;

        deadPlayer.gameObject.GetComponent<Health>().TogglePlayerBodyAndHealth(true);
        deadPlayer.GameUIManager.DisableDeathScreen();
        deadPlayer.PlayerControllerRef.enabled = true;
        deadPlayer.PlayerControllerRef.AnimationController.ResetAnimations();
    }

    [PunRPC]
    public void MasterGetSpawnPoint(int playerID)
    {
        int randSpawnID = GetAvailableSpawnPoint();
        SetSpawnPointAvailability(randSpawnID, false);

        photonView.RPC(nameof(Respawn), RpcTarget.All, playerID, randSpawnID);
    }

    /// <summary>
    /// when spawn points are all unavailable, makes them all available again.
    /// </summary>
    void RefreshSpawnPoints()
    {
        //set all spawn points availability to true.
        foreach (var point in spawnPointsHash)
            spawnPointsHash[point.Key] = true;

        //UpdateRoomSpawnProperties();
    }

    void UpdateRoomSpawnProperties()
    {
        //get current properties
        Hashtable currentProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        //update or add properties
        currentProperties[SPAWN_POINTS_KEY] = spawnPointsHash;

        foreach (var point in spawnPointsHash)
        {
            print("point " + point.Key + ": " + point.Value);
        }

        //PhotonNetwork.CurrentRoom.SetCustomProperties(currentProperties);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.TryGetValue(SPAWN_POINTS_KEY, out object spawnPointsObject))
        {
            print("onroomp prp update spawn points");
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
