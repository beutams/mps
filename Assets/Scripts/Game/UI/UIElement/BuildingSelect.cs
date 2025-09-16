using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelect : MonoBehaviour
{
    protected Transform space;
    protected Button button;
    public void Init(Transform space)
    {
        this.space = space;
        button = GetComponent<Button>();
    }
    public void OnSelected(object data)
    {
        GameEntry.ObjectPoolComponent.Release(gameObject);
        GameEntry.ObjectPoolComponent.Release(space.gameObject);
        GameEntry.ObjectPoolComponent.Get(data as string);

        OnCancel();
    }
    public void OnClick()
    {
        GameEntry.EventComponent.Subscribe(GameEvent.GameSelectBuildEvent, OnSelected);
        ShowSelectUI();
    }
    public void OnCancel()
    {
        GameEntry.EventComponent.Desubscribe(GameEvent.GameSelectBuildEvent, OnSelected);
    }
    public void ShowSelectUI()
    {
        GameEntry.UIComponent.ShowUI("BuildingSelectUI");
    }
}
