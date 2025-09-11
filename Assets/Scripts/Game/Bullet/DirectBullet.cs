using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectBullet : Bullet
{
    public override void Move()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        speed += data.accelerateSpeed * Time.deltaTime;
    }
}
