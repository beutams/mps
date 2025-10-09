using UnityEngine;

public class GameObjectStayStatu : AGameObjectStatu<GameObjectStatus>
{
    public override void OnEnter(GameObjectStatus obj)
    {
        UnitController unitController = obj.controller as UnitController;
        if (unitController != null)
        {
            unitController.isMove = false;
        }
    }

    public override void OnExit(GameObjectStatus obj)
    {
        UnitController unitController = obj.controller as UnitController;
        if (unitController != null)
        {
            unitController.isMove = true;
        }
    }
    public override void OnStep(GameObjectStatus obj)
    {
        obj.controller.GetNearestTarget();
        if (obj.controller.CanAttack()) return;
        if (obj.controller.CanMove()) return;
    }
}