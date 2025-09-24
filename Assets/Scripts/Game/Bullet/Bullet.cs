using Mirror;
using System.Security.Principal;
using Unity.Services.Analytics.Platform;
using UnityEngine;
using UnityEngine.Events;

public abstract class Bullet : MonoBehaviour
{
    public BulletData data;
    public QuadTreeStat target { get; protected set; }
    public QuadTreeStat quadStat { get; protected set; }
    public Player player { get; protected set; }
    
    //自身
    protected float curTime;
    protected float speed;

    public UnityAction<Collision> onCollision;
    public UnityAction onStart;

    protected Quaternion startRotation;
    
    #region Init
    private void Awake()
    {
        startRotation = transform.rotation;
        quadStat = GetComponent<QuadTreeStat>();
    }
    
    public virtual void Init(Vector3 position, Quaternion rotation, QuadTreeStat target, Player player)
    {
        if (data.isEntity)
            QuadTreeManager.instance.Insert(QuadTreeType.Bullet, quadStat);
        gameObject.SetActive(true);
        
        // 设置子弹位置
        transform.position = position;
        
        // 计算飞行方向
        Vector3 flyDirection = rotation * Vector3.forward;
        
        // 让capsule的上方(Y轴)指向飞行方向
        // 这样capsule的长轴就会与飞行方向对齐
        transform.rotation = Quaternion.LookRotation(Vector3.up, flyDirection);
        
        // 如果capsule的长轴是Z轴方向，使用这个：
        // transform.rotation = Quaternion.LookRotation(flyDirection, Vector3.up);
        
        this.player = player; 
        this.target = target;
        curTime = 0;
        speed = data.startSpeed;
        
        Debug.Log($"子弹发射 - 位置: {position}");
        Debug.Log($"子弹旋转 - 飞行方向: {flyDirection}, 最终旋转: {transform.rotation.eulerAngles}");
        Debug.Log($"子弹朝向 - Forward: {transform.forward}, Up: {transform.up}, Right: {transform.right}");
        
        onStart?.Invoke();
    }
    #endregion
    
    public virtual void Update()
    {
        if (!RoomController.instance.gameReady) return;
        CanDestory();
        Move();
    }
    
    public abstract void Move();
    
    public virtual void CanDestory()
    {
        if (curTime < data.liveTime)
            curTime += Time.deltaTime;
        else
        {
            GameEntry.ObjectPoolComponent.Release(gameObject);
            if (data.isEntity)
                QuadTreeManager.instance.Delete(QuadTreeType.Bullet, quadStat);
        }

    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        onCollision?.Invoke(collision);
        GameEntry.ObjectPoolComponent.Release(gameObject);
        if (data.isEntity)
            QuadTreeManager.instance.Delete(QuadTreeType.Bullet, quadStat);
    }
    
    /// <summary>
    /// 获取子弹的飞行方向
    /// 如果capsule的上方指向飞行方向，则返回transform.up
    /// 如果capsule的前方指向飞行方向，则返回transform.forward
    /// </summary>
    public Vector3 GetFlyDirection()
    {
        // 根据您的capsule方向调整
        return transform.up; // capsule上方指向飞行方向
        // return transform.forward; // 如果capsule前方指向飞行方向
    }
}