using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLocalGameManager : MonoBehaviour
{
    public static TempLocalGameManager Instance;

    [SerializeField] List<OnlinePlayer> playerCharacters = new();

    public List<OnlinePlayer> PlayerCharacters => playerCharacters;

    private void Awake()
    {
        Instance = this;
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
}
