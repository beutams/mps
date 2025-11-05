
using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : NetworkBehaviour
{
    [SyncVar]
    public string poolKey;
    [Command(requiresAuthority = false)]
    public void CommandSetPoolKey(string key)
    {
        RpcSetPoolKey(key);
    }
    [ClientRpc]
    public void RpcSetPoolKey(string key)
    {
        poolKey = key;
        gameObject.name = key;

        // 确保客户端的对象池有这个 key
        if (!GameEntry.ObjectPoolComponent.poolDic.ContainsKey(key))
        {
            GameEntry.ObjectPoolComponent.poolDic.Add(key, new Queue<GameObject>());
        }
    }
    [Command(requiresAuthority = false)]
    public void CommandReturnToPool()
    {
        Debug.Log($"Object Pool Net: {gameObject.name} is return to the pool command");
        RpcReturnToPool();
    }
    [ClientRpc]
    public void RpcReturnToPool()
    {
        if (!string.IsNullOrEmpty(poolKey))
        {
            Debug.Log($"Object Pool Net: {gameObject.name} is return to the pool");
            GameEntry.ObjectPoolComponent.Release(gameObject,false,false);
        }
    }
}