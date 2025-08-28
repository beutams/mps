using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AutoSpawn", menuName = "ScriptableObject/Universal/AutoSpawn")]
public class AutoSpawn : AutoAbility
{
    public Vector3 spawnPosition;
    public List<string> prefabs;
    public Vector3 targetPoint;
    public float intevalTime;
    private bool run;
    private Timer intervalTimer;
    private int index;
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        run = false;
        intervalTimer = new Timer();
        intervalTimer.Init(intevalTime, Spawn, false, false);
        TimerManager.instance.AddTimer(intervalTimer);
    }
    public override bool CanDo()
    {
        return !run;
    }
    public override void Do()
    {
        base.Do();
        run = true;
    }
    public override void OnTimerComplete()
    {
        base.OnTimerCompletePosition();
        intervalTimer.Lanuch();
    }
    private void Spawn()
    {
        if (index < prefabs.Count)
        {
            GameObject obj = GameEntry.ObjectPoolComponent.Get("UnitStat",prefabs[index]);
            obj.transform.position = spawnPosition;
            obj.transform.rotation = Quaternion.identity;
            obj.GetComponent<GameObjectController>().events.onSpawn?.Invoke(owner.player);
            if(obj.TryGetComponent(out UnitController unit))
            {
                unit.SetMoveTarget(null, targetPoint);
            }
            index++;
        }
        else
        {
            index = 0;
            intervalTimer.Pause();
        }
        intervalTimer.Reset();
    }
}