using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerOnlineManager : MonoBehaviourPun
{
    public static PlayerOnlineManager instance;
    
    [SerializeField] UpdateInGameUIData updateInGameUIData;
    InGameData _gameData;
    private void Start()
    {
        _gameData = new InGameData(PhotonNetwork.CurrentRoom.PlayerCount);
        updateInGameUIData.SetPlayersAliveCountText(_gameData);
    }

    //called on playes death
    public void DecreasePlayersAliveCountEvent()//called on players death
    {
        _gameData.DecreasePlayersAliveCount();
        updateInGameUIData.SetPlayersAliveCountText(_gameData);
    }

}
