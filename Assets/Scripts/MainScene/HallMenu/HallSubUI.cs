using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallSubUI : SubUIBase
{
    protected float timer = 0;
    protected float maxTimer = 2;
    public Transform content;
    public Transform roomUI;
    protected RoomItem itemPerfab;
    protected Dictionary<ServerResponse, RoomItem> rooms;
    protected override void Awake()
    {
        base.Awake();
        itemPerfab = content.GetChild(0).GetComponent<RoomItem>();
        itemPerfab.gameObject.SetActive(false);
    }
    protected override void OnClose()
    {
        
    }

    protected override void OnOpen()
    {
        Find();
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
/*        GameEntry.WebComponent.gameDiscover.Discovery();
        RefreshRoomList();*/
    }
    protected void RefreshRoomList()
    {
        if (GameEntry.WebComponent.gameDiscover.discoveredServers.Count == 0) return;
        foreach(var room in rooms)
        {
            if (!GameEntry.WebComponent.gameDiscover.discoveredServers.ContainsValue(room.Key))
            {
                rooms.Remove(room.Key);
                Destroy(room.Value);
            }
        }
        foreach(var conn in GameEntry.WebComponent.gameDiscover.discoveredServers.Values)
        {
            if (!rooms.ContainsKey(conn))
            {
                var room = Instantiate(itemPerfab);
                room.transform.parent = content;
                rooms.Add(conn, room);
            }
        }
    }
    protected void OnCreateRoomClick()
    {
        NetworkManager.singleton.StartHost();
        roomUI.gameObject.SetActive(true);
    }
}
