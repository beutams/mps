using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IRoomController
{
    public static IRoomController instance;
    public ArmoryData armoryData { get; set; }
    public Player localPlayer { get; }
    public Player noCampPlayer { get; }
    public Dictionary<PlayerSite, Player> playerDic { get; }
    public abstract void InitLocalPlayer();
    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    public abstract void OnGameStart();
    public static IRoomController Instance()
    {
        if (instance != null)
            return instance;
        if((instance = GameObject.FindAnyObjectByType<OfflineRoomController>()) != null)
            return instance;
        else if ((instance = GameObject.FindAnyObjectByType<OnlineRoomController>()) != null)
            return instance;
        else 
            return null;
    }
}
public enum PlayerSite : byte
{
    NoCamp = 0,
    Left = 1,
    Right = 2,
}