using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ParadropAbility", menuName = "ScriptableObject/GlobalSkill/ParadropAbility")]
public class ParadropAbility : CoverAbility
{
    public GameObject spawnObject;
    public override void Do(Vector3 target)
    {
        base.Do(target);
        GameObject obj = Instantiate(spawnObject);
        obj.transform.position = target;
    }
}
