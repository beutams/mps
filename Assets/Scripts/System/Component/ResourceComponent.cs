using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ResourceComponent : BaseComponent<ResourceComponent>
{
    public static string scriptablepath = "/SaveData/ScriptableData.txt";
    protected Dictionary<string, Dictionary<int, ScriptableObject>> dataDic = new Dictionary<string, Dictionary<int, ScriptableObject>>();
    protected Dictionary<string, Dictionary<ScriptableObject, int>> indexDic = new Dictionary<string, Dictionary<ScriptableObject, int>>();
    protected Dictionary<string, Dictionary<int, GameObject>> prefabDic = new Dictionary<string, Dictionary<int, GameObject>>();
    protected Dictionary<string, Dictionary<GameObject, int>> indexpDic = new Dictionary<string, Dictionary<GameObject, int>>();
    public void Start()
    {
        DataToDictionary();
        PrefabToDictionary();
    }
    #region Interface
    public int GetPrefabIndex(string id, GameObject obj)
    {
        return indexpDic[id][obj];
    }
    public int GetDataIndex(string id, ScriptableObject obj)
    {
        return indexDic[id][obj];
    }
    public Dictionary<int, GameObject> GetAllPrefabResource(string id)
    {
        if (prefabDic.ContainsKey(id))
            return prefabDic[id];
        return null;
    }
    public Dictionary<int, ScriptableObject> GetAllDataResource(string id)
    {
        if (dataDic.ContainsKey(id))
            return dataDic[id];
        return null;
    }
    public GameObject GetPrefabResource(string id, string name = null)
    {
        if (prefabDic.ContainsKey(id))
        {
            if (name == null)
                return prefabDic[id].First().Value;
            foreach (var prefab in prefabDic[id].Values)
            {
                if (prefab.name == name)
                    return prefab;
            }
        }

        return null;
    }
    public ScriptableObject GetDataResource(string id, string name = null)
    {
        if (dataDic.ContainsKey(id))
        {
            if (name == null)
                return dataDic[id].First().Value;
            foreach (var data in dataDic[id].Values)
            {
                if (data.name == name)
                    return data;
            }
        }
        return null;
    }
    public GameObject GetPrefabResource(string id, int inid)
    {

        if (prefabDic.ContainsKey(id))
        {
            if (prefabDic[id].ContainsKey(inid))
                return prefabDic[id][inid];
        }
        return null;
    }
    public ScriptableObject GetDataResource(string id, int inid)
    {
        if (dataDic.ContainsKey(id))
        {
            if (dataDic[id].ContainsKey(inid))
                return dataDic[id][inid];
        }
        return null;
    }
    public Sprite GetImage(string path)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path.Replace('\\', '/'));
        return sprite;
    }
    #endregion
    protected Stack<T> GetAllAssets<T>(string path, string suffix) where T : UnityEngine.Object
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
    protected void PrefabToDictionary()
    {
        Stack<GameObject> objs = GetAllAssets<GameObject>("/Prefabs", "*.prefab");
        foreach (GameObject obj in objs)
        {
            if (!(obj.TryGetComponent(out ID idComponent))) continue;
            string name = idComponent.searchName == IDType.None ? obj.name : idComponent.searchName.ToString();
            if (!prefabDic.ContainsKey(name))
            {
                prefabDic[name] = new Dictionary<int, GameObject> { { idComponent.ID,obj } };
                indexpDic[name] = new Dictionary<GameObject, int> { { obj, idComponent.ID } };
            }
            else
            {
                int id = idComponent.ID == 0 ? -prefabDic[name].Count : idComponent.ID;
                if (prefabDic[name].ContainsKey(id))
                {
                    id = -prefabDic[name].Count;
                }
                prefabDic[name].Add(id, obj);
                indexpDic[name].Add(obj, id);
            }
        }
    }
    protected void DataToDictionary()
    {
        try
        {
            Stack<ScriptableObject> objs = GetAllAssets<ScriptableObject>("/ScriptableObjects", "*.asset");
            foreach (var obj in objs)
            {
                ID idComponent;
                if ((idComponent = obj as ID) == null) continue;
                string name = idComponent.searchName == IDType.None ? obj.name : idComponent.searchName.ToString();
                if (!dataDic.ContainsKey(name))
                {
                    dataDic[name] = new Dictionary<int, ScriptableObject> { { idComponent.ID, obj } };
                    indexDic[name] = new Dictionary<ScriptableObject, int> { { obj, idComponent.ID } };
                }
                else
                {
                    int id = idComponent.ID == 0 ? -dataDic[name].Count : idComponent.ID;
                    if (dataDic[name].ContainsKey(id))
                    {
                        id = -dataDic[name].Count;
                    }
                    dataDic[name].Add(id, obj);
                    indexDic[name].Add(obj, id);
                }
            }
        }
        catch
        {
            Debug.LogException(new Exception("ID冲突"));
        }
    }
}
public enum IDType
{
    None,
    UIBase,
    Ability,
    HeroStats,
    GameObjectStats,
    WeapenBase,
    GlobalSkillData
}
