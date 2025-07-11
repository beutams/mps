using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineRoomController : SingletonNetBehaviour<OnlineRoomController>, IRoomController
{
    public Guid roomId;
    public Player localPlayer;
    public Player noCampPlayer;
    protected ArmoryData armoryData;
    public Dictionary<Player, bool> ready = new Dictionary<Player, bool>();
    public Dictionary<PlayerSite, Player> playerDic { get; set; }
    Player IRoomController.localPlayer { get => localPlayer; }
    Player IRoomController.noCampPlayer { get => noCampPlayer; }
    ArmoryData IRoomController.armoryData { get => armoryData; set => armoryData = value; }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this);
        playerDic = new Dictionary<PlayerSite, Player>();
    }
    public void InitLocalPlayer()
    {

    }
    public void InitOnlineNoCamp()
    {
        noCampPlayer = Instantiate(noCampPlayer);
        playerDic.Add(PlayerSite.NoCamp, noCampPlayer);
    }
    public virtual void OnGameStart()
    {
        string name = SceneManager.GetActiveScene().name;
        foreach (var player in playerDic.Values)
        {
            player.playerItem = Instantiate(player.playerItem, Vector3.zero, Quaternion.identity);
            player.playerItem.name = player.playerItem.name.ToString() + player.site.ToString();
            player.playerItem.SetParent(GameObject.Find("GameObjects").transform);
            player.units = player.playerItem.transform.GetChild(0);
            player.constructions = player.playerItem.transform.GetChild(1);
            GameObjectInit[] objs = FindObjectsByType<GameObjectInit>(FindObjectsSortMode.None);
            foreach (GameObjectInit obj in objs)
            {
                if (obj.site == player.site)
                {
                    foreach (var gobj in obj.objList)
                    {
                        gobj.events.onSpawn?.Invoke(player);
                    }
                }
            }
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            InitLocalPlayer();
            OnGameStart();
        }
    }
    #region Init
/*    [ClientRpc]
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
    public void AddPlayer(PlayerSite site, Player player)
    {
        playerDic.Add(site, player);
        ready.Add(player, false);
        player.site = site;
    }*/
    #endregion
}
