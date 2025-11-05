using Mirror;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class OfflineRoomController : RoomController, ID
{
    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;
    public int ID => id;
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
    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            OnGameStart();
            GameEntry.ObjectPoolComponent.Clear();
        }
    }
}