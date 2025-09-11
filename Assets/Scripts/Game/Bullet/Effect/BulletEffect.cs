using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletEffect : MonoBehaviour
{
    protected Bullet bullet;
    protected void Start()
    {
        bullet = GetComponent<Bullet>();
        bullet.onStart += OnBulletStart;
        bullet.onCollision += OnBulletCollision;
    }
    protected virtual void OnBulletStart()
    {

    }
    protected virtual void OnBulletCollision(Collision collision)
    {

    }
}
