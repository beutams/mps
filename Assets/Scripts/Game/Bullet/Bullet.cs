using Mirror;
using Unity.Services.Analytics.Platform;
using UnityEngine;
using UnityEngine.Events;

public abstract class Bullet : MonoBehaviour
{
    public BulletData data;
    public GameObjectController target { get; protected set; }
    public Player player { get; protected set; }
    //自身
    protected float curTime;
    protected float speed;

    public UnityAction<Collision> onCollision;
    public UnityAction onStart;

    protected Quaternion startRotation;
    protected 
    #region Init
    private void Awake()
    {
        startRotation = transform.rotation;
    }
    public virtual void Init(Vector3 position, Quaternion rotation, GameObjectController target, Player player)
    {
        gameObject.SetActive(true);
        Quaternion x = Quaternion.AngleAxis(startRotation.eulerAngles.x, Vector3.right);
        transform.position = position;
        transform.rotation = rotation * x;
        this.player = player; 
        this.target = target;
        curTime = 0;
        speed = data.startSpeed;
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
            GameEntry.ObjectPoolComponent.Release(gameObject);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        onCollision?.Invoke(collision);
        GameEntry.ObjectPoolComponent.Release(gameObject);
    }
}