using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroupBase : MonoBehaviour
{
    public TextMeshProUGUI title;
    public int group { get; private set; }
    public void Init(string name,Transform parent,int group)
    {
        title.text = name;
        transform.SetParent(parent);
        this.group = group;
    }
}
