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
    private void Awake()
    {
        Init();
        SaveData(settingData);
    }
    private void Init()
    {
        settingData = GetData<SettingData>();
        RegisterFields(settingData);
    }
    private T GetData<T>() where T : new()
    {
        string settingDataPath = $"{mainPath}/{typeof(T).Name}.json";
        try
        {
            if (File.Exists(settingDataPath))
            {
                string dataStr = File.ReadAllText(settingDataPath);
                T obj = JsonUtility.FromJson<T>(dataStr);
                if (obj != null)
                    return obj;
            }
            return new T(); 
        }
        catch
        {
            return new T();
        }
    }
    private void RegisterFields<T>(T data)
    {
        foreach(var item in data.GetType().GetFields())
        {
            settingItemDic.Add(item.Name, item);
        }
    }
    private void SetData<T>(string key, object value,T data)
    {
        settingItemDic[key].SetValue(data, value);
    }
    public void SaveData<T>(T data)
    {
        try
        {
            string settingDataPath = $"{mainPath}/{typeof(T).Name}.json";
            if (File.Exists(settingDataPath))
                File.Delete(settingDataPath);
            File.Create(settingDataPath).Close();
            File.WriteAllText(settingDataPath, JsonUtility.ToJson(data));
        }
        catch(Exception e)
        {
            Debug.LogError($"Setting : {data.GetType().Name} save exception, Log : {e}");
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
}