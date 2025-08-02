using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WebComponent : BaseComponent<WebComponent> 
{
    public GameDiscovery gameDiscover;
    public GameNetworkManager gameNetworkManager;

    public Dictionary<NetworkConnectionToClient, bool> playerReadyDic = new Dictionary<NetworkConnectionToClient, bool>();

    private void Awake()
    {
        gameDiscover = GetComponent<GameDiscovery>();
        gameNetworkManager = GetComponent<GameNetworkManager>(); 
    }
    [ServerCallback]
    public void StartGame()
    {
        foreach (var player in playerReadyDic.Values)
            if (!player) return;
        IRoomController roomController = Instantiate(GameEntry.ResourceComponent.GetPrefabResource("OnlineRoomController")).GetComponent<IRoomController>();
        Player noCampPlayer = Instantiate(gameNetworkManager.playerPrefab).GetComponent<Player>();

    }
}
