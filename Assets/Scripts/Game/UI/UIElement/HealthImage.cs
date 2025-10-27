using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HealthImage : MonoBehaviour, ID
{
    private Image health;
    private float maxWidth;
    public float distance {  get; set; }

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
/*        Vector3 viewDir = (objPosition - Camera.main.transform.position).normalized;
        Vector3 verticalDown = -Vector3.up; // 竖直向下的向量
        Vector3 rotationAxis = Vector3.Cross(viewDir.normalized, verticalDown).normalized;
        Quaternion rotation = Quaternion.AngleAxis(-90, rotationAxis);
        Vector3 rotatedDirection = rotation * viewDir;
        Vector3 targetPos = objPosition + rotatedDirection * distance;*/

        Vector3 objViewPosition = Camera.main.WorldToScreenPoint(objPosition);
        Vector3 objTopViewPosition = Camera.main.WorldToScreenPoint(objPosition + Vector3.up * distance);
        transform.position = new Vector3(objViewPosition.x, objTopViewPosition.y, objTopViewPosition.z);
        health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, curHealth / maxHealth * maxWidth);
    }
}