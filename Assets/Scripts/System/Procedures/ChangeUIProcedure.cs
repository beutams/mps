using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeUIProcedure : ProcedureBase
{
    private AsyncOperation async;
    public override void OnEnter(string data)
    {
        if (SceneManager.GetSceneByName(data) != null)
        {
            async = SceneManager.LoadSceneAsync(data);
        }
        else
        {
            GameEntry.ProcedureComponent.Change<ExitProcedure>();
        }
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