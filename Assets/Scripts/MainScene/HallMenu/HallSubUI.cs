using Michsky.UI.Shift;
using Mirror;
using Mirror.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallSubUI : SubUIBase
{
    protected float timer = 0;
    protected float maxTimer = 2;
    public RoomData roomData;
    public Transform content;
    public Transform roomUI;
    public Button createButton;
    protected RoomItem itemPerfab;
    protected GameObject playerUIPrefab;
    protected Dictionary<RoomItem, DiscoveryResponse> rooms = new Dictionary<RoomItem, DiscoveryResponse>();
    public bool isSercer { get; set; }

    protected Dictionary<NetworkConnectionToClient, GameObject> playerDic = new Dictionary<NetworkConnectionToClient, GameObject>();
    protected override void Awake()
    {
        base.Awake();
        itemPerfab = content.GetChild(0).GetComponent<RoomItem>();
        itemPerfab.gameObject.SetActive(false);
        createButton.onClick.AddListener(OnCreateClick);
        GameEntry.EventComponent.Subscribe(GameEvent.CreateRoomEvent,OnCreateRoom);
    }
    protected override void OnStep()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else
            Find();
    }
    protected void Find()
    {
        //GameEntry.WebComponent.gameDiscover.Discovery();
        //RefreshRoomList();
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
        foreach(var conn in GameEntry.WebComponent.gameDiscover.discoveredServers.Keys)
        {
            if (!rooms.ContainsValue(conn))
            {
                var room = Instantiate(itemPerfab);
                room.transform.parent = content;
                room.GetComponent<RoomItem>().SetData(conn);
                rooms.Add(room, conn);
            }
        }
    }
    protected void InitRoomUI()
    {
        roomUI.gameObject.SetActive(false);
        roomUI.GetChild(2).GetComponent<MainButton>().buttonText = isSercer ? "开始" : "准备";
        roomUI.GetChild(3).GetComponent<MainButton>().buttonText = "退出";
        Transform players = roomUI.GetChild(1);
        for(int i = 0; i < players.childCount; i++)
        {
            Destroy(players.GetChild(i));
        }
    }
    public void OnCreateRoom(object data)
    {
        RoomData roomData = (RoomData)data;
        roomUI.gameObject.SetActive(true);
    }
    #region Button
    protected void OnCreateClick()
    {
        GameEntry.UIComponent.ShowUI("CreateRoomUI");
    }

    
    protected void OnReadyClick()
    {
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Ready });
    }
    protected void OnStartClick()
    {
        if (!isSercer) return;
        NetworkClient.Send(new ServerMessage { option = ServerMessageOption.Start });
    }
    #endregion

    #region CallBack
    [ClientCallback]
    public void OnUpdateRoom(ClientMessage msg)
    {
        Dictionary<NetworkConnectionToClient, bool> readyDic = msg.data as Dictionary<NetworkConnectionToClient, bool>;
        foreach(var kvp in readyDic)
        {
            if (playerDic.ContainsKey(kvp.Key))
            {

            }
            else
            {
                Transform transform = Instantiate(playerUIPrefab).transform;
                transform.SetParent(roomUI.GetChild(1));
                playerDic.Add(kvp.Key, transform.gameObject);
            }
        }
    }
    [ClientCallback]
    public void OnStartGame(ClientMessage msg)
    {
        InitRoomUI();
    }
    #endregion
}
