using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkMatch))]
public class RoomController : SingletonNetBehaviour<RoomController>
{
    public Guid roomId;
    public Dictionary<PlayerSite, Player> playerDic;
    public Dictionary<Player, bool> ready = new Dictionary<Player, bool>();
    public Player localPlayer;
    public GameObject noCampPlayer;
    private void Awake()
    {
        playerDic = new Dictionary<PlayerSite, Player>();
        DontDestroyOnLoad(gameObject);
    }
    public void OnReady()
    {
        localPlayer.OnGameStart();
    }
    #region Init
    [ClientRpc]
    public void Init(Guid roomId)
    {
        this.roomId = roomId;
        foreach (var player in playerDic)
        {
            if (NetworkClient.localPlayer == player.Value.netIdentity)
            {
                localPlayer = player.Value;
            }
        }
    }
    [ClientRpc]
    public void AddPlayer(PlayerSite site,Player player)
    {
        playerDic.Add(site, player);
        ready.Add(player, false);
        player.site = site;
    }
    #endregion
}
