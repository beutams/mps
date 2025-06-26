using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmoryItem : MonoBehaviour, IPointerClickHandler
{
    public Action onClick;
    public Image img;
    public TextMeshProUGUI text;
    public void Init(string imgPath, string name, Action action)
    {
        img.sprite = GameEntry.ResourceComponent.GetImage(imgPath);
        text.text = name;
        onClick += action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
