using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    

    public void ConnectPlayerToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// calls when the the player connected to the main server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
