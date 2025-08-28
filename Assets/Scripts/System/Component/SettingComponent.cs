using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public class SettingComponent : BaseComponent<SettingComponent>
{
    public SettingData settingData;
    protected string mainPath = Path.Combine(Path.Combine(Application.dataPath,"../"),"SaveData");
    protected Dictionary<string, FieldInfo> settingItemDic = new Dictionary<string, FieldInfo>();
    private void Start()
    {
        Init();
        GameEntry.SaveDataComponent.Save(settingData, "SettingData");
    }
    private void Init()
    {
        settingData = GameEntry.SaveDataComponent.Read<SettingData>("SettingData");
        RegisterFields(settingData);
    }
    private void RegisterFields<T>(T data)
    {
        foreach(var item in data.GetType().GetFields())
        {
            settingItemDic.Add(item.Name, item);
        }
    }
}
[Serializable]
public class SettingData
{
    [Header("Camera")]
    public float CameraMoveSpeed = 6f;
    [Header("QuadTree")]
    public int maxDepth = 5;
    public int maxObject = 2;
    public float mapSize = 102;
    [Header("MiniMapColor")]
    public Color local = Color.green;
    public Color noCamp = Color.white;
    public Color partner = Color.blue;
    public Color enemy = Color.red;
}