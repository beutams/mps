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
    public void Notify(GameEvent arg, object data)
    {
        if (!eventDic.ContainsKey(arg))
            return;
        eventDic[arg]?.Invoke(data);
    }
}
public enum GameEvent
{
    ChapterSelectEvent,
    ChapterCancelEvent,
    CreateRoomEvent,
}

