using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("MainSkill")]
    [SerializeField] protected Transform mainSkill;
    [Header("Info")]
    [SerializeField] protected TextMeshProUGUI population;
    [SerializeField] protected TextMeshProUGUI property;
    [Header("Sount")]
    [SerializeField] protected TextMeshProUGUI sount;
    //[Header("MiniMap")]
    [Header("HeroPanel")]
    [SerializeField] protected Image health;
    [SerializeField] protected Image icon;
    //[Header("WeapenPanel")]
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        List<GlobalSkillData> list = new List<GlobalSkillData>();
        foreach(var skill in IRoomController.Instance().armoryData.globalSkills)
        {
            list.Add(GameEntry.ResourceComponent.GetResource<GlobalSkillData>(skill));
        }
        GlobalSkill globalSkill = mainSkill.GetComponent<GlobalSkill>();
        globalSkill.InitAbilities(list);
    }
}
