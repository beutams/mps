using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : UnitController
{
    public List<Solt> solts;
    public Dictionary<int, List<WeapenBase>> weapenGroup = new Dictionary<int, List<WeapenBase>>();
    protected int currentGroup = 1;

    protected virtual void Start()
    {
        RefreshWeapens();
    }
    protected virtual void RefreshWeapens()
    {
        weapenGroup.Clear();
        foreach(var item in solts)
        {
            if(item.weapen != null)
            {
                if (!weapenGroup.ContainsKey(1))
                {
                    weapenGroup.Add(1, new List<WeapenBase>());
                }
                weapenGroup[1].Add(item.weapen);
                item.weapen.Init(player);
            }
        }
    }
    public int GetCurrentGroup()
    {
        return currentGroup;
    }
}
[Serializable]
public class Solt
{
    public Vector3 offset;
    public WeapenBase weapen;
}