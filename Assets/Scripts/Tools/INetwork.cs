using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface INetwork
{
    public Dictionary<string, UnityAction> funcDic { get; set; }
    public Dictionary<string, UnityAction<object>> func1Dic { get; set; }
    public Dictionary<string, UnityAction<object, object>> func2Dic { get; set; }
    public Dictionary<string, UnityAction<object, object, object>> func3Dic { get; set; }
    public Dictionary<string, Func<object>> funcRDic { get; set; }
    public Dictionary<string, Func<object, object>> funcR1Dic { get; set; }
    public Dictionary<string, Func<object, object, object>> funcR2Dic { get; set; }
    public void Invoke(string e)
    {
        funcDic[e]?.Invoke();
    }
    public void Invoke(string e,object obj1)
    {
        func1Dic[e]?.Invoke(obj1);
    }
    public void Invoke(string e, object obj1, object obj2)
    {
        func2Dic[e]?.Invoke(obj1,obj2);
    }
    public void Invoke(string e, object obj1, object obj2, object obj3)
    {
        func3Dic[e]?.Invoke(obj1,obj2,obj3);
    }
    public object InvokeF(string e)
    {
        return funcRDic[e]?.Invoke();
    }
    public object InvokeF(string e, object obj1)
    {
        return funcR1Dic[e]?.Invoke(obj1);
    }
    public object InvokeF(string e, object obj1, object obj2)
    {
        return funcR2Dic[e]?.Invoke(obj1, obj2);
    }
}