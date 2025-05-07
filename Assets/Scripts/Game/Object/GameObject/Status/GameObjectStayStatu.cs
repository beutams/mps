using UnityEngine;

public class GameObjectStayStatu : AGameObjectStatu<GameObjectStatus>
{
    public override void OnEnter(GameObjectStatus obj)
    {
        
    }

    public override void OnExit(GameObjectStatus obj)
    {
        
    }
    public override void OnStep(GameObjectStatus obj)
    {
        obj.controller.GetNearestTarget();
        if (obj.controller.CanAttack()) return;
        if (obj.controller.CanMove()) return;
    }
}