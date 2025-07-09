using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUI : UIBase
{
    public Button continueBtn;
    public Button ConfigBtn;
    public Button exitBtn;
    private void Start()
    {
        
    }
    public void ShowUI(string prefab)
    {
        GameEntry.UIComponent.ShowUI(prefab);
    }
}
