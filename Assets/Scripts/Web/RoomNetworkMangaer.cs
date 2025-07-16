using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomNetworkMangaer : MonoBehaviour
{
    private HallSubUI hallSubUI;
    private GameNetworkManager gameNetworkManager;
    private Dictionary<NetworkConnectionToClient, bool> readyDic = new Dictionary<NetworkConnectionToClient, bool>();
    
    private void Awake()
    {
        hallSubUI = FindAnyObjectByType<HallSubUI>();
    }
    [ServerCallback]
    public void OnStartServer()
    {
        NetworkServer.RegisterHandler<ServerMessage>(OnServerMessage);
        hallSubUI.isSercer = true;
    }
    [ServerCallback]
    public void OnClientDisconnectServer(NetworkConnectionToClient conn)
    {
        readyDic.Remove(conn);
        foreach(var playerConn in readyDic.Keys)
        {
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = readyDic });
        }
    }
    [ServerCallback]
    public void OnClientConnectServer(NetworkConnectionToClient conn)
    {
        readyDic.Add(conn,false);
        foreach (var playerConn in readyDic.Keys)
        {
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = readyDic });
        }
    }
    [ClientCallback]
    public void OnStartClient()
    {
        NetworkClient.RegisterHandler<ClientMessage>(OnClientMessage);
    }
    [ClientCallback]
    public void OnClientDisconnect()
    {
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
                foreach (var ready in readyDic.Values)
                    if (!ready) return;
                foreach (var playerConn in readyDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.Started, data = readyDic });
                OnStartGame();
                break;
            case ServerMessageOption.Ready:
                readyDic[conn] = !readyDic[conn];
                foreach(var playerConn in readyDic.Keys)
                    playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, data = readyDic });
                break;
        }
    }
    private void OnStartGame()
    {
        IRoomController roomController = Instantiate(GameEntry.ResourceComponent.prefabDic["OnlineRoomController"][0]).GetComponent<OnlineRoomController>();
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
                hallSubUI.OnUpdateRoom(msg);
                break;
            case ClientMessageOption.Started:
                hallSubUI.OnStartGame(msg);
                break;
        }
    }
    #endregion
}
