using System.Collections.Generic;
using UnityEngine;
public class GameObjectStats : ScriptableObject, ID, IArmoryObject
{
    public List<Ability> abilities;

    public GameObject healthPerfab;
    public string imgPath;
    public float maxHealth;
    public float defense;
    public float radius;
    public float searchRadius;
    public bool canMove;
    public bool canAttack;
    [Header("ID")]
    [SerializeField] protected int id;
    public int ID => id;
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