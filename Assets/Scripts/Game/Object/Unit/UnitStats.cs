using UnityEngine;
[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObject/Stats/Unit")]
public class UnitStats : GameObjectStats
{
    [Header("寻路")]
    public static float timeHorizon = 2f;
    public static float obsTimeHorizon = 3f;
    public static float findDistance = 1.5f;
    [Header("移动")]
    public float speed = 2f;
    public float rotateForce;
    public float accelerateForce;
    public bool canAutoMove;
}