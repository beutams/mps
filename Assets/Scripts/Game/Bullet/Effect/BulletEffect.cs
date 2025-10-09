using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletEffect : MonoBehaviour
{
    protected Bullet bullet;
    protected void Awake()
    {
        bullet = GetComponent<Bullet>();
        bullet.onStart += OnBulletStart;
        bullet.onTrigger += OnBulletTrigger;
    }
    protected virtual void OnBulletStart()
    {

    }
    protected virtual void OnBulletTrigger(Collider collider)
    {

    }
}
