using UnityEngine;

public class CoverAbility : Ability
{
    protected Timer timer;
    public float time;

    protected bool isReady;
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        timer = new Timer();
        timer.Init(time, Ready, false, false);
        TimerManager.instance.AddTimer(timer);
    }
    public override bool CanDo()
    {
        return isReady;
    }
    public override bool CanDo(GameObjectController target)
    {
        return isReady;
    }
    public override bool CanDo(Vector3 target)
    {
        return isReady;
    }
    public override void OnAbilityDestroy()
    {
        base.OnAbilityDestroy();
        TimerManager.instance.RemoveTimer(timer);
    }
    public override void Do()
    {
        base.Do();
        ResetTimer();
        isReady = false;
    }
    public override void Do(GameObjectController target)
    {
        base.Do(target);
        ResetTimer();
        isReady = false;
    }
    public override void Do(Vector3 target)
    {
        base.Do(target);
        ResetTimer();
        isReady = false;
    }
    protected virtual void ResetTimer()
    {
        timer.Reset();
    }
    protected virtual void Ready()
    {
        isReady = true;
    }
}