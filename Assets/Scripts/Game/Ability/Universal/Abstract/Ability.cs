using UnityEngine;
public abstract class Ability : ScriptableObject
{
    protected GameObjectController owner;
    public virtual void Init(GameObjectController owner) { this.owner = owner; }
    public virtual void Do() { }
    public virtual bool CanDo() { return false; }
    public virtual void Do(GameObjectController target) { }
    public virtual bool CanDo(GameObjectController target) { return false; }
    public virtual void Do(Vector3 target) { }
    public virtual bool CanDo(Vector3 target){ return false; }
    public virtual void OnAbilityDestroy() { }
}