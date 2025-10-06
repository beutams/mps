using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObject/Universal/Attack")]
public class Attack : AutoAbility
{
    public float atk;
    public float attackSpeed;
    public float attackDistance;
    public bool enable;

    public override void Init(GameObjectController owner)
    {
        time = 1/attackSpeed;
        base.Init(owner);
        owner.SetAttackDistance(attackDistance);
    }
    public override bool CanDo(GameObjectController target)
    {
        if(!enable) return false;
        return owner.status.GetStatu().GetType() == typeof(GameObjectAttackStatu) && target.player != owner.player;
    }
    public override void OnTimerCompleteGameObject()
    {
        base.OnTimerCompleteGameObject();
        if(target != null)
        {
            target.UnderAttack(atk);
            owner.animator.Play("Attack");
        }
    }
}