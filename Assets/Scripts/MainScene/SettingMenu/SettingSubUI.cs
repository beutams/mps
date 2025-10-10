using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSubUI : SubUIBase
{
    public List<SettingItem> settingItems = new List<SettingItem>();
    protected Transform settingItemsTransform;
    protected override void OnClose()
    {
        GameEntry.SaveDataComponent.Save(GameEntry.SettingComponent.settingData, "SettingData");
    }

    protected override void OnOpen()
    {
        settingItemsTransform = transform.Find("SettingItems");
        for (int i = 0; i < settingItemsTransform.childCount; i++)
        {
            if (settingItemsTransform.GetChild(i).TryGetComponent(out SettingItem item))
            {
                settingItems.Add(item);
            }
        }
        foreach (var item in settingItems)
        {
            item.Init();
        }
    }
}
