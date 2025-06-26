using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectToJson
{
/*    [MenuItem("Tools/DicToData")]
    public static void ToData()
    {
        Dictionary<string, List<Object>> dic = ResourceComponent.DataToDictionary();
        StringWriter stream = new StringWriter();
        foreach (var kvp in dic)
        {
            int index = 0;
            foreach (var obj in kvp.Value)
            {
                index++;
                stream.Write($"{kvp.Key}|{index}|{JsonUtility.ToJson(obj)}\n");
            }
        }
        if (File.Exists(Application.dataPath + ResourceComponent.scriptablepath))
        {
            File.Delete(Application.dataPath + ResourceComponent.scriptablepath);
        }
        File.WriteAllText(Application.dataPath + ResourceComponent.scriptablepath, stream.ToString());
    }*/
}
