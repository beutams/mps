using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TimerManager : SingletonNetBehaviour<TimerManager>
{
    public List<Timer> timers = new List<Timer>();
    public void AddTimer(Timer timer)
    {
        timer.Pause();
        timers.Add(timer);
    }
    public void RemoveTimer(Timer timer)
    {
        if (timers.Contains(timer))
            timers.Remove(timer);
    }
    private void Update()
    {
        for(int i = 0; i < timers.Count; i++)
        {
            timers[i].Step(Time.deltaTime);
        }
    }
}
public class Timer
{
    private float currentTime;
    private float time;
    private bool autoReset;
    private bool enable;
    private bool canInvoke;
    private bool doRightNow;
    private UnityAction onTimerComplete;

    public void Init(float time,UnityAction onTimerComplete,bool autoReset,bool doRightNow)
    {
        this.time = time;
        currentTime = doRightNow ? 0 : time;
        this.onTimerComplete = onTimerComplete;
        this.autoReset = autoReset;
        this.doRightNow = doRightNow;
        canInvoke = true;
    }
    public void ChangeInit(float time, UnityAction onTimerComplete, bool autoReset, bool doRightNow)
    {
        this.time = time;
        this.onTimerComplete = onTimerComplete;
        this.autoReset = autoReset;
        this.doRightNow = doRightNow;
    }
    public void Reset()
    {
        if (doRightNow)
            currentTime = 0;
        else
            currentTime = time;
    }
    public void Lanuch()
    {
        enable = true;
        canInvoke = true;
    }
    public void Pause()
    {
        enable = false;
    }
    public void PauseInvoke()
    {
        canInvoke = false;
    }
    public void Step(float deltaTime)
    {
        if (!enable) return;
        currentTime -= deltaTime;
        if(currentTime < 0)
        {
            if (canInvoke)
            {
                onTimerComplete?.Invoke();
                if (autoReset)
                    AutoReset();
            }
        }
    }
    public bool IsDone()
    {
        return currentTime < 0;
    }
    public float GetProgress()
    {
        return (time - currentTime) / time;
    }
    public bool isRun()
    {
        return enable;
    }
    private void AutoReset()
    {
        currentTime = time;
    }
}