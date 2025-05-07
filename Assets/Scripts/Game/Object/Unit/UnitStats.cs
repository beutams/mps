using UnityEngine;
[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObject/Stats/Unit")]
public class UnitStats : GameObjectStats
{
    [Header("ORCA")]
    public static float timeHorizon = 5f;
    public static float obsTimeHorizon = 5f;
    public static float speed = 2f;
    public static float findDistance = 1.5f;
}