using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ResourceComponent : BaseComponent<ResourceComponent>
{
    public static string scriptablepath = "/SaveData/ScriptableData.txt";
    public Dictionary<string, Dictionary<int, ScriptableObject>> dataDic = new Dictionary<string, Dictionary<int, ScriptableObject>>();
    public Dictionary<string, Dictionary<ScriptableObject, int>> indexDic = new Dictionary<string, Dictionary<ScriptableObject, int>>();
    public void Start()
    {
        DataToDictionary();
    }
    public T GetResource<T>(string name) where T : class
    {
        return null;
    }
    public Sprite GetImage(string path)
    {
        return null;
    }
    public void DataToDictionary()
    {
        try
        {
            Stack<string> directories = new Stack<string>();
            Stack<ScriptableObject> objs = new Stack<ScriptableObject>();
            directories.Push(Application.dataPath + "/ScriptableObjects");
            while (directories.Count > 0)
            {
                string cur = directories.Pop();
                string[] next = Directory.GetDirectories(cur);
                if (next != null && next.Length > 0)
                    foreach (string s in next)
                        directories.Push(s);
                string[] obj = Directory.GetFiles(cur, "*.asset");
                if (obj != null && obj.Length > 0)
                    foreach (string o in obj)
                    {
                        string sub = o.Substring(Application.dataPath.Length - 6);
                        string p = sub.Replace("\\", "/");
                        objs.Push(AssetDatabase.LoadAssetAtPath<ScriptableObject>(p));
                    }
            }
            foreach (var obj in objs)
            {
                string name = obj.GetType().ToString();
                if (obj is Ability)
                    name = "Ability";
                else if (obj is HeroStats)
                    name = "HeroStats";
                else if (obj is GameObjectStats)
                    name = "GameObjectStats";
                else if (obj is WeapenBase)
                    name = "WeapenBase";
                else if (obj is GlobalSkillData)
                    name = "GlobalSkillData";
                if (!(obj is ID)) continue;
                if (!dataDic.ContainsKey(name))
                {
                    Dictionary<int, ScriptableObject> table = new Dictionary<int, ScriptableObject> { { ((ID)obj).ID, obj } };
                    Dictionary<ScriptableObject, int> indexTable = new Dictionary<ScriptableObject, int> { { obj, ((ID)obj).ID } };
                    dataDic[name] = table;
                    indexDic[name] = indexTable;
                }
                else
                {
                    dataDic[name].Add(((ID)obj).ID, obj);
                    indexDic[name].Add(obj, ((ID)obj).ID);
                }
            }
        }
        catch
        {
            Debug.LogException(new Exception("ID冲突"));
        }
    }
/*    public void DataToDic()
    {
        if (!File.Exists(Application.dataPath + scriptablepath)) return;
        string str = File.ReadAllText(Application.dataPath + scriptablepath);
        string[] strs = str.Split('\n');
        foreach (var datas in strs)
        {
            string[] data = datas.Split('|');
            if (data.Length != 3) return;
            if (dataDic.ContainsKey(data[0]))
            {
                dataDic[data[0]].Add(int.Parse(data[1]), JsonUtility.FromJson<Object>(data[2]));
            }
            else
            {
                Dictionary<int, Object> table = new Dictionary<int, Object> { { int.Parse(data[1]), JsonUtility.FromJson<Object>(data[2]) } };
                dataDic[data[0]] = table;
            }
        }
    }*/
}
