using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : UIBase, IPointerDownHandler, IPointerUpHandler
{
    public Transform shopContent;
    public RectTransform info;
    public Image dragImage;
    public Transform weapenContent;
    protected ProductItem last;
    protected ProductItem currentItem;
    protected bool isDrag;
    protected List<Weapen> productList = new List<Weapen>();
    protected Timer timer;
    public static List<ProductItem> allItem = new List<ProductItem>();
    private void Start()
    {
        timer = new Timer();
        timer.Init(3f, OnTimerComplete, false, false);
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