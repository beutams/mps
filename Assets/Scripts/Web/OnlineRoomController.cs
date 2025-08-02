using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineRoomController : SingletonNetBehaviour<OnlineRoomController>, IRoomController, ID
{
    protected Player localPlayer;
    protected Player noCampPlayer;
    protected ArmoryData armoryData;
    Player IRoomController.localPlayer { get => localPlayer; set => localPlayer = value; }
    Player IRoomController.noCampPlayer { get => noCampPlayer; set => noCampPlayer = value; }
    ArmoryData IRoomController.armoryData { get => armoryData; set => armoryData = value; }
    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;
    public int ID => id;
    public Dictionary<PlayerSite, Player> playerDic { get; set; }
    public Dictionary<NetworkConnectionToClient, Player> connDic = new Dictionary<NetworkConnectionToClient, Player>();

    public virtual void Awake()
    {
        DontDestroyOnLoad(this);
        playerDic = new Dictionary<PlayerSite, Player>();
    }
    [ClientRpc]
    public virtual void OnGameStart()
    {
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
            OnGameStart();
        }
    }
}
