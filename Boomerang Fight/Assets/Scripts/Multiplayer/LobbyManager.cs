using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("room settings/monitors")]
    [SerializeField] private int _maxPlayersInRoom = 4;
    [SerializeField] private TextMeshProUGUI _currentRoomPlayersTXT;
    [Header("LobbyPanels")]
    [SerializeField] private GameObject SearchingPlayersPanel;
    [SerializeField] private GameObject QuickMatchPanel;

    private const string GAME_SCENE_NAME = "Game Scene";

    private void RefreshPlayerCountTXT()
    {
        _currentRoomPlayersTXT.text = $"Found Players " +
            $"{string.Format("{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers)}";
    }
    /// <summary>
    /// creates and enters room.
    /// </summary>
    private void CreateRoom()
    {
        //set room options
        RoomOptions roomOptions = new RoomOptions() {  MaxPlayers = (byte)_maxPlayersInRoom }; 
        //create and enter room
        PhotonNetwork.CreateRoom($"Room {PhotonNetwork.NetworkingClient.RoomsCount + 1}", roomOptions);
    }
    /// <summary>
    /// tries to join a random room. if there is no room or failed, OnJoinRandomFailed will call.
    /// used onclick play button.
    /// </summary>
    public void QuickMatch()
    {
        //joins a random room.
        PhotonNetwork.JoinRandomRoom();
    }
    #region IMatchmakingCallbacks
    #region UsedCallBacks
    
    public override void OnCreatedRoom()
    {
        //Debug.Log("room created" + PhotonNetwork.NetworkingClient.CurrentRoom.Name);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"<color=green>player: {PhotonNetwork.LocalPlayer.NickName} joined room {PhotonNetwork.NetworkingClient.CurrentRoom.Name}</color>");
        QuickMatchPanel.SetActive(false);
        SearchingPlayersPanel.SetActive(true);
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            photonView.RPC(nameof(LoadGame),RpcTarget.MasterClient);
        RefreshPlayerCountTXT();
    }
    public override void OnLeftRoom()
    {
        QuickMatchPanel.SetActive(true);
        SearchingPlayersPanel.SetActive(false);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    [PunRPC]
    private void LoadGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    /// <summary>
    /// create new room if it failed
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("<color=red>joined failed</color>");
        CreateRoom();
    }
    #endregion
    #region UnusedCallBacks
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("<color=red>creating room has failed</color>");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RefreshPlayerCountTXT();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshPlayerCountTXT();
    }

    #endregion

    #endregion

}
