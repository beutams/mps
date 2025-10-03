using UnityEngine;

public class QuadTreeStat : MonoBehaviour
{
    public Vector3 position => transform.position;
    public float radius {  get; set; }
    public Player player { get; set; }
}