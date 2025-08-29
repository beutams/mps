using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcedure : ProcedureBase
{
    public override void OnEnter(string data)
    {
        GameEntry.UIComponent.ShowUI("GameUI");
    }

    public override void OnExit(string data)
    {

    }

    public override void OnStep()
    {

    }
}
