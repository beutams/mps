using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface INetwork
{
    public Dictionary<string, UnityAction> funcDic { get; set; }
    public Dictionary<string, UnityAction<object>> func1Dic { get; set; }
    public Dictionary<string, UnityAction<object, object>> func2Dic { get; set; }
    public Dictionary<string, UnityAction<object, object, object>> func3Dic { get; set; }
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
}