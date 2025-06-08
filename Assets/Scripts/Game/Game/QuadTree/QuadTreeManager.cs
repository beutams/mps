using UnityEngine;

public class QuadTreeManager : SingletonMonoBehaviour<QuadTreeManager>
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