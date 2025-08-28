using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectPoolComponent : BaseComponent<ObjectPoolComponent>
{
    private Dictionary<string, Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();
    public GameObject Get(string key,string name = null)
    {
        try
        {
            GameObject result;
            if (poolDic.ContainsKey(key))
            {
                if (poolDic[key].Count > 0)
                    result = poolDic[key].Dequeue();
                else
                    result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key,name));
            }
            else
            {
                poolDic.Add(key, new Queue<GameObject>());
                result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, name));
            }
            result.name = key;
            result.SetActive(true);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"ObjectPool Instantiate Failed , Key = {key} , Exception : {e}");
            return null;
        }
    }
    public GameObject Get(string key, int name)
    {
        try
        {
            GameObject result;
            if (poolDic.ContainsKey(key))
            {
                if (poolDic[key].Count > 0)
                    result = poolDic[key].Dequeue();
                else
                    result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, name));
            }
            else
            {
                poolDic.Add(key, new Queue<GameObject>());
                result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, name));
            }
            result.name = key;
            result.SetActive(true);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"ObjectPool Instantiate Failed , Key = {key} , Exception : {e}");
            return null;
        }
    }
    public void Release(GameObject obj)
    {
        if (poolDic.ContainsKey(obj.name))
        {
            obj.SetActive(false);
            poolDic[obj.name].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}
