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
        if(!photonView.IsMine) gameObject.SetActive(false);
    }
    public void SetPlayersAliveCountText(InGameData data)
    {
        _playersAliveCount_TMP.text = $"Players Left: {data.PlayersAliveCount}";
    }


}
