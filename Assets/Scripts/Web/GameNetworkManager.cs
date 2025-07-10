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
    public IRoomController roomController;
    public string roomScene;
    public string gameScene;
    public HashSet<PendingPlayer> pendingPlayers = new HashSet<PendingPlayer>();
    public HashSet<IRoomController> roomControllers = new HashSet<IRoomController>();
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }
    public override void OnClientDisconnect() 
    { 
        base.OnClientDisconnect();
    }
    public override void OnClientNotReady() 
    { 
        base.OnClientNotReady();
    }
}