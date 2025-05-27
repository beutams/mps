using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIToggle : UIGroup, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected List<GameObject> status = new List<GameObject>();
    [SerializeField] public bool isSwitch;
    [SerializeField] public GameObject Stay;
    [SerializeField] public GameObject Default;
    [SerializeField] public GameObject Select;

    [SerializeField] public UnityEvent<UIToggle> onEnter;
    [SerializeField] public UnityEvent<UIToggle> onExit;
    [SerializeField] public UnityEvent<UIToggle> onClick;
    protected bool isOn;
    protected override void Awake()
    {
        status.Add(Stay);
        status.Add(Default);
        status.Add(Select);
        if(Default != null)
        {
            Refresh(Default);
        }
    }
    protected void Refresh(GameObject active)
    {
        foreach(var item in status)
        {
            item.SetActive(item == active);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Select == null) return;
        if(group != string.Empty && !isSwitch)
        {
            if (globalDic.ContainsKey(group))
            {
                foreach(var ui in globalDic[group])
                {
                    //ui.isOn = false;
                    //ui.Refresh(Default);
                }
                isOn = true;
                onClick?.Invoke(this);
                Refresh(Select);
            }
        }
        else
        {
            if(isSwitch || !isOn)
            {
                isOn = !isOn;
                onClick?.Invoke(this);
                Refresh(isOn ? Select : Stay);
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        if (Stay == null) return;
        if (!isOn)
        {
            onEnter?.Invoke(this);
            Refresh(Stay);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        if (Default == null) return;
        if (!isOn)
        {
            onExit?.Invoke(this);
            Refresh(Default);
        }
    }
}
