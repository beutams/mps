using System.Collections.Generic;
using UnityEngine;

public class ORCAManager : MonoBehaviour
{
    private static ORCAManager instance;
    public static ORCAManager Instance
    {
        get
        {
            if(instance == null) 
                instance = FindObjectOfType<ORCAManager>();
            return instance;
        }
    }
    public List<Obstacle> allObstacles = new List<Obstacle>();
    public Dictionary<GameObject, List<Obstacle>> obstaclesDic;

    private void Start()
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
        var angle = item.transform.rotation.eulerAngles.y;
        float minX = coll.transform.position.x -
         coll.size.x * coll.transform.lossyScale.x * 0.5f;
        float minZ = coll.transform.position.z -
                     coll.size.z * coll.transform.lossyScale.z * 0.5f;
        float maxX = coll.transform.position.x +
                     coll.size.x * coll.transform.lossyScale.x * 0.5f;
        float maxZ = coll.transform.position.z +
                     coll.size.z * coll.transform.lossyScale.z * 0.5f;

        List<Vector2> obstacle = new List<Vector2>
            {
                new Vector2(maxX, maxZ),
                new Vector2(minX, maxZ),
                new Vector2(minX, minZ),
                new Vector2(maxX, minZ)
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
    #endregion
}
