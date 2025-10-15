using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    public static Dictionary<string,GameObject> regDic = new Dictionary<string, GameObject>();

    void Awake()
    {
        RegisterPrefabSafe(gameObject);
    }

    void RegisterPrefabSafe(GameObject prefab)
    {
        NetworkIdentity netId = prefab.GetComponent<NetworkIdentity>();
        if (netId == null)
        {
            Debug.LogError($"{prefab.name} 没有 NetworkIdentity 组件");
            return;
        }
        GameObject obj = GameEntry.ResourceComponent.GetPrefabResource(prefab.name);
        if(regDic.ContainsKey(prefab.name))
            return;

        if (obj != null)
        {
            NetworkClient.RegisterPrefab(obj);
            regDic[prefab.name] = prefab;
            Debug.Log($"成功注册 Prefab: {prefab.name}");
        }

    }
}
