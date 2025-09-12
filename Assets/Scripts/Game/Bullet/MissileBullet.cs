using UnityEngine;

public class MissileBullet : Bullet
{
    public override void Move()
    {
        if (target != null)
        {
            Vector3 cos = Vector3.Dot(transform.forward, target.transform.position) * (target.transform.position - transform.position).normalized;
            Vector3 sin = transform.forward - cos;
            cos = Mathf.Clamp(cos.magnitude + data.turnSpeed * Time.deltaTime, 0, speed) * cos.normalized;
            sin = Mathf.Clamp(sin.magnitude - data.turnSpeed * Time.deltaTime, 0, speed) * sin.normalized;
            transform.forward = (cos + sin).normalized;
        }
    }
}