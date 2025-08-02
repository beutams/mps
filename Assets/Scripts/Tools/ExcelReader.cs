using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// #   变量名1 变量名2 变量名3
/// #   类型1   类型2   类型3
/// key 数值1   数值2   数值3
/// </summary>
public static class ExcelReader
{
    // <表名,<key,数据串>>
    public static Dictionary<string, Dictionary<string, string>> dataDic = new Dictionary<string, Dictionary<string, string>>();
    // <表名,<变量名,类型>>
    public static Dictionary<string, Dictionary<string, string>> typeDic = new Dictionary<string, Dictionary<string, string>>();
    public static void ExcelInitLoad()
    {
        string path = Directory.GetParent(Application.dataPath).FullName.Replace("\\","/") + "/Data/ShopData.xlsx";
        ReadFromExcel(path);
    }
    public static Dictionary<string,string> Read(string tableName, string key)
    {
        string dataStr = dataDic[tableName][key];
        string[] datas = dataStr.Split(',');
        Dictionary<string,string> datasDic = new Dictionary<string,string>();
        for(int i = 0; i < datas.Length;i++)
        {
            if(datas[i] != string.Empty && datas[i] != null && datas[i] != "")
            {
                string[] kvp = datas[i].Split('=');
                if(kvp.Length == 2)
                    datasDic.Add(kvp[0], kvp[1]);
            }
        }
        return datasDic;
    }
    public static string ReadValue(string tableName,string key,string value)
    {
        Dictionary<string,string> datas = Read(tableName, key);
        return datas[value];
    }
    public static List<string> ReadFromExcel(string path)
    {
        using(var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            string[] s = path.Split('/');
            string fileName = s[s.Length - 1].Split('.')[0];
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();
            if (result.Tables.Count > 0)
            {
                //统计列
                int columnMax = result.Tables[0].Columns.Count;
                //统计类型
                Dictionary<string, string> types = new Dictionary<string, string>();
                for (int i = 1; i < columnMax; i++)
                {
                    types.Add(result.Tables[0].Rows[0][i].ToString(), result.Tables[0].Rows[1][i].ToString());
                }
                Dictionary<string, string> datas = new Dictionary<string, string>();
                //读取数据
                for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                {
                    if (result.Tables[0].Rows[i][0].ToString() == "#" || result.Tables[0].Rows[i][0].ToString() == "") continue;
                    string key = result.Tables[0].Rows[i][0].ToString();
                    StringBuilder builder = new StringBuilder();
                    for (int j = 1; j < columnMax; j++)
                    {
                        builder.Append(result.Tables[0].Rows[0][j].ToString());
                        builder.Append("=");
                        builder.Append(result.Tables[0].Rows[i][j].ToString());
                        builder.Append(",");
                    }
                    string value = builder.ToString();
                    datas.Add(key, value);
                }
                if(!dataDic.ContainsKey(fileName))
                {
                    dataDic.Add(fileName, datas);
                }
                if(!typeDic.ContainsKey(fileName))
                {
                    typeDic.Add(fileName, types);
                }
            }
        }
        return null;
    }
}