using System.Collections.Generic;
using UnityEngine;
public class GameObjectStats : ScriptableObject, ID, IArmoryObject
{
    [Header("能力")]
    public List<Ability> abilities;
    [Header("显示")]
    public GameObject healthPerfab;
    public string objName;
    public string imgPath;
    public float maxHealth;
    public float defense;
    [Header("数值")]
    public float radius;
    public float searchRadius;
    public bool canMove;
    public bool canAttack;
    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    public int ID => id;
    public IDType searchName => idType;
    [Header("Armory")]
    [SerializeField] protected ArmorySubUI.ArmoryType type;
    public ArmorySubUI.ArmoryType Type => type;

    public T[] GetAbilities<T>() where T : Ability
    {
        List<T> ts = new List<T>();
        foreach(var ability in abilities)
        {
            if (ability.GetType() == typeof(T))
                ts.Add(ability as T);
        }
        return ts.ToArray();
    }
}