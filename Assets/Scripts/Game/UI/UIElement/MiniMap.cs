using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isDrag;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
    }
    private void Update()
    {
        if (isDrag) LocateCamera();
    }

    public void LocateCamera()
    {
        Vector2 position = Input.mousePosition;
        Vector2 min = new Vector2(rectTransform.position.x - rectTransform.rect.width / 2, rectTransform.position.y - rectTransform.rect.height / 2);
        Vector3 percent = new Vector3(Mathf.Clamp01((position.x - min.x) / rectTransform.rect.width),0, Mathf.Clamp01((position.y - min.y) / rectTransform.rect.height));
        Vector3 targetPosition = percent * GameEntry.SettingComponent.settingData.mapSize;

        float angleX = 90 - Camera.main.transform.rotation.eulerAngles.x;
        float height = Camera.main.transform.position.y;
        float forwardOffset = Mathf.Tan(Mathf.Deg2Rad * angleX) * height;
        float angleY = Camera.main.transform.rotation.eulerAngles.y;

        Vector3 vector = - new Vector3(Mathf.Sin(angleY) * forwardOffset, 0, Mathf.Cos(angleY) * forwardOffset) + targetPosition;
        Camera.main.transform.position = vector + new Vector3(0, height, 0);
    }
}
