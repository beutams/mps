using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroup : MonoBehaviour 
{
    [SerializeField] protected string group;
    public static Dictionary<string, List<UIGroup>> globalDic = new Dictionary<string, List<UIGroup>>();
    protected virtual void Awake()
    {
        if (!globalDic.ContainsKey(group))
        {
            globalDic.Add(group, new List<UIGroup>());
            globalDic[group].Add(this);
        }
        globalDic[group].Add(this);
    }
    public string GetGroup()
    {
        return group;
    }
}