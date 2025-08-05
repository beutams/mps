using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomNetworkMangaer : MonoBehaviour
{
    private HallSubUI hallSubUI;
    private Dictionary<NetworkConnectionToClient, bool> readyDic = new Dictionary<NetworkConnectionToClient, bool>();
    
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
        readyDic.Remove(conn as NetworkConnectionToClient);
        foreach(var playerConn in readyDic.Keys)
        {
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = readyDic });
        }
    }
    [ServerCallback]
    public void OnClientConnectServer(object conn)
    {
        Debug.Log("Room OnClientConnectServer");
        readyDic.Add(conn as NetworkConnectionToClient,false);
        foreach (var playerConn in readyDic.Keys)
        {
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = readyDic });
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
                foreach (var ready in readyDic.Values)
                    if (!ready) return;
                foreach (var playerConn in readyDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.Started, data = readyDic });
                OnStartGame();
                break;
            case ServerMessageOption.Ready:
                Debug.Log($"Server Message : {conn} is Ready");
                readyDic[conn] = !readyDic[conn];
                foreach(var playerConn in readyDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = readyDic });
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
        foreach(var playerConn in readyDic)
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
