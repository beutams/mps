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
            Color color = new Color((int)Random.Range(0, 255), (int)Random.Range(0, 255), (int)Random.Range(0, 255));
            target.GetComponent<MeshRenderer>().material.color = color;
            target.UnderAttack(atk);
        }
    }
}