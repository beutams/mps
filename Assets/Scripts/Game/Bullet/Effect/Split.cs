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
            Vector3 originalFlyDirection = bullet.GetFlyDirection();
            Quaternion newRotation = Quaternion.LookRotation(Vector3.up, Quaternion.Euler(0, Random.Range(-splitAngle, splitAngle), 0) * originalFlyDirection);
            sub.Init(position, newRotation, bullet.target, bullet.player,false);
        }
        GameEntry.ObjectPoolComponent.Release(gameObject);
    }
}
