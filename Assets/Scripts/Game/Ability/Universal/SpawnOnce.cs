using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpawnOnce", menuName = "ScriptableObject/Universal/SpawnOnce")]
public class SpawnOnce : Ability
{
    public Vector3 spawnPosition;
    public Vector3 targetPoint;
    public List<GameObject> perfabs;
    public float delayTime;
    private Timer delayTimer;
    private bool start;
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        delayTimer = new Timer();
        delayTimer.Init(delayTime, Spawn, false, false);
        TimerManager.instance.AddTimer(delayTimer);
    }
    public override bool CanDo()
    {
        return !start;
    }
    public override void Do()
    {
        base.Do();
        delayTimer.Reset();
        delayTimer.Lanuch();
    }
    private void Spawn()
    {
        foreach(var item in perfabs)
        {
            GameObject obj = GameEntry.ObjectPoolComponent.Get(item.name);
            obj.transform.position = spawnPosition;
            obj.transform.rotation = Quaternion.identity;
            obj.GetComponent<GameObjectController>().events.onSpawn?.Invoke(owner.player);
        }
    }
    public override void OnAbilityDestroy()
    {
        base.OnAbilityDestroy();
        TimerManager.instance.RemoveTimer(delayTimer);
    }
}