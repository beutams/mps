using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : BaseComponent<UIComponent>
{
    protected Stack<UIBase> uiStack = new Stack<UIBase>();
    protected Dictionary<string, UIBase> uiDic = new Dictionary<string, UIBase>();

    public void RegisterUI(UIBase ui)
    {
        string name = ui.name;
        if(!uiDic.ContainsKey(name))
            uiDic.Add(name, ui);
        else
            uiDic[name] = ui;
    }
    public void ShowUI(UIBase ui)
    {
        uiStack.Peek().gameObject.SetActive(false);
        ui.Init();
        uiStack.Push(ui);
        uiStack.Peek().gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        uiStack.Pop();
        uiStack.Peek().gameObject.SetActive(true);
    }
}

