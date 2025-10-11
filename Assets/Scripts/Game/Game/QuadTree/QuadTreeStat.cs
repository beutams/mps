using Mirror;
using UnityEngine;

public class QuadTreeStat : NetworkBehaviour
{
    public Vector3 position => transform.position;
    public float radius {  get; set; }
    public Player player { get; set; }
}