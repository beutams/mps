using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HealthImage : MonoBehaviour
{
    private Image health;
    private float maxWidth;
    private void Awake()
    {
        health = transform.GetChild(0).GetComponent<Image>();
        maxWidth = health.rectTransform.rect.width;
    }
    public void Locate(Vector3 objPosition,float curHealth, float maxHealth)
    {
        transform.position = Camera.main.WorldToScreenPoint(objPosition + Vector3.up * 3);
        health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, curHealth / maxHealth * maxWidth);
    }
}