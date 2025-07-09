using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductItem : MonoBehaviour
{
    public ShopUI shopUI;
    public ProductData data;
    protected Image img;
    protected TextMeshProUGUI cost;
    private void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        cost = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void Refresh()
    {
        img.sprite = GameEntry.ResourceComponent.GetImage(data.imgPath);
        cost.text = data.cost.ToString();
    }
}