using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectBullet : Bullet
{
    private bool hasLoggedDirection = false;
    
    public override void Move()
    {
        // 第一次移动时记录方向信息
        if (!hasLoggedDirection)
        {
            Debug.Log($"DirectBullet {name} 开始移动:");
            Debug.Log($"  位置: {transform.position}");
            Debug.Log($"  旋转: {transform.rotation.eulerAngles}");
            Debug.Log($"  Up方向(飞行方向): {transform.up}");
            Debug.Log($"  Forward方向: {transform.forward}");
            Debug.Log($"  速度: {speed}");
            hasLoggedDirection = true;
        }
        
        // 使用capsule上方作为飞行方向移动
        Vector3 moveDirection = GetFlyDirection();
        Vector3 movement = moveDirection * Time.deltaTime * speed;
        transform.position += movement;
        
        // 更新速度
        speed += data.accelerateSpeed * Time.deltaTime;
    }
    
    // 重置标志，用于对象池回收
    private void OnEnable()
    {
        hasLoggedDirection = false;
    }
}