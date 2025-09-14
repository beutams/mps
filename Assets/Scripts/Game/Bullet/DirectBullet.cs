using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectBullet : Bullet
{
    public override void Move()
    {
        transform.position += new Vector3(Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y), 0, Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y)).normalized * Time.deltaTime * speed;
        speed += data.accelerateSpeed * Time.deltaTime;
    }
}
