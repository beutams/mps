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

    }
    public virtual void Close()
    {
        GameEntry.UIComponent.CloseUI(this);
    }
}
