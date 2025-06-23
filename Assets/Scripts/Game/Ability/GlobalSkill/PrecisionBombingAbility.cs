using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
