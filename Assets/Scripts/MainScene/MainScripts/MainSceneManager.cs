using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public void Start()
    {
        foreach(var ui in UIGroup.globalDic["TitleMenu"])
        {
            ui.GetComponent<MainPanelButton>()?.onClick.AddListener(() => OnTitleClicked(ui));
        }
        UIGroup.globalDic["TitleMenu"].Find(s => s.name == "Main").GetComponent<MainPanelButton>()?.OnPointerClick(null);
    }
    public void OnTitleClicked(UIGroup btn)
    {
        SubUIBase<MainSubUI>.ChangeUI(btn);
    }
}
