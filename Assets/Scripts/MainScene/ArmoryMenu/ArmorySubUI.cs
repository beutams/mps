using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ArmorySubUI : SubUIBase<ArmorySubUI>
{
    [Header("Left")]
    [SerializeField] protected Transform heroItemList;
    [SerializeField] protected Transform skillItemList;
    [SerializeField] protected ArmoryItem itemPrefab;
    [SerializeField] protected SettingsButton heroButton;
    [SerializeField] protected SettingsButton skillButton;
    [Header("Right")]
    [SerializeField] protected Transform selectIconList;
    [SerializeField] protected TextMeshProUGUI title;
    [SerializeField] protected TextMeshProUGUI info;

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
        InitButton();
    }
    public void InitData()
    {
        foreach (var hero in GameEntry.ResourceComponent.dataDic["HeroStats"].Values)
        {
            HeroStats stats = hero as HeroStats;
            ArmoryItem item = Instantiate(itemPrefab);
            int id = GameEntry.ResourceComponent.indexDic["HeroStats"][stats];
            item.Init(stats.imgPath, stats.objName,stats,
                () => {
                    if (data.hero == id)
                        SetData(ArmoryType.Hero, id);
                    else
                        SetData(ArmoryType.Hero, -1);
                });
            item.transform.parent = heroItemList;
            heroList.Add(item, stats);

        }
        foreach (var skill in GameEntry.ResourceComponent.dataDic["GlobalSkillData"].Values)
        {
            GlobalSkillData sdata = skill as GlobalSkillData;
            int id = GameEntry.ResourceComponent.indexDic["GlobalSkillData"][sdata];
            ArmoryItem item = Instantiate(itemPrefab);
            item.Init(sdata.imgPath, sdata.skillName, sdata, () => {
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
    public void InitButton()
    {
        heroButton.onClick += () => OnSwitchClick(0);
        skillButton.onClick += () => OnSwitchClick(1);
        OnSwitchClick(0);
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
                string path = defaultImgPath;
                if (data.hero == -1)
                {
                    HeroStats stats = GameEntry.ResourceComponent.dataDic["HeroStats"][data.hero] as HeroStats;
                    path = stats.imgPath;
                }
                imgList[i].sprite = GameEntry.ResourceComponent.GetImage(path);
            }
            else
            {
                string path = defaultImgPath;
                if (data.globalSkills.Count >= i)
                {
                    GlobalSkillData sdata = GameEntry.ResourceComponent.dataDic["GlobalSkillData"][data.globalSkills[i-1]] as GlobalSkillData;
                    path = sdata.imgPath;
                }
                imgList[i].sprite = GameEntry.ResourceComponent.GetImage(path);
            }
        }
    }
    public void OnSwitchClick(int value)
    {
        bool isHero = value == 0 ? true : false;
        heroItemList.gameObject.SetActive(isHero);
        skillItemList.gameObject.SetActive(!isHero);
        
    }
    public void ShowObjectInfo(ScriptableObject obj)
    {
        if(obj is HeroStats)
            ShowHero(obj as HeroStats);
        else if (obj is GlobalSkillData)
            ShowSkill(obj as GlobalSkillData);
    }
    protected void ShowHero(HeroStats heroStats)
    {
        title.text = heroStats.name;
        info.text = heroStats.description;
    }
    protected void ShowSkill(GlobalSkillData data)
    {
        title.text = data.name;
        title.text = data.description;
    }
    public enum ArmoryType 
    { 
        None,
        Hero,
        GlobalSkillsAdd,
        GlobalSkillsRemove,
    }
}
