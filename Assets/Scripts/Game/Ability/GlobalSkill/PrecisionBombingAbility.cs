using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PrecisionBombingAbility", menuName = "ScriptableObject/GlobalSkill/PrecisionBombingAbility")]
public class PrecisionBombingAbility : CoverAbility
{
    public GameObject spawnObject;
    public float waitTime;
    public override void Do(GameObjectController target)
    {
        base.Do(target);
        Instantiate(spawnObject).GetComponent<MissileTarget>().Init(waitTime,target);
    }
}
