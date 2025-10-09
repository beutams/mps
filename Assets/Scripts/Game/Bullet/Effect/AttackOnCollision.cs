using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnCollision : BulletEffect
{
    public int damage;
    protected override void OnBulletTrigger(Collider collider)
    {
        if(collider.transform.TryGetComponent(out GameObjectController controller))
        {
            if(controller.player != RoomController.instance.localPlayer)
            {
                controller.UnderAttack(damage);
            }
        }
    }
}
