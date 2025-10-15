using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectBuildingUI : UIBase
{
    [SerializeField] protected RectTransform buildingParent;
    List<BuildingItem> building = new List<BuildingItem>();
    
    public override void OnOpen()
    {
        base.OnOpen();
        
        // 清理之前的UI元素
        ClearBuildingItems();
        
        foreach(var item in ExcelReader.ReadAllItem("ConstructionData"))
        {
            string name = ExcelReader.ReadOneItemValue("ConstructionData", item.Key, "Name");
            string imgPath = ExcelReader.ReadOneItemValue("ConstructionData", item.Key, "Img");
            RectTransform obj = GameEntry.ObjectPoolComponent.Get("BuildingItem").GetComponent<RectTransform>();
            BuildingItem buildingItem = obj.GetComponent<BuildingItem>();
            obj.transform.SetParent(buildingParent);
            buildingItem.Init(imgPath, name);
            building.Add(buildingItem);
        }
    }
    
    public override void OnClose()
    {
        base.OnClose();
        ClearBuildingItems();
        GameEntry.EventComponent.Notify(GameEvent.BuildSelectEvent, null);
    }
    
    private void ClearBuildingItems()
    {
        // 将BuildingItem返回到对象池
        foreach(var buildingItem in building)
        {
            if (buildingItem != null && buildingItem.gameObject != null)
            {
                GameEntry.ObjectPoolComponent.Release(buildingItem.gameObject);
            }
        }
        building.Clear();
    }
}