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
            string keyName = name == null ? key : $"{key}_{name}";
            GameObject result;
            if (poolDic.ContainsKey(keyName))
            {
                if (poolDic[keyName].Count > 0)
                    result = poolDic[keyName].Dequeue();
                else
                    result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key,name));
            }
            else
            {
                poolDic.Add(keyName, new Queue<GameObject>());
                result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, name));
            }
            result.name = keyName;
            result.SetActive(true);
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
            GameObject result;
            if (poolDic.ContainsKey(keyName))
            {
                if (poolDic[keyName].Count > 0)
                    result = poolDic[keyName].Dequeue();
                else
                    result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, id));
            }
            else
            {
                poolDic.Add(keyName, new Queue<GameObject>());
                result = Instantiate(GameEntry.ResourceComponent.GetPrefabResource(key, id));
            }
            result.name = keyName;
            result.SetActive(true);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"ObjectPool Instantiate Failed , Key = {key}_{id} , Exception : {e}");
            return null;
        }
    }
    public void Release(GameObject obj)
    {
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
}
