using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ProductItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected ProductData data;
    protected Sprite sprite;
    protected ShopUI shopUI;
    public Image img;
    public TextMeshProUGUI cost;

    protected float timer;
    protected bool enter;
    private void Start()
    {
        shopUI = GameObject.FindAnyObjectByType<ShopUI>();
    }
    public void Refresh(ProductData data)
    {
        this.data = data;
        sprite = GameEntry.ResourceComponent.GetWeapenImage(data.name);
        img.sprite = sprite;
        cost.text = data.cost.ToString();
    }
    public Sprite GetWeapenImage()
    {
        return sprite;
    }
    public WeapenBase GetWeapen()
    {
        return data.weapen;
    }
    public int GetCost()
    {
        return data.cost;
    }
    public ProductData GetData()
    {
        return data;
    }

    #region Info
    public void OnPointerExit(PointerEventData eventData)
    {
        enter = false;
        timer = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enter = true;
    }
    public void Update()
    {
        if (enter)
        {
            timer += Time.deltaTime;
            if (timer > GameEntry.SettingComponent.settingData.stayTime)
                OnShowInfo();
        }
    }
    public void OnShowInfo()
    {
        enter = false;
    }
    #endregion
}