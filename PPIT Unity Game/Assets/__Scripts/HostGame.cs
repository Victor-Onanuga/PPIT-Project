using UnityEngine;
using UnityEngine.Networking;

public class HostGame : NetworkBehaviour
{
    [SerializeField] private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;

        if(networkManager.matchMaker == null)// checks to see if a room exists already
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    // Creating Room
    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
            // Create room
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, " ", " ", " ", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
