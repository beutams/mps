using System;
using UnityEngine;

[Serializable]
public class AGameObjectStatu<T> where T : GameObjectStatus
{
    public virtual void OnEnter(T obj) { }
    public virtual void OnExit(T obj) { }
    public virtual void OnStep(T obj) { }
}