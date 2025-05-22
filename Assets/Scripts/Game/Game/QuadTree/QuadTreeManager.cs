using UnityEngine;

public class QuadTreeManager : SingletonNetBehaviour<QuadTreeManager>
{
    private QuadTreeNode root;

    private void Start()
    {
        
    }
    private void InitTree()
    {
        root = new QuadTreeNode();
    }
    private void Insert(GameObjectController obj)
    {

    } 
}