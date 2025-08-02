using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuProcedure : ProcedureBase
{
    public override void OnEnter(string data)
    {
        GameEntry.UIComponent.ShowUI("MainUI");
    }

    public override void OnExit(string data)
    {
        
    }

    public override void OnStep()
    {
        
    }
    public void EnterGame()
    {
        GameEntry.ProcedureComponent.Change<ChangeSceneProcedure>("Game");
    }
}
