using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AutoFire", menuName = "ScriptableObject/Hero/AutoFire")]
public class AutoFire : Ability
{
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        owner.StartCoroutine(AutoFireCorourine());
    }

    IEnumerator AutoFireCorourine()
    {
        while(RoomController.instance.localPlayer != null)
        {
            while (RoomController.instance.localPlayer.hero == null) yield return null;
            foreach (var item in RoomController.instance.localPlayer.hero.autoFireDic)
            {
                if (RoomController.instance.localPlayer.hero.WeapenCanAutoFire(item.Key))
                {
                    foreach(var weapen in RoomController.instance.localPlayer.hero.weapenGroup[item.Key])
                    {
                        QuadTreeStat target = null;
                        if (weapen.weapen.canIntercept)
                            target = QuadTreeManager.instance.FindNearest(QuadTreeType.Object, Tools.V3ToV2(weapen.transform.position), weapen.weapen.fireDistance, owner.player);
                        if(target == null)
                            target = QuadTreeManager.instance.FindNearest(QuadTreeType.Object,Tools.V3ToV2(weapen.transform.position), weapen.weapen.fireDistance, owner.player);
                        if (target != null && Tools.GetDistance(Tools.V3ToV2(target.position), Tools.V3ToV2(weapen.transform.position)) < owner.stats.searchRadius)
                        {
                            weapen.TurnTowardsMouse(target.transform.position);
                            weapen.weapen.Fire(target, Vector3.zero, weapen);
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
