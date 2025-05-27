using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventComponent : SingletonNetBehaviour<EventComponent>
{
    public void Subscribe(GameEvent arg, UnityAction<object> action)
    {

    }
    public void Desubscribe(GameEvent arg, UnityAction<object> action)
    {

    }
    public void Notify(GameEvent arg, object data)
    {

    }
}
public enum GameEvent
{
    ChapterSelectEvent,
    ChapterCancelEvent,

}

