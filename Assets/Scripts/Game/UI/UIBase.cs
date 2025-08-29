using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour, ID
{
    [Header("ID")]
    [SerializeField] protected IDType idType;
    [SerializeField] protected int id;
    public int ID => id;
    public IDType searchName => idType;
    public virtual void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if(canvas != null)
            transform.SetParent(canvas.transform);
    }
    public virtual void Close()
    {
        GameEntry.UIComponent.CloseUI(this);
    }
}
