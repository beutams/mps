using Unity.VisualScripting;
using UnityEngine;

public class GameSetting : Singleton<GameSetting>
{
    [Header("Camera")]
    public float CameraMoveSpeed = 6f;
    [Header("QuadTree")]
    public int maxDepth = 5;
    public int maxObject = 5;
}