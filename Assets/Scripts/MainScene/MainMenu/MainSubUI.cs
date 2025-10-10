using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSubUI : SubUIBase
{
    public SettingsButton exit;
    protected override void OnClose()
    {
        exit.onClick.RemoveListener(Exit);
    }

    protected override void OnOpen()
    {
        exit.onClick.AddListener(Exit);
    }
    protected void Exit()
    {
        GameEntry.SaveDataComponent.Save(GameEntry.SettingComponent.settingData, "SettingData");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
