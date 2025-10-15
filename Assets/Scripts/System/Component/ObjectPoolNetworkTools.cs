using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolNetworkTools : SingletonNetBehaviour<ObjectPoolNetworkTools>
{
    protected ObjectPoolComponent objectPool;
    private void Awake()
    {
        objectPool = GetComponent<ObjectPoolComponent>();
    }
    public void SetStat(Transform t, string keyName)
    {
        SetStatServer(t, keyName);
    }
    [Command(requiresAuthority = false)]
    public void SetStatServer(Transform t, string keyName)
    {
        SetStatClient(t, keyName);
    }
    [ClientRpc]
    public void SetStatClient(Transform t, string keyName)
    {
        t.gameObject.name = keyName;
        t.gameObject.SetActive(true);
    }
    public void ReleaseStat(Transform t)
    {
        ReleaseStatServer(t);
    }
    [Command(requiresAuthority = false)]
    public void ReleaseStatServer(Transform t)
    {
        ReleaseStatClient(t);
    }
    [ClientRpc]
    public void ReleaseStatClient(Transform t)
    {
        t.gameObject.SetActive(false);
        if (!t.TryGetComponent<RectTransform>(out _))
            t.SetParent(transform);
        objectPool.poolDic[t.gameObject.name].Enqueue(t.gameObject);
    }
}
