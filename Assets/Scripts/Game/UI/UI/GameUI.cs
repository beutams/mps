using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase, ID
{
    [Header("MainSkill")]
    [SerializeField] protected Transform mainSkill;
    [Header("Info")]
    [SerializeField] protected TextMeshProUGUI population;
    [SerializeField] protected TextMeshProUGUI property;
    [Header("Sount")]
    [SerializeField] protected TextMeshProUGUI sount;
    [Header("HeroPanel")]
    [SerializeField] protected Image health;
    [SerializeField] protected Image icon;
    [Header("WeapenPanel")]
    [SerializeField] protected Transform weapenPanel;

    private void Start()
    {
        GameEntry.EventComponent.Subscribe(GameEvent.ClientChangeSceneSuccessEvent,(_) => OnReadyInit());
        GameEntry.EventComponent.Subscribe(GameEvent.UICloseEvent, UpdateWeapen);
    }
    private void Update()
    {
        if (!RoomController.instance.gameReady) return;
        UpdateInfo();
    }
    public void OnReadyInit()
    {
        InitSkill();
    }
    public void InitSkill()
    {
        List<GlobalSkillData> list = new List<GlobalSkillData>();
        foreach(var data in RoomController.instance.localPlayer.globalSkills.Values)
        {
            list.Add(data);
        }
        GlobalSkill globalSkill = mainSkill.GetComponent<GlobalSkill>();
        globalSkill.InitAbilities(list);
    }
    public void UpdateInfo()
    {
        population.text = RoomController.instance.localPlayer.population + "/10";
        property.text = RoomController.instance.localPlayer.property.ToString();
    }
    public void UpdateWeapen(object data)
    {
        if(data is string str && str == "ShopUI")
        {
            for(int i = 0; i < weapenPanel.childCount; i++)
            {
                if (!weapenPanel.GetChild(i).name.StartsWith("WeapenGroupUI")) continue;
                Transform list = weapenPanel.GetChild(i).Find("WeapenList");
                while(list.childCount != 0)
                    GameEntry.ObjectPoolComponent.Release(list.GetChild(0).gameObject);
                GameEntry.ObjectPoolComponent.Release(weapenPanel.GetChild(i).gameObject);
            }
            HeroController hero = RoomController.instance.localPlayer.hero;
            for (int i = 0; i < hero.weapenGroup.Count; i++)
            {
                List<WeapenModel> weapens = hero.weapenGroup[i+1];
                if (weapens.Count == 0) continue;
                Transform group = GameEntry.ObjectPoolComponent.Get("WeapenGroupUI").transform;
                group.parent = weapenPanel;
                foreach (var weapen in weapens)
                {
                    WeapenUIItem item = GameEntry.ObjectPoolComponent.Get("WeapenUI").GetComponent<WeapenUIItem>();
                    item.transform.SetParent(group.Find("WeapenList"));
                    item.SetWeapen(weapen.weapen);
                }
            }
        }
    }
}
