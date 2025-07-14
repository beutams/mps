using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmoryItem : DoubleClick
{
    protected ScriptableObject obj;
    public Image img;
    public TextMeshProUGUI text;
    public void Init(string imgPath, string name,ScriptableObject obj, Action action)
    {
        this.obj = obj;
        img.sprite = GameEntry.ResourceComponent.GetImage(imgPath);
        text.text = name;
        onDoubleClick += action;
        onClick += OnClick;
    }
    public void OnClick()
    {
        ArmorySubUI.instance.ShowObjectInfo(obj);
    }
}
