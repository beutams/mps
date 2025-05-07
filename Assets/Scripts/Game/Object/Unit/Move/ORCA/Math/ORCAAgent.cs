using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORCAAgent 
{
    private Player player;
    private UnitController unitController;
    public ORCAAgent(Player player, UnitController unitController)
    {
        neighborAgents = new List<ORCAAgent>();
        neighborObstacles = new List<Obstacle>();
        orcaLines = new List<Line>();
        this.player = player;
        this.unitController = unitController;
    }

    public List<ORCAAgent> neighborAgents;
    public List<Obstacle> neighborObstacles;
    public List<Line> orcaLines;

    //setting
    private float agentTimeHorizon;  //距离到速度尺度的缩放时间，时间越长反应越灵敏，但是速度区域越少
    private float obstacleTimeHorizon;
    private float radius;
    private float maxSpeed;
    //current
    private Vector2 preVelocity;
    private Vector2 position;
    private Vector2 velocity;
    private bool isStop;
    //new
    public Vector2 newVelocity;
    public void Init(float agentTimeHorizon, float obstacleTimeHorizon, float radius, float maxSpeed)
    {
        this.agentTimeHorizon = agentTimeHorizon;
        this.obstacleTimeHorizon = obstacleTimeHorizon;
        this.radius = radius;
        this.maxSpeed = maxSpeed;
    }
    public Vector3 Step(Vector3 position,Vector3 velocity,Vector3 preVelocity,bool isStop)
    {
        this.position = Tools.V3ToV2(position);
        this.velocity = Tools.V3ToV2(velocity);
        this.preVelocity = Tools.V3ToV2(preVelocity);
        this.isStop = isStop;
        if (!isStop)
        {
            CountNeighbors();
            ComputeNewVelocity();
        }
        return Tools.V2ToV3(newVelocity);
    }
    #region Math
    private void CountNeighbors()
    {
/*        neighborObstacles.Clear();
        foreach (var obs in ORCAManager.Instance.allObstacles)
        {
            if (Tools.GetIntersectionPoint(position, position + velocity, obs.point, obs.next.point) != Vector2.zero || Tools.PointToLineDistance(position, obs.point, obs.next.point) < 2f)
                neighborObstacles.Add(obs);
        }*/
        neighborAgents.Clear();
        foreach(var player in RoomController.instance.playerDic.Values)
        {
            foreach (var agent in player.soldierList)
            {
                if (Tools.GetDistance(Tools.V3ToV2(agent.transform.position), position) <= unitController.stats.searchRadius)
                {
                    neighborAgents.Add(agent.orcaAgent);
                }
            }
        }

    }
    public void ComputeNewVelocity()
    {
        orcaLines.Clear();
        float rangeScalingAgent = 1 / agentTimeHorizon; //距离缩放比例
        float rangeScalingObstacle = 1 / obstacleTimeHorizon;
        for(int i = 0; i< neighborObstacles.Count; i++)
        {
            Obstacle obs1 = neighborObstacles[i]; //外侧-左端点
            Obstacle obs2 = obs1.next; //外侧-右端点

            Vector2 relativePosition1 = obs1.point - position;
            Vector2 relativePosition2 = obs2.point - position;

            float relativePostion1Sq = Tools.Pow2(relativePosition1);
            float relativePostion2Sq = Tools.Pow2(relativePosition2);
            float radiusSq = Tools.Pow2(radius);
            Vector2 obsVector = obs2.point - obs1.point;
            float projRP1ToObsVector = Vector2.Dot(-relativePosition1, obsVector);
            float projRP1ToObsVectorPercent = projRP1ToObsVector / Tools.Pow2(obsVector);
            float positionToObsLineDistane = Tools.PointToLineDistance(position, obs1.point, obs2.point);

            Line line;
            #region 已经碰撞
            if(projRP1ToObsVectorPercent < 0f && relativePostion1Sq <= radiusSq) //1顶点碰撞，反方向调整
            {
                if (obs1.convex)
                {
                    line.point = Vector2.zero;
                    line.direction = Tools.RotateRight90(-relativePosition1).normalized;
                    orcaLines.Add(line);
                }
                continue;
            }
            else if(projRP1ToObsVectorPercent > 1f && relativePostion2Sq <= radiusSq) //2顶点碰撞，反方向调整
            {
                if (obs2.convex && Tools.Det(relativePosition2, obs2.direction) >= 0f)//障碍物外向里挤,会被obs2处理
                {
                    line.point = Vector2.zero;
                    line.direction = Tools.RotateRight90(-relativePosition2).normalized;
                    orcaLines.Add(line);
                }
                continue;
            }
            else if(projRP1ToObsVectorPercent > 0f && projRP1ToObsVectorPercent < 1f && positionToObsLineDistane <= radius) //障碍线碰撞
            {
                line.point = Vector2.zero;
                line.direction = obs1.direction;
                orcaLines.Add(line);
                continue;
            }
            #endregion

            #region 没有碰撞
            Vector2 leftLegDirection, rightLegDirection;
            if(projRP1ToObsVectorPercent < 0f && positionToObsLineDistane <= radius) //1外侧，line延长线上，障碍作为1的点存在
            {
                if (!obs1.convex) continue; //非凸，pre处理
                obs2 = obs1;
                float legLength = Mathf.Sqrt(relativePostion1Sq - radiusSq);
                leftLegDirection = new Vector2(relativePosition1.x * legLength - relativePosition1.y * radius, relativePosition1.x * radius + relativePosition1.y * legLength) / relativePostion1Sq;
                rightLegDirection = new Vector2(relativePosition1.x * legLength + relativePosition1.y * radius, -relativePosition1.x * radius + relativePosition1.y * legLength) / relativePostion1Sq;
            }
            else if(projRP1ToObsVectorPercent >1f && positionToObsLineDistane <= radius)
            {
                if (!obs2.convex) continue; //非凸，next处理
                obs1 = obs2;
                float legLength = Mathf.Sqrt(relativePostion2Sq - radiusSq);
                leftLegDirection = new Vector2(relativePosition2.x * legLength - relativePosition2.y * radius, relativePosition2.x * radius + relativePosition2.y * legLength) / relativePostion2Sq;
                rightLegDirection = new Vector2(relativePosition2.x * legLength + relativePosition2.y * radius, -relativePosition2.x * radius + relativePosition2.y * legLength) / relativePostion2Sq;
            }
            else //通常情况，躲避障碍线
            {
                if (obs1.convex)//凸，leg是position+半径角度
                {
                    float legLength = Mathf.Sqrt(relativePostion1Sq - radiusSq);
                    leftLegDirection = new Vector2(relativePosition1.x * legLength - relativePosition1.y * radius, relativePosition1.x * radius + relativePosition1.y * legLength) / relativePostion1Sq;
                }
                else//凹,leg和障碍线平行
                {
                    leftLegDirection = -obs1.direction;
                }

                if (obs2.convex)//凸，leg是position+半径角度
                {
                    float legLength = Mathf.Sqrt(relativePostion2Sq - radiusSq);
                    rightLegDirection = new Vector2(relativePosition2.x * legLength + relativePosition2.y * radius, -relativePosition2.x * radius + relativePosition2.y * legLength) / relativePostion2Sq;
                }
                else//凹，leg和障碍线平行
                {
                    rightLegDirection = obs2.direction;
                }
            }
            Obstacle leftNeighbor = obs1.previous;
            bool isLeftLegForeign = false;
            bool isRightLegForeign = false;
            if(obs1.convex && Tools.Det(leftLegDirection,-leftNeighbor.direction) >= 0f)//如果leg在临边内部，则必须向外投射到临边的切线上
            {
                leftLegDirection = -leftNeighbor.direction;
                isLeftLegForeign = true;
            }
            if (obs2.convex && Tools.Det(rightLegDirection, obs2.direction) <= 0.0f) //同理
            {
                rightLegDirection = obs2.direction;
                isRightLegForeign = true;
            }
            Vector2 toleftPoint_V = (obs1.point - position) * rangeScalingObstacle;
            Vector2 toRightPoint_V = (obs2.point - position) * rangeScalingObstacle;
            Vector2 leftToRight = toRightPoint_V - toleftPoint_V;
            float projLeftWToObs = obs1 == obs2 ? 0.5f : Vector2.Dot((velocity - toleftPoint_V), leftToRight) / Mathf.Abs(leftToRight.magnitude); //W，也就是point点指向速度的向量，在obs上的投影
            float projLeftLeg = Vector2.Dot((velocity - toleftPoint_V), leftLegDirection); //W在Leg上的投影
            float projRightLeg = Vector2.Dot((velocity - toRightPoint_V), rightLegDirection);
            if(projLeftWToObs < 0 &&  projLeftLeg < 0 || (obs1 == obs2 && projLeftLeg < 0 && projRightLeg < 0))//调整到左leg
            {
                Vector2 w = (velocity - toleftPoint_V).normalized;
                line.direction = Tools.RotateRight90(w);
                line.point = toleftPoint_V + radius * rangeScalingObstacle * w;
                orcaLines.Add(line);
                continue;
            }
            else if(projLeftWToObs > 1 && projRightLeg < 0)//调整到右leg
            {
                Vector2 w = (velocity - toRightPoint_V).normalized;
                line.direction = Tools.RotateRight90(w);
                line.point = toRightPoint_V + radius * rangeScalingObstacle * w;
                orcaLines.Add(line);
                continue;
            }
            float distSqVtoObs = (projLeftWToObs < 0.0f || projLeftWToObs > 1.0f || obs1 == obs2) ? float.PositiveInfinity : Tools.Pow2(velocity - (toleftPoint_V + projLeftWToObs * leftToRight));
            float distSqLeft = projLeftLeg < 0 ? float.PositiveInfinity : Tools.Pow2(velocity - (toleftPoint_V + projLeftLeg * leftLegDirection));
            float distSqRight = projRightLeg < 0 ? float.PositiveInfinity : Tools.Pow2(velocity - (toRightPoint_V + projRightLeg * rightLegDirection));

            if(distSqVtoObs <= distSqLeft && distSqVtoObs <= distSqRight)
            {
                line.direction = -obs1.direction;
                line.point = toleftPoint_V + radius * rangeScalingObstacle * Tools.RotateLeft90(line.direction);
                orcaLines.Add(line);
                continue;
            }
            if(distSqLeft <= distSqRight)
            {
                if(isLeftLegForeign) continue;
                line.direction = leftLegDirection;
                line.point = toleftPoint_V + radius * rangeScalingObstacle * Tools.RotateLeft90(line.direction);
                orcaLines.Add(line);
                continue;
            }
            else
            {
                if (isRightLegForeign) continue;
                line.direction = -rightLegDirection;
                line.point = toRightPoint_V + radius * rangeScalingObstacle * Tools.RotateLeft90(line.direction);
                orcaLines.Add(line);
                continue;
            }
            #endregion
        }
        int numObstLines = orcaLines.Count;
        for (int i = 0; i < neighborAgents.Count; i++)
        {
            ORCAAgent other = neighborAgents[i];
            Line line;
            Vector2 u;

            Vector2 relativePosition_V = (other.position - position) * rangeScalingAgent;  //this -> other 相对位移
            float combinedRadius_V = (radius + other.radius) * rangeScalingAgent;
            Vector2 relativeVelocity = velocity - other.velocity;  //other不动下的this速度

            float relativePositionSq = Tools.Pow2(relativePosition_V);
            float combinedRadiusSq = Tools.Pow2(combinedRadius_V);

            if(relativePositionSq > combinedRadiusSq) //还没碰撞
            {
                Vector2 w = relativeVelocity - relativePosition_V;//other -> V
                float wpAngle = Vector2.Angle(-relativePosition_V, w);
                float cutoffAngle = Tools.CosRectToAngle(combinedRadius_V, relativePosition_V.magnitude) * 180 / Mathf.PI;
                if(wpAngle < cutoffAngle)//cutoff圆调整
                {
                    line.direction = Tools.RotateRight90(w.normalized);
                    u = w.normalized * combinedRadius_V - w;
                }
                else
                {
                    bool rightLeg = Tools.Det(w,relativePosition_V) > 0;
                    float leg = Mathf.Sqrt(relativePositionSq - combinedRadiusSq);
                    if(!rightLeg)//direction方向与leg相同,指向原点
                    {
                        //两角和公式 左腿
                        line.direction = new Vector2(relativePosition_V.x * leg - relativePosition_V.y * combinedRadius_V, relativePosition_V.x * combinedRadius_V + relativePosition_V.y * leg) / relativePositionSq;
                    }
                    else
                    {
                        //两角差公式 右腿
                        line.direction = -new Vector2(relativePosition_V.x * leg + relativePosition_V.y * combinedRadius_V, -relativePosition_V.x * combinedRadius_V + relativePosition_V.y * leg) / relativePositionSq;
                    }
                    float VToDirProjection = Vector2.Dot(relativePosition_V, line.direction);
                    u = VToDirProjection * line.direction - relativeVelocity;
                }
            }
            else //已经碰撞
            {
                Vector2 w = relativeVelocity - relativePosition_V * agentTimeHorizon * agentTimeHorizon;
                line.direction = Tools.RotateRight90(w.normalized);
                u = (combinedRadius_V * agentTimeHorizon * agentTimeHorizon - w.magnitude) * w.normalized;
                /*                Vector2 w = relativeVelocity - relativePosition_V * agentTimeHorizon;
                                line.direction = Tools.RotateRight90(-relativePosition_V);
                                u = - relativePosition_V.normalized * radius * agentTimeHorizon - w;*/
            }
            line.point = velocity + (other.isStop ? 1f : 0.5f) * u;
            orcaLines.Add(line);
        }
        int lineFail = AdjustSpeed(orcaLines, maxSpeed, preVelocity, false, ref newVelocity);
        if (lineFail < orcaLines.Count)
            FailedAdjust(orcaLines, numObstLines, lineFail, maxSpeed, ref newVelocity);
    }
    private int AdjustSpeed(List<Line> lines, float radius, Vector2 preVelocity ,bool shouldAdjust, ref Vector2 result)
    {
        if (shouldAdjust)
            result = preVelocity * radius;
        else if (Tools.Pow2(preVelocity) > Tools.Pow2(radius))
            result = preVelocity.normalized * radius;
        else
            result = preVelocity;
        for(int i = 0; i < lines.Count; i++)
        {
            if (Tools.Det(lines[i].direction, lines[i].point - result) > 0f)//速度在半平面外，需要调整
            {
                Vector2 tempResult = result;
                if(!AdjustSpeedInner(lines, i, radius, preVelocity, shouldAdjust,ref result))
                {
                    result = tempResult;

                    return i;//失败返回初速度和失败的line
                }
            }
        }
        return lines.Count;//成功返回调整的速度和lineCount
    }
    private bool AdjustSpeedInner(List<Line> lines, int lineNo, float radius, Vector2 preVelocity, bool shouldAdjust, ref Vector2 result)
    {
        float rectLine = Vector2.Dot(lines[lineNo].point, lines[lineNo].direction); //垂线-point边组成的直角三角形，垂点到point的直角边
        float rectRadiuSq = Tools.Pow2(radius) - Tools.Pow2(lines[lineNo].point) + Tools.Pow2(rectLine); //radius与direction交点 到 垂点的距离

        if(rectRadiuSq < 0f)
            return false; //半径没有交半平面，没有可用速度
        float rectRadiu = Mathf.Sqrt(rectRadiuSq);
        float tLeft = - rectRadiu - rectLine; //point到左交点的偏移 left->right 沿着direction方向
        float tRight = rectRadiu - rectLine; //point到右交点的偏移

        for (int i =0; i < lineNo; i++)
        {
            /* 求两直线交点I 向量表示 (点p1,方向v1 点p2,方向v2) I表示为p1+tv1 = I
             * I与p2的直线与v2平行 (I - p2) * v2 = 0
             * (p1 + tv1 - p2) * v2 = 0    */
            float denominator = Tools.Det(lines[lineNo].direction, lines[i].direction); //方向向量叉乘
            float numerator = Tools.Det(lines[i].direction, lines[lineNo].point - lines[i].point);
            if(Mathf.Abs(denominator) <= 0.000001f)
            {
                if (numerator < 0f)
                    return false;
                continue;
            }
            float t = numerator / denominator;
            if (denominator >= 0.0f) //i在左,朝向左，交点右边舍去
                tRight = Math.Min(tRight, t);
            else//i在右边朝向左，交点左边舍去
                tLeft = Math.Max(tLeft, t);
            if(tLeft > tRight) 
                return false; //两条line没有公共可用区域
        }
        if (shouldAdjust)
        {
            if (Vector2.Dot(preVelocity,lines[lineNo].direction) > 0.0f)
            {
                result = lines[lineNo].point + tRight * lines[lineNo].direction;
            }
            else
            {
                result = lines[lineNo].point + tLeft * lines[lineNo].direction;
            }
        }
        else
        {
            float t = Vector2.Dot(lines[lineNo].direction, (preVelocity - lines[lineNo].point)); //preVelocity在direction的投影，到point的偏移
            if (t < tLeft)//调整到半平面线上
                result = lines[lineNo].point + tLeft * lines[lineNo].direction;
            else if (t > tRight)
                result = lines[lineNo].point + tRight * lines[lineNo].direction;
            else
                result = lines[lineNo].point + t * lines[lineNo].direction;
        }
        return true;
    }
    private void FailedAdjust(List<Line> lines, int numObsLines, int beginLine, float radius, ref Vector2 result)
    {
        float distance = 0f;
        for(int i = beginLine; i < lines.Count; i++)//从失败的线开始 往后遍历
        {
            if (Tools.Det(lines[i].direction, lines[i].point - result) > distance) //后面有线不满足
            {
                List<Line> projLines = new List<Line>();
                //todo: 加入静态障碍线
                for (int ii = 0; ii < numObsLines; ++ii)
                {
                    projLines.Add(lines[ii]);
                }
                for(int j = numObsLines;j < i; j++)
                {
                    Line line;
                    float determinant = Tools.Det(lines[i].direction, lines[j].direction);
                    if(Mathf.Abs(determinant) < 0.00001f)//平行
                    {
                        if (Vector2.Dot(lines[i].direction, lines[j].direction) > 0.0f)
                            continue;
                        else
                            line.point = 0.5f * (lines[i].point + lines[j].point);
                    }
                    else
                    {
                        line.point = lines[i].point + Tools.Det(lines[j].direction, lines[i].point - lines[j].point) / determinant * lines[i].direction;//角平分线交点
                    }
                    line.direction = (lines[j].direction - lines[i].direction).normalized;
                    projLines.Add(line);
                }
                Vector2 tempResult = result;
                if (AdjustSpeed(projLines, radius, Tools.RotateLeft90(lines[i].direction),true,ref result) < projLines.Count)
                {
                    result = tempResult;
                }
                distance = Tools.Det(lines[i].direction, lines[i].point - result);
            }
        }
    }
    #endregion
}
#region Help
public struct Line
{
    public Vector2 point;
    public Vector2 direction;
}
public class Obstacle
{
    public Obstacle next;
    public Obstacle previous;
    public Vector2 direction;
    public Vector2 point;
    public bool convex;
}
#endregion