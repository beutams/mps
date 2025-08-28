using UnityEngine;

public class HeroStatus : UnitStatus, ID
{
    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;
    public int ID => id;
}