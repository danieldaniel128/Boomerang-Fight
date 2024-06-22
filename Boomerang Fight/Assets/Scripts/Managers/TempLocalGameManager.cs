using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempLocalGameManager : MonoBehaviourPunCallbacks
{
    public static TempLocalGameManager Instance;

    [SerializeField] List<GameObject> playerCharacters = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayerCharacter(GameObject go)
    {
        playerCharacters.Add(go);
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public GameObject GetPlayerGameObjectBasedOnID(int id)
    {
        foreach (var character in playerCharacters)
        {
            if(character.TryGetComponent(out OnlinePlayer currentPlayer))
            {
                if(currentPlayer.ID == id)
                {
                    return character;
                }
            }
        }
        return null;
    }

    public OnlinePlayer GetOnlinePlayer(int id)
    {
        foreach(var character in playerCharacters)
        {
            if (character.TryGetComponent(out OnlinePlayer currentPlayer))
            {
                if (currentPlayer.ID == id)
                {
                    return currentPlayer;
                }
            }
        }
        return null;
    }
}
