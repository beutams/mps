using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataComponent : BaseComponent<SaveDataComponent>
{
    protected string mainPath = Path.Combine(Path.Combine(Application.dataPath, "../"), "SaveData");
    public void Save<T>(T data, string path)
    {
        try
        {
            string dataPath = $"{mainPath}/{path}.json";
            string datas = JsonConvert.SerializeObject(data);
            if (File.Exists(dataPath))
                File.Delete(dataPath);
            File.Create(dataPath).Close();
            File.WriteAllText(dataPath, datas);
        }
        catch(Exception e)
        {
            Debug.LogError($"Setting : {data.GetType().Name} save exception, Log : {e}");
        }
    }
    public T Read<T>(string path) where T : new()
    {
        string dataPath = $"{mainPath}/{path}.json";
        try
        {
            if (File.Exists(dataPath))
            {
                string dataStr = File.ReadAllText(dataPath);
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
}
