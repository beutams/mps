using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataComponent : BaseComponent<SaveDataComponent>
{
    public static void Save<T>(T data, string path)
    {
        string datas = JsonConvert.SerializeObject(data);
        if (File.Exists(path))
            File.Delete(path);
        File.WriteAllText(path, datas);
    }
}
