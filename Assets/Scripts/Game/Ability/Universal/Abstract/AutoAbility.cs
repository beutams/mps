using UnityEngine;

public abstract class AutoAbility : Ability
{
    protected Timer timer;
    public float time;
    public bool autoRun;

    protected GameObjectController target;
    protected Vector3 position;

    private bool run;
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        run = false;
        timer = new Timer();
        timer.Init(time, null, true, false);
        TimerManager.instance.AddTimer(timer);
    }
    public override void Do(GameObjectController target) 
    {
        Clear();
        this.target = target;
        if(autoRun)
            run = true;
        timer.ChangeInit(time, OnTimerCompleteGameObject, true, true);
        timer.Lanuch();
    }
    public override void Do(Vector3 target) 
    {
        Clear();
        position = target;
        timer.ChangeInit(time, OnTimerCompletePosition, true, true);
        timer.Lanuch();
    }
    public override void Do()
    {
        Clear();
        timer.ChangeInit(time, OnTimerComplete, true, true);
        timer.Lanuch();
    }
    public override void OnAbilityDestroy()
    {
        base.OnAbilityDestroy();
        TimerManager.instance.RemoveTimer(timer);
    }
    public virtual void OnTimerCompleteGameObject() { }
    public virtual void OnTimerCompletePosition() { }
    public virtual void OnTimerComplete() { }
    public virtual void OnAbilityStop()
    {
        timer.PauseInvoke();
    }
    protected virtual void Clear()
    {
        position = Vector3.zero;
        target = null;
    }
    public override bool CanDo()
    {
        if(autoRun)
            return !run;
        return false;
    }
}