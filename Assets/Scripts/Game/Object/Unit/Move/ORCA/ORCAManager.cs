using System.Collections.Generic;
using UnityEngine;

public class ORCAManager : SingletonMonoBehaviour<ORCAManager>
{
    public List<Obstacle> allObstacles = new List<Obstacle>();
    public Dictionary<GameObject, List<Obstacle>> obstaclesDic;

    private void Awake()
    {
        obstaclesDic = new Dictionary<GameObject, List<Obstacle>>();
        GenerateVerticesList();
    }
    #region Obstacle
    public void GenerateVerticesList()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach(var item in obstacles)
        {
            AddObstacle(item,false);
        }
    }
    public void AddObstacle(GameObject item, bool reg)
    {
        var coll = item.GetComponent<BoxCollider>();
        float pointX = coll.transform.position.x;
        float pointY = coll.transform.position.z;
        float halfX = coll.size.x * coll.transform.lossyScale.x * 0.5f;
        float halfY = coll.size.z * coll.transform.lossyScale.z * 0.5f;
        Vector2 self = new Vector2(pointX, pointY);
        Vector2 p1 = Tools.V3ToV2(item.transform.rotation * (new Vector3(pointX - halfX, 0, pointY - halfY) - Tools.V2ToV3(self))) + self;
        Vector2 p2 = Tools.V3ToV2(item.transform.rotation * (new Vector3(pointX - halfX, 0, pointY + halfY) - Tools.V2ToV3(self))) + self;
        Vector2 p3 = Tools.V3ToV2(item.transform.rotation * (new Vector3(pointX + halfX, 0, pointY + halfY) - Tools.V2ToV3(self))) + self;
        Vector2 p4 = Tools.V3ToV2(item.transform.rotation * (new Vector3(pointX + halfX, 0, pointY - halfY) - Tools.V2ToV3(self))) + self;

        List<Vector2> obstacle = new List<Vector2>
            {
                p1,p2,p3,p4
            };
        AddObstacle(item ,obstacle, reg);
    }
    public void AddObstacle(GameObject obj ,List<Vector2> vertices, bool reg)
    {
        if (vertices.Count < 2) return;
        List<Obstacle> obstacles = new List<Obstacle>();
        for(int i  = 0; i < vertices.Count; i++)
        {
            Obstacle current = new Obstacle();
            current.point = vertices[i];
            if(i != 0)
            {
                current.previous = obstacles[obstacles.Count - 1];
                obstacles[obstacles.Count - 1].next = current;
            }
            if(i == vertices.Count - 1)
            {
                current.next = obstacles[0];
                obstacles[0].previous = current;
            }
            current.direction = vertices[(i == vertices.Count - 1 ? 0 : i + 1)] - vertices[i];
            if(vertices.Count == 2)
            {
                current.convex = true;
            }
            else
            {
                current.convex = Tools.LeftOf(vertices[(i == 0 ? vertices.Count - 1 : i - 1)], vertices[i], vertices[(i == vertices.Count - 1 ? 0 : i + 1)]);
            }
            obstacles.Add(current);
        }
        foreach(var item in obstacles)
        {
            allObstacles.Add(item);
        }
        obstaclesDic.Add(obj, obstacles);
    }
    public void RemoveObstacle(GameObject obj)
    {
        if(obstaclesDic.ContainsKey(obj))
        {
            foreach(var item in obstaclesDic[obj])
            {
                allObstacles.Remove(item);
            }
            obstaclesDic.Remove(obj);
        }
    }
    private void OnDrawGizmos()
    {
        if(allObstacles != null)
            foreach (var item in allObstacles)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(item.point.x,2f,item.point.y), new Vector3(item.previous.point.x, 2f, item.previous.point.y));
            }
    }
    #endregion
}
