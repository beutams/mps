using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Color green;
    public Color blue;
    public Color red;
    private Transform healthBarCanvas;
    private Transform minimapParent;
    private Dictionary<GameObjectController, HealthImage> healthImages = new Dictionary<GameObjectController, HealthImage>();
    private Dictionary<GameObjectController, MiniMapItem> minimapItems = new Dictionary<GameObjectController, MiniMapItem>();
    private void Awake()
    {
        healthBarCanvas = GameObject.Find("HealthBarCanvas").transform;
        minimapParent = GameObject.Find("MiniMapParent").transform;
    }
    private void Update()
    {
        DrawHealthBar();
        DrawMiniMap();
    }
    private void DrawHealthBar()
    {
        foreach(var obj in healthImages)
        {
            obj.Value.Locate(obj.Key.transform.position, obj.Key.GetHealth(), obj.Key.stats.maxHealth);
        }
    }
    public void AddHealthBar(GameObjectController obj, string name)
    {
        GameObject img = GameEntry.ObjectPoolComponent.Get(name);
        healthImages.Add(obj, img.GetComponent<HealthImage>());
        img.transform.SetParent(healthBarCanvas);
    }
    public void RemoveHealthBar(GameObjectController obj)
    {
        if (healthImages.ContainsKey(obj))
        {
            GameEntry.ObjectPoolComponent.Release(healthImages[obj].gameObject);
            healthImages.Remove(obj);
        }
    }
    private void DrawMiniMap()
    {
        foreach (var obj in minimapItems)
        {
            obj.Value.Locate(obj.Key.transform.position);
        }
    }
    public void AddMiniMapItem(GameObjectController obj, string name)
    {
        Debug.Log($"Create MiniMapItem {obj},Current Numbet is {minimapItems.Count}");
        GameObject img = GameEntry.ObjectPoolComponent.Get(name);
        minimapItems.Add(obj, img.GetComponent<MiniMapItem>());
        img.transform.SetParent(minimapParent);
        SpriteRenderer image = img.GetComponent<SpriteRenderer>();
        if (obj.player == RoomController.instance.localPlayer)
            image.color = ColorTool.ToColor(GameEntry.SettingComponent.settingData.local);
        else if (obj.player == RoomController.instance.playerDic[PlayerSite.NoCamp])
            image.color = ColorTool.ToColor(GameEntry.SettingComponent.settingData.noCamp);
        else
            image.color = ColorTool.ToColor(GameEntry.SettingComponent.settingData.partner);
    }
    public void RemoveMiniMapItem(GameObjectController obj)
    {
        Debug.Log($"Remove MiniMapItem {obj},Current Numbet is {minimapItems.Count}");
        GameEntry.ObjectPoolComponent.Release(minimapItems[obj].gameObject);
        minimapItems.Remove(obj);
    }
}