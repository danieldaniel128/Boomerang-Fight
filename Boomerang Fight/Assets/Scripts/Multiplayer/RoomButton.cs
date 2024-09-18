using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _amountOfPlayers;
    [SerializeField] TextMeshProUGUI _roomLabel;//join/exit
    [SerializeField] int _roomId;
    private int _playerCount;
    //public int PlayerCount { get { return _playerCount; } set { _playerCount = value; RefreshRoomButtonPlayerAmount(_playerCount,_maxPlayers); } }
    [SerializeField] private int _maxPlayers;
    public Button RoomBTN;
    private string _roomName;
    public bool IsInRoom = false;
    //public void RefreshRoomButtonPlayerAmount(int playerCount,int maxPlayers)
    //{
    //    _amountOfPlayers.text = $"{string.Format("{0}/{1}", playerCount, _maxPlayers)}";
    //}

    public void SetRoomID(int roomID)
    {
        _roomId = roomID;
    }
    public void SetMaxPlayers(int maxPlayers) 
    {
        _roomName = $"{_maxPlayers} Players";
    }
    public int GetMaxPlayers()
    {
        return _maxPlayers;
    }
    public int GetRoomID()
    {
        return _roomId; 
    }
    public string GetName()
    {
        return _roomName;
    }
    public void EnterExitRoom()
    {
        IsInRoom = !IsInRoom;
        _roomLabel.text = !IsInRoom ? "Join" : "Exit";
    }
}
