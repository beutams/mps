using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineRoomController : SingletonNetBehaviour<OnlineRoomController>, IRoomController, ID
{
    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;
    public int ID => id;
    protected Player localPlayer;
    protected Player noCampPlayer;
    protected ArmoryData armoryData;
    Player IRoomController.localPlayer { get => localPlayer; set => localPlayer = value; }
    Player IRoomController.noCampPlayer { get => noCampPlayer; set => noCampPlayer = value; }
    ArmoryData IRoomController.armoryData { get => armoryData; set => armoryData = value; }
    public Dictionary<PlayerSite, Player> playerDic { get; set; }
    [SyncVar]
    public Dictionary<NetworkConnectionToClient, Player> connDic = new Dictionary<NetworkConnectionToClient, Player>();

    public virtual void Awake()
    {
        DontDestroyOnLoad(this);
        playerDic = new Dictionary<PlayerSite, Player>();
    }
    [ClientCallback]
    public virtual void OnGameStart()
    {
        Debug.Log($"Client OnGameStart,PlayerNumber : {connDic.Count()}");
        List<Player> playerlist = new List<Player>();
        connDic.Values.ToList().CopyTo(playerlist);
        playerlist.Add(noCampPlayer);
        foreach (var player in playerlist)
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
            foreach (var obj in objs)
            {
                Destroy(obj.gameObject);
            }
        }
    }
    public void InitLoadPlayer()
    {
        foreach(var conn in connDic)
        {
            if(conn.Value == NetworkClient.localPlayer.GetComponent<Player>())
            {
                localPlayer = conn.Value;
            }
        }
    }
    [ClientCallback]
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Client OnSceneLoaded");
        if (scene.name == "GameScene")
        {
            OnGameStart();
            InitLoadPlayer();
        }
    }
}
