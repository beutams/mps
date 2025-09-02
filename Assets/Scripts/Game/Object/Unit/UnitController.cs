using Mirror;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class UnitController : GameObjectController
{
    public static string unitHealthBar = "UnitHealthBar";
    public static string unitMiniMap = "UnitMiniMapItem";
    protected Vector3[] pathPoint;
    protected NavMeshPath path;

    public ORCAAgent orcaAgent;
    protected Vector3 velocity;

    protected Timer followTimer;
    public bool isMove;

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
    public virtual void SetMoveTarget(GameObjectController controller, Vector3 point)
    {
        SetTarget(controller, point);
        if (target != null)
        {
            followTimer.Reset();
            followTimer.Lanuch();
        }
        NavMeshStep();
    }
    public virtual void RefreshTarget()
    {
        if(target != null)
        {
            targetPosition = target.transform.position;
            NavMeshStep();
        }
    }
    private void NavMeshStep()
    {
        if (NavMesh.CalculatePath(new Vector3(position.x, 0, position.z), targetPosition, NavMesh.AllAreas, path))
        {
            pathPoint = path.corners;
        }
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
        if (Vector3.Distance(position, pathPoint[0]) < 0.3f)
        {
            pathPoint = Enumerable.Skip(pathPoint, 1).ToArray();
        }
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
    }
}