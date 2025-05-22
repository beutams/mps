using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected static Dictionary<string, List<UIToggle>> globalDic = new Dictionary<string, List<UIToggle>>();
    protected List<GameObject> status = new List<GameObject>();
    [SerializeField] public string group;
    [SerializeField] public bool isSwitch;
    [SerializeField] public GameObject Stay;
    [SerializeField] public GameObject Default;
    [SerializeField] public GameObject Select;

    [SerializeField] public UnityEvent onEnter;
    [SerializeField] public UnityEvent onExit;
    [SerializeField] public UnityEvent onClick;
    protected bool isOn;
    protected void Awake()
    {
        status.Add(Stay);
        status.Add(Default);
        status.Add(Select);
        if (!globalDic.ContainsKey(group))
        {
            globalDic.Add(group, new List<UIToggle>());
            globalDic[group].Add(this);
        }
        globalDic[group].Add(this);
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
                    ui.isOn = false;
                    ui.Refresh(Default);
                }
                isOn = true;
                onClick?.Invoke();
                Refresh(Select);
            }
        }
        else
        {
            if(isSwitch || !isOn)
            {
                isOn = !isOn;
                onClick?.Invoke();
                Refresh(isOn ? Select : Stay);
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Stay == null) return;
        if (!isOn)
        {
            onEnter?.Invoke();
            Refresh(Stay);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Default == null) return;
        if (!isOn)
        {
            onExit?.Invoke();
            Refresh(Default);
        }
    }
}
