using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSupportAbility", menuName = "ScriptableObject/GlobalSkill/FireSupportAbility")]
public class FireSupportAbility : CoverAbility
{
    public GameObject spawnObject;
    public string effectName;
    public float radius;
    public float damage;
    private Timer waitTimer = new Timer();
    protected float waitTime;

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

        GameObject obj = GameEntry.ObjectPoolComponent.Get(effectName);
        obj.transform.position = point;
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        List<QuadTreeStat> list = new List<QuadTreeStat>();
        QuadTreeManager.instance.Find(QuadTreeType.Object, point, radius, ref list);
        foreach (QuadTreeStat stat in list)
        {
            if (Tools.GetDistance(Tools.V3ToV2(stat.transform.position), Tools.V3ToV2(point)) > radius || stat.player == RoomController.instance.localPlayer)
                continue;
            stat.GetComponent<GameObjectController>().UnderAttack(damage);
        }
    }
}
