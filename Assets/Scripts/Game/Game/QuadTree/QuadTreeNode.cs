using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuadTreeNode
{
    public float size;
    public Vector2 center;
    public List<GameObjectController> objList;
    public QuadTreeNode[] children;
    public QuadTreeNode parent;
    private int depth;
    public bool isDivide => children != null;
    public QuadTreeNode()
    {
        size = 0;
        center = Vector2.zero;
        objList = new List<GameObjectController>();
        depth = 0;
    }
    public QuadTreeNode(float size, Vector2 center, int depth, QuadTreeNode parent) 
    {
        objList = new List<GameObjectController>();
        this.size = size;
        this.center = center;
        this.depth = depth;
        this.parent = parent;
    }
    #region 插入
    public void Insert(GameObjectController obj)
    {
        //0在外边 1在里面 2在边上
        int stat = 0;
        stat = Overlaps(obj.transform.position, obj.stats.radius) ? 1 : stat;
        stat = CrossSplitLine(obj.transform.position, obj.stats.radius) ? 2 : stat;
        if((stat == 1 && !isDivide && (objList.Count < GameEntry.SettingComponent.maxObject || depth == GameEntry.SettingComponent.maxDepth)) || (stat == 2))
        {
            if (!objList.Contains(obj))
            {
                objList.Add(obj);
            }
        }
        else if(stat == 1)
        {
            if (!isDivide)
                Divide();
            foreach (QuadTreeNode child in children) 
                child.Insert(obj);
        }
    }
    private void Divide()
    {
        float halfSize = size / 2;
        float quarSize = halfSize / 2;
        Vector2 LT = new Vector2(-quarSize, quarSize) + center;
        Vector2 RT = new Vector2(quarSize, quarSize) + center;
        Vector2 LB = new Vector2(-quarSize, -quarSize) + center;
        Vector2 RB = new Vector2(quarSize, -quarSize) + center;
        children = new QuadTreeNode[4];
        children[0] = new QuadTreeNode(halfSize, LT, depth + 1, this);
        children[1] = new QuadTreeNode(halfSize, RT, depth + 1, this);
        children[2] = new QuadTreeNode(halfSize, LB, depth + 1, this);
        children[3] = new QuadTreeNode(halfSize, RB, depth + 1, this);
        List<GameObjectController> objs = new List<GameObjectController>();
        foreach (GameObjectController obj in objList)
            objs.Add(obj);
        objList.Clear();
        foreach (GameObjectController obj in objs)
            Insert(obj);
    }
    private bool Overlaps(Vector3 position, float radius)
    {
        float minx = center.x - size/2;
        float miny = center.y - size/2;
        float maxx = center.x + size/2;
        float maxy = center.y + size/2;
        if (position.x - radius < minx) return false;
        if (position.x + radius > maxx) return false;
        if (position.z - radius < miny) return false;
        if (position.z + radius > maxy) return false;
        return true;
    }
    private bool ChildOverlaps(Vector3 position, float radius)
    {
        if (!isDivide) return false;
        foreach(var child in children)
        {
            if (child.Overlaps(position, radius))
                return true;
        }
        return false;
    }
    private bool CrossSplitLine(Vector3 position, float radius)
    {
        Vector2 min = Tools.V3ToV2(position) + new Vector2(-radius, -radius);
        Vector2 max = Tools.V3ToV2(position) + new Vector2(radius, radius);
        if (min.x < center.x && max.x > center.x && (Mathf.Abs(min.y - center.y) <= size/2 || Mathf.Abs(max.y - center.y) <= size / 2)) return true;
        if (min.y < center.y && max.y > center.y && (Mathf.Abs(min.x - center.x) <= size / 2 || Mathf.Abs(max.x - center.x) <= size / 2)) return true;
        return false;
    }
    #endregion

    #region 删除
    public void Delete(GameObjectController obj)
    {
        QuadTreeNode node = FindObj(obj);
        if (node == null) return;
        node.objList.Remove(obj);
        node.parent?.TryCombine();
    }
    private void TryCombine()
    {
        if (isDivide)
        {
            if (Count() <= GameEntry.SettingComponent.maxObject)
            {
                foreach (var child in children)
                {
                    foreach (var cobj in child.objList)
                    {
                        objList.Add(cobj);
                    }
                }
                children = null;
                parent?.TryCombine();
            }
        }
        else
        {
            parent?.TryCombine();
        }
    }
    private int Count()
    {
        if (!isDivide)
            return objList.Count;
        else
        {
            int count = 0;
            count += objList.Count;
            foreach(var child in children)
            {
                count += child.Count();
            }
            return count;
        }
    }
    #endregion

    #region 查找
    public void Find(Vector2 minObj, Vector2 maxObj,ref List<GameObjectController> list)
    {
        float minX = center.x - size / 2;
        float maxX = center.x + size / 2;
        float minY = center.y - size / 2;
        float maxY = center.y + size / 2;
        if((minX > minObj.x && minX < maxObj.x && minY > minObj.y && minY < maxObj.y)
            || (minX > minObj.x && minX < maxObj.x && maxY > minObj.y && maxY < maxObj.y)
            || (maxX > minObj.x && maxX < maxObj.x && minY > minObj.y && minY < maxObj.y)
            || (maxX > minObj.x && maxX < maxObj.x && maxY > minObj.y && maxY < maxObj.y))
        {
            if (isDivide)
                foreach (var child in children)
                    child.Find(minObj, maxObj, ref list);
            foreach (var obj in objList)
                list.Add(obj);
        }
    }
    public QuadTreeNode FindObj(GameObjectController obj)
    {
        if (objList.Contains(obj))
            return this;
        else
        {
            if(isDivide)
            {
                foreach(var child in children)
                {
                    var node = child.FindObj(obj);
                    if (node != null)
                        return node;
                }
            }
            return null;
        }
    }
    #endregion

    #region 更新
    public void Update()
    {
        if (objList.Count != 0)
        {
            List<GameObjectController> controllers = new List<GameObjectController>();
            foreach (var obj in objList)
            {
                if (!Overlaps(obj.transform.position, obj.stats.radius) || ChildOverlaps(obj.transform.position, obj.stats.radius))
                {
                    controllers.Add(obj);
                }
            }
            foreach(var obj in controllers)
            {
                QuadTreeNode node = QuadTreeManager.instance.FindTarget(obj);
                objList.Remove(obj);
                node.objList.Add(obj);
                if(!node.isDivide && node.objList.Count > GameEntry.SettingComponent.maxObject)
                {
                    node.Divide();
                }
                if(objList.Count == 0)
                {
                    TryCombine();
                }
            }
        }
        if (isDivide)
            foreach (var child in children)
                child.Update();
    }
    public QuadTreeNode FindTarget(GameObjectController obj)
    {
        QuadTreeNode node;
        if (Overlaps(obj.transform.position, obj.stats.radius))
        {
            if (isDivide)
            {
                foreach(var child in children)
                {
                    node = child.FindTarget(obj);
                    if(node != null) return node;
                }
                return this;
            }
            else
            {
                return this;
            }
        }
        return null;
    }
    #endregion
    public void DrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 p1 = new Vector3 { x = center.x - size / 2, y = 3, z = center.y - size / 2 };
        Vector3 p2 = new Vector3 { x = center.x - size / 2, y = 3, z = center.y + size / 2 };
        Vector3 p3 = new Vector3 { x = center.x + size / 2, y = 3, z = center.y + size / 2 };
        Vector3 p4 = new Vector3 { x = center.x + size / 2, y = 3, z = center.y - size / 2 };
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
        if (isDivide)
        {
            foreach (var child in children)
                child.DrawGizmos();
        }
        Gizmos.color = Color.blue;
        if(objList.Count > 0)
        {
            foreach(var item in objList)
            {

                Gizmos.DrawWireCube(item.transform.position, new Vector3(item.stats.radius*2, item.stats.radius * 2, item.stats.radius * 2));
            }
        }
    }
}