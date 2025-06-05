using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    private Transform healthBarCanvas;
    private Dictionary<GameObjectController, HealthImage> healthImages = new Dictionary<GameObjectController, HealthImage>();
    private void Awake()
    {
        healthBarCanvas = GameObject.Find("HealthBarCanvas").transform;
    }
    private void Update()
    {
        DrawHealthBar();
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
        GameEntry.ObjectPoolComponent.Release(obj.gameObject);
        healthImages.Remove(obj);
    }
}