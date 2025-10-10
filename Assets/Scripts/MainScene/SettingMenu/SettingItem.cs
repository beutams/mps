using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingItem : MonoBehaviour
{
    protected float value;
    [SerializeField] protected string fieldName;
    public void SetSettingValueByTypeName()
    {
        var field = typeof(SettingData).GetField(fieldName);
        if (field != null)
        {
            field.SetValue(GameEntry.SettingComponent.settingData, value);
        }
    }
    public void Init()
    {
        var field = typeof(SettingData).GetField(fieldName);
        if (field != null)
        {
            value = (float)field.GetValue(GameEntry.SettingComponent.settingData);
        }
    }
    public void ChangeValue(float value)
    {
        this.value = value;
        SetSettingValueByTypeName();
    }
}
