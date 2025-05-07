using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HealthImage : NetworkBehaviour
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
        Camera.main.WorldToScreenPoint(objPosition);
        health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, curHealth / maxHealth * maxWidth);
    }
}