using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : BulletEffect
{
    public string effectName;
    public int radius;
    public float damage;
    public float time;
    protected override void OnBulletTrigger(Collider collider)
    {
        GameObject obj = GameEntry.ObjectPoolComponent.Get(effectName);
        obj.transform.position = transform.position;
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        List<QuadTreeStat> list = new List<QuadTreeStat>();
        QuadTreeManager.instance.Find(QuadTreeType.Object, Tools.V3ToV2(transform.position), radius, ref list);
        foreach (QuadTreeStat stat in list)
        {
            if (Tools.GetDistance(Tools.V3ToV2(stat.transform.position), Tools.V3ToV2(transform.position)) > radius || stat.player == RoomController.instance.localPlayer)
                continue;
            stat.GetComponent<GameObjectController>().UnderAttack(damage);
        }
    }
}
