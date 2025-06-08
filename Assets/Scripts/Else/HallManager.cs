/*using Mirror;
using Mirror.Examples.Basic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HallManager : MonoBehaviour
{
    public HallGUI hallGUI;
    public GameObject roomControllerPerfab;
    public bool isServer;
    public bool isClient;
    private HallNetworkManager networkManager;
    private string gameScene = "GameScene";

    private static List<NetworkConnectionToClient> waitingList; //等待加入的客户端

    public static Dictionary<Guid, RoomInfo> openRooms; //记录未开始的room
    public static Dictionary<Guid, HashSet<NetworkConnectionToClient>> roomDic; //记录未开始的room对应的玩家
    public static Dictionary<NetworkConnectionToClient, Guid> roomOwnerDic;  //记录每个房主对应的room
    public static Dictionary<NetworkConnectionToClient, PlayerInfo> playersDic; //所有客户端-其info

    private Guid localPlayerRoom = Guid.Empty;
    private Guid localJoinedRoom = Guid.Empty;
    private Guid selectedRoom = Guid.Empty;
    private int playerIndex;

    public bool isOwner => localPlayerRoom != Guid.Empty;
    #region Connect
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        networkManager = FindAnyObjectByType<HallNetworkManager>();
        hallGUI = FindAnyObjectByType<HallGUI>();
        waitingList = new List<NetworkConnectionToClient>();
        openRooms = new Dictionary<Guid, RoomInfo>();
        roomDic = new Dictionary<Guid, HashSet<NetworkConnectionToClient>>();
        roomOwnerDic = new Dictionary<NetworkConnectionToClient, Guid>();
        playersDic = new Dictionary<NetworkConnectionToClient, PlayerInfo>();
    }
    private void Update()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (isServer && isClient)
                networkManager.StartHost();
            else if(isClient)
                networkManager.StartClient();
            else if(isServer)
                networkManager.StartServer();
        }
    }
    [ServerCallback]
    public void OnServerReady(NetworkConnectionToClient conn) //player加入playersDic
    {
        if (playersDic.ContainsKey(conn)) return;
        waitingList.Add(conn);
        playersDic.Add(conn, new PlayerInfo { playerIndex = this.playerIndex, ready = false });
        playerIndex++;
        SendRoomList(conn);
    }
    [ServerCallback]
    public IEnumerator OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (roomOwnerDic.TryGetValue(conn, out Guid roomId))
        {
            roomOwnerDic.Remove(conn);
            openRooms.Remove(roomId);

            foreach (var playerConn in roomDic[roomId])
            {
                PlayerInfo pInfo = playersDic[playerConn];
                pInfo.ready = false;
                pInfo.roomId = Guid.Empty;
                playersDic[playerConn] = pInfo;
                playerConn.Send(new ClientMessage { option = ClientMessageOption.Departed });
            }
        }
        foreach (var kvp in roomDic)
        {
            kvp.Value.Remove(conn);
        }
        PlayerInfo playerInfo = playersDic[conn];
        if (playerInfo.roomId != Guid.Empty)
        {
            if (openRooms.TryGetValue(playerInfo.roomId, out RoomInfo roomInfo))
            {
                roomInfo.players--;
                openRooms[roomInfo.roomId] = roomInfo;
            }
            if (roomDic.TryGetValue(playerInfo.roomId, out HashSet<NetworkConnectionToClient> connections))
            {
                PlayerInfo[] infos = connections.Select(playerConn => playersDic[playerConn]).ToArray();
                foreach (var playerConn in roomDic[playerInfo.roomId])
                {
                    if (playerConn != conn)
                        playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, playerInfos = infos });
                }
            }
        }
        SendRoomList();
        yield return null;
    }
    [ServerCallback]
    public void OnStartServer()
    {
        InitData();
        NetworkServer.RegisterHandler<ServerMessage>(OnServerMessage);
    }
    [ServerCallback]
    public void OnStopServer()
    {
        InitData();
    }
    [ClientCallback]
    public void OnStartClient()
    {
        InitData();
        NetworkClient.RegisterHandler<ClientMessage>(OnClientMessage);
    }
    [ClientCallback]
    public void OnClientDisconnect()
    {
        InitData();
    }
    [ClientCallback]
    public void OnStopClient()
    {
        InitData();
    }
    #endregion

    #region ServerCallback
    [ServerCallback]
    private void OnServerMessage(NetworkConnectionToClient conn, ServerMessage msg)
    {
        switch (msg.option)
        {
            case ServerMessageOption.None:
                break;
            case ServerMessageOption.Create: //创建房间
                OnServerCreateRoom(conn);
                break;
            case ServerMessageOption.Cancel: //取消房间
                OnServerCancelRoom(conn);
                break;
            case ServerMessageOption.Start:  //开始游戏
                OnServerStartGame(conn);
                break;
            case ServerMessageOption.Join:  //加入房间
                OnServerJoinRoom(conn, msg.roomId);
                break;
            case ServerMessageOption.Leave:  //离开房间
                OnServerLeaveRoom(conn, msg.roomId);
                break;
            case ServerMessageOption.Ready:  //准备
                OnServerReadyGame(conn, msg.roomId);
                break;
            case ServerMessageOption.Match:  //匹配
                OnServerMatchGame(conn);
                break;
        }
    }
    [ServerCallback]
    private void OnServerCreateRoom(NetworkConnectionToClient conn)
    {
        if (roomOwnerDic.ContainsKey(conn)) return;

        Guid newRoomId = Guid.NewGuid();
        roomDic.Add(newRoomId, new HashSet<NetworkConnectionToClient>());
        roomDic[newRoomId].Add(conn);
        roomOwnerDic.Add(conn, newRoomId);
        openRooms.Add(newRoomId, new RoomInfo { roomId = newRoomId, maxPlayers = 4, players = 1 });

        PlayerInfo playerInfo = playersDic[conn];
        playerInfo.ready = false;
        playerInfo.roomId = newRoomId;
        playerInfo.playerSite = 1;
        playersDic[conn] = playerInfo;

        PlayerInfo[] infos = roomDic[newRoomId].Select(playerConn => playersDic[playerConn]).ToArray();
        conn.Send(new ClientMessage { option = ClientMessageOption.Created, roomId = newRoomId, playerInfos = infos });
        SendRoomList();
    }
    [ServerCallback]
    private void OnServerCancelRoom(NetworkConnectionToClient conn)
    {
        if (!roomOwnerDic.ContainsKey(conn)) return;
        Guid roomId;
        if (roomOwnerDic.TryGetValue(conn, out roomId))
        {
            roomOwnerDic.Remove(conn);
            openRooms.Remove(roomId);
            foreach (NetworkConnectionToClient playerConn in roomDic[roomId])
            {
                PlayerInfo playerInfo = playersDic[playerConn];
                playerInfo.ready = false;
                playerInfo.roomId = Guid.Empty;
                playerInfo.playerSite = 0;
                playersDic[playerConn] = playerInfo;
                playerConn.Send(new ClientMessage { option = ClientMessageOption.Departed });
            }
            SendRoomList();
        }
        conn.Send(new ClientMessage { option = ClientMessageOption.Cancelled });
    }
    [ServerCallback]
    private void OnServerStartGame(NetworkConnectionToClient conn)
    {
        if (!roomOwnerDic.ContainsKey(conn)) return;
        if (roomOwnerDic.TryGetValue(conn, out Guid roomId))
        {
            GameObject roomControllerObj = Instantiate(roomControllerPerfab);

            IRoomController roomController = roomControllerObj.GetComponent<IRoomController>();
            roomController.GetComponent<NetworkMatch>().matchId = roomId;

            Player noCamp = Instantiate(roomController.noCampPlayer).GetComponent<Player>();
            roomController.playerDic.Add(PlayerSite.NoCamp, noCamp);
            roomController.ready.Add(noCamp, true);

            Dictionary<NetworkConnectionToClient, Player> players = new Dictionary<NetworkConnectionToClient, Player>();
            Dictionary<PlayerSite, Player> sites = new Dictionary<PlayerSite, Player>();

            foreach (NetworkConnectionToClient playerConn in roomDic[roomId])
            {
                GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
                player.transform.position = Vector3.zero;
                player.GetComponent<NetworkMatch>().matchId = roomId;
                Player p = player.GetComponent<Player>();

                PlayerInfo playerInfo = playersDic[playerConn];
                playerInfo.ready = false;
                playersDic[playerConn] = playerInfo;

                players.Add(playerConn, p);
                sites.Add((PlayerSite)playerInfo.playerSite, p);

                playerConn.Send(new ClientMessage { option = ClientMessageOption.Started });
                waitingList.Remove(playerConn);
            }
            NetworkServer.Spawn(noCamp.gameObject);
            NetworkServer.Spawn(roomControllerObj);
            foreach(var kvp in players)
            {
                NetworkServer.AddPlayerForConnection(kvp.Key, kvp.Value.gameObject);
            }

            foreach(var kvp in sites)
            {
                roomController.AddPlayer(kvp.Key, kvp.Value);
            }
            roomController.Init(roomId);

            roomOwnerDic.Remove(conn);
            openRooms.Remove(roomId);
            roomDic.Remove(roomId);
            SendRoomList();
            NetworkManager.singleton.ServerChangeScene(gameScene);
        }
    }
    [ServerCallback]
    private void OnServerJoinRoom(NetworkConnectionToClient conn, Guid roomId)
    {
        if (!roomDic.ContainsKey(roomId) || !openRooms.ContainsKey(roomId)) return;
        RoomInfo roomInfo = openRooms[roomId];
        if (roomInfo.players >= roomInfo.maxPlayers) return;
        roomInfo.players++;
        openRooms[roomId] = roomInfo;
        roomDic[roomId].Add(conn);

        PlayerInfo playerInfo = playersDic[conn];
        playerInfo.ready = false;
        playerInfo.roomId = roomId;
        playerInfo.playerSite = ChooseSite(roomId);
        playersDic[conn] = playerInfo;

        PlayerInfo[] infos = roomDic[roomId].Select(playerConn => playersDic[playerConn]).ToArray();
        SendRoomList();
        conn.Send(new ClientMessage { option = ClientMessageOption.Joined, roomId = roomId, playerInfos = infos });

        foreach (NetworkConnectionToClient playerConn in roomDic[roomId])
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, playerInfos = infos });
    }
    [ServerCallback]
    private void OnServerLeaveRoom(NetworkConnectionToClient conn, Guid roomId)
    {
        RoomInfo roomInfo = openRooms[roomId];
        roomInfo.players--;
        openRooms[roomId] = roomInfo;

        PlayerInfo playerInfo = playersDic[conn];
        playerInfo.ready = false;
        playerInfo.roomId = Guid.Empty;
        playerInfo.playerSite = 0;
        playersDic[conn] = playerInfo;

        roomDic[roomId].Remove(conn);

        HashSet<NetworkConnectionToClient> connections = roomDic[roomId];
        PlayerInfo[] infos = connections.Select(playerConn => playersDic[playerConn]).ToArray();

        foreach (NetworkConnectionToClient playerConn in roomDic[roomId])
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, playerInfos = infos });

        SendRoomList();
        conn.Send(new ClientMessage { option = ClientMessageOption.Departed });
    }
    [ServerCallback]
    private void OnServerReadyGame(NetworkConnectionToClient conn, Guid roomId)
    {
        PlayerInfo playerInfo = playersDic[conn];
        playerInfo.ready = !playerInfo.ready;
        playersDic[conn] = playerInfo;

        HashSet<NetworkConnectionToClient> connections = roomDic[roomId];
        PlayerInfo[] infos = connections.Select(playerConn => playersDic[playerConn]).ToArray();

        foreach (NetworkConnectionToClient playerConn in roomDic[roomId])
            playerConn.Send(new ClientMessage { option = ClientMessageOption.UpdateRoom, playerInfos = infos });
    }
    [ServerCallback]
    private void OnServerMatchGame(NetworkConnectionToClient conn)
    {

    }
    #endregion

    #region ClientCallback
    private void OnClientMessage(ClientMessage msg)
    {
        switch (msg.option)
        {
            case ClientMessageOption.None:
                break;
            case ClientMessageOption.List:
                openRooms.Clear();
                foreach (var roomInfo in msg.rooms)
                    openRooms.Add(roomInfo.roomId, roomInfo);
                hallGUI?.RefreshRoomList();
                break;
            case ClientMessageOption.Created:
                localPlayerRoom = msg.roomId;
                hallGUI.OnJoinRoom(msg.playerInfos);
                break;
            case ClientMessageOption.Cancelled:
                localPlayerRoom = Guid.Empty;
                hallGUI.OnLeaveRoom();
                break;
            case ClientMessageOption.Joined:
                localJoinedRoom = msg.roomId;
                hallGUI.OnJoinRoom(msg.playerInfos);
                break;
            case ClientMessageOption.Departed:
                localJoinedRoom = Guid.Empty;
                hallGUI.OnLeaveRoom();
                break;
            case ClientMessageOption.UpdateRoom:
                hallGUI.RefreshRoom(msg.playerInfos);
                break;
            case ClientMessageOption.Started:
                hallGUI.OnStartGame();
                break;
        }
    }
    #endregion

    #region Logic
    private byte ChooseSite(Guid roomId)
    {
        for (byte i = 1; i <= 4; i++)
        {
            bool exist = false;
            foreach (var item in roomDic[roomId])
            {
                if (playersDic[item].playerSite == i)
                {
                    exist = true;
                    continue;
                }
            }
            if (!exist)
                return i;
        }
        return 0;
    }
    [ServerCallback]
    private void SendRoomList(NetworkConnectionToClient conn = null)
    {
        if (conn != null)
            conn.Send(new ClientMessage { option = ClientMessageOption.List, rooms = openRooms.Values.ToArray() });
        else
            foreach (NetworkConnectionToClient waiter in waitingList)
                waiter.Send(new ClientMessage { option = ClientMessageOption.List, rooms = openRooms.Values.ToArray() });
    }
    private void InitData()
    {
        openRooms.Clear();
        roomDic.Clear();
        roomOwnerDic.Clear();
        waitingList.Clear();
        playersDic.Clear();
        localPlayerRoom = Guid.Empty;
        localJoinedRoom = Guid.Empty;
        selectedRoom = Guid.Empty;
    }
    [ClientCallback]
    public void SelectRoom(Guid roomId)
    {
        if (roomId == Guid.Empty)
            selectedRoom = Guid.Empty;
        else
        {
            if (!openRooms.Keys.Contains(roomId)) return;
            selectedRoom = roomId;
        }
    }
    [ClientCallback]
    public void RequestCreateRoom()
    {
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Create });
    }
    [ClientCallback]
    public void RequestCancelRoom()
    {
        if (localPlayerRoom == Guid.Empty) return;
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Cancel });
    }
    [ClientCallback]
    public void RequestJoinRoom()
    {
        if (selectedRoom == Guid.Empty) return;
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Join, roomId = selectedRoom });
    }
    [ClientCallback]
    public void RequestLeaveRoom()
    {
        if (localJoinedRoom == Guid.Empty) return;
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Leave, roomId = localJoinedRoom });
    }
    [ClientCallback]
    public void RequestReadyChange()
    {
        if (localPlayerRoom == Guid.Empty && localJoinedRoom == Guid.Empty) return;
        Guid roomId = localPlayerRoom == Guid.Empty ? localJoinedRoom : localPlayerRoom;
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Ready, roomId = roomId });
    }
    [ClientCallback]
    public void RequestStartRoom()
    {
        if (localPlayerRoom == Guid.Empty) return;
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Start });
    }
    
    #endregion
}
*/