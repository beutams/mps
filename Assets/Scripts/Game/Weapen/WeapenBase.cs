using UnityEngine;
public abstract class WeapenBase : ScriptableObject, ID
{
    public string bullet = "Bullet";
    public int maxBulletCount;
    public float fireInterval;
    public float loadTime;
    public bool autoLoad;

    public Timer fireTimer;
    public Timer loadTimer;
    protected Player player;
    public int bulletCount { get; private set; }
    [Header("ID")]
    [SerializeField] protected int id;
    public int ID => id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;

    public void Init(Player player)
    {
        this.player = player;
        bulletCount = maxBulletCount;
        fireTimer = new Timer();
        fireTimer.Init(fireInterval, null, false, false);
        TimerManager.instance.AddTimer(fireTimer);
        fireTimer.Lanuch();
        if (autoLoad)
        {
            loadTimer = new Timer();
            loadTimer.Init(loadTime, Load, false, false);
            TimerManager.instance.AddTimer(loadTimer);
            loadTimer.Pause();
        }
    }
    public void Fire(GameObjectController self,GameObjectController target,Vector3 targetPosition)
    {
        if(bulletCount > 0 && fireTimer.IsDone())
        {
            bulletCount--;
            FireInner(self,target, targetPosition);
            fireTimer.Reset();
        }
        if(bulletCount <= 0 && !loadTimer.isRun())
        {
            loadTimer.Reset();
            loadTimer.Lanuch();
        }
    }
    public void FireInner(GameObjectController self,GameObjectController target, Vector3 targetPosition)
    {
        Bullet obj = GameEntry.ObjectPoolComponent.Get(bullet).GetComponent<Bullet>();
        obj.Init(self.transform.position, self.transform.rotation);
        obj.SetTarget(target, targetPosition, player);
    }
    public void Load()
    {
        bulletCount = maxBulletCount;
        loadTimer.Pause();
    }
}