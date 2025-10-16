using Mirror;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class GameObjectController : NetworkBehaviour
{
    public GameObjectStats stats;
    public GameObjectAnimator animator;
    protected bool isSelect;
    protected bool isDead;
    #region 设定

    #endregion

    #region 字段
    protected float currentHealth;
    #endregion

    #region 属性
    public List<Ability> abilities { get; protected set; }
    public GameObjectEvents events { get; protected set; }
    public GameObjectStatus status { get; protected set; }
    public Player player { get; set; }
    public GameObjectController target { get; protected set; }
    public Vector3 targetPosition { get; protected set; }
    public float attackDistance { get; protected set; }
    public QuadTreeStat quadTreeStat { get; protected set; }
    #endregion

    #region 初始化
    protected virtual void Awake()
    {
        InitEvents();
    }
    protected void InitQuadTree()
    {
        quadTreeStat = GetComponent<QuadTreeStat>();
        quadTreeStat.radius = stats.radius;
        quadTreeStat.player = player;
        QuadTreeManager.instance.Insert(QuadTreeType.Object, quadTreeStat);
    }
    protected virtual void InitStats()
    {
        status = GetComponent<GameObjectStatus>();
        animator = GetComponent<GameObjectAnimator>();
        currentHealth = stats.maxHealth;
        isDead = false;
    }
    protected virtual void InitEvents()
    {
        events = new GameObjectEvents();
        events.onSpawn.AddListener(OnObjectSpawn);
        events.onDead.AddListener(OnObjectDead);
    }
    public virtual void InitAbility()
    {
        abilities = new List<Ability>();
        for (int i = 0; i < stats.abilities.Count; i++) 
        {
            abilities.Add(Instantiate(stats.abilities[i]));
            abilities[i].Init(this);
            if (abilities[i].CanDo())
                abilities[i].Do();
        }
    }
    public virtual void PlayerInit(Player player)
    {
        this.player = player;
        isSelect = false;
        player.AddObject(this);
    }
    #endregion

    #region 更新
    protected virtual void Update()
    {
        if (!RoomController.instance.gameReady) return;
        if (target != null)
            OnFindObject(target);
        if (targetPosition != Vector3.zero)
            OnFindVector3(targetPosition);
    }
    #endregion

    #region 生成/销毁
    protected virtual void OnObjectSpawn(Player player)
    {
        PlayerInit(player);
        InitStats();
        InitAbility();
        InitQuadTree();
        SpawnInit();
    }
    protected virtual void OnObjectDead()
    {
        StopAbility();
        Logout();
        QuadTreeManager.instance.Delete(QuadTreeType.Object,quadTreeStat);
    }
    protected virtual void SpawnInit()
    {
        foreach(var ability in abilities)
        {
            if (ability.CanDo())
            {
                ability.Do();
            }
        }
    }
    protected virtual void StopAbility()
    {
        if (abilities != null && abilities.Count > 0)
        {
            foreach (var ability in abilities)
            {
                ability.OnAbilityDestroy();
            }
        }
    }
    protected abstract void Logout();
    #endregion

    #region Ability
    public virtual void OnFindVector3(Vector3 target)
    {
        foreach(var ability in abilities)
        {
            if(ability.CanDo(target))
            {
                ability.Do(target);
            }
        }
    }
    public virtual void OnFindObject(GameObjectController target)
    {
        foreach (var ability in abilities)
        {
            if (ability.CanDo(target))
            {
                ability.Do(target);
            }
        }
    }
    public virtual void SetAttackDistance(float distance)
    {
        attackDistance = Mathf.Max(distance, attackDistance);
    }
    #endregion

    #region 功能
    public virtual void SetTarget(GameObjectController controller, Vector3 point)
    {
        target = controller;
        targetPosition = point;
    }
    public virtual void GetNearestTarget()
    {
        if (this is HeroController) return;
        GameObjectController result = null;
        Player nocamp = RoomController.instance.noCampPlayer;
        if (nocamp.unitList.Count > 0)
        {
            GameObjectController min = Tools.GetNearestGameObject(nocamp.unitList.ToArray(), this) as UnitController;
            if (min != null && Tools.GetDistance(min.transform.position, transform.position) < stats.searchRadius)
                result = min;
        }
        if (nocamp.constructionList.Count > 0)
        {
            GameObjectController min = Tools.GetNearestGameObject(nocamp.unitList.ToArray(), this) as ConstructionController;
            if (min != null && Tools.GetDistance(min.transform.position, transform.position) < stats.searchRadius)
            {
                if (result == null)
                    result = min;
                else
                    result = Tools.GetDistance(result.transform.position, transform.position) > Tools.GetDistance(min.transform.position, transform.position) ? min : result;
            }
        }
        target = result;
    }
    [Command(requiresAuthority = false)]
    public virtual void UnderAttackServer(float damage)
    {
        if (!authority) return;
        UnderAttackClient(damage);
    }
    [ClientRpc]
    public virtual void UnderAttackClient(float damage)
    {
        if (currentHealth > 0)
            currentHealth -= damage - stats.defense > 0 ? 1 : damage - stats.defense;
        if (currentHealth <= 0 && !isDead)
        {
            if (authority)
            {
                GameEntry.ObjectPoolComponent.Release(gameObject);
                isDead = true;
            }

        }
    }
    public virtual float GetHealth()
    {
        return currentHealth;
    }
    #endregion

    #region 切换
    public virtual bool CanAttack()
    {
        if (stats.canAttack)
        {
            if (target == null) return false;
            if (Tools.GetDistance(transform.position, target.transform.position) < attackDistance)
            {
                return status.Change<GameObjectAttackStatu>();
            }
        }
        return false;
    }
    public virtual bool CanMove()
    {
        if (stats.canMove)
        {
            if (targetPosition != Vector3.zero && Tools.GetDistance(transform.position, targetPosition) > 0.3f)
            {
                return status.Change<UnitMoveStatu>();
            }
            if (target != null && Tools.GetDistance(transform.position, target.transform.position) > attackDistance)
            {
                return status.Change<UnitMoveStatu>();
            }
        }
        return false;
    }
    public virtual bool CanStay()
    {
        if (targetPosition != Vector3.zero && target == null)
        {
            // 对于UnitController，使用其moveTargetRadius作为判断距离
            float checkDistance = 0.3f;
            UnitController unitController = this as UnitController;
            if (unitController != null)
            {
                checkDistance = unitController.moveTargetRadius;
            }
            
            if (Tools.GetDistance(transform.position, targetPosition) < checkDistance)
            {
                return status.Change<GameObjectStayStatu>();
            }
        }
        return false;
    }
    #endregion
}