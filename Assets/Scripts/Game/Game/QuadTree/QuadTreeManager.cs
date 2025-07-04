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
    public void Find(Vector2 min, Vector2 max, ref List<GameObjectController> list)
    {
        root.Find(min, max, ref list);
    }
    public QuadTreeNode FindTarget(GameObjectController obj)
    {
        return root.FindTarget(obj);
    }
    private void Update()
    {
        root.Update();
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos || root == null) return;
        Gizmos.color = Color.white;
        root.DrawGizmos();
    }

}