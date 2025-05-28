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
    public Transform units;
    public Transform constructions;
    #endregion

    #region 属性
    public PlayerSite site {  get; set; }
    public List<UnitController> unitList {  get; private set; }
    public List<ConstructionController> constructionList {  get; private set; }
    public HeroController hero {  get; private set; }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
        constructionList = new List<ConstructionController>();
        unitList = new List<UnitController>();
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
            unitList.Add(controller as UnitController);
            controller.transform.SetParent(units);
        }
        else if (controller is ConstructionController)
        {
            constructionList.Add(controller as ConstructionController);
            controller.transform.SetParent(constructions);
        }

    }
}
