using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmoryItem : DoubleClick, ID
{
    protected ScriptableObject obj;
    [SerializeField] protected Image img;
    [SerializeField] protected TextMeshProUGUI text;

    [Header("ID")]
    [SerializeField] protected int id;
    [SerializeField] protected IDType type;
    public int ID => id;
    public IDType searchName => type;

    public void Init(string imgPath, string name,ScriptableObject obj, Action action)
    {
        this.obj = obj;
        img.sprite = GameEntry.ResourceComponent.GetImage(imgPath);
        text.text = name;
        onDoubleClick += action;
        onClick += () => GameEntry.EventComponent.Notify(GameEvent.ArmoryItemClick, obj);
    }
}
