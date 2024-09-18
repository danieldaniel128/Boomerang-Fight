using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempLocalGameManager : MonoBehaviourPunCallbacks
{
    public static TempLocalGameManager Instance;

    [SerializeField] List<OnlinePlayer> playerCharacters = new();

    public GameObject endGameScreen;

    public List<OnlinePlayer> PlayerCharacters => playerCharacters;

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void AddPlayerCharacter(OnlinePlayer player)
    {
        playerCharacters.Add(player);
    }
    //public GameObject GetPlayerGameObjectBasedOnID(int id)
    //{
    //    foreach (var character in playerCharacters)
    //    {
    //        if(character.TryGetComponent(out OnlinePlayer currentPlayer))
    //        {
    //            if(currentPlayer.ID == id)
    //            {
    //                return character;
    //            }
    //        }
    //    }
    //    return null;
    //}

    public OnlinePlayer GetOnlinePlayer(int id)
    {
        foreach(var player in playerCharacters)
        {
            if (player.ID == id) return player;
        }
        return null;
    }

    public void ActivateEndGameScreen()
    {
        Invoke(nameof(EnableEndGameScreen), 1.5f);
    }

    void EnableEndGameScreen()
    {
        endGameScreen.SetActive(true);
    }

    public void RemovePlayer()
    {
        // Cancel any invokes or coroutines that might cause issues when leaving the room
        CancelInvoke();
        StopAllCoroutines();
        PhotonNetwork.LeaveRoom();
    }

}
