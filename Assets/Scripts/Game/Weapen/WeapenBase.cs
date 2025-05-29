using UnityEngine;
public abstract class WeapenBase : ScriptableObject
{
    public string weapon = "Weapen";
    public string bullet = "Bullet";
    public int maxBulletCount;
    public float fireInterval;
    public float loadTime;
    public Timer fireTimer;
    public Timer loadTimer;
    public bool autoLoad;

    protected Player player;
    protected int bulletCount;
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