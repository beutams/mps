using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectToJson
{
    public static string path = "/SaveData/SaveData.txt";
    [MenuItem("Tools/SToData")]
    public static void ToData()
    {
        Stack<string> directories = new Stack<string>();
        Stack<ScriptableObject> objs = new Stack<ScriptableObject>();
        Dictionary<string, List<Object>> objDic = new Dictionary<string, List<Object>>();
        directories.Push(Application.dataPath + "/ScriptableObjects");
        while(directories.Count > 0)
        {
            string cur = directories.Pop();
            string[] next = Directory.GetDirectories(cur);
            if(next != null && next.Length > 0)
                foreach(string s in next) 
                    directories.Push(s);
            string[] obj = Directory.GetFiles(cur,"*.asset");
            if (obj != null && obj.Length > 0)
                foreach (string o in obj)
                {
                    string sub = o.Substring(Application.dataPath.Length - 6);
                    string p = sub.Replace("\\", "/");
                    objs.Push(AssetDatabase.LoadAssetAtPath<ScriptableObject>(p));
                }
        }
        foreach(var obj in objs)
        {
            string name = obj.GetType().ToString();
            if (!objDic.ContainsKey(name))
            {
                List<Object> list = new List<Object>() { obj };
                objDic[name] = list;
            }
            else
            {
                objDic[name].Add(obj);
            }
        }
        DicToData(objDic);
    }
    public static void DicToData(Dictionary<string, List<Object>> dic)
    {
        StringWriter stream = new StringWriter();
        foreach (var kvp in dic) 
        {
            foreach(var obj in kvp.Value)
            {
                stream.Write($"{kvp.Key}|{JsonUtility.ToJson(obj)}\n");
            }
        }
        if(File.Exists(Application.dataPath + path))
        {
            File.Delete(Application.dataPath + path);
        }
        File.WriteAllText(Application.dataPath + path,stream.ToString());
    }
}
