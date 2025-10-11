using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AutoFire", menuName = "ScriptableObject/Hero/AutoFire")]
public class AutoFire : Ability
{
    [Header("预判设置")]
    public float predictionTime = 0.5f; // 预判时间（秒）
    
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        owner.StartCoroutine(AutoFireCorourine());
    }

    IEnumerator AutoFireCorourine()
    {
        while(RoomController.instance.localPlayer != null)
        {
            while (RoomController.instance.localPlayer.hero == null) yield return null;
            foreach (var item in RoomController.instance.localPlayer.hero.autoFireDic)
            {
                if (RoomController.instance.localPlayer.hero.WeapenCanAutoFire(item.Key))
                {
                    foreach(var weapen in RoomController.instance.localPlayer.hero.weapenGroup[item.Key])
                    {
                        QuadTreeStat target = null;
                        if (weapen.weapen.canIntercept)
                            target = QuadTreeManager.instance.FindNearest(QuadTreeType.Object, Tools.V3ToV2(weapen.transform.position), weapen.weapen.fireDistance, owner.player);
                        if(target == null)
                            target = QuadTreeManager.instance.FindNearest(QuadTreeType.Object,Tools.V3ToV2(weapen.transform.position), weapen.weapen.fireDistance, owner.player);
                        if (target != null && Tools.GetDistance(Tools.V3ToV2(target.position), Tools.V3ToV2(weapen.transform.position)) < owner.stats.searchRadius)
                        {
                            // 计算预判位置
                            Vector3 predictedPosition = CalculatePredictedPosition(target, weapen);
                            
                            weapen.TurnTowardsMouse(predictedPosition);
                            weapen.weapen.Fire(target, predictedPosition, weapen);
                        }
                    }
                }
            }
            yield return null;
        }
    }
    
    /// <summary>
    /// 计算目标的预判位置（简化版）
    /// </summary>
    private Vector3 CalculatePredictedPosition(QuadTreeStat target, WeapenModel weapen)
    {
        // 尝试获取目标的速度
        Vector3 velocity = target.GetComponent<UnitController>().velocity;
        // 如果速度太小，不进行预判
        if (velocity.magnitude < 0.5f)
        {
            return target.position;
        }
        
        // 计算子弹飞行时间
        float distance = Vector3.Distance(weapen.transform.position, target.position);
        float bulletSpeed = weapen.weapen.bullet != null ? 
            GameEntry.ObjectPoolComponent.Get(weapen.weapen.bullet).GetComponent<Bullet>().data.startSpeed : 20f;
        
        float flightTime = distance / bulletSpeed;
        
        // 限制预判时间范围
        flightTime = Mathf.Clamp(flightTime, 0.1f, predictionTime);
        
        // 计算预判位置（假设目标匀速运动）
        Vector3 predictedPosition = target.position + velocity * flightTime;
        
        return predictedPosition;
    }
}
