using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    public float size;
    public Vector2 center;
    public List<GameObjectController> objList;
    public QuadTreeNode[] children;
    private int depth;
    public bool isDivide => children != null;
    public QuadTreeNode()
    {
        size = 0;
        center = Vector2.zero;
        objList = new List<GameObjectController>();
        children = new QuadTreeNode[4];
        depth = 0;
    }
    public QuadTreeNode(float size, Vector2 center, int depth) 
    {
        this.size = size;
        this.center = center;
        this.depth = depth;
    }
    public void Insert(GameObjectController obj)
    {
        if (!Overlaps(obj.transform.position, obj.stats.radius)) return;
        if((!isDivide && objList.Count < GameSetting.instance.maxObject )|| CrossSplitLine(obj.transform.position,obj.stats.radius) || depth == GameSetting.instance.maxDepth)
        {
            if (!objList.Contains(obj))
            {
                objList.Add(obj);
            }
        }
        else
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
        Vector2 LT = new Vector2(-quarSize, quarSize);
        Vector2 RT = new Vector2(quarSize, quarSize);
        Vector2 LB = new Vector2(-quarSize, -quarSize);
        Vector2 RB = new Vector2(quarSize, quarSize);
        children[0] = new QuadTreeNode(halfSize,center + LT,depth + 1);
        children[1] = new QuadTreeNode(halfSize, center + RT, depth + 1);
        children[2] = new QuadTreeNode(halfSize, center + LB, depth + 1);
        children[3] = new QuadTreeNode(halfSize, center + RB, depth + 1);
        List<GameObjectController> objs = objList;
        objList.Clear();
        foreach (GameObjectController obj in objs)
             Insert(obj);
    }
    private bool Overlaps(Vector2 position, float radius)
    {
        float minx = center.x - size;
        float miny = center.y - size;
        float maxx = center.x + size;
        float maxy = center.y + size;
        if (position.x - radius < minx) return false;
        if (position.x + radius > maxx) return false;
        if (position.y - radius < miny) return false;
        if (position.y + radius > maxy) return false;
        return true;
    }
    private bool CrossSplitLine(Vector2 position, float radius)
    {
        Vector2 min = position + new Vector2(-radius, -radius);
        Vector2 max = position + new Vector2(radius, radius);
        if (min.x < center.x && max.x > center.x) return true;
        if (min.y < center.y && max.y > center.y) return true;
        return false;
    }
}