using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapenGroup : MonoBehaviour
{
    protected Transform active;
    protected Transform disActive;
    private void Awake()
    {
        active = transform.Find("Auto/Active");
        disActive = transform.Find("Auto/DisActive");
    }
    public void Refresh(bool flag)
    {
        Debug.Log($"{name} is change auto fire to {flag}");
        active.gameObject.SetActive(flag);
        disActive.gameObject.SetActive(!flag);
    }
}
