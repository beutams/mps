using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 size;
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = size;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
