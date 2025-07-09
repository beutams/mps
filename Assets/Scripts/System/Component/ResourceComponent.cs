using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ResourceComponent : BaseComponent<ResourceComponent>
{
    public static string scriptablepath = "/SaveData/ScriptableData.txt";
    public Dictionary<string, Dictionary<int, ScriptableObject>> dataDic = new Dictionary<string, Dictionary<int, ScriptableObject>>();
    public Dictionary<string, Dictionary<ScriptableObject, int>> indexDic = new Dictionary<string, Dictionary<ScriptableObject, int>>();
    public Dictionary<string, Dictionary<int, GameObject>> prefabDic = new Dictionary<string, Dictionary<int, GameObject>>();
    public Dictionary<string, Dictionary<GameObject, int>> indexpDic = new Dictionary<string, Dictionary<GameObject, int>>();
    public void Start()
    {
        DataToDictionary();
        PrefabToDictionary();
    }
    public T GetResource<T>(string name) where T : class
    {
        return null;
    }
    public Sprite GetImage(string path)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path.Replace('\\','/'));
        return sprite;
    }
    public Stack<T> GetAllAssets<T>(string path, string suffix) where T : UnityEngine.Object
    {
        Stack<string> directories = new Stack<string>();
        Stack<T> objs = new Stack<T>();
        directories.Push(Application.dataPath + path);
        while (directories.Count > 0)
        {
            string cur = directories.Pop();
            string[] next = Directory.GetDirectories(cur);
            if (next != null && next.Length > 0)
                foreach (string s in next)
                    directories.Push(s);
            string[] obj = Directory.GetFiles(cur, suffix);
            if (obj != null && obj.Length > 0)
                foreach (string o in obj)
                {
                    string sub = o.Substring(Application.dataPath.Length - 6);
                    string p = sub.Replace("\\", "/");
                    objs.Push(AssetDatabase.LoadAssetAtPath<T>(p));
                }
        }
        return objs;
    }
    public void PrefabToDictionary()
    {
        try
        {
            Stack<GameObject> objs = GetAllAssets<GameObject>("/Prefabs", "*.prefab");
            foreach (GameObject obj in objs)
            {
                string name = obj.GetType().ToString();
                if (obj.TryGetComponent(out UIBase ui))
                    name = "UIBase";
                if (!(obj.TryGetComponent(out IDCompnent id))) continue;
                if (!dataDic.ContainsKey(name))
                {
                    Dictionary<int, GameObject> table = new Dictionary<int, GameObject> { { id.ID, obj } };
                    Dictionary<GameObject, int> indexTable = new Dictionary<GameObject, int> { { obj, id.ID } };
                    prefabDic[name] = table;
                    indexpDic[name] = indexTable;
                }
                else
                {
                    prefabDic[name].Add(id.ID, obj);
                    indexpDic[name].Add(obj, id.ID);
                }
            }
        }
        catch
        {
            Debug.LogException(new Exception("ID冲突"));
        }
    }
    public void DataToDictionary()
    {
        try
        {
/*            Stack<string> directories = new Stack<string>();
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
            }*/
            Stack<ScriptableObject> objs = GetAllAssets<ScriptableObject>("/ScriptableObjects", "*.asset");
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
}
