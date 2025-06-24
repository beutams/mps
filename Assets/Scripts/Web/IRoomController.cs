using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IRoomController
{
    public ArmoryData armoryData { get; set; }
    public Player localPlayer { get; }
    public Player noCampPlayer { get; }
    public Dictionary<PlayerSite, Player> playerDic { get; }
    public abstract void InitLocalPlayer();
    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    public abstract void OnGameStart();
    public static IRoomController Instance()
    {
        IRoomController roomController;
        if((roomController = GameObject.FindAnyObjectByType<OfflineRoomController>()) != null)
            return roomController;
/*        else if ((roomController = GameObject.FindAnyObjectByType<OnlineRoomController>()) != null)
            return roomController;*/
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