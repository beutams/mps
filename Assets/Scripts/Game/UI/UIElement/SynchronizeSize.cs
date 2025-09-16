using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class SynchronizeSize : MonoBehaviour
{
    public RectTransform synchronizeTarget;
    public float offset;

    public bool allChild;
    public bool width;
    public bool height;

    protected RectTransform rectTransform;
    protected void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    protected void Update()
    {
        float heightR = synchronizeTarget.rect.height, widthR = synchronizeTarget.rect.width;
        if(allChild)
        {
            heightR = widthR = 0;
            for(int i = 0; i < synchronizeTarget.childCount; i++)
            {
                RectTransform rect = synchronizeTarget.GetChild(i).GetComponent<RectTransform>();
                heightR += rect.rect.height;
                widthR += rect.rect.width;
            }
        }
        if (height)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightR + offset);
        if (width)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthR + offset);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}
