using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IRoomController
{
    public static IRoomController instance;
    public ArmoryData armoryData { get; set; }
    public Player localPlayer { get; set; }
    public Player noCampPlayer { get; set; }
    public Dictionary<PlayerSite, Player> playerDic { get; }
    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    public abstract void OnGameStart();
    public static IRoomController Instance()
    {
        if (instance != null)
            return instance;
        if((instance = Object.FindAnyObjectByType<OfflineRoomController>()) != null)
            return instance;
        else if ((instance = Object.FindAnyObjectByType<OnlineRoomController>()) != null)
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