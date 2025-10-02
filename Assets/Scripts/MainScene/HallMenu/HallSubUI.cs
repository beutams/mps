using Michsky.UI.Shift;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallSubUI : SubUIBase
{
    protected float timer = 0;
    protected float maxTimer = 5;
    public RoomData roomData;
    public Transform content;
    public Transform roomUI;
    public MainButton createButton;
    public MainButton startReadyButton;
    public MainButton exitButton;
    protected RoomItem itemPerfab;
    public Dictionary<RoomItem, DiscoveryResponse> rooms = new Dictionary<RoomItem, DiscoveryResponse>();
    protected Dictionary<PlayerInfo, PlayerUIItem> playerDic = new Dictionary<PlayerInfo, PlayerUIItem>();
    public bool isSercer { get; set; }
    protected override void Awake()
    {
        base.Awake();
        itemPerfab = content.GetChild(0).GetComponent<RoomItem>();
        itemPerfab.gameObject.SetActive(false);
        createButton.onClick.AddListener(OnCreateClick);
        GameEntry.EventComponent.Subscribe(GameEvent.CreateRoomEvent, OnCreateRoom);
        GameEntry.EventComponent.Subscribe(GameEvent.ClientReadyConnectEvent, OnJoinRoom);
        GameEntry.EventComponent.Subscribe(GameEvent.ClientDisconnectEvent, OnDisconnect);
        startReadyButton.onClick.AddListener(OnStartReadyClick);
        exitButton.onClick.AddListener(OnExitClick);
    }
    protected override void OnStep()
    {
        RefreshRoomList();
    }
    protected void RefreshRoomList()
    {
        if (GameEntry.WebComponent.gameDiscover.discoveredServers.Count == 0) return;
        foreach(var room in rooms)
        {
            if (!GameEntry.WebComponent.gameDiscover.discoveredServers.ContainsKey(room.Value))
            {
                rooms.Remove(room.Key);
                Destroy(room.Key);
            }
        }
        foreach(var conn in GameEntry.WebComponent.gameDiscover.discoveredServers)
        {
            if (!rooms.ContainsValue(conn.Key))
            {
                var room = Instantiate(itemPerfab);
                room.transform.parent = content;
                room.GetComponent<RoomItem>().SetData(conn.Key);
                room.gameObject.SetActive(true);
                rooms.Add(room, conn.Key);
            }
        }
    }
    protected void InitRoomUI()
    {
        if (roomUI == null) return;
        roomUI.gameObject.SetActive(false);
        Transform players = roomUI.GetChild(1);
        for(int i = 0; i < players.childCount; i++)
        {
            GameEntry.ObjectPoolComponent.Release(players.GetChild(i).gameObject);
        }
    }
    protected void OnDisconnect(object data)
    {
        InitRoomUI();
        GameEntry.WebComponent.gameDiscover.Discovery();
    }
    public void OnCreateRoom(object data)
    {
        Debug.Log($"Server set roomData {(RoomData)data} to hallUI");
        NetworkManager.singleton.StartHost();
        isSercer = true;
        roomData = (RoomData)data;
        roomUI.gameObject.SetActive(true);
        roomUI.GetChild(2).GetComponent<MainButton>().SetText(isSercer ? "Start" : "Ready");
        roomUI.GetChild(3).GetComponent<MainButton>().SetText("Exit");
    }
    public void OnJoinRoom(object data)
    {
        Debug.Log($"Client set roomData {((DiscoveryResponse)data).roomData} to hallUI, RoomUI Active");
        roomData = ((DiscoveryResponse)data).roomData;
        roomUI.gameObject.SetActive(true);
        roomUI.GetChild(2).GetComponent<MainButton>().SetText(isSercer ? "Start" : "Ready");
        roomUI.GetChild(3).GetComponent<MainButton>().SetText("Exit");
    }
    #region Button
    protected void OnCreateClick()
    {
        GameEntry.UIComponent.ShowUI("CreateRoomUI");
    }
    protected void OnStartReadyClick()
    {
        if (isSercer) 
            NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Start });
        else
            NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Ready });
    }
    protected void OnExitClick()
    {
        isSercer = false;
        NetworkClient.Disconnect();
    }
    #endregion

    #region CallBack
    [ClientCallback]
    public void OnUpdateRoom(ClientMessage msg)
    {
        PlayerInfo[] infos = msg.data;
        Debug.Log("UpdateRoom:" + msg.ToString());
        foreach(var obj in playerDic.Values)
        {
            GameEntry.ObjectPoolComponent.Release(obj.gameObject);
        }
        playerDic.Clear();
        foreach(var info in infos)
        {
            if (!playerDic.ContainsKey(info))
            {
                Transform transform = GameEntry.ObjectPoolComponent.Get("PlayerUI").transform;
                transform.SetParent(roomUI.GetChild(1));
                playerDic.Add(info, transform.GetComponent<PlayerUIItem>());
            }
            playerDic[info].Refresh(info);
        }
    }
    #endregion
}
