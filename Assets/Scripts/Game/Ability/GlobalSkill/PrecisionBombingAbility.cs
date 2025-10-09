using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
[CreateAssetMenu(fileName = "PrecisionBombingAbility", menuName = "ScriptableObject/GlobalSkill/PrecisionBombingAbility")]
public class PrecisionBombingAbility : CoverAbility
{
    public string spawnObject;
    public float waitTime;
    public float damage;
    public string effectName;
    public override void Do(GameObjectController target)
    {
        base.Do(target);
        GameObject obj = GameEntry.ObjectPoolComponent.Get(effectName);
        obj.transform.parent = target.transform;
        obj.transform.position = new Vector3(0,0,0);
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        GameEntry.ObjectPoolComponent.Get(spawnObject).GetComponent<MissileTarget>().Init(waitTime,target, damage);
    }
}
