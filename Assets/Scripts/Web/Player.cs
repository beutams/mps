using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 设定
    public Transform playerItem;
    #endregion

    #region 字段

    #endregion

    #region 属性
    public Transform units { get; set; }
    public Transform constructions { get; set; }
    public PlayerSite site {  get; set; }
    public List<UnitController> unitList {  get; private set; }
    public List<ConstructionController> constructionList {  get; private set; }
    public HeroController hero {  get; private set; }
    public ArmoryData armory { get; set; }
    public int property {  get; set; }
    public int population { get; set; }
    public Dictionary<int, GlobalSkillData> globalSkills = new Dictionary<int, GlobalSkillData>();
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
        constructionList = new List<ConstructionController>();
        unitList = new List<UnitController>();
    }
    public void InitSkills()
    {
        for(int i = 1;i <= 3;i++)
        {
            globalSkills.Add(i, GameEntry.ResourceComponent.dataDic["GlobalSkillData"][armory.globalSkills[i-1]] as GlobalSkillData);
        }
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
