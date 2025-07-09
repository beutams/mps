using System;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Kinetic,//动能
    Explode,//爆炸
    Penertate,//穿透
    Energy,//能量
}
public enum DefenceType
{
    Structure,//结构
    Armoured,//装甲
    Shield,//护盾
}
[CreateAssetMenu(fileName ="Damage",menuName = "ScriptableObject/Damage")]
public class DamageDate : ScriptableObject
{
    public DamageType type;
    public List<KVP<DefenceType, float>> damageMultiplier;
}
[Serializable]
public struct KVP<T1,T2>
{
    public T1 v1;
    public T2 v2;
}
