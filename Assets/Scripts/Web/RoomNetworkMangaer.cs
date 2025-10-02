using Mirror;
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
        gameObject.SetActive(false);
    }
    [ServerCallback]
    public void OnStartServer(object data)
    {
        Debug.Log("Room OnStartSercer");
        NetworkServer.RegisterHandler<ServerMessage>(OnServerMessage);
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
    }
    [ClientCallback]
    public void OnClientConnect(object data)
    {
        Debug.Log("Room OnClientConnect");
        NetworkClient.RegisterHandler<ClientMessage>(OnClientMessage);
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Register, data = GameEntry.UserComponent.Get("ArmoryData") as ArmoryData });
        Debug.Log($"Register armoryData {GameEntry.UserComponent.Get("ArmoryData") as ArmoryData}");
    }
    [ClientCallback]
    public void OnClientDisconnect(object data)
    {
        Debug.Log("Room OnClientDisconnect");
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
/*                foreach (var player in connDic.Values)
                    if (!player.ready) return;*/
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
            case ServerMessageOption.Register:
                Debug.Log("Server Message : Register");
                connDic[conn] = new PlayerInfo { data = msg.data, index = connDic.Count, name = connDic.Count.ToString(), ready = false };
                Debug.Log($"Server Register PlayerInfo:{connDic[conn]}");
                foreach (var playerConn in connDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = connDic.Values.ToArray() });
                break;
        }
    }
    private void OnStartGame()
    {
        OnlineRoomController roomController = Instantiate(GameEntry.ResourceComponent.GetPrefabResource("OnlineRoomController")).GetComponent<OnlineRoomController>();
        Player noCamp = Instantiate(NetworkManager.singleton.playerPrefab).GetComponent<Player>();
        NetworkServer.Spawn(roomController.gameObject);
        NetworkServer.Spawn(noCamp.gameObject);
        roomController.AddNoCampPlayer(noCamp.gameObject);
        foreach (var playerConn in connDic)
        {
            Player player = Instantiate(NetworkManager.singleton.playerPrefab).GetComponent<Player>();
            NetworkServer.AddPlayerForConnection(playerConn.Key, player.gameObject);
            roomController.AddPlayer(playerConn.Value, player.gameObject);
        }
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
                SceneManager.sceneLoaded += (scene,mode) => 
                { 
                    FindAnyObjectByType<OnlineRoomController>().OnSceneLoaded(scene, mode); 
                };
                Debug.Log($"Client Message : Started");
                break;
        }
    }
    #endregion
}
