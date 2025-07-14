using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSkill", menuName = "ScriptableObject/Universal/GlobalSkill")]
public class GlobalSkillData : ScriptableObject, ID
{
    public string skillName;
    public string imgPath;
    public CoverAbility ability;
    [Header("ID")]
    [SerializeField] protected int id;
    [Header("介绍")]
    [TextArea(5,5)]
    [SerializeField] public string description;

    public int ID => id;
}
