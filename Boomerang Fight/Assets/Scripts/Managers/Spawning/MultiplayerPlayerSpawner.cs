using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPlayerSpawner : MonoBehaviourPun
{
    public static MultiplayerPlayerSpawner Instance;
    [SerializeField] SpawnPoint[] _spawnPoints;
    [SerializeField] int _playerLayerIndex = 3; // Change to your desired player layer index
    [SerializeField] int _enemyLayerIndex = 7;  // Change to your desired enemy layer index
    int index;
    int _myPlayerIndex;
    Player _myPlayer;
    const string PLAYER_RESOURCE_NAME = "Player";
    [SerializeField] List<PlayerController> _playersPlayerControllers;
    [SerializeField] PlayerController _localPlayerController;

    public void RegisterPlayerController(PlayerController playerController)
    {
        _playersPlayerControllers.Add(playerController);
    }
    public void SetMyPlayerController(PlayerController playerController)
    {
        _localPlayerController = playerController;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GetMyPlayer();
        SpawnPlayers();
        SetEnemiesLayerRPC();
        StartCoroutine(SetLayerAfterFrame());
    }
    IEnumerator SetLayerAfterFrame()
    {
        yield return null;
        SetEnemiesLayer();
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
    void SetEnemiesLayer()
    {
        if (PhotonNetwork.IsMasterClient)
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                photonView.RPC(nameof(SetEnemiesLayerRPC), player);
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
    void InstantiationPlayerRPC(int index)
    {
        GameObject playerGameobject = PhotonNetwork.Instantiate(PLAYER_RESOURCE_NAME, _spawnPoints[index].SpawnPosition, Quaternion.identity, 0);
            playerGameobject.layer = _playerLayerIndex;
        _myPlayerIndex = index;
    }

    [PunRPC]
    void SetEnemiesLayerRPC()
    {
        Debug.Log(_playersPlayerControllers);
        foreach (var playerController in _playersPlayerControllers)
        {
            if (playerController != _localPlayerController)
            {
                playerController.gameObject.layer = _enemyLayerIndex;
            }
        }
    }
}   
