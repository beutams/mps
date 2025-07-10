using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebComponent : BaseComponent<WebComponent> 
{
    public GameDiscovery gameDiscover;
    public GameNetworkManager gameNetworkManager;

    private void Awake()
    {
        gameDiscover = GetComponent<GameDiscovery>();
        gameNetworkManager = GetComponent<GameNetworkManager>();
    }
}
