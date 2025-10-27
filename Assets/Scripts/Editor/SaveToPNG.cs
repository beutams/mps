using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;

public static class SaveToPNG
{
    [MenuItem("Tools/SaveToPNG")]
    static public Texture2D SaveRenderToPng()
    {
        RenderTexture renderT = GameObject.Find("MiniMap Camera").GetComponent<Camera>().targetTexture;
        string folderName = "test";
        string name = "png";
        int width = renderT.width;
        int height = renderT.height;
        Texture2D tex2d = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderT;
        tex2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex2d.Apply();

        byte[] b = tex2d.EncodeToPNG();
        string sysPath = "c:/" + folderName;
        if (!Directory.Exists(sysPath))
            Directory.CreateDirectory(sysPath);
        FileStream file = File.Open(sysPath + "/" + name + GetTimeName() + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(b);
        file.Close();

        return tex2d;
    }

    static public string GetTimeName()
    {
        return System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() +
            System.DateTime.Now.Day.ToString() + System.DateTime.Now.Hour.ToString() +
            System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() +
            System.DateTime.Now.Millisecond.ToString();
    }
}