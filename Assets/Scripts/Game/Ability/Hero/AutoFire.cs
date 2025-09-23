using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFire : Ability
{
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        owner.StartCoroutine(AutoFireCorourine());
    }

    IEnumerator AutoFireCorourine()
    {
        foreach (var item in RoomController.instance.localPlayer.hero.autoFireDic)
        {
            if (RoomController.instance.localPlayer.hero.WeapenCanAutoFire(item.Key))
            {
                foreach(var weapen in RoomController.instance.localPlayer.hero.weapenGroup[item.Key])
                {
                    GameObjectController target = QuadTreeManager.instance.FindNearest(Tools.V3ToV2(weapen.transform.position), weapen.weapen.fireDistance);
                    weapen.weapen.Fire(target, Vector3.zero, weapen);
                }
            }
            yield return null;
        }
    }
}
