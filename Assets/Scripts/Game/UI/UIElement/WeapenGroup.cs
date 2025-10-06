using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeapenGroup : MonoBehaviour
{
    protected Transform active;
    protected Transform disActive;
    protected TextMeshProUGUI indexText;
    private void Awake()
    {
        active = transform.Find("Auto/Active");
        disActive = transform.Find("Auto/DisActive");
        indexText = transform.Find("Index").GetComponent<TextMeshProUGUI>();
    }
    public void Init(int index)
    {
        indexText.text = index.ToString();
    }
    public void Refresh(bool flag)
    {
        Debug.Log($"{name} is change auto fire to {flag}");
        active.gameObject.SetActive(flag);
        disActive.gameObject.SetActive(!flag);
    }
}
