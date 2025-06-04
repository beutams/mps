using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectStatus : MonoBehaviour
{
    #region 设定
    public List<string> statusList;
    #endregion

    #region 字段
    protected Dictionary<Type, AGameObjectStatu<GameObjectStatus>> statuPool;
    protected AGameObjectStatu<GameObjectStatus> currentStatu;
    #endregion

    #region 属性
    public GameObjectController controller { get; protected set; }
    public UnityEvent onEnter{ get; protected set; }
    public UnityEvent onExit {  get; protected set; }
    #endregion
    private void Awake()
    {
        controller = GetComponent<GameObjectController>();
        statuPool = new Dictionary<Type, AGameObjectStatu<GameObjectStatus>>();
        foreach (var item in statusList)
        {
            statuPool.Add(Type.GetType(item), Type.GetType(item).Instantiate() as AGameObjectStatu<GameObjectStatus>);
        }
        currentStatu = statuPool.First().Value;
        currentStatu.OnEnter(this);
        onEnter?.Invoke();
    }
    protected void Update()
    {
        currentStatu.OnStep(this);
    }
    public bool Change<T>() where T : AGameObjectStatu<GameObjectStatus> 
    {
        if (!statuPool.ContainsKey(typeof(T))) return false;
        currentStatu.OnExit(this);
        onExit?.Invoke();

        currentStatu = statuPool[typeof(T)];

        currentStatu.OnEnter(this);
        onEnter?.Invoke();

        Debug.Log($"{controller.gameObject.name} Change To {currentStatu}");
        return true;
    }
    public AGameObjectStatu<GameObjectStatus> GetStatu()
    {
        return currentStatu;
    }
}