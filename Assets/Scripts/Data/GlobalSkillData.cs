using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSkill", menuName = "ScriptableObject/Universal/GlobalSkill")]
public class GlobalSkillData : ScriptableObject, ID
{
    public string imgPath;
    public CoverAbility ability;
    [Header("ID")]
    [SerializeField] protected int id;
    public int ID => id;
}
