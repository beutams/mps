using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuProcedure : ProcedureBase
{
    public override void OnEnter(string data)
    {
        GameEntry.UIComponent.ShowUI("MainUI");
        GameEntry.EventComponent.Subscribe(GameEvent.ClientChangeSceneSuccessEvent, ChangeToGameProcedure);
    }

    public override void OnExit(string data)
    {
        GameEntry.EventComponent.Desubscribe(GameEvent.ClientChangeSceneSuccessEvent, ChangeToGameProcedure);
    }

    public override void OnStep()
    {
        
    }
    public void ChangeToGameProcedure(object data)
    {
        GameEntry.ProcedureComponent.Change<GameProcedure>();
    }
}
