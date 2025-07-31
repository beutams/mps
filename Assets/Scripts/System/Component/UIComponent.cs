using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : BaseComponent<UIComponent>
{
    protected Stack<UIBase> uiStack = new Stack<UIBase>();
    protected Dictionary<string, UIBase> uiDic = new Dictionary<string, UIBase>();

    private void Start()
    {
        foreach(var ui in GameEntry.ResourceComponent.GetAllPrefabResource("UIBase").Values)
        {
            RegisterUI(ui.GetComponent<UIBase>());
        }
    }
    public void RegisterUI(UIBase ui)
    {
        string name = ui.name;
        if(!uiDic.ContainsKey(name))
            uiDic.Add(name, ui);
        else
            uiDic[name] = ui;
    }
    public void ShowUI(string ui)
    {
        if(uiStack.Count > 0)
            uiStack.Peek()?.gameObject.SetActive(false);
        uiStack.Push(Instantiate(uiDic[ui].gameObject).GetComponent<UIBase>());
        uiStack.Peek().Init();
        uiStack.Peek().gameObject.SetActive(true);
    }
    public void CloseUI(UIBase ui)
    {
        if (uiStack.Peek() != ui) return;
        uiStack.Pop();
        uiStack.Peek().gameObject.SetActive(true);
    }
}

