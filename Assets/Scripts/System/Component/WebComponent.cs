using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebComponent : BaseComponent<WebComponent> 
{
    public GameDiscover gameDiscover;
    public GameNetworkManager gameNetworkManager;

    private void Awake()
    {
        gameDiscover = GetComponent<GameDiscover>();
        gameNetworkManager = GetComponent<GameNetworkManager>();
    }
}
