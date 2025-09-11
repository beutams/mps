using Mirror;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class UnitController : GameObjectController
{
    public static string unitHealthBar = "UnitHealthBar";
    public static string unitMiniMap = "UnitMiniMapItem";
    protected Vector3[] pathPoint;
    protected Vector3[] conerPoints;
    protected NavMeshPath path;

    public ORCAAgent orcaAgent;
    protected Vector3 velocity;

    protected Timer followTimer;
    public bool isMove;

    protected float r;

    public float cornerAngleThreshold = 120f;
    protected Vector3 position => transform.position;
    public UnitStats unitStats => stats as UnitStats;
    protected override void OnObjectSpawn(Player player)
    {
        base.OnObjectSpawn(player);
        UIManager.instance.AddHealthBar(this, unitHealthBar);
        UIManager.instance.AddMiniMapItem(this, unitMiniMap);
    }
    protected override void OnObjectDead()
    {
        base.OnObjectDead();
        UIManager.instance.RemoveHealthBar(this);
        UIManager.instance.RemoveMiniMapItem(this);
    }
    protected override void Awake()
    {
        base.Awake();
        path = new NavMeshPath();
        followTimer = new Timer();
        followTimer.Init(1f, RefreshTarget, true,true);
        TimerManager.instance.AddTimer(followTimer);
        isMove = true;
    }
    #region Behaviour
    protected override void Update()
    {
        base.Update();
        if (!RoomController.instance.gameReady) return;
        if (isMove)
        {
            ORCAStep();
            NavMeshStep();
            DoMove();
            EndMove();
        }
    }
    #endregion

    #region Move
    public virtual void Stand()
    {
        followTimer.Pause();
        followTimer.Reset();
    }
    public virtual void SetMoveTarget(GameObjectController controller, Vector3 point, float r = 0.5f)
    {
        SetTarget(controller, point);
        this.r = r;
        if (target != null)
        {
            followTimer.Reset();
            followTimer.Lanuch();
        }
        NavMeshSet();
    }
    public virtual void RefreshTarget()
    {
        if(target != null)
        {
            targetPosition = target.transform.position;
            NavMeshSet();
        }
    }
    private void NavMeshSet()
    {
        if (NavMesh.CalculatePath(new Vector3(position.x, 0, position.z), targetPosition, NavMesh.AllAreas, path))
        {
            pathPoint = path.corners;
            if(path.corners.Length >= 3)
                conerPoints = path.corners.Take(3).ToArray();
            else
                conerPoints = null;
        }
    }
    private void NavMeshStep()
    {
        List<int> cornerIndices = new List<int>();
        if (conerPoints == null) return; // 路径点太少，无法形成拐角
        // 1. 计算拐角的“前后区域”分界线（垂直于拐角的角平分线）
        Vector3 dirToCorner = (conerPoints[1] - conerPoints[0]).normalized;
        Vector3 dirFromCorner = (conerPoints[2] - conerPoints[1]).normalized;
        Vector3 cornerBisector = (dirToCorner + dirFromCorner).normalized; // 角平分线方向
        Vector3 boundaryNormal = Vector3.Cross(cornerBisector, Vector3.up).normalized; // 分界线法线

        // 2. 计算单位和“拐角前点”在分界线的哪一侧
        // 单位需要移动到与“拐角前点”相反的一侧，才算绕过
        float prevPointSide = Vector3.Dot(conerPoints[0] - conerPoints[1], boundaryNormal);
        float unitSide = Vector3.Dot(transform.position - conerPoints[1], boundaryNormal);

        // 3. 额外检查：单位需远离拐角超过一定距离（避免刚过线就被挤回）
        float distanceToCorner = Vector3.Distance(transform.position, conerPoints[1]);
        bool isFarEnough = distanceToCorner > stats.radius * 0.5f;

        // 两侧不同且距离足够，视为已绕过
        if ((Mathf.Sign(unitSide) != Mathf.Sign(prevPointSide)) && isFarEnough)
            EndMoveInner();

    }
    private void ORCAStep()
    {
        if (pathPoint == null || pathPoint.Length <= 0) return;
        velocity = orcaAgent.Step(position, velocity, (pathPoint[0] - position).normalized * UnitStats.speed, !isMove);
    }

    private void DoMove()
    {
        if (pathPoint == null ||pathPoint.Length <= 0) return;
        Vector3 turnForward = Vector3.RotateTowards(transform.forward, velocity, unitStats.rotateForce * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(turnForward);
        transform.position += velocity * Time.deltaTime;
    }
    private void EndMove()
    {
        if (pathPoint == null || pathPoint.Length <= 0) return;
        if (Vector3.Distance(position, pathPoint[0]) < r)
        {
            EndMoveInner();
        }
    }
    private void EndMoveInner()
    {
        if (pathPoint.Length >= 3)
            conerPoints = pathPoint.Take(3).ToArray();
        else
            conerPoints = null;
        pathPoint = Enumerable.Skip(pathPoint, 1).ToArray();
    }
    #endregion

    #region Init
    public override void PlayerInit(Player player)
    {
        base.PlayerInit(player);
        ORCAInit();
    }
    private void ORCAInit()
    {
        orcaAgent = new ORCAAgent(player, this);
        orcaAgent.Init(UnitStats.timeHorizon, UnitStats.obsTimeHorizon, unitStats.radius, UnitStats.speed);
    }

    protected override void Logout()
    {
        player.unitList.Remove(this);
    }
    protected override void SpawnInit()
    {
        base.SpawnInit();
        CheckPopulation();
    }
    protected virtual void CheckPopulation()
    {
        if(this is not HeroController)
        {
            if (RoomController.instance.localPlayer.population >= GameEntry.SettingComponent.settingData.maxPopulation)
                GameEntry.ObjectPoolComponent.Release(gameObject);
            else
                RoomController.instance.localPlayer.population++;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (pathPoint == null || pathPoint.Count() == 0) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, pathPoint[0]);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        orcaAgent.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, stats.radius);
    }
}