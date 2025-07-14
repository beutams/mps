using UnityEngine;

[CreateAssetMenu(fileName = "HeroStats", menuName = "ScriptableObject/Stats/HeroStats")]
public class HeroStats : UnitStats
{
    [Header("介绍")]
    [TextArea(5,20)]
    public string description;
}