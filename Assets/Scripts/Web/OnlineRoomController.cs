using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineRoomController : RoomController, ID
{
    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    protected PlayerSite localSite;
    public IDType searchName => idType;
    public int ID => id;
    private void Start()
    {
        GameEntry.EventComponent.Subscribe(GameEvent.ClientChangeSceneSuccessEvent, (_) =>
        {
            InitLocalPlayer();
            OnGameStart();
            InitPlayerData();
            localPlayer.InitArmoryData();
        });
    }
    [ClientCallback]
    public override void OnGameStart()
    {
        base.OnGameStart();
    }
    [ClientCallback]
    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Client OnSceneLoaded: {scene.name}");
        if (scene.name == "GameScene")
        {
            StartCoroutine(WaitOnSceneLoad());
        }
        
/*        // 通知场景加载追踪器
        if (SceneLoadTracker.Instance != null)
        {
            SceneLoadTracker.Instance.NotifySceneLoaded(scene.name);
        }*/
    }
    public void InitLocalPlayer()
    {
        Debug.Log($"InitLocalPlayer");
        foreach (var conn in playerDic)
        {
            if(conn.Value == NetworkClient.localPlayer.GetComponent<Player>())
            {
                localPlayer = conn.Value;
                localSite = conn.Key;
            }
        }
    }
    [ClientRpc]
    public void AddPlayer(PlayerInfo info, GameObject player)
    {
        Debug.Log($"AddPlayer : {info}");
        Player playerComp = player.GetComponent<Player>();
        playerComp.site = (PlayerSite)(info.index + 1);
        playerComp.armory = info.data;
        player.transform.position = Vector3.zero;
        playerDic[playerComp.site] = playerComp;
    }
    [ClientRpc]
    public void AddNoCampPlayer(GameObject player)
    {
        Debug.Log($"AddNoCampPlayer");
        playerDic[PlayerSite.NoCamp] = player.GetComponent<Player>();
        player.transform.position = Vector3.zero;
    }
    IEnumerator WaitOnSceneLoad()
    {
        yield return null;
        Ready();
    }
    public void InitPlayerData()
    {
        foreach (var player in playerDic.Values)
        {
            player.property = 1500;
        }
    }
}
