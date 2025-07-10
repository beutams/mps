using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscUI : UIBase
{
    public Button continueBtn;
    public Button configBtn;
    public Button exitBtn;
    private void Start()
    {
        continueBtn.onClick.AddListener(OnContinueClick);
        configBtn.onClick.AddListener(OnConfigClick);
        exitBtn.onClick.AddListener(OnExitClick);
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

    }
    public void OnExitClick()
    {
        GameEntry.SceneComponent.LoadScene("MainScene");
    }
}
