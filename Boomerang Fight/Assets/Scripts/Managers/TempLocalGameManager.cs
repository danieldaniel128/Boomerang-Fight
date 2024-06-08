using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLocalGameManager : MonoBehaviour
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

    public GameObject GetPlayerCharacterBasedOnID(int id)
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
}
