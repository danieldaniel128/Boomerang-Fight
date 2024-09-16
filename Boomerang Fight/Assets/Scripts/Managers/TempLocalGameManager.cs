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
    }

    public void AddPlayerCharacter(OnlinePlayer player)
    {
        playerCharacters.Add(player);
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
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

    public void RemovePlayer()
    {
        PhotonNetwork.LeaveRoom();
    }

}
