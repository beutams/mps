using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HealthImage : MonoBehaviour, ID
{
    private Image health;
    private float maxWidth;

    [SerializeField] private int id;
    [SerializeField] private IDType type;
    public int ID => id;

    public IDType searchName => type;

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