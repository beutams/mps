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
    private void Update()
    {
        UpdateInfo();
    }
    public void Init()
    {
        List<GlobalSkillData> list = new List<GlobalSkillData>();
        foreach(var data in IRoomController.Instance().localPlayer.globalSkills.Values)
        {
            list.Add(data);
        }
        GlobalSkill globalSkill = mainSkill.GetComponent<GlobalSkill>();
        globalSkill.InitAbilities(list);
    }
    public void UpdateInfo()
    {
        population.text = IRoomController.Instance().localPlayer.population + "/10";
        property.text = IRoomController.Instance().localPlayer.property.ToString();
    }
}
