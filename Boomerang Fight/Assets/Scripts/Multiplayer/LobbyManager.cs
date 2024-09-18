using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string GAME_SCENE_NAME = "Game Scene";

    [Header("room settings/monitors")]
    [SerializeField] private int _maxPlayersInRoom = 4;
    [SerializeField] private TextMeshProUGUI _currentRoomPlayersTXT;
    [Header("LobbyPanels")]
    [SerializeField] private GameObject SelectRoomsPanel;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject waitingForPlayersPanel;

    List<RoomInfo> _roomsList = new List<RoomInfo>();
    [SerializeField] List<RoomButton> _roomsButtons;
    [SerializeField] Button _startGameBTN;
    int playerLastRoomID;
    //[SerializeField] Button _quickMatchBTN;
    bool _isCreatingRooms = true;
    private const byte PLAYER_COUNT_EVENT = 1;
    //private void OnEnable()
    //{
    //    _quickMatchBTN.interactable = !CheckPlayersInGameStatus();
    //}

    public override void OnEnable()
    {
        base.OnEnable();
        MainPanel.SetActive(true);
    }
    private void OnDisable()
    {
    }
    //private void OnEvent(EventData photonEvent)
    //{
    //    if (photonEvent.Code == PLAYER_COUNT_EVENT)
    //    {
    //        object[] data = (object[])photonEvent.CustomData;
    //        int roomID = (int)data[0];
    //        int change = (int)data[1];

    //        // Now update the room UI
    //        RoomButton roomButton = _roomsButtons.Find(btn => btn.GetRoomID() == roomID);
    //        if (roomButton != null)
    //        {
    //            roomButton.PlayerCount += change;
    //            Debug.Log($"Updated player count for room {roomID} by {change}. New count: {roomButton.PlayerCount}");
    //             roomButton.RefreshRoomButtonPlayerAmount(roomButton.PlayerCount, roomButton.GetMaxPlayers());
    //        }
    //    }
    //}

    void InitRoomButtons()
    {
        for (int i = PhotonNetwork.CountOfRooms; i < 4; i++)
        {
            _roomsButtons[i].SetRoomID(i);
        }
    }
    void CreateRoom(RoomButton roomButton)
    {

        RoomOptions roomOptions = new RoomOptions();
        int maxPlayer = (roomButton.GetRoomID() + 1);
        roomOptions.MaxPlayers = (byte)maxPlayer;
        roomButton.SetMaxPlayers(maxPlayer);
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.PlayerTtl = 0;
        
        // Create rooms but don't join them
        PhotonNetwork.JoinOrCreateRoom("Room " + (maxPlayer), roomOptions, TypedLobby.Default);
    }
    
    private void Start()
    {
        //set their on click
        InitRoomButtons();
        for (int i = 0; i < _roomsButtons.Count; i++)
        {
            RoomButton roomButton = _roomsButtons[i];
            roomButton.RoomBTN.onClick.AddListener(() => JoinExitRoom(roomButton));
        }
    }
    void JoinExitRoom(RoomButton roomButton)
    {
        Debug.Log("joinexit");
        playerLastRoomID = roomButton.GetRoomID();
        if (!roomButton.IsInRoom)
            CreateRoom(roomButton);
        else
            LeaveRoom();
    }

    void RequestRoomList()
    {
        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
    }
    public RoomButton GetButton(int buttonID)
    {
        return _roomsButtons.Find(btn => btn.GetRoomID() == (buttonID));
    }


    // Called when the room list is updated (i.e., after joining the lobby or when rooms change)


    // Coroutine to leave the room right after creating it


    /// <summary>
    /// creates and enters room.
    /// </summary>


    #region IMatchmakingCallbacks
    #region UsedCallBacks

    public override void OnCreatedRoom()
    {
        Debug.Log("room created" + PhotonNetwork.NetworkingClient.CurrentRoom.Name);
        GetButton(playerLastRoomID);
    }
    public override void OnJoinedRoom()
    {
        _roomsButtons[playerLastRoomID].EnterExitRoom();
        //5RaisePlayerCountEvent(playerLastRoomID, 1); // +1 for joining
        foreach (RoomButton roomButton in _roomsButtons.Where(c=>!c.IsInRoom))
            roomButton.RoomBTN.interactable = false;
        Debug.Log("joined room " + PhotonNetwork.NetworkingClient.CurrentRoom.Name);
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            StartGame();//_startGameBTN.interactable=true;
        //UpdateRoomPlayerCount();
        // _roomsButtons[playerLastRoomID].RefreshRoomButtonPlayerAmount(PhotonNetwork.CurrentRoom.PlayerCount,0);
        RefreshPlayerCountTXT();
        WaintingToPlayersPanel();
    }
    public void StartGame()
    {
        photonView.RPC(nameof(LoadGameEnable), RpcTarget.MasterClient);
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        _roomsButtons[playerLastRoomID].EnterExitRoom();
        foreach (RoomButton roomButton in _roomsButtons.Where(c => !c.IsInRoom))
            roomButton.RoomBTN.interactable = true;
        _startGameBTN.interactable = false;
        Debug.Log("left room");
        GoToSelectRoom();
    }
    public void BackToMainMenu()
    {
        MainPanel.SetActive(true);
        SelectRoomsPanel.SetActive(false);
        waitingForPlayersPanel.SetActive(false);
    }
    public void WaintingToPlayersPanel()
    {
        MainPanel.SetActive(false);
        SelectRoomsPanel.SetActive(false);
        waitingForPlayersPanel.SetActive(true);
    }
    public void GoToSelectRoom()
    {
        MainPanel.SetActive(false);
        SelectRoomsPanel.SetActive(true);
        waitingForPlayersPanel.SetActive(false);
    }
    public void LeaveRoom()
    {
        //_roomsButtons[playerLastRoomID].RefreshRoomButtonPlayerAmount(PhotonNetwork.CurrentRoom.PlayerCount,0);
        //DecreaseUpdateRoomPlayerCountBeforeLeft();
        PhotonNetwork.LeaveRoom();
    }
    [PunRPC]
    public void LoadGameEnable()
    {

        _startGameBTN.interactable = true ;
    }
    public void LoadLevel()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
        
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
    private void RefreshPlayerCountTXT()
    {
        _currentRoomPlayersTXT.text = $"Found Players " +
            $"{string.Format("{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers)}";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RefreshPlayerCountTXT();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshPlayerCountTXT();
        _startGameBTN.interactable=false;
    }


    private void DecreaseUpdateRoomPlayerCountBeforeLeft()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerCount", (byte)(Convert.ToInt32(PhotonNetwork.CurrentRoom.PlayerCount)-1) }
        });
        }

    }
    private void UpdateRoomPlayerCount()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerCount", PhotonNetwork.CurrentRoom.PlayerCount }
        });
        }
    }
    // Call this when a player joins or leaves the room
    private void RaisePlayerCountEvent(int roomID, int change)
    {
        object[] content = new object[] { roomID, change }; // Data to send (roomID and player count change)
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Send to everyone
        PhotonNetwork.RaiseEvent(PLAYER_COUNT_EVENT, content, raiseEventOptions, SendOptions.SendReliable);
    }


    #endregion

    #endregion

}
