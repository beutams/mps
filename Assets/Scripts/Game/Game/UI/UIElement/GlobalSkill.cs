using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSkill : MonoBehaviour
{
    protected List<GlobalSkillItem> items = new List<GlobalSkillItem>();
    public GlobalSkillItem item;
    public Transform content;
    public Transform info;
    public void InitAbilities(List<GlobalSkillData> datas)
    {
        foreach (var data in datas)
        {
            GlobalSkillItem globalItem = Instantiate(item);
            globalItem.Init(data);
            globalItem.transform.parent = content;
            items.Add(globalItem);
        }
    }
    public void OnShowInfo(string name)
    {
        
    }
    public string GetIntroduce(string name)
    {
        return null;
    }
}
