using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineGameManager : MonoBehaviourPunCallbacks
{
    public static OnlineGameManager instance;
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
    
    
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)//player won
            {
                StartCoroutine(WinCoro());
            }
        }

    }
    IEnumerator WinCoro()
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
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
        return null;
    }
    
    IEnumerator SetLayerOnAllPlayersJoined()
    {
        //wait until all players are in game
        yield return new WaitUntil(() => _onlinePlayersManagers.Count == PhotonNetwork.CountOfPlayers);
        
    }
    
    public void RegisterPlayerController(OnlinePlayerManager onlinePlayerManager)
    {
        _onlinePlayersManagers.Add(onlinePlayerManager);
    }
    public void SetMyPlayerController(OnlinePlayerManager onlinePlayerManager)
    {
        _onlinePlayerManager = onlinePlayerManager;
    }
    

}
