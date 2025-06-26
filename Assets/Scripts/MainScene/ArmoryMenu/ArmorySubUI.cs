using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ArmorySubUI : SubUIBase
{
    [Header("Left")]
    public Transform heroItemList;
    public Transform skillItemList;
    public ArmoryItem itemPrefab;
    public SettingsButton heroButton;
    public SettingsButton skillButton;
    [Header("Right")]
    public Transform selectIconList;

    public Dictionary<ArmoryItem, HeroStats> heroList = new Dictionary<ArmoryItem, HeroStats>();
    public Dictionary<ArmoryItem, GlobalSkillData> skillList = new Dictionary<ArmoryItem, GlobalSkillData>();
    protected List<Image> imgList = new List<Image>();
    public static ArmoryData data;
    protected int switchId = 0;
    protected string defaultImgPath = string.Empty;
    protected override void Awake()
    {
        base.Awake();
        data = new ArmoryData();
    }
    protected virtual void Start()
    {
        InitData();
        InitComponent();
    }
    public void InitData()
    {
        foreach(var hero in GameEntry.ResourceComponent.dataDic["HeroStats"].Values)
        {
            HeroStats stats = hero as HeroStats;
            ArmoryItem item = Instantiate(itemPrefab);
            item.Init(stats.imgPath, stats.name,
                () => SetData(ArmoryType.Hero, GameEntry.ResourceComponent.indexDic["HeroStats"][stats]));
            item.transform.parent = heroItemList;
            heroList.Add(item, stats);

        }
        foreach (var skill in GameEntry.ResourceComponent.dataDic["GlobalSkillData"].Values)
        {
            GlobalSkillData sdata = skill as GlobalSkillData;
            int id = GameEntry.ResourceComponent.indexDic["GlobalSkillData"][sdata];
            ArmoryItem item = Instantiate(itemPrefab);
            item.Init(sdata.imgPath, sdata.name, () => {
            if (data.globalSkills.Contains(id))
                SetData(ArmoryType.GlobalSkillsRemove, id);
            else if (data.globalSkills.Count < 3)
                SetData(ArmoryType.GlobalSkillsAdd, id);
            });
            item.transform.parent = skillItemList;
            skillList.Add(item, sdata);
        }
    }
    public void InitComponent()
    {
        for(int i = 0;i < selectIconList.childCount; i++)
            imgList.Add(selectIconList.GetChild(i).GetComponent<Image>());
    }
    public void SetData(ArmoryType item, int value)
    {
        switch (item)
        {
            case ArmoryType.Hero:
                data.hero = value;
                break;
            case ArmoryType.GlobalSkillsAdd:
                data.globalSkills.Add(value);
                break;
            case ArmoryType.GlobalSkillsRemove:
                data.globalSkills.Remove(value);
                break;
        }
        for(int i = 0;i < imgList.Count; i++)
        {
            if(i == 0)
            {
                HeroStats stats = GameEntry.ResourceComponent.dataDic["HeroStats"][data.hero] as HeroStats;
                imgList[i].sprite = GameEntry.ResourceComponent.GetImage(stats.imgPath);
            }
            else
            {
                GlobalSkillData sdata = GameEntry.ResourceComponent.dataDic["GlobalSkillData"][data.globalSkills[i-1]] as GlobalSkillData;
                if(sdata == null)
                    imgList[i].sprite = GameEntry.ResourceComponent.GetImage(defaultImgPath);
                else
                    imgList[i].sprite = GameEntry.ResourceComponent.GetImage(sdata.imgPath);
            }
        }
    }
    public void OnSwitchClick(int value)
    {
        bool isHero = value == 0 ? true : false;
        heroItemList.gameObject.SetActive(isHero);
        skillItemList.gameObject.SetActive(!isHero);
        
    }
    protected override void OnClose()
    {
        
    }

    protected override void OnOpen()
    {

    }
    public enum ArmoryType 
    { 
        None,
        Hero,
        GlobalSkillsAdd,
        GlobalSkillsRemove,
    }
}
