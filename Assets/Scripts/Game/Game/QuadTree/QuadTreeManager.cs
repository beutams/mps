using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class QuadTreeManager : SingletonMonoBehaviour<QuadTreeManager>
{
    public bool showGizmos;
    private Dictionary<QuadTreeType, QuadTreeNode> quadTrees;
    [ReadOnly][SerializeField] protected int[] objs;
    #region Inner
    private void Awake()
    {
        objs = new int[2] { 0, 0 };
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
        objs[(int)type]++;
    }
    public void Delete(QuadTreeType type,QuadTreeStat obj)
    {
        quadTrees[type].Delete(obj);
        objs[(int)type]--;
    }
    public void Find(QuadTreeType type,Vector2 position, float radius, ref List<QuadTreeStat> list)
    {
        quadTrees[type].Find(new Vector2(position.x - radius, position.y - radius), new Vector2(position.x + radius, position.y + radius), ref list);
    }
    public QuadTreeStat FindNearest(QuadTreeType type,Vector2 position, float radius,Player player)
    {
        List<QuadTreeStat> list = new List<QuadTreeStat>();
        Find(type,position,radius,ref list);
        if(list.Count == 0)
            return null;
        float min = 999;
        QuadTreeStat minItem = null;
        foreach(var item in list)
        {
            if (item.GetComponent<HeroController>() != null) continue;
            if (player == item.player) continue;
            float dis = Tools.GetDistance(position, Tools.V3ToV2(item.position));
            if (dis < min)
            {
                min = dis;
                minItem = item; 
            }
        }
        return minItem;
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
    Object = 0,
    Bullet = 1,
}