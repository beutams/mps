using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ArmorySubUI : SubUIBase
{
    public Transform heroItemList;
    public Transform skillItemList;
    public ArmoryItem itemPrefab;
    public Dictionary<ArmoryItem, HeroStats> heroList = new Dictionary<ArmoryItem, HeroStats>();
    public Dictionary<ArmoryItem, GlobalSkillData> skillList = new Dictionary<ArmoryItem, GlobalSkillData>();
    public static ArmoryData data;
    protected override void Awake()
    {
        base.Awake();
        data = new ArmoryData();
    }
    protected virtual void Start()
    {
        Init();
    }
    public void Init()
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
