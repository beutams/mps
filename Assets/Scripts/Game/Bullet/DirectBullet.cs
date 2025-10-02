using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectBullet : Bullet
{
    private bool hasLoggedDirection = false;
    
    public override void Move()
    {
        // 使用capsule上方作为飞行方向移动
        Vector3 moveDirection = GetFlyDirection();
        Vector3 movement = moveDirection * Time.deltaTime * speed;
        transform.position += movement;
        
        // 更新速度
        speed += data.accelerateSpeed * Time.deltaTime;
    }
}