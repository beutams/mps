using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSkill", menuName = "ScriptableObject/Universal/GlobalSkill")]
public class GlobalSkillData : ScriptableObject, ID
{
    public string skillName;
    public string imgPath;
    public CoverAbility ability;
    [Header("介绍")]
    [TextArea(5,5)]
    [SerializeField] public string description;

    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;
    public int ID => id;
}
