using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateInGameUIData : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI _playersAliveCount_TMP;

    private void Start()
    {
        SetPlayersAliveCountText();
    }
    public void SetPlayersAliveCountText()
    {
        _playersAliveCount_TMP.text = $"Players Left: {PhotonNetwork.CurrentRoom.PlayerCount}";
    }


}
