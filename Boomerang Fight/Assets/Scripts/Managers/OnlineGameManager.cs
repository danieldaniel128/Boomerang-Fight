using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameManager : MonoBehaviourPun
{
    public static OnlineGameManager instance;
    [SerializeField] UpdateInGameUIData _updateInGameUIData;
    [Header("setting")]
    [SerializeField] int _playerLayerIndex = 3; // Change to your desired player layer index
    [SerializeField] int _enemyLayerIndex = 7;  // Change to your desired enemy layer index
    [SerializeField] private List<OnlinePlayerManager> _onlinePlayersManagers;
    [SerializeField] OnlinePlayerManager _onlinePlayerManager;
    InGameData _gameData;
    public static OnlineGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        SubscribeToPlayersOnDeath();
    }
    private void OnDisable()
    {
        UnSubscribeToPlayersOnDeath();
    }
    void SubscribeToPlayersOnDeath()
    {
        foreach (var item in _onlinePlayersManagers)
        {
            item.OnPlayerDeath += DecreasePlayersAliveCountEvent;
        }
    }
    void UnSubscribeToPlayersOnDeath()
    {
        foreach (var item in _onlinePlayersManagers)
        {
            item.OnPlayerDeath -= DecreasePlayersAliveCountEvent;
        }
    }
    public Player GetMyPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsLocal)
            {
                return player;
            }
        }
        Debug.Log("returned Null");
        return null;
    }
    private void Start()
    {
        StartCoroutine(SetLayerOnAllPlayersJoined());
        //_gameData = new InGameData(PhotonNetwork.CurrentRoom.PlayerCount);
        //set ui of game data
        //_updateInGameUIData.SetPlayersAliveCountText(_gameData);
        //set layers to all players
    }
    IEnumerator SetLayerOnAllPlayersJoined()
    {
        //wait until all players are in game
        yield return new WaitUntil(() => _onlinePlayersManagers.Count == PhotonNetwork.CountOfPlayers);
        SetEnemiesLayer();
    }
    void SetEnemiesLayer()
    {
        foreach (var onlinePlayerManager in _onlinePlayersManagers)
        {
            if (onlinePlayerManager == _onlinePlayerManager)//if my player.
                onlinePlayerManager.gameObject.layer = _playerLayerIndex;
            else//if its not my player.
                onlinePlayerManager.gameObject.layer = _enemyLayerIndex;
        }
    }
    public void RegisterPlayerController(OnlinePlayerManager onlinePlayerManager)
    {
        _onlinePlayersManagers.Add(onlinePlayerManager);
    }
    public void SetMyPlayerController(OnlinePlayerManager onlinePlayerManager)
    {
        _onlinePlayerManager = onlinePlayerManager;
    }
    //called on playes death
    public void DecreasePlayersAliveCountEvent()//called on players death
    {
        _gameData.DecreasePlayersAliveCount();
        _updateInGameUIData.SetPlayersAliveCountText(_gameData);
    }

}
