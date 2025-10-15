using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTarget : MonoBehaviour
{
    protected Timer timer;
    protected GameObjectController target;
    protected float damage;
    public void Start()
    {
        timer = new Timer();
    }
    public void Init(float waitTime, GameObjectController target, float damage)
    {
        this.target = target;
        this.damage = damage;
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        timer.Init(waitTime, OnTimerComplete, false, false);
        timer.Lanuch();
    }
    public void OnTimerComplete()
    {
        timer.Reset();
        timer.Pause();
        target.UnderAttackServer(damage);
    }
}
