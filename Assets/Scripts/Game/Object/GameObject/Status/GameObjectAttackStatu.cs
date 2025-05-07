public class GameObjectAttackStatu : AGameObjectStatu<GameObjectStatus>
{
    public override void OnEnter(GameObjectStatus obj)
    {
        UnitController unitController = obj.controller as UnitController;
        unitController.isMove = false;
        foreach (var ability in obj.controller.abilities)
        {
            if (ability is Attack)
            {
                Attack atk = ability as Attack;
                if(atk.CanDo(obj.controller.target))
                    atk.Do(obj.controller.target);
            }
        }
    }
    public override void OnExit(GameObjectStatus obj)
    {
        UnitController unitController = obj.controller as UnitController;
        unitController.isMove = true;
        foreach (var ability in obj.controller.abilities)
        {
            if(ability is Attack)
            {
                Attack atk = ability as Attack;
                atk.OnAbilityStop();
            }
        }
    }
    public override void OnStep(GameObjectStatus obj)
    {
        if(obj.controller.target == null)
        {
            obj.controller.GetNearestTarget();
            if (obj.controller.CanMove())
                return;
        }
    }
}