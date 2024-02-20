using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateInGameUIData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playersAliveCount_TMP;

    public void SetPlayersAliveCountText(InGameData data)
    {
        _playersAliveCount_TMP.text = $"Players Left: {data.PlayersAliveCount}";
    }


}
