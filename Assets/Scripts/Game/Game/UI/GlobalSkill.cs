using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSkill : MonoBehaviour
{
    protected List<GlobalSkillItem> items = new List<GlobalSkillItem>();
    public GlobalSkillItem item;
    public Transform content;
    public Transform info;
    public void InitAbilities(List<CoverAbility> abilities)
    {
        foreach (var ability in abilities)
        {
            GlobalSkillItem globalItem = Instantiate(item);
            globalItem.Init(Instantiate(ability));
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
