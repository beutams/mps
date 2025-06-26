using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSupportAbility", menuName = "ScriptableObject/GlobalSkill/FireSupportAbility")]
public class FireSupportAbility : CoverAbility
{
    public GameObject spawnObject;
    public override void Do(Vector3 target)
    {
        base.Do(target);

    }
}
