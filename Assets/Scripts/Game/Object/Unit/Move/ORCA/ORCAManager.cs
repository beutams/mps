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
    public List<UnitController> allAgents = new List<UnitController>();
    public List<Obstacle> allObstacles = new List<Obstacle>();

    private void Start()
    {
        GenerateVerticesList();
    }
    #region Obstacle
    public void GenerateVerticesList()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach(var item in obstacles)
        {
            var coll = item.GetComponent<BoxCollider>();
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
            AddObstacle(obstacle);
        }
    }
    public void AddObstacle(List<Vector2> vertices)
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
    }
    #endregion
}
