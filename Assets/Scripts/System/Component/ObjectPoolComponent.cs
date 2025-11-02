using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectPoolComponent : BaseComponent<ObjectPoolComponent>
{
    public Dictionary<string, Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();
    protected ObjectPoolNetworkTools networkTools;

    private void Awake()
    {
        networkTools = GetComponent<ObjectPoolNetworkTools>();
    }
    public GameObject Get(string key,string name = null)
    {
        try
        {
            string keyName = name == null ? key : $"{key}_{name}";
            bool isNew = false;
            GameObject result;
            if (poolDic.ContainsKey(keyName))
            {
                if (poolDic[keyName].Count > 0)
                    result = poolDic[keyName].Dequeue();
                else
                {
                    result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, name));
                    isNew = true;
                }
            }
            else
            {
                poolDic.Add(keyName, new Queue<GameObject>());
                result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, name));
                isNew = true;
            }

            result.name = keyName;
            result.SetActive(true);

            if (result.TryGetComponent<NetworkObject>(out var pooled))
            {
                if (isNew)
                    NetworkServer.Spawn(result);
                pooled.CommandSetPoolKey(keyName);
            }
            //networkTools.SetStat(result.transform,keyName);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"ObjectPool Instantiate Failed , Key = {key}_{name} , Exception : {e}");
            return null;
        }
    }
    public GameObject Get(string key, int id)
    {
        try
        {
            string keyName = $"{key}_{id}";
            bool isNew = false;
            GameObject result;
            if (poolDic.ContainsKey(keyName))
            {
                if (poolDic[keyName].Count > 0)
                    result = poolDic[keyName].Dequeue();
                else
                {
                    result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, id));
                    isNew = true;
                }
            }
            else
            {
                poolDic.Add(keyName, new Queue<GameObject>());
                result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, id));
                isNew = true;
            }
            result.name = keyName;
            result.SetActive(true);
            if (result.TryGetComponent<NetworkObject>(out var pooled))
            {
                if (isNew)
                    NetworkServer.Spawn(result);
                pooled.CommandSetPoolKey(keyName);
            }
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"ObjectPool Instantiate Failed , Key = {key}_{id} , Exception : {e}");
            return null;
        }
    }
    public void Release(GameObject obj, bool notDead = false, bool netCheck = true)
    {
        if (obj == null) return;
        if (obj.name.Contains('('))
            obj.name = obj.name.Split('(')[0];
        if (netCheck && obj.TryGetComponent<NetworkObject>(out var pooled))
        {
            pooled.CommandReturnToPool();
            return;
        }
        var gameObjectController = obj.GetComponent<GameObjectController>();
        if (gameObjectController != null && !notDead)
        {
            gameObjectController.events?.onDead?.Invoke();
        }
        if (poolDic.ContainsKey(obj.name))
        {
            obj.SetActive(false);
            if(!obj.TryGetComponent<RectTransform>(out _))
                obj.transform.SetParent(transform);
            poolDic[obj.name].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
    public void Clear()
    {
        foreach(var item in poolDic.Values)
        {
            for(int i = 0; i < item.Count; i++)
            {
                var obj = item.Dequeue();
                if(obj != null)
                    item.Enqueue(obj);
            }
        }
    }
}