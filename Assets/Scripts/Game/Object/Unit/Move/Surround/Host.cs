using System;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour
{
    private Vector3 position;
    private float radius;
    public Dictionary<Attacker,Vector3> attackList = new Dictionary<Attacker, Vector3>();
    public Vector3 GenerateAttackPoint(Attacker attacker, Vector3 attackerPosition)
    {
        float r = radius + attacker.attackDistance + attacker.radius;
        Vector3 direction = Vector3.Normalize(attackerPosition - position);
        Vector3 originalPoint = position + direction * r;
        Queue<Vector3> pointList = new Queue<Vector3>();
        Dictionary<Vector3, int> directionDic = new Dictionary<Vector3, int>();
        if (CheckPointExistUnit(originalPoint,out Attacker existAttacker))
        {
            if (existAttacker != null)
            {
                Vector3 rightPoint = GetNextPoint(attackList[existAttacker], existAttacker.radius + radius, 1);
                Vector3 leftPoint = GetNextPoint(attackList[existAttacker], existAttacker.radius + radius, -1);
                pointList.Enqueue(rightPoint);
                pointList.Enqueue(leftPoint);
                directionDic.Add(rightPoint, 1);
                directionDic.Add(leftPoint, -1);
            }
        }
        else
        {
            return originalPoint;
        }
        float rightAngle = 0;
        float leftAngle = 0;
        bool fail = false;
        while (pointList.Count >0 && CheckPointExistUnit(pointList.Peek(),out Attacker currentAttacker))
        {
            if(currentAttacker != null)
            {
                Vector3 nextPoint = GetNextPoint(attackList[currentAttacker], currentAttacker.radius + radius, directionDic[pointList.Peek()]);
                pointList.Enqueue(nextPoint);
                directionDic.Add(nextPoint, directionDic[pointList.Peek()]);
                directionDic.Remove(pointList.Dequeue());
            }
            if (leftAngle + rightAngle > 360)
            {
                fail = true;
                break;
            }
        }
        if (fail)
            return Vector3.zero;
        else
            return pointList.Peek();
    }
    public bool CheckPointExistUnit(Vector3 originalPoint,out Attacker attacker)
    {
        attacker = null;
        return true;
    }
    public Vector3 GetNextPoint(Vector3 position, float d, int direction)
    {
        return Vector3.zero;
    }
}
