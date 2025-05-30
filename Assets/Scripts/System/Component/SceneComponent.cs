using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneComponent : BaseComponent<SceneComponent>
{
    public AsyncOperation LoadScene(string name)
    {
        if (SceneManager.GetSceneByName(name) != null)
        {
            return SceneManager.LoadSceneAsync(name);
        }
        return null;
    }
}