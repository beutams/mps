using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeapenUIItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI weapenName;
    [SerializeField] TextMeshProUGUI bulletCount;
    [SerializeField] RectTransform reloadBar;
    private float loadBarWidth = 50f;
    private WeapenBase weapen;
    private void Awake()
    {
        loadBarWidth = reloadBar.rect.width;
    }
    public void SetWeapen(WeapenBase weapen)
    {
        this.weapen = weapen;
        weapenName.text = weapen.name;
    }
    public void Update()
    {
        bulletCount.text = weapen.bulletCount.ToString();
        reloadBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, weapen.loadTimer.GetProgress() * loadBarWidth);
    }
}
