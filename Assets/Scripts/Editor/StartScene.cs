using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class StartScene
{
    [MenuItem("BuildTools/PlayModeUseFirstScene")]
    public static void UpdatePlayModeUserFirstScene()
    {
        SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
        EditorSceneManager.playModeStartScene = scene;
    }
}
