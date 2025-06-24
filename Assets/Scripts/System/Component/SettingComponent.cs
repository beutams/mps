using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingComponent : BaseComponent<SettingComponent>
{
    [Header("Camera")]
    public float CameraMoveSpeed = 6f;
    [Header("QuadTree")]
    public int maxDepth = 5;
    public int maxObject = 2;
    public float mapSize = 102;
    [Header("Path")]
    public string settingPath = Application.streamingAssetsPath + "/SaveData/Setting.json";
}
