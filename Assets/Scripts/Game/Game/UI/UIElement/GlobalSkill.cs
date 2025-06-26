using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSkill : MonoBehaviour
{
    protected List<GlobalSkillItem> items = new List<GlobalSkillItem>();
    public GlobalSkillItem item;
    public Transform info;
    
    public void InitAbilities(List<GlobalSkillData> datas)
    {
        int index = 0;
        foreach (var data in datas)
        {
            index++;
            GlobalSkillItem globalItem = Instantiate(item);
            globalItem.Init(data, index);
            globalItem.transform.parent = transform;
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
