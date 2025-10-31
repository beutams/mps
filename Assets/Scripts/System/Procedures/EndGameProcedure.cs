using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameProcedure : ProcedureBase
{
    public override void OnEnter(string data)
    {
        
    }

    public override void OnExit(string data)
    {
        
    }

    public override void OnStep()
    {
        GameEntry.UIComponent.Clear();
        GameEntry.ProcedureComponent.Change<ChangeSceneProcedure>("MainScene");
    }
}
