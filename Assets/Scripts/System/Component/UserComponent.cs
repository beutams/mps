using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserComponent : BaseComponent<UserComponent>
{
    public Dictionary<string, UserData> userData = new Dictionary<string, UserData>();
    public Dictionary<string, UserData> userDataSave = new Dictionary<string, UserData>();
    private void Start()
    {
        LoadData();
    }
    public UserData Get(string key, bool save = true)
    {
        Dictionary<string, UserData> datas = save ? userDataSave : userData;
        if(datas.ContainsKey(key))
            return datas[key];
        return null;
    }
    public void Set(string key, UserData value,bool save = true)
    {
        Dictionary<string, UserData> datas = save ? userDataSave : userData;
        datas[key] = value;
        SaveData();
    }
    protected void LoadData()
    {
        userDataSave = GameEntry.SaveDataComponent.Read<Dictionary<string, UserData>>("UserDataSave");
    }
    protected void SaveData()
    {
        GameEntry.SaveDataComponent.Save(userDataSave, "UserDataSave");
    }
}
