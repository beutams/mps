using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeapenGroup : MonoBehaviour
{
    protected Transform active;
    protected Transform disActive;
    protected TextMeshProUGUI indexText;

    protected GameObject backDisActive;
    protected GameObject backActive;
    protected int index;
    private void Awake()
    {
        active = transform.Find("Auto/Active");
        disActive = transform.Find("Auto/DisActive");
        backActive = transform.Find("BackActive").gameObject;
        backDisActive = transform.Find("BackDisActive").gameObject;
        indexText = transform.Find("Index").GetComponent<TextMeshProUGUI>();
    }
    public void Init(int index)
    {
        indexText.text = index.ToString();
        this.index = index;
    }
    public void Refresh(bool flag)
    {
        Debug.Log($"{name} is change auto fire to {flag}");
        active.gameObject.SetActive(flag);
        disActive.gameObject.SetActive(!flag);
        bool unlock = RoomController.instance.localPlayer.hero.GetCurrentGroup() == index;
        backActive.gameObject.SetActive(unlock);
        backDisActive.gameObject.SetActive(!unlock);
    }
}
