using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    public struct PendingPlayer
    {
        public NetworkConnectionToClient conn;
        public GameObject roomPlayer;
    }
    public OnlineRoomController roomController;
    public string roomScene;
    public string gameScene;
    public HashSet<PendingPlayer> pendingPlayers = new HashSet<PendingPlayer>();
    public HashSet<OnlineRoomController> roomControllers = new HashSet<OnlineRoomController>();
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }
}