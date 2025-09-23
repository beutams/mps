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
    [Header("WeapenPanel")]
    [SerializeField] protected Transform weapenPanel;
    [Header("MiniMap")]
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected RectTransform miniMap;

    protected Dictionary<int,WeapenGroup> weapenGroups = new Dictionary<int, WeapenGroup>();
    private void Start()
    {
        OnReadyInit();
        GameEntry.EventComponent.Subscribe(GameEvent.UICloseEvent, UpdateWeapen);
    }
    private void Update()
    {
        if (!RoomController.instance.gameReady) return;
        UpdateInfo();
        UpdateMiniMap();
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
        population.text = $"{RoomController.instance.localPlayer.population}/{GameEntry.SettingComponent.settingData.maxUnits}";
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
                for(int j = 0; j < list.childCount; j++)
                {
                    if (list.GetChild(j).gameObject.activeSelf)
                        GameEntry.ObjectPoolComponent.Release(list.GetChild(0).gameObject);
                }
                GameEntry.ObjectPoolComponent.Release(weapenPanel.GetChild(i).gameObject);
            }
            HeroController hero = RoomController.instance.localPlayer.hero;
            for (int i = 0; i < hero.weapenGroup.Count; i++)
            {
                List<WeapenModel> weapens = hero.weapenGroup[i+1];
                if (weapens.Count == 0) continue;
                Transform group = GameEntry.ObjectPoolComponent.Get("WeapenGroupUI").transform;
                weapenGroups.Add(i+1,group.GetComponent<WeapenGroup>());
                group.parent = weapenPanel;
                group.name += i+1;
                foreach (var weapen in weapens)
                {
                    WeapenUIItem item = GameEntry.ObjectPoolComponent.Get("WeapenUI").GetComponent<WeapenUIItem>();
                    item.transform.SetParent(group.Find("WeapenList"));
                    item.SetWeapen(weapen.weapen);
                }
            }
            RefreshAutoWeapen();
        }
    }
    public void UpdateMiniMap()
    {
        float angleX = 90 - Camera.main.transform.rotation.eulerAngles.x;
        float height = Camera.main.transform.position.y;
        float forwardOffset = Mathf.Tan(Mathf.Deg2Rad * angleX) * height;
        float angleY = Camera.main.transform.rotation.eulerAngles.y;
        Vector3 targetPosition = new Vector3(Mathf.Sin(angleY) * forwardOffset, 0, Mathf.Cos(angleY) * forwardOffset) + Camera.main.transform.position;
        Vector3 percent = targetPosition / GameEntry.SettingComponent.settingData.mapSize;
        Vector3 percentClamp = (percent - new Vector3(0.5f, 0, 0.5f)) * 2;
        rect.localPosition = new Vector3(miniMap.rect.width * percentClamp.x / 2, miniMap.rect.height * percentClamp.z / 2);

        this.targetPosition = targetPosition;
    }
    public void RefreshAutoWeapen()
    {
        foreach(var kvp in weapenGroups)
        {
            kvp.Value.Refresh(RoomController.instance.localPlayer.hero.autoFireDic[kvp.Key]);
        }
    }
    protected Vector3 targetPosition;
    public void OnDrawGizmos()
    {
        if (RoomController.instance == null || !RoomController.instance.gameReady) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position, new Vector3(targetPosition.x,0,targetPosition.z));
    }

}
