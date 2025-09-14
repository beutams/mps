using System;
using UnityEngine;

public class WeapenModel : MonoBehaviour
{
    public WeapenBase weapen;
    public float directionAngle;
    public float includedAngle;

    protected Quaternion startQuaternion;
    public int group { get; set; }
    private void Awake()
    {
        startQuaternion = transform.localRotation;
    }

    private void Update()
    {
        if (weapen == null) return;
        Turn();
    }
    public void Turn()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float t = -mouseRay.origin.y / mouseRay.direction.y;
        Vector3 targetVector = mouseRay.origin + mouseRay.direction * t;
        Quaternion targetRotation = Quaternion.LookRotation(targetVector - transform.position);
        Quaternion constrainedRotation = ApplyRotationLimits(targetRotation);
        float difference = Quaternion.Angle(transform.rotation, constrainedRotation);
        if(difference < 1f) 
            transform.rotation = constrainedRotation * startQuaternion;
        else
            transform.rotation = Quaternion.Slerp(transform.rotation * Quaternion.Inverse(startQuaternion), constrainedRotation, weapen.turnSpeed * Time.deltaTime) * startQuaternion;

        this.targetVector = targetVector;
    }

    private Quaternion ApplyRotationLimits(Quaternion targetRotation)
    {
        // 提取目标旋转的Y轴角度（绕Y轴旋转角度）
        float targetYAngle = targetRotation.eulerAngles.y;

        // 计算目标角度与初始角度的差值（获取最短路径角度）
        float angleDiff = Mathf.DeltaAngle(directionAngle, targetYAngle);

        // 限制角度差在允许范围内
        float constrainedDiff = Mathf.Clamp(angleDiff, -includedAngle/2, includedAngle/2);

        // 计算受限制的最终Y角度
        float finalYAngle = directionAngle + constrainedDiff;

        // 构建仅绕Y轴的受限旋转
        return Quaternion.Euler(0, finalYAngle, 0);
    }

    protected Vector3 targetVector;
    private void OnDrawGizmos()
    {
        if (weapen == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetVector);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.rotation * Vector3.forward).normalized * 100 + transform.position);
        Gizmos.DrawLine(transform.position, ((RoomController.instance.localPlayer.hero.transform.rotation * Quaternion.Euler(0, -includedAngle / 2, 0)) * Vector3.forward).normalized * 100 + transform.position);
        Gizmos.DrawLine(transform.position, ((RoomController.instance.localPlayer.hero.transform.rotation * Quaternion.Euler(0, includedAngle / 2, 0)) * Vector3.forward).normalized * 100 + transform.position);
    }
}