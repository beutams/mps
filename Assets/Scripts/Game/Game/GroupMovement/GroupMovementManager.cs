using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GroupMovementManager : SingletonMonoBehaviour<GroupMovementManager>
{
    [Header("管理设置")]
    public float gapSize = 0.1f; // 单位间隙大小，0为完全紧贴
    public float targetAreaRadiusMultiplier = 0.3f; // 目标区域半径乘数

    public void CaclTargetPoint(List<UnitController> units,Vector3 targetPosition)
    {
        AssignTargets(units,targetPosition);
    }
    /// <summary>
    /// 分配目标点给所有单位
    /// </summary>
    public void AssignTargets(List<UnitController> units, Vector3 targetPoint)
    {
        if (units.Count == 0) return;
        //计算群体中心
        Vector3 groupCenter = Vector3.zero;
        foreach (var unit in units)
            groupCenter += unit.transform.position;
        groupCenter = groupCenter / units.Count;
        Dictionary<UnitController, UnitTargetData> dirDic = new Dictionary<UnitController, UnitTargetData>();
        foreach (var unit in units)
        {
            Vector3 dirToUnit = unit.transform.position - groupCenter;
            dirDic.Add(unit, new UnitTargetData { originalAngle = Mathf.Atan2(dirToUnit.z, dirToUnit.x) * Mathf.Rad2Deg, originalDistance = dirToUnit.magnitude });
        }

        // 按初始距离从近到远排序
        var sortedUnits = dirDic.OrderBy(u => u.Value.originalDistance);
        // 计算最大距离（用于归一化）
        float maxDistance = sortedUnits.Max(u => u.Value.originalDistance);
        if (maxDistance <= 0) maxDistance = 1;
        // 计算目标区域半径
        float targetAreaRadius = Mathf.Max(2f, units.Count * targetAreaRadiusMultiplier);
        // 分配目标点
        List<Vector3> assignedTargets = new List<Vector3>();
        foreach (var unit in sortedUnits)
        {
            // 计算归一化距离
            float normalizedDistance = unit.Value.originalDistance / maxDistance;
            // 计算距离目标点的距离（近的单位离目标点近）
            float distanceFromTarget = targetAreaRadius * (1 - normalizedDistance);
            // 计算首选位置（保持原始方向）
            float radAngle = unit.Value.originalAngle * Mathf.Deg2Rad;
            Vector3 preferredDir = new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
            Vector3 preferredPosition = targetPoint + preferredDir * distanceFromTarget;
            // 找到最近的有效位置
            Vector3 finalPosition = FindNearestValidPosition(preferredPosition, assignedTargets, unit.Key.stats.radius);
            // 分配目标点
            unit.Key.SetMoveTarget(null,finalPosition);
            assignedTargets.Add(finalPosition);
        }
    }

    /// <summary>
    /// 找到最近的有效位置（不与已有目标点冲突）
    /// </summary>
    private Vector3 FindNearestValidPosition(Vector3 preferredPosition, List<Vector3> assignedTargets, float unitRadius)
    {
        // 检查首选位置是否有效
        if (IsPositionValid(preferredPosition, assignedTargets, unitRadius))
        {
            return preferredPosition;
        }

        // 螺旋式搜索
        float maxRadius = 5f;
        float stepSize = 0.1f;
        int anglesPerCircle = 16;

        for (float r = stepSize; r <= maxRadius; r += stepSize)
        {
            for (int a = 0; a < anglesPerCircle; a++)
            {
                float angle = (a / (float)anglesPerCircle) * Mathf.PI * 2;
                Vector3 offset = new Vector3(
                    Mathf.Cos(angle) * r,
                    0,
                    Mathf.Sin(angle) * r
                );

                Vector3 candidate = preferredPosition + offset;
                if (IsPositionValid(candidate, assignedTargets, unitRadius))
                {
                    return candidate;
                }
            }
        }

        // 如果找不到有效位置，返回首选位置
        return preferredPosition;
    }

    /// <summary>
    /// 检查位置是否有效（不与已有目标点冲突）
    /// </summary>
    private bool IsPositionValid(Vector3 position, List<Vector3> assignedTargets, float unitRadius)
    {
        float requiredDistance = (unitRadius * 2) + gapSize;

        foreach (var target in assignedTargets)
        {
            if (Vector3.Distance(position, target) < requiredDistance)
            {
                return false;
            }
        }
        return true;
    }
}
public struct UnitTargetData
{
    public float originalDistance; // 到群体中心的初始距离
    public float originalAngle;    // 相对于群体中心的初始角度
}