using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuadTreeManager : SingletonMonoBehaviour<QuadTreeManager>
{
    public bool showGizmos;
    private Dictionary<QuadTreeType, QuadTreeNode> quadTrees;
    #region Inner
    private void Awake()
    {
        quadTrees = new Dictionary<QuadTreeType, QuadTreeNode>
        {
            { QuadTreeType.Object, new QuadTreeNode(GameEntry.SettingComponent.settingData.mapSize, new Vector2 { x = GameEntry.SettingComponent.settingData.mapSize / 2, y = GameEntry.SettingComponent.settingData.mapSize / 2 }, 0, null) },
            { QuadTreeType.Bullet, new QuadTreeNode(GameEntry.SettingComponent.settingData.mapSize, new Vector2 { x = GameEntry.SettingComponent.settingData.mapSize / 2, y = GameEntry.SettingComponent.settingData.mapSize / 2 }, 0, null) }
        };
    }
    public QuadTreeNode FindTarget(QuadTreeType type, QuadTreeStat obj)
    {
        return quadTrees[type].FindTarget(obj);
    }
    private void Update()
    {
        if (!RoomController.instance.gameReady) return;
        foreach (var root in quadTrees)
        {
            root.Value.Update();
        }
    }
    #endregion

    #region Interface
    public void Insert(QuadTreeType type,QuadTreeStat obj)
    {
        quadTrees[type].Insert(obj);
    }
    public void Delete(QuadTreeType type,QuadTreeStat obj)
    {
        quadTrees[type].Delete(obj);
    }
    public void Find(QuadTreeType type,Vector2 position, float radius, ref List<QuadTreeStat> list)
    {
        quadTrees[type].Find(new Vector2(position.x - radius, position.y - radius), new Vector2(position.x + radius, position.y + radius), ref list);
    }
    public QuadTreeStat FindNearest(QuadTreeType type,Vector2 position, float radius)
    {
        return null;
    }
    #endregion
    private void OnDrawGizmos()
    {
        if (!showGizmos || quadTrees == null) return;
        Gizmos.color = Color.white;
        quadTrees[QuadTreeType.Object].DrawGizmos();
    }
}
public enum QuadTreeType
{
    Object,
    Bullet,
}