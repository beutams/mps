using Mirror;
using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomNetworkMangaer : MonoBehaviour
{
    private HallSubUI hallSubUI;
    private Dictionary<NetworkConnectionToClient, PlayerInfo> connDic = new Dictionary<NetworkConnectionToClient, PlayerInfo>();
    private void Awake()
    {
        hallSubUI = FindAnyObjectByType<HallSubUI>();
        GameEntry.EventComponent.Subscribe(GameEvent.ServerStartEvent, OnStartServer);
        GameEntry.EventComponent.Subscribe(GameEvent.ServerConnectEvent, OnClientConnectServer);
        GameEntry.EventComponent.Subscribe(GameEvent.ServerDisconnectEvent, OnClientDisconnectServer);
        GameEntry.EventComponent.Subscribe(GameEvent.ClientDisconnectEvent, OnClientDisconnect);
        GameEntry.EventComponent.Subscribe(GameEvent.ClientConnectEvent, OnClientConnect);
    }
    [ServerCallback]
    public void OnStartServer(object data)
    {
        Debug.Log("Room OnStartSercer");
        NetworkServer.RegisterHandler<ServerMessage>(OnServerMessage);
        hallSubUI.isSercer = true;
    }
    [ServerCallback]
    public void OnClientDisconnectServer(object conn)
    {
        Debug.Log("Room OnClientDisconnectServer");
        connDic.Remove(conn as NetworkConnectionToClient);
        foreach(var playerConn in connDic.Keys)
        {
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = connDic.Values.ToArray() }) ;
        }
    }
    [ServerCallback]
    public void OnClientConnectServer(object conn)
    {
        Debug.Log("Room OnClientConnectServer");
        connDic.Add(conn as NetworkConnectionToClient, new PlayerInfo { index = connDic.Count, name = connDic.Count.ToString(), ready = false }) ;
        foreach (var playerConn in connDic.Keys)
        {
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = connDic.Values.ToArray() });
        }
    }
    [ClientCallback]
    public void OnClientConnect(object data)
    {
        Debug.Log("Room OnClientConnect");
        NetworkClient.RegisterHandler<ClientMessage>(OnClientMessage);
    }
    [ClientCallback]
    public void OnClientDisconnect(object data)
    {
        Debug.Log("Room OnClientDisconnect");
        hallSubUI.roomUI.gameObject.SetActive(false);
    }

    #region Server
    private void OnServerMessage(NetworkConnectionToClient conn, ServerMessage msg)
    {
        switch (msg.option)
        {
            case ServerMessageOption.None:
                break;
            case ServerMessageOption.Start:
                Debug.Log("Server Message : Start");
                foreach (var player in connDic.Values)
                    if (!player.ready) return;
                foreach (var playerConn in connDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.Started, data = connDic.Values.ToArray() });
                OnStartGame();
                break;
            case ServerMessageOption.Ready:
                Debug.Log($"Server Message : {conn} is Ready");
                PlayerInfo info = connDic[conn];
                info.ready = ! info.ready;
                connDic[conn] = info;
                foreach(var playerConn in connDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = connDic.Values.ToArray() });
                break;
        }
    }
    private void OnStartGame()
    {
        IRoomController roomController = Instantiate(GameEntry.ResourceComponent.GetPrefabResource("OnlineRoomController")).GetComponent<OnlineRoomController>();
        OnlineRoomController onlineRoomController = roomController as OnlineRoomController;
        Player noCamp = Instantiate(NetworkManager.singleton.playerPrefab).GetComponent<Player>();
        noCamp.transform.position = Vector3.zero;
        roomController.noCampPlayer = noCamp;
        foreach(var playerConn in connDic)
        {
            Player player = Instantiate(NetworkManager.singleton.playerPrefab).GetComponent<Player>();
            player.transform.position = Vector3.zero;
            onlineRoomController.connDic.Add(playerConn.Key, player);
            NetworkServer.AddPlayerForConnection(playerConn.Key, player.gameObject);
        }
        NetworkServer.Spawn(onlineRoomController.gameObject);
        SceneManager.sceneLoaded += onlineRoomController.OnSceneLoaded;
        NetworkManager.singleton.ServerChangeScene("GameScene");
    }
    #endregion

    #region Client
    private void OnClientMessage(ClientMessage msg)
    {
        switch (msg.option)
        {
            case ClientMessageOption.None:
                break;
            case ClientMessageOption.UpdateRoom:
                Debug.Log($"Client Message : UpdateRoom");
                hallSubUI.OnUpdateRoom(msg);
                break;
            case ClientMessageOption.Started:
                Debug.Log($"Client Message : Started");
                hallSubUI.OnStartGame(msg);
                break;
        }
    }
    #endregion
}
