using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("room settings/monitors")]
    [SerializeField] private int _maxPlayersInRoom = 4;
    int _playerInRoomCount = 1;
    [SerializeField] private TMP_Text _currentRoomPlayersTXT;
    [SerializeField] private TMP_Text _roomPlayersCountPlayersTXT;
    [Header("LobbyPanels")]
    [SerializeField] private GameObject SearchingPlayersPanel;
    [SerializeField] private GameObject QuickMatchPanel;
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _createRoomBtn;

    private const string GAME_SCENE_NAME = "Game Scene";

    private void OnEnable()
    {
        SetPlayerCountInRoomTXTBtnEvent();
    }
    private void SetPlayerCountInRoomTXTBtnEvent()
    {
        _roomPlayersCountPlayersTXT.text = _playerInRoomCount.ToString();
    }
    public void AddPlayerToSetting()
    {
        _playerInRoomCount++;
        _playerInRoomCount = Mathf.Clamp(_playerInRoomCount, 1, _maxPlayersInRoom);
        SetPlayerCountInRoomTXTBtnEvent();
    }
    public void DecreasePlayerToSetting()
    {
        _playerInRoomCount--;
        _playerInRoomCount = Mathf.Clamp(_playerInRoomCount, 1, _maxPlayersInRoom);
        SetPlayerCountInRoomTXTBtnEvent();
    }
    private void RefreshPlayerCountTXT()
    {
        _currentRoomPlayersTXT.text = $"Found Players " +
            $"{string.Format("{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers)}";
    }

    public void CreateSettingsRoom()
    {
        CreateRoom();
    }
    /// <summary>
    /// creates and enters room.
    /// </summary>
    private void CreateRoom()
    {
        if (PhotonNetwork.InRoom)
            return;
        ////set room options
        RoomOptions roomOptions = new RoomOptions() {  MaxPlayers = (byte)_maxPlayersInRoom };
        ////create and enter room
        PhotonNetwork.JoinOrCreateRoom($"Room {PhotonNetwork.NetworkingClient.RoomsCount + 1}", roomOptions,null);
        _createRoomBtn.interactable = false;
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

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(LoadGame), RpcTarget.MasterClient);
        //if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
    }
    #region IMatchmakingCallbacks
    #region UsedCallBacks
    
    public override void OnCreatedRoom()
    {
        Debug.Log("dasdsa");
        SearchingPlayersPanel.SetActive(true);
        Debug.Log("room created" + PhotonNetwork.NetworkingClient.CurrentRoom.Name);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"<color=green>player: {PhotonNetwork.LocalPlayer.NickName} joined room {PhotonNetwork.NetworkingClient.CurrentRoom.Name}</color>");
        QuickMatchPanel.SetActive(false);
        SearchingPlayersPanel.SetActive(true);
        RefreshPlayerCountTXT();
        if(!PhotonNetwork.IsMasterClient)
            _startGameBtn.interactable = false;
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
