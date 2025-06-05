using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : MonoBehaviour
{
    public Transform items;
    public GameObject itemPrefab;
    private void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
        List<ArmoryItem> allItems = GetAllItems();
        foreach (ArmoryItem item in allItems)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.transform.parent = items;
        }
    }
    public List<ArmoryItem> GetAllItems()
    {
        return new List<ArmoryItem>() { new ArmoryItem(), new ArmoryItem() };
    }
    public void OnClick()
    {
        items.gameObject.SetActive(!items.gameObject.activeSelf);
    }
}
