using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InGameData
{
    public int PlayersAliveCount { get; private set; }

    public InGameData(int PlayersCount)
    {
        PlayersAliveCount = PlayersCount;
    }
    public void DecreasePlayersAliveCount()
    {
        PlayersAliveCount--;
    }
}
