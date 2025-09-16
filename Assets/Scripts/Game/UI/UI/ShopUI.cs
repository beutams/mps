using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : UIBase, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Shop")]
    public Transform shopContent;
    protected static List<ProductItem> allItem = new List<ProductItem>();
    [Header("Weapen")]
    public Transform weapenContent;
    protected List<WeapenSoltItem> weapenItems = new List<WeapenSoltItem>();
    [Header("Else")]
    public RectTransform info;
    public Image dragImage;
    public TextMeshProUGUI property;

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
    public override void OnOpen()
    {
        base.OnOpen();
        RefreshWeapen();
    }
    private void InitTimer()
    {
        timer = new Timer();
        timer.Init(3f, OnTimerComplete, false, false);
        TimerManager.instance.AddTimer(timer);
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
            ProductItem item = GameEntry.ObjectPoolComponent.Get("ProductItem").GetComponent<ProductItem>();
            item.transform.SetParent(shopContent);
            ProductData data = new ProductData()
            {
                cost = int.Parse(GetData(kvp.Key, "Cost")),
                name = GetData(kvp.Key, "Name"),
                weapen = GameEntry.ResourceComponent.GetDataResource("WeapenBase", GetData(kvp.Key, "Name")) as Weapen,
                info = GetData(kvp.Key, "Info"),
            };
            item.Refresh(data);
        }
    }
    //# small,300,300 | small,400,400
    private void InitWeapenData()
    {
        List<string> soltData = ExcelReader.GetList("HeroData", RoomController.instance.localPlayer.armory.hero.ToString(),"Solt");
        foreach(var solt in soltData)
        {
            WeapenSoltItem item = GameEntry.ObjectPoolComponent.Get("WeapenSolt").GetComponent<WeapenSoltItem>();
            item.transform.SetParent(weapenContent);
            string[] data = solt.Split(',');
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(data[0]),int.Parse(data[1]));
            weapenItems.Add(item);
        }
    }
    private void Update()
    {
        if (!isDrag)
        {
            currentItem = RaycastItem<ProductItem>();
            WaitTime();
        }
        else
        {
            dragImage.transform.position = Input.mousePosition;
        }
        property.text = RoomController.instance.localPlayer.property.ToString();
    }
    public void WaitTime()
    {
        if (currentItem == last) return;
        last = currentItem;
        info.gameObject.SetActive(false);
        if(last != null)
        {
            timer.Reset();
            timer.Lanuch();
        }
        else
        {
            timer.Pause();
            info.gameObject.SetActive(false);
        }
    }
    public void OnTimerComplete()
    {
        timer.Pause();
        Vector3 position = Input.mousePosition;
        info.position = new Vector3(position.x - info.rect.width / 2,position.y - info.rect.height /2, 0);
        info.gameObject.SetActive(true);
        info.GetChild(0).GetComponent<Image>().sprite = currentItem.GetWeapenImage();
        info.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{currentItem.GetData().name}\n{currentItem.GetData().info}";
    }
    public T RaycastItem<T>() where T : MonoBehaviour
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
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
        if (eventData.button != PointerEventData.InputButton.Left || currentItem == null)
            return;
        isDrag = false;
        dragImage.gameObject.SetActive(false);
        dragImage.sprite = null;
        dragImage.transform.position = Vector3.zero;
        WeapenSoltItem result = RaycastItem<WeapenSoltItem>();
        if(result!= null)
        {
            if (RoomController.instance.localPlayer.property >= currentItem.GetCost())
            {
                RoomController.instance.localPlayer.property -= currentItem.GetCost();
                result.Equip(weapenItems.IndexOf(result) + 1,currentItem.GetWeapen(), currentItem.GetCost());
                RefreshWeapen();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (currentItem != null)
        {
            isDrag = true;
            dragImage.sprite = currentItem.GetWeapenImage();
            dragImage.gameObject.SetActive(true);
        }
    }
    protected void RefreshWeapen()
    {
        foreach(var item in weapenItems)
        {
            item.weapen.gameObject.SetActive(false);
        }
        foreach(var item in RoomController.instance.localPlayer.hero.weapenDic)
        {
            if(item.Value.weapen != null)
            {
                weapenItems[item.Key-1].weapen.gameObject.SetActive(true);
                weapenItems[item.Key-1].weapen.sprite = GameEntry.ResourceComponent.GetWeapenImage(item.Value.weapen.name);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            WeapenSoltItem result = RaycastItem<WeapenSoltItem>();
            if (result != null)
                result.Sell();
            RefreshWeapen();
        }
    }
}