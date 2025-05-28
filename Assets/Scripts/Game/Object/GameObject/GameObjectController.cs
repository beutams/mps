using Mirror;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class GameObjectController : NetworkBehaviour
{
    public GameObjectStats stats;
    protected bool isSelect;
    #region 设定

    #endregion
    #region 字段
    protected float currentHealth;
    #endregion
    #region 属性
    public List<Ability> abilities;
    public GameObjectEvents events { get; protected set; }
    public GameObjectStatus status { get; protected set; }
    public Player player { get; set; }
    public GameObjectController target { get; protected set; }
    public Vector3 targetPosition { get; protected set; }
    public float attackDistance { get; protected set; }
    #endregion

    #region 初始化
    protected virtual void Awake()
    {
        InitEvents();
        InitStats();
        InitAbility();
    }
    protected virtual void InitStats()
    {
        status = GetComponent<GameObjectStatus>();
        currentHealth = stats.maxHealth;
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
        SpawnInit();
    }
    protected virtual void OnObjectDead()
    {
        StopAbility();
        Logout();
        Destory();
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
    public virtual void Destory()
    {
        Destroy(gameObject);
    }
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
        GameObjectController result = null;
        foreach (var player in OnlineRoomController.instance.playerDic.Values)
        {
            if(player == this.player) continue;
            if (player.unitList.Count > 0)
            {
                GameObjectController min = Tools.GetNearestGameObject(player.unitList.ToArray(), this) as UnitController;
                if (min == null || Tools.GetDistance(min.transform.position, transform.position) > stats.searchRadius) continue;
                if (result == null) result = min;
                if(min != null) result = Tools.GetDistance(result.transform.position, transform.position) > Tools.GetDistance(min.transform.position, transform.position) ? min : result;
            }
            if(player.constructionList.Count > 0)
            {
                GameObjectController min = Tools.GetNearestGameObject(player.unitList.ToArray(), this) as ConstructionController;
                if (min == null || Tools.GetDistance(min.transform.position, transform.position) > stats.searchRadius) continue;
                if (result == null) result = min;
                result = Tools.GetDistance(result.transform.position, transform.position) > Tools.GetDistance(min.transform.position, transform.position) ? min : result;
            }
        }
        target = result;
    }
    public virtual void UnderAttack(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            events.onDead?.Invoke();
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
        if (targetPosition != Vector3.zero && target == null && Tools.GetDistance(transform.position, targetPosition) < 0.3f)
        {
            return status.Change<GameObjectStayStatu>();
        }
        return false;
    }
    #endregion
}