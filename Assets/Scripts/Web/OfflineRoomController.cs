using Mirror;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class OfflineRoomController : SingletonMonoBehaviour<OfflineRoomController>, IRoomController
{
    public Player localPlayer;
    public Player noCampPlayer;
    public GameObject heroPrefab;
    protected ArmoryData armoryData;
    public Dictionary<PlayerSite, Player> playerDic { get; set; }
    Player IRoomController.localPlayer { get => localPlayer; set => localPlayer = value; }
    Player IRoomController.noCampPlayer { get => noCampPlayer; set => noCampPlayer = value; }
    ArmoryData IRoomController.armoryData { get => armoryData; set => armoryData = value; }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        playerDic = new Dictionary<PlayerSite, Player>();
    }
    public void InitLocalPlayer()
    {
        localPlayer = Instantiate(localPlayer);
        localPlayer.name = "LocalPlayer";
        localPlayer.site = PlayerSite.Left;
        playerDic.Add(PlayerSite.Left, localPlayer);
    }

    public void InitNoCamp()
    {
        noCampPlayer = Instantiate(noCampPlayer);
        noCampPlayer.name = "NoCampPlayer";
        noCampPlayer.site = PlayerSite.NoCamp;
        playerDic.Add(PlayerSite.NoCamp, noCampPlayer);
    }
    public void OnGameStart()
    {
        string name = SceneManager.GetActiveScene().name;
        foreach (var player in playerDic.Values)
        {
            player.playerItem = GameObject.Instantiate(player.playerItem, Vector3.zero, Quaternion.identity);
            player.playerItem.name = player.playerItem.name.ToString() + player.site.ToString();
            player.playerItem.SetParent(GameObject.Find("GameObjects").transform);
            player.units = player.playerItem.transform.GetChild(0);
            player.constructions = player.playerItem.transform.GetChild(1);
            GameObjectInit[] objs = GameObject.FindObjectsByType<GameObjectInit>(FindObjectsSortMode.None);
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
        //HeroController hero = Instantiate(GameEntry.ResourceComponent.prefabDic["HeroController"][armoryData.hero]).GetComponent<HeroController>();
        HeroController hero = Instantiate(heroPrefab).GetComponent<HeroController>();
        hero.transform.position = Vector3.zero;
        localPlayer.armory = armoryData;
        localPlayer.AddObject(hero);
        localPlayer.InitSkills();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            InitNoCamp();
            InitLocalPlayer();
            OnGameStart();
        }
    }
}