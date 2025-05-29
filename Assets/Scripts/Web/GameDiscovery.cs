using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameDiscover : NetworkDiscovery
{
    public Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    public override void Start()
    {
        base.Start();
        Init();
    }
    protected virtual void Init()
    {
        if (!Enumerable.Range(0, OnServerFound.GetPersistentEventCount()).Any(i => OnServerFound.GetPersistentMethodName(i) == nameof(OnDiscoveredServer1)))
            OnServerFound.AddListener(OnDiscoveredServer1);
    }
    public virtual void Discovery()
    {
        discoveredServers.Clear();
        StartDiscovery();
    }
    public void OnDiscoveredServer1(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
        Debug.Log(discoveredServers.Count);
    }
}
