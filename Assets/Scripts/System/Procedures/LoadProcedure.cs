using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadProcedure : ProcedureBase
{
    public override void OnEnter(string data)
    {
        
    }

    public override void OnExit(string data)
    {
        
    }

    public override void OnStep()
    {
        GameEntry.ProcedureComponent.Change<ChangeSceneProcedure>("MainScene");
        ExcelReader.ExcelInitLoad();
    }
}
