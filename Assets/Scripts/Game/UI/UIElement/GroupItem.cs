using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GroupItem : MonoBehaviour
{
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI text;
    public GroupBase groupBase {  get; set; }
    protected ShopUI shopUI;
    public void Init(GroupBase parent,ShopUI shop)
    {
        this.shopUI = shop;
        ChangeGroup(parent);
        SetActive(false);
    }
    public void Refresh(Sprite img,string content)
    {
        icon.sprite = img;
        text.text = content;
    }
    public void ChangeGroup(GroupBase parent)
    {
        groupBase = parent;
        transform.SetParent(parent.transform);
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
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
    public Sprite GetSprite()
    {
        return icon.sprite;
    }
    public string GetText()
    {
        return text.text;
    }
}
