using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : UIBase, IPointerDownHandler, IPointerUpHandler
{
    [Header("Shop")]
    public Transform shopContent;
    protected static List<ProductItem> allItem = new List<ProductItem>();
    [Header("Weapen")]
    public Transform weapenContent;
    protected List<WeapenItem> weapenItems = new List<WeapenItem>();
    [Header("Else")]
    public RectTransform info;
    public Image dragImage;
    protected ProductItem last;
    protected ProductItem currentItem;
    protected bool isDrag;
    protected Timer timer;

    private void Start()
    {
        InitTimer();
        InitShopData();
        InitWeapenData();
    }
    private void InitTimer()
    {
        timer = new Timer();
        timer.Init(3f, OnTimerComplete, false, false);
    }
    private string GetData(string key,string value)
    {
        return ExcelReader.ReadValue("ShopData", key, value);
    }
    private void InitShopData()
    {
        Dictionary<string,string> datas = ExcelReader.dataDic["ShopData"];
        foreach(var kvp in datas)
        {
            ProductItem item = Instantiate(GameEntry.ResourceComponent.GetPrefabResource("ProductItem",kvp.Value)).GetComponent<ProductItem>();
            item.transform.SetParent(shopContent);
            ProductData data = new ProductData() 
            { 
                cost = int.Parse(GetData(kvp.Key, "Cost")), 
                imgPath = GetData(kvp.Key, "ImgPath"), 
                weapen = GameEntry.ResourceComponent.GetDataResource("WeapenBase", GetData(kvp.Key, "Name")) as Weapen 
            };
            item.data = data;item.Refresh();
        }
    }
    //# small,300,300 | small,400,400
    private void InitWeapenData()
    {
        Dictionary<string,string> heroDatas = ExcelReader.Read("WeapenData", RoomController.instance.localPlayer.armory.hero.ToString());
        string[] solts = heroDatas["Solt"].Split('|');
        foreach(var solt in solts)
        {
            WeapenItem item = Instantiate(GameEntry.ResourceComponent.GetPrefabResource("WeapenItem").GetComponent<WeapenItem>());
            item.transform.SetParent(weapenContent);
            string[] data = solt.Split(',');
            item.transform.localPosition = new Vector3(int.Parse(data[1]),int.Parse(data[2]), 0);
            weapenItems.Add(item);
        }
    }
    private void Update()
    {
        if (!isDrag)
        {
            WaitTime();
            currentItem = RaycastItem<ProductItem>();
        }
        else
        {
            dragImage.transform.position = Camera.main.WorldToScreenPoint(Input.mousePosition);
        }
    }
    public void WaitTime()
    {
        if (currentItem == last) return;
        last = currentItem;
        info.gameObject.SetActive(false);
        timer.Reset();
    }
    public void OnTimerComplete()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(Input.mousePosition);
        info.position = new Vector3(position.x - info.rect.width / 2,position.y - info.rect.height /2, 0);
        info.gameObject.SetActive(true);
    }
    public T RaycastItem<T>() where T : MonoBehaviour
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Camera.main.WorldToScreenPoint(Input.mousePosition);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out T t))
            {
                return t;
            }
        }
        return null;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
        dragImage.gameObject.SetActive(false);
        dragImage.transform.position = Vector3.zero;
        WeapenSoltItem result = RaycastItem<WeapenSoltItem>();
        if(result!= null)
        {
            result.Equip(currentItem.data.weapen);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            isDrag = true;
            dragImage.gameObject.SetActive(true);
        }
    }
}