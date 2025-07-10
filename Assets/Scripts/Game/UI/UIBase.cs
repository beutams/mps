using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void Init()
    {

    }
    public virtual void Close()
    {
        GameEntry.UIComponent.CloseUI(this);
    }
}
