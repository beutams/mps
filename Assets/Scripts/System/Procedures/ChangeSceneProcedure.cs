using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneProcedure : ProcedureBase
{
    private AsyncOperation async;
    private string nextScene;
    public override void OnEnter(string data)
    {
        if (SceneManager.GetSceneByName(data) != null)
        {
            async = SceneManager.LoadSceneAsync(data);
            nextScene = data;
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
        if(async != null)
        {
            if (async.isDone)
            {
                switch(nextScene)
                {
                    case "MainScene":
                        GameEntry.ProcedureComponent.Change<MenuProcedure>();
                        break;
                }
                nextScene = null;
            }
        }
        else
        {
            GameEntry.ProcedureComponent.Change<ExitProcedure>();
        }
    }
}