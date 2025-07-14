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
    [Header("WeapenPanel")]
    [SerializeField] protected Transform weapenPanel;
    [SerializeField] protected GameObject weapenGroup;
    [SerializeField] protected GameObject weapenPrefab;
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
    public void UpdateWeapen()
    {
        HeroController hero = IRoomController.instance.localPlayer.hero;
        for(int i = 0; i < hero.weapenGroup.Count; i++)
        {
            Transform group = Instantiate(weapenGroup).transform;
            group.parent = weapenPanel;
            List<WeapenBase> weapens = hero.weapenGroup[i];
            foreach(var weapen in weapens)
            {
                WeapenUIItem item = Instantiate(weapenPrefab).GetComponent<WeapenUIItem>();
                item.transform.parent = group.GetChild(2);
                item.SetWeapen(weapen);
            }
        }
    }
}
