using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    #region 设定
    public Transform playerItem;
    #endregion

    #region 字段
    private Transform units;
    private Transform constructions;

    #endregion

    #region 属性
    public PlayerSite site {  get; set; }
    public List<UnitController> soldierList {  get; private set; }
    public List<ConstructionController> constructionList {  get; private set; }
    public HeroController hero {  get; private set; }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
        constructionList = new List<ConstructionController>();
        soldierList = new List<UnitController>();
    }
    public void AddObject(GameObjectController controller)
    {
        if (controller is HeroController && hero == null)
        {
            hero = controller as HeroController;
            controller.transform.SetParent(playerItem);
        }
        else if (controller is UnitController)
        {
            soldierList.Add(controller as UnitController);
            controller.transform.SetParent(units);
        }
        else if (controller is ConstructionController)
        {
            constructionList.Add(controller as ConstructionController);
            controller.transform.SetParent(constructions);
        }

    }
    [Command]
    public void OnGameStart()
    {
        RoomController.instance.ready[this] = true;
        foreach (var kvp in RoomController.instance.ready)
        {
            if (!RoomController.instance.ready[kvp.Key]) return;
        }
        foreach (var player in RoomController.instance.playerDic.Values)
        {
            player.OnGameStartInner();
        }
    }
    [ClientRpc]
    public void OnGameStartInner()
    {
        playerItem = Instantiate(playerItem, Vector3.zero, Quaternion.identity);
        playerItem.name = playerItem.name.ToString() + site.ToString();
        playerItem.SetParent(GameObject.Find("GameObjects").transform);
        units = playerItem.transform.GetChild(0);
        constructions = playerItem.transform.GetChild(1);
        GameObjectInit[] objs = FindObjectsByType<GameObjectInit>(FindObjectsSortMode.None);
        foreach (GameObjectInit obj in objs)
        {
            if (obj.site == site)
            {
                foreach (var gobj in obj.objList)
                {
                    gobj.events.onSpawn?.Invoke(this);
                }
            }
        }
    }
}
