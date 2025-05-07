using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonNetBehaviour<UIManager>
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
            //bj.Value.Locate(obj.Key.transform.position, obj.Key.GetHealth(), obj.Key.stats.maxHealth);
        }
    }
    public void AddHealthBar(GameObjectController obj, string name)
    {
        GameObject img = ObjectPool.instance.Get(name);
        healthImages.Add(obj, img.GetComponent<HealthImage>());
        img.transform.SetParent(healthBarCanvas);
    }
    public void RemoveHealthBar(GameObjectController obj)
    {
        ObjectPool.instance.Release(obj.gameObject);
        healthImages.Remove(obj);
    }
}