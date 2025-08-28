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

    #endregion

    #region 属性
    public PlayerSite site {  get; set; }
    public ArmoryData armory { get; set; }
    public HeroController hero {  get; private set; }
    public Dictionary<int, GlobalSkillData> globalSkills = new Dictionary<int, GlobalSkillData>();
    public Transform units { get; set; }
    public Transform constructions { get; set; }
    public List<UnitController> unitList {  get; private set; }
    public List<ConstructionController> constructionList {  get; private set; }
    public int property {  get; set; }
    public int population { get; set; }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
        constructionList = new List<ConstructionController>();
        unitList = new List<UnitController>();
    }
    public void AddObject(GameObjectController controller)
    {
        AddObjectServer(controller);
    }
    [Command]
    public void AddObjectServer(GameObjectController controller)
    {
        AddObjectClient(controller);
    }
    [ClientRpc]
    public void AddObjectClient(GameObjectController controller)
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
    public void InitArmoryData()
    {
        InitArmoryDataServer();
    }
    [Command]
    public void InitArmoryDataServer()
    {
        Debug.Log($"Server Init Armory, Player is {site}");
        HeroController controller = GameEntry.ObjectPoolComponent.Get("HeroStats", armory.hero).GetComponent<HeroController>();
        controller.transform.position = GameObject.Find("HeroStartPoint").transform.position + new Vector3(Random.Range(0,1),0, Random.Range(0, 1));
        NetworkServer.Spawn(controller.gameObject);
        InitArmoryDataClient(controller.gameObject);
    }
    [ClientRpc]
    public virtual void InitArmoryDataClient(GameObject controller)
    {
        Debug.Log($"Client Init Armory, Player is {site}");
        for (int i = 1; i <= 3; i++)
            globalSkills.Add(i, GameEntry.ResourceComponent.GetDataResource("GlobalSkillData", armory.globalSkills[i - 1]) as GlobalSkillData);
        controller.GetComponent<HeroController>().events.onSpawn?.Invoke(this);
    }
}
