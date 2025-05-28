using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallNetworkManager : NetworkManager
{
    private HallManager hallManager;
    public override void Awake()
    {
        base.Awake();
        hallManager = GetComponent<HallManager>();
    }
    public override void OnServerReady(NetworkConnectionToClient conn) //当有一个客户端连接时服务器调用
    {
        base.OnServerReady(conn);
        hallManager.OnServerReady(conn);
    }
    public override void OnServerDisconnect(NetworkConnectionToClient conn) //当有一个客户端断开连接服务器时服务器调用
    {
        base.OnServerDisconnect(conn);
        StartCoroutine(DoServerDisconnect(conn));
    }
    public override void OnClientDisconnect() //当客户端自身断开连接时客户端调用
    {
        hallManager.OnClientDisconnect();
        base.OnClientDisconnect();
    }
    public override void OnStartServer()
    {
        hallManager.OnStartServer();
    }
    public override void OnStopServer()
    {
        hallManager.OnStopServer();
    }
    public override void OnStartClient()
    {
        hallManager.OnStartClient();
    }
    public override void OnStopClient()
    {
        hallManager.OnStopClient();
    }

    private IEnumerator DoServerDisconnect(NetworkConnectionToClient conn)
    {
        yield return hallManager.OnServerDisconnect(conn);
        base.OnServerDisconnect(conn);
    }
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        //OnlineRoomController.instance.OnReady();
    }
}
