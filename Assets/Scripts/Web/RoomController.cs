using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : SingletonNetBehaviour<RoomController>
{
    public Player localPlayer;
    public Player noCampPlayer;
    public Dictionary<PlayerSite, Player> playerDic;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        playerDic = new Dictionary<PlayerSite, Player>();
    }
    protected virtual void InitNoCamp()
    {
        if(this is OnlineRoomController)
        {
            if (isServer)
            {
                InitOnlineNoCamp();
            }
        }
        else
        {
            noCampPlayer = Instantiate(noCampPlayer);
            noCampPlayer.site = PlayerSite.NoCamp;
            playerDic.Add(PlayerSite.NoCamp, noCampPlayer);
        }
    }
    protected virtual void InitLocalPlayer()
    {
        if(this is OnlineRoomController)
        {
            if (isServer)
            {

            }
        }
        else
        {
            localPlayer = Instantiate(localPlayer);
            noCampPlayer.site = PlayerSite.Left;
            playerDic.Add(PlayerSite.Left, localPlayer);
        }
    }
    [ClientRpc]
    protected virtual void InitOnlineNoCamp()
    {
        noCampPlayer = Instantiate(noCampPlayer);
        playerDic.Add(PlayerSite.NoCamp, noCampPlayer);
    }
    public void OnSceneLoadedSingle(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
        {
            InitNoCamp();
            InitLocalPlayer();
            OnGameStart();
        }
    }
    public void OnGameStart()
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
}