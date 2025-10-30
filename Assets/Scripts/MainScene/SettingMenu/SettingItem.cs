using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SettingItem : MonoBehaviour
{
    protected float value;
    [SerializeField] protected string fieldName;
    public UnityEvent<float> initEvent;
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
            initEvent?.Invoke(value);
        }
    }
    public void ChangeValue(float value)
    {
        this.value = value;
        SetSettingValueByTypeName();
    }
}
