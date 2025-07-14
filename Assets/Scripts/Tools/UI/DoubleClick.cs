using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DoubleClick : MonoBehaviour, IPointerClickHandler
{
    public Action onClick;
    public Action onDoubleClick;
    protected float doubleClickTime = 0.1f;
    private int clickCount = 0;
    private float curTime;
    private void Update()
    {
        if(clickCount > 0)
        {
            if (curTime > doubleClickTime)
            {
                if (clickCount == 1)
                    onClick?.Invoke();
                else if (clickCount == 2)
                    onDoubleClick?.Invoke();
                clickCount = 0;
            }
            if(clickCount >= 2)
            {
                clickCount = 0;
                onDoubleClick?.Invoke();
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount++;
    }
}
