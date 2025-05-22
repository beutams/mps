using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneComponent : BaseComponent
{
    public static List<SceneBase> allScenes = new List<SceneBase>();
    public SceneBase currentScene { get; private set; }
    public AsyncOperation LoadScene(string name)
    {
        SceneBase scene = allScenes.Find(s => s.name == name);
        if (scene != null)
        {
            return SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Single);
        }
        return null;
    }
}
public class SceneBase
{
    public string name;
}