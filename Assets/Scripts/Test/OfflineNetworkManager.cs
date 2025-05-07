using Mirror;
using Mirror.Discovery;
using System.Collections.Generic;
using UnityEngine;

public class OfflineNetworkManager : NetworkManager
{
    public Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;
    public OfflineGUI offlineGUI;

    public float refreshTime = 1f;
    private float timer;
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Start();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }
    }
}
