using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using UnityEngine;

public abstract class SubUIBase : MonoBehaviour
{
    public static Dictionary<UIGroup, SubUIBase> allTitles = new Dictionary<UIGroup, SubUIBase> ();
    public static SubUIBase current;
    public UIGroup button;
    protected virtual void Awake()
    {
        allTitles.Add(button,this);
    }
    protected virtual void Update()
    {
        if (current == this)
        {
            OnStep();
        }
    }
    public static void ChangeUI(UIGroup button)
    {
        foreach (var kv in allTitles)
        {
            if(kv.Key == button)
            {
                kv.Value.gameObject.SetActive(true);
                kv.Value.OnOpen();
                current = kv.Value;
            }

            else
            {
                kv.Value.gameObject.SetActive(false);
                kv.Value.OnClose();
            }

        }
    }
    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
    protected virtual void OnStep()
    {

    }

}
