using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductItem : MonoBehaviour
{
    protected ProductData data;
    public Image img;
    public TextMeshProUGUI cost;
    public void Refresh(ProductData data)
    {
        this.data = data;
        img.sprite = GameEntry.ResourceComponent.GetImage(data.imgPath);
        cost.text = data.cost.ToString();
    }
    public Weapen GetWeapen()
    {
        return data.weapen;
    }
}