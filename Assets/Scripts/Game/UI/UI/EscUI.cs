using Michsky.UI.Shift;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscUI : UIBase
{
    public SettingsButton continueBtn;
    public SettingsButton configBtn;
    public SettingsButton exitBtn;
    private void Start()
    {
        continueBtn?.onClick.AddListener(OnContinueClick);
        configBtn?.onClick.AddListener(OnConfigClick);
        exitBtn?.onClick.AddListener(OnExitClick);
    }
    public void ShowUI(string prefab)
    {
        GameEntry.UIComponent.ShowUI(prefab);
    }
    public void OnContinueClick()
    {
        Close();
    }
    public void OnConfigClick()
    {
        GameEntry.UIComponent.ShowUI("ConfigUI");
    }
    public void OnExitClick()
    {
        GameEntry.ProcedureComponent.Change<ChangeSceneProcedure>("MainScene");
        NetworkClient.Disconnect();
    }
}
