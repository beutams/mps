using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : BaseComponent<UIComponent>
{
    protected Stack<UIBase> uiStack = new Stack<UIBase>();
    protected Dictionary<string, UIBase> uiDic = new Dictionary<string, UIBase>();

    private void Start()
    {
        GameEntry.EventComponent.Subscribe(GameEvent.ClientChangeSceneSuccessEvent, (s) => { uiStack.Clear(); });
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
        uiStack.Push(GameEntry.ObjectPoolComponent.Get("UIBase",uiDic[ui].name).GetComponent<UIBase>());
        uiStack.Peek().OnOpen();
        uiStack.Peek().gameObject.SetActive(true);
        GameEntry.EventComponent.Notify(GameEvent.UIOpenEvent, ui);
    }
    public void CloseUI(UIBase ui)
    {
        if (uiStack.Peek() != ui) return;
        UIBase obj = uiStack.Pop();
        uiStack.Peek().gameObject.SetActive(true);
        obj.OnClose();
        GameEntry.ObjectPoolComponent.Release(obj.gameObject);
        GameEntry.EventComponent.Notify(GameEvent.UICloseEvent, ui);
    }
    public void CloseUI(string ui)
    {
        string name = uiStack.Peek().name.Split("_")[1];
        if (name != ui) return;
        UIBase obj = uiStack.Pop();
        uiStack.Peek().gameObject.SetActive(true);
        obj.OnClose();
        GameEntry.ObjectPoolComponent.Release(obj.gameObject);
        GameEntry.EventComponent.Notify(GameEvent.UICloseEvent, ui);
    }
    public UIBase GetTopUI()
    {
        return uiStack.Peek();
    }
}

