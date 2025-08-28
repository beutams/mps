using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class RoomController : SingletonNetBehaviour<RoomController>
{
    public Player localPlayer { get; set; }
    public Player noCampPlayer { get; set; }
    public bool gameReady {  get; set; }
    public Dictionary<PlayerSite, Player> playerDic { get; set; }
    public UnityEvent onGameReady;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        playerDic = new Dictionary<PlayerSite, Player>();
    }
    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);

    public virtual void OnGameStart()
    {
        Debug.Log($"Client OnGameStart,PlayerNumber : {playerDic.Count()}");
        foreach (var player in playerDic)
        {
            player.Value.playerItem = Instantiate(player.Value.playerItem, Vector3.zero, Quaternion.identity);
            player.Value.playerItem.name = player.Value.playerItem.name.ToString() + player.Key.ToString();
            player.Value.playerItem.SetParent(GameObject.Find("GameObjects").transform);
            player.Value.units = player.Value.playerItem.transform.GetChild(0);
            player.Value.constructions = player.Value.playerItem.transform.GetChild(1);
            GameObjectInit[] objs = FindObjectsByType<GameObjectInit>(FindObjectsSortMode.None);
            foreach (GameObjectInit obj in objs)
            {
                if (obj.site == player.Key)
                {
                    foreach (var gobj in obj.objList)
                    {
                        gobj.events.onSpawn?.Invoke(player.Value);
                    }
                }
            }
            foreach (var obj in objs)
            {
                Destroy(obj.gameObject);
            }
        }
    }
    public virtual void Ready()
    {
        gameReady = true;
        GameEntry.EventComponent.Notify(GameEvent.ClientChangeSceneSuccessEvent);
    }
}
public enum PlayerSite : byte
{
    NoCamp = 0,
    Left = 1,
    Right = 2,
}