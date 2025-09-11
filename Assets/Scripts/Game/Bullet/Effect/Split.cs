using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : BulletEffect
{
    public string subBullet;
    public float splitNumber;
    public float splitAngle;
    public float splitDistance;
    protected override void OnBulletStart()
    {
        for(int i = 0; i < splitNumber; i++)
        {
            Bullet sub = GameEntry.ObjectPoolComponent.Get(subBullet).GetComponent<Bullet>();
            Vector3 position = bullet.transform.position + new Vector3(Random.Range(-splitDistance, splitDistance), Random.Range(-splitDistance, splitDistance), Random.Range(-splitDistance, splitDistance));
            Vector3 direction = bullet.transform.forward + new Vector3(0, Random.Range(-splitAngle, splitAngle), 0);
            sub.Init(position, Quaternion.Euler(direction), bullet.target, bullet.player);
        }

    }
}
