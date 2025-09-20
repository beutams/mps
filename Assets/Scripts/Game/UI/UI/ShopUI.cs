using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : UIBase, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerClickHandler
{
    [Header("Shop")]
    public Transform shopContent;
    protected static List<ProductItem> allItem = new List<ProductItem>();
    [Header("Weapen")]
    public Transform weapenContent;
    protected List<WeapenSoltItem> weapenItems = new List<WeapenSoltItem>();
    [Header("Group")]
    public Transform group;
    public Transform groupDragItem;
    public Image groupDragImage;
    public TextMeshProUGUI groupDragText;
    protected Dictionary<WeapenSoltItem, GroupItem> groupDic = new Dictionary<WeapenSoltItem, GroupItem>();
    protected List<GroupBase> groupList = new List<GroupBase>();
    [Header("Else")]
    public RectTransform info;
    public Image dragImage;
    public TextMeshProUGUI property;

    protected ProductItem last;
    protected ProductItem currentItem;
    protected bool isDrag;
    protected Timer timer;

    protected GroupItem currentGroupItem;

    #region Init
    private void Start()
    {
        InitTimer();
        InitShopData();
        InitGroup();
        InitWeapenData();
    }
    public override void OnOpen()
    {
        base.OnOpen();
        Refresh();
    }
    private void InitGroup()
    {
        foreach(var kvp in RoomController.instance.localPlayer.hero.weapenGroup)
        {
            GroupBase groupBase = GameEntry.ObjectPoolComponent.Get("GroupBase").GetComponent<GroupBase>();
            groupBase.Init($"Group{kvp.Key}", group, kvp.Key);
            groupList.Add(groupBase);
        }
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
        int i = 0;
        foreach(var solt in soltData)
        {
            WeapenSoltItem item = GameEntry.ObjectPoolComponent.Get("WeapenSolt").GetComponent<WeapenSoltItem>();
            item.model = RoomController.instance.localPlayer.hero.weapenDic[i+1];
            item.transform.SetParent(weapenContent);
            string[] data = solt.Split(',');
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(data[0]),int.Parse(data[1]));
            weapenItems.Add(item);

            GroupItem gItem = GameEntry.ObjectPoolComponent.Get("GroupItem").GetComponent<GroupItem>();
            gItem.Init(groupList.First(),this);
            groupDic.Add(item, gItem);
            i++;
        }
    }
    #endregion

    #region MouseEvent
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            WeapenSoltItem result = RaycastItem<WeapenSoltItem>();
            if (result != null)
                result.Sell();
            Refresh();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        isDrag = true;
        OnWeapenBeginDrag();
        OnGroupBeginDrag();
    }
    protected void OnWeapenBeginDrag()
    {
        if (currentItem != null)
        {
            dragImage.sprite = currentItem.GetWeapenImage();
            dragImage.gameObject.SetActive(true);
        }
    }
    protected void OnGroupBeginDrag()
    {
        if (currentGroupItem != null)
        {
            groupDragImage.sprite = currentGroupItem.GetSprite();
            groupDragText.text = currentGroupItem.GetText();
            groupDragItem.gameObject.SetActive(true);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        isDrag = false;
        OnWeapenEndDrag();
        OnGroupEndDrag();
    }
    protected void OnWeapenEndDrag()
    {
        if(currentItem != null)
        {
            dragImage.gameObject.SetActive(false);
            dragImage.sprite = null;
            isDrag = false;
            dragImage.transform.position = Vector3.zero;
            WeapenSoltItem result = RaycastItem<WeapenSoltItem>();
            if (result != null)
            {
                if (RoomController.instance.localPlayer.property >= currentItem.GetCost())
                {
                    RoomController.instance.localPlayer.property -= currentItem.GetCost();
                    result.Equip(weapenItems.IndexOf(result) + 1, currentItem.GetWeapen(), currentItem.GetCost());
                    Refresh();
                }
            }
        }
    }
    protected void OnGroupEndDrag()
    {
        if (currentGroupItem != null)
        {
            groupDragItem.gameObject.SetActive(false);
            groupDragImage.sprite = null;
            groupDragItem.transform.position = Vector3.zero;
            GroupBase result = RaycastItem<GroupBase>();
            if (result != null)
            {
                currentGroupItem.ChangeGroup(result);
                foreach(var kvp in groupDic)
                {
                    if(kvp.Value == currentGroupItem)
                    {
                        kvp.Key.ChangeGroup(result.group);
                    }
                }
                Refresh();
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        OnWeapenDrag();
        OnGroupDrag();
    }
    protected void OnWeapenDrag()
    {
        if(currentItem != null)
            dragImage.transform.position = Input.mousePosition;
    }
    protected void OnGroupDrag()
    {
        if (currentGroupItem != null)
            groupDragItem.transform.position = Input.mousePosition;
    }
    #endregion
    private void Update()
    {
        if (!isDrag)
        {
            currentGroupItem = RaycastItem<GroupItem>();
            currentItem = RaycastItem<ProductItem>();
            WaitTime();
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
        if (isDrag)
        {
            timer.Pause();
            return;
        }
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
    public void Refresh()
    {
        foreach(var item in weapenItems)
        {
            item.weapen.gameObject.SetActive(false);
            groupDic[item].SetActive(false);
        }
        foreach(var item in RoomController.instance.localPlayer.hero.weapenDic)
        {
            if(item.Value.weapen != null)
            {
                WeapenSoltItem solt = weapenItems[item.Key - 1];
                Sprite sprite = GameEntry.ResourceComponent.GetWeapenImage(item.Value.weapen.name);
                solt.weapen.gameObject.SetActive(true);
                solt.weapen.sprite = sprite;
                groupDic[solt].SetActive(true);
                groupDic[solt].Refresh(sprite, item.Value.weapen.name);
            }
        }
    }

}