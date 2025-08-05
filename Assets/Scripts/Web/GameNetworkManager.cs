using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        Debug.Log("GameNetworkManager : OnStartServer");
        base.OnStartServer();
        GameEntry.EventComponent.Notify(GameEvent.ServerStartEvent);
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log("GameNetworkManager : OnServerConnect");
        base.OnServerConnect(conn);
        GameEntry.EventComponent.Notify(GameEvent.ServerConnectEvent,conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("GameNetworkManager : OnServerDisconnect");
        base.OnServerDisconnect(conn);
        GameEntry.EventComponent.Notify(GameEvent.ServerDisconnectEvent, conn);
    }
    public override void OnClientDisconnect() 
    {
        Debug.Log("GameNetworkManager : OnClientDisconnect");
        base.OnClientDisconnect();
        GameEntry.EventComponent.Notify(GameEvent.ClientDisconnectEvent);
    }
    public override void OnClientConnect()
    {
        Debug.Log("GameNetworkManager : OnClientConnect");
        base.OnClientConnect();
        GameEntry.EventComponent.Notify(GameEvent.ClientConnectEvent);
    }
}