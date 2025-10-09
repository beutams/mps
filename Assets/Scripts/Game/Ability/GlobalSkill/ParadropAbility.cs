using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
[CreateAssetMenu(fileName = "ParadropAbility", menuName = "ScriptableObject/GlobalSkill/ParadropAbility")]
public class ParadropAbility : CoverAbility
{
    public List<string> spawnObjects;
    public List<Vector2> offset;
    public float waitTime;
    private Timer waitTimer = new Timer();
    protected Vector3 point;
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        waitTimer.Init(waitTime, OnTimerComplete, false, false);
        TimerManager.instance.AddTimer(waitTimer);
    }
    public override void Do(Vector3 target)
    {
        base.Do(target);
        waitTimer.Lanuch();
        point = target;
    }
    protected void OnTimerComplete()
    {
        waitTimer.Reset();
        waitTimer.Pause();
        point = Vector3.zero;

        for (int i = 0; i < spawnObjects.Count; i++)
        {
            GameObject obj = GameEntry.ObjectPoolComponent.Get(spawnObjects[i]);
            obj.transform.position = point + Tools.V2ToV3(offset[i]);
        }
    }
}
