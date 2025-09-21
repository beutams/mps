using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Image img;
    [SerializeField] protected TextMeshProUGUI text;
    
    private string buildingName;
    
    public void Init(string imgPath, string name)
    {
        this.buildingName = name;
        this.text.text = name;
        this.img.sprite = GameEntry.ResourceComponent.GetConstructionImage(imgPath);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // 发出建筑选择事件，携带建筑名称作为参数
        GameEntry.EventComponent.Notify(GameEvent.BuildSelectEvent, buildingName);
        Debug.Log($"BuildingItem: 选中建筑 {buildingName}");
        GameEntry.UIComponent.CloseUI("SelectBuildingUI");
    }
}