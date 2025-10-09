using UnityEngine;

public class MissileBullet : Bullet
{
    public override void Move()
    {
        if (target != null)
        {
            // 计算朝向目标的方向（水平面）
            Vector3 directionToTarget = target.transform.position - transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            
            if (directionToTarget.sqrMagnitude > 0.001f) // 避免除零
            {
                Vector3 currentDirection = GetFlyDirection();
                Vector3 newDirection;
                
                // 距离自适应转向：距离越近，转向速度越快
                float closeRangeBoost = 1f;
                float closeRange = 5f; // 近距离阈值
                float veryCloseRange = 2f; // 极近距离阈值
                
                if (distanceToTarget < veryCloseRange)
                {
                    // 极近距离：直接瞄准目标，忽略转向限制
                    newDirection = directionToTarget.normalized;
                }
                else if (distanceToTarget < closeRange)
                {
                    // 近距离：增加转向速度（距离越近，加成越大）
                    closeRangeBoost = Mathf.Lerp(3f, 1f, distanceToTarget / closeRange);
                    newDirection = Vector3.RotateTowards(
                        currentDirection,
                        directionToTarget.normalized,
                        data.turnSpeed * closeRangeBoost * Time.deltaTime,
                        0f
                    );
                }
                else
                {
                    // 正常距离：使用标准转向速度
                    newDirection = Vector3.RotateTowards(
                        currentDirection,
                        directionToTarget.normalized,
                        data.turnSpeed * Time.deltaTime,
                        0f
                    );
                }
                
                // 根据新方向更新旋转（capsule的up方向指向飞行方向）
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(newDirection, transform.right), newDirection);
            }
        }
        // 使用当前飞行方向移动
        Vector3 moveDirection = GetFlyDirection();
        transform.position += moveDirection * Time.deltaTime * speed;

        // 更新速度（加速）
        speed += data.accelerateSpeed * Time.deltaTime;
    }
}