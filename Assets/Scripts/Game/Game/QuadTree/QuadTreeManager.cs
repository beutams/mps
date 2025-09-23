using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuadTreeManager : SingletonMonoBehaviour<QuadTreeManager>
{
    public bool showGizmos;
    private QuadTreeNode root;
    private void Start()
    {
        InitTree();
    }
    private void InitTree()
    {
        root = new QuadTreeNode(GameEntry.SettingComponent.settingData.mapSize, new Vector2 { x= GameEntry.SettingComponent.settingData.mapSize / 2, y= GameEntry.SettingComponent.settingData.mapSize / 2 },0,null);
    }
    public void Insert(GameObjectController obj)
    {
        root.Insert(obj);
    }
    public void Delete(GameObjectController obj)
    {
        root.Delete(obj);
    }
    public void Find(Vector2 position, float radius, ref List<GameObjectController> list)
    {
        root.Find(new Vector2(position.x - radius, position.y - radius), new Vector2(position.x + radius, position.y + radius), ref list);
    }
    public GameObjectController FindNearest(Vector2 position, float radius)
    {
        return null;
    }
    public QuadTreeNode FindTarget(GameObjectController obj)
    {
        return root.FindTarget(obj);
    }
    private void Update()
    {
        if (!RoomController.instance.gameReady) return;
        root.Update();
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos || root == null) return;
        Gizmos.color = Color.white;
        root.DrawGizmos();
    }
}
public enum QuadTreeType
{
    Object,
    Bullet,
}