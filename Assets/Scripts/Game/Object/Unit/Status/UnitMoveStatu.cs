using UnityEngine;

public class UnitMoveStatu : AGameObjectStatu<GameObjectStatus>
{
    public override void OnEnter(GameObjectStatus obj)
    {
        UnitController unitController = obj.controller as UnitController;
        //unitController.isMove = true;
        unitController.RefreshTarget();
    }
    public override void OnExit(GameObjectStatus obj)
    {
        UnitController unitController = obj.controller as UnitController;
        unitController.Stand();
        //unitController.isMove = false;
    }

    public override void OnStep(GameObjectStatus obj)
    {
        UnitController unitStatus = obj.controller as UnitController;
        if(unitStatus.CanAttack()) return;
        if(unitStatus.CanStay()) return;
    }
}