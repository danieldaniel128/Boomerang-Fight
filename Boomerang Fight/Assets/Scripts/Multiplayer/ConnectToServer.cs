using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField _playerNameInput;
    [SerializeField] TextMeshProUGUI _connectingState_TMP;
    [SerializeField] GameObject _lobbyPanel;

    /// <summary>
    /// tries to connect to server using setting and seting player nickname
    /// </summary>
    public void ConnectPlayerToServer()
    {
        PhotonNetwork.NickName = _playerNameInput.text;
        _connectingState_TMP.text = "Connecting...";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// calls when the the player connected to the main server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        EnterLobby();
    }
    /// <summary>
    /// switches to select rooms when connected
    /// </summary>
    private void EnterLobby()
    {
        //select room scene
        SceneManager.LoadScene(1);
    }
}
