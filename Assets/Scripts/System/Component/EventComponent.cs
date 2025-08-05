using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventComponent : BaseComponent<EventComponent>
{
    protected Dictionary<GameEvent, UnityAction<object>> eventDic = new Dictionary<GameEvent, UnityAction<object>>();
    public void Subscribe(GameEvent arg, UnityAction<object> action)
    {
        if (!eventDic.ContainsKey(arg))
            eventDic.Add(arg, null);
        eventDic[arg] += action;
    }
    public void Desubscribe(GameEvent arg, UnityAction<object> action)
    {
        if (!eventDic.ContainsKey(arg))
            return;
        eventDic[arg] -= action;
    }
    public void Notify(GameEvent arg, object data = null)
    {
        if (!eventDic.ContainsKey(arg))
            return;
        Debug.Log($"EventComponent : Notify {arg} Event, Invoke {eventDic[arg].GetInvocationList().Length} Method");
        eventDic[arg]?.Invoke(data);
    }
}
public enum GameEvent
{
    ChapterSelectEvent,
    ChapterCancelEvent,
    CreateRoomEvent,
    ArmoryItemClick,
    //Server
    ServerStartEvent, //启动服务器
    ServerConnectEvent,//客户端连入服务器
    ServerDisconnectEvent, //客户端断开连接
    //Client
    ClientStartEvent, //启动客户端
    ClientReadyConnectEvent, //客户端将要连接服务器
    ClientConnectEvent, //客户端连入服务器
    ClientDisconnectEvent, //客户端断开连接

}

