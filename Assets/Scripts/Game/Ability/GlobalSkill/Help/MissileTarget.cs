using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTarget : MonoBehaviour
{
    public Timer timer;
    public void Start()
    {
        timer = new Timer();
    }
    public void Init(float waitTime, GameObjectController target)
    {
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, -target.GetComponent<CapsuleCollider>().height / 2, 0);
        timer.Init(waitTime, SpawnMissile, false, false);
    }
    public void SpawnMissile()
    {

    }
}
