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
    protected SettingDataNotSave notSaveData;
    protected string mainPath = Path.Combine(Path.Combine(Application.dataPath,"../"),"SaveData");
    private void Awake()
    {
        notSaveData = new SettingDataNotSave();     
    }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        settingData = GameEntry.SaveDataComponent.Read<SettingData>("SettingData");
        notSaveData.ToSettingData();
        GameEntry.SaveDataComponent.Save(settingData, "SettingData");
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
    [HideInInspector] public float[] local;
    [HideInInspector] public float[] noCamp;
    [HideInInspector] public float[] partner;
    [HideInInspector] public float[] enemy;
    [Header("Game")]
    public int maxPopulation = 10;
    public int maxCommand = 3;
}
[Serializable]
public class SettingDataNotSave
{
    public Color local = Color.green;
    public Color noCamp = Color.red;
    public Color partner = Color.blue;
    public Color enemy = Color.red;

    public void ToSettingData()
    {
        SettingData data = GameEntry.SettingComponent.settingData;
        data.local = ColorTool.ToFloat(local);
        data.noCamp = ColorTool.ToFloat(noCamp);
        data.partner = ColorTool.ToFloat(partner);
        data.enemy = ColorTool.ToFloat(enemy);
    }
}
