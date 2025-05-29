using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ObjectPoolComponent : BaseComponent
{
    private Dictionary<string, Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> perfabDic = new Dictionary<string, GameObject>();
    public GameObject Get(string key)
    {
        try
        {
            GameObject result;
            if (poolDic.ContainsKey(key) && perfabDic.ContainsKey(key))
            {
                if (poolDic[key].Count > 0)
                {
                    result = poolDic[key].Dequeue();
                }
                else
                {
                    result = Instantiate(perfabDic[key]);
                }
            }
            else
            {
                poolDic.Add(key, new Queue<GameObject>());
#if UNITY_EDITOR
                string[] allObjs = Directory.GetFiles("Assets/Prefabs", "*.*", SearchOption.AllDirectories);
                foreach (var obj in allObjs)
                {
                    if (!obj.EndsWith($"{key}.prefab")) continue;
                    perfabDic.Add(key, AssetDatabase.LoadAssetAtPath<GameObject>(obj));
                }
#else
        
#endif
                result = Instantiate(perfabDic[key]);
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
