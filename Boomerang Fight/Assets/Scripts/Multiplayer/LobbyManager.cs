using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] int _maxPlayersInRoom = 4;

    /// <summary>
    /// creates and enters room.
    /// </summary>
    private void CreateRoom()
    {
        //set room options
        RoomOptions roomOptions = new RoomOptions() {  MaxPlayers = (byte)_maxPlayersInRoom }; 
        //set enteredroom prams
        EnterRoomParams enterRoomParams = new EnterRoomParams() {RoomName = $"Room {PhotonNetwork.NetworkingClient.RoomsCount + 1}", RoomOptions = roomOptions };
        //create and enter room
        PhotonNetwork.NetworkingClient.OpCreateRoom(enterRoomParams);
    }
    /// <summary>
    /// tries to join a random room. if there is no room or failed, OnJoinRandomFailed will call.
    /// used onclick play button.
    /// </summary>
    public void QuickMatch()
    {
        //joins a random room.
        PhotonNetwork.NetworkingClient.OpJoinRandomRoom();
    }
    #region IMatchmakingCallbacks
    #region UsedCallBacks
    
    public override void OnCreatedRoom()
    {
        //Debug.Log("room created" + PhotonNetwork.NetworkingClient.CurrentRoom.Name);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"player: {PhotonNetwork.LocalPlayer.NickName} joined room {PhotonNetwork.NetworkingClient.CurrentRoom.Name}");
    }

    /// <summary>
    /// create new room if it failed
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("joined failed");
        CreateRoom();
    }
    #endregion

    #region UnusedCallBacks
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("creating room has failed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
    }
    #endregion

    #endregion

}
