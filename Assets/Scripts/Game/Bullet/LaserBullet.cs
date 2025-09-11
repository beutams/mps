using UnityEngine;

public class LaserBullet : Bullet
{
    public float maxDistance;

    protected bool first;
    public override void Move()
    {
        if (!first)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                if (hit.collider != null)
                {
                    transform.position = hit.point;
                }
                else
                {
                    transform.position = transform.position + transform.forward * maxDistance;
                }
            }
            first = true;
        }
    }
}