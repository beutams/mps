using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using UnityEngine;

public abstract class SubUIBase : MonoBehaviour
{
    public static Dictionary<UIGroup, SubUIBase> allTitles = new Dictionary<UIGroup, SubUIBase> ();
    public UIGroup button;
    private void Awake()
    {
        allTitles.Add(button,this);
    }
    public static void ChangeUI(UIGroup button)
    {
        foreach (var kv in allTitles)
        {
            if(kv.Key == button)
            {
                kv.Value.gameObject.SetActive(true);
                kv.Value.OnOpen();
            }

            else
            {
                kv.Value.gameObject.SetActive(false);
                kv.Value.OnClose();
            }

        }
    }
    protected abstract void OnOpen();
    protected abstract void OnClose();
}
