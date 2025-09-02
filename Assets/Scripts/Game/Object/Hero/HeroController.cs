using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : UnitController
{
    public Dictionary<int, List<WeapenModel>> weapenGroup = new Dictionary<int, List<WeapenModel>>();
    public Dictionary<int, WeapenModel> weapenDic = new Dictionary<int, WeapenModel>();
    protected int currentGroup = 1;

    protected override void Start()
    {
        base.Start();
        InitWeapens();
    }
    protected virtual void InitWeapens()
    {
        weapenGroup.Clear();
        for(int i = 0; i < 9; i++)
            weapenGroup.Add(i+1, new List<WeapenModel>());
        Transform weapens = transform.Find("Weapens");
        for(int i = 0;i < weapens.childCount; i++)
        {
            Transform weapen = weapens.GetChild(i);
            weapenDic.Add(int.Parse(weapen.gameObject.name), weapen.GetComponent<WeapenModel>());
        }
    }
    public int GetCurrentGroup()
    {
        return currentGroup;
    }
    public void Equip(int index, WeapenBase weapen)
    {
        if (weapenDic[index].weapen != null) return;
        weapenDic[index].weapen = weapen;
        weapenDic[index].group = 1;
        weapenGroup[1].Add(weapenDic[index]);
        GameObject obj = GameEntry.ObjectPoolComponent.Get(weapen.name);
        obj.transform.SetParent(weapenDic[index].transform);
        obj.transform.localPosition = Vector3.zero;
    }
    public void UnEquip(int index)
    {
        if (weapenDic[index] == null) return;
        weapenDic[index].weapen = null;
        GameEntry.ObjectPoolComponent.Release(weapenDic[index].transform.GetChild(0).gameObject);
        if(weapenGroup[weapenDic[index].group].Contains(weapenDic[index]))
            weapenGroup[weapenDic[index].group].Remove(weapenDic[index]);
    }
    public void ChangeGroup(int index,WeapenModel model)
    {
        if (weapenGroup[model.group].Contains(model))
        {
            weapenGroup[model.group].Remove(model);
            model.group = index;
            weapenGroup[index].Add(model);
        }
    }
}