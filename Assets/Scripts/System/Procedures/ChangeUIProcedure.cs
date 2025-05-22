using UnityEngine;

public class ChangeUIProcedure : ProcedureBase
{
    private AsyncOperation async;
    public override void OnEnter(string data)
    {
        async = GameEntry.SceneComponent.LoadScene(data);
    }

    public override void OnExit(string data)
    {
        
    }

    public override void OnStep()
    {
        if(async != null && async.isDone)
        {
            GameEntry.ProcedureComponent.Change<MenuProcedure>();
        }
        GameEntry.ProcedureComponent.Change<ExitProcedure>();
    }
}