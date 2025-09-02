using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ProductItem : MonoBehaviour
{
    protected ProductData data;
    protected Sprite sprite;
    public Image img;
    public TextMeshProUGUI cost;
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
}