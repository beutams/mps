using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    private RoomNetworkMangaer roomNetwork;
    public override void OnStartServer()
    {
        base.OnStartServer();
        roomNetwork.OnStartServer();
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        roomNetwork.OnClientConnectServer(conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        roomNetwork.OnClientDisconnectServer(conn);
    }
    public override void OnClientDisconnect() 
    {
        base.OnClientDisconnect();
        roomNetwork.OnClientDisconnect();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        roomNetwork.OnStartClient();
    }
}