using Mirror;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HeroController : UnitController
{
    public Dictionary<int, List<WeapenModel>> weapenGroup = new Dictionary<int, List<WeapenModel>>();
    public Dictionary<int, WeapenModel> weapenDic = new Dictionary<int, WeapenModel>();
    public Dictionary<int, bool> autoFireDic = new Dictionary<int, bool>();
    protected int currentGroup = 1;

    protected void Start()
    {
        InitWeapens();
    }
    protected virtual void InitWeapens()
    {
        weapenGroup.Clear();
        for(int i = 0; i < GameEntry.SettingComponent.settingData.groupNumber; i++)
        {
            weapenGroup.Add(i+1, new List<WeapenModel>());
            autoFireDic.Add(i + 1, false);
        }
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
        Debug.Log($"HeroControler : Equip {weapen.name}");
        if (weapenDic[index].weapen != null) return;
        weapenDic[index].weapen = weapen;
        weapenDic[index].group = 1;
        weapenGroup[1].Add(weapenDic[index]);
        GameObject obj = GameEntry.ObjectPoolComponent.Get(weapen.name);
        SpawnModelServer(index, obj.transform);
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
    public void ChangeCurGroup(int index)
    {
        if(weapenGroup[index].Count > 0)
            currentGroup = index;
        GameObject.FindAnyObjectByType<GameUI>().RefreshWeapen();
    }
    public void ChangeAutoStatu(int index)
    {
        autoFireDic[index] = !autoFireDic[index];
        GameObject.FindAnyObjectByType<GameUI>().RefreshWeapen();
    }
    public bool WeapenCanAutoFire(int index)
    {
        return autoFireDic[index] && currentGroup != index;
    }
    protected override void Update()
    {
        base.Update();
        UpdateORCA();
        foreach (var model in weapenGroup[currentGroup])
        {
            model.TurnTowardsMouse();
        }
    }
    protected void UpdateORCA()
    {
        orcaAgent.Step(position, velocity, Vector3.zero, !isMove && unitStats.canAutoMove);
    }
    public void ReceiveMove(Vector2 dir)
    {
        velocity = Tools.V2ToV3(dir) * unitStats.speed;
        transform.position += velocity * Time.deltaTime;
    }
    public void ReceiveTurn(float dir)
    {
        transform.Rotate(Vector3.up * unitStats.rotateForce * dir * Time.deltaTime);
    }
    #region Network
    [Command(requiresAuthority = false)]
    public void SpawnModelServer(int index,Transform obj)
    {
        Debug.Log($"HeroControler Server: Equip {obj}");
        SpawnModelClent(index, obj.transform);
    }
    [ClientRpc]
    public void SpawnModelClent(int index, Transform obj)
    {
        Debug.Log($"HeroControler Client: Equip {obj}");
        weapenDic[index].firePoint = obj.Find("FirePoint");
        obj.SetParent(weapenDic[index].transform);
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.Euler(0, 0, 0);
    }
    #endregion
}