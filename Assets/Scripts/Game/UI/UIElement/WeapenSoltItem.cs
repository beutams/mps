using UnityEngine;
using UnityEngine.UI;

public class WeapenSoltItem : MonoBehaviour
{
    public Image weapen;
    public int cost;
    protected int index;
    public void Equip(int index,WeapenBase weapen,int cost)
    {
        Debug.Log($"Hero Equip Weapen :{weapen}");
        this.cost = cost;
        this.index = index;
        RoomController.instance.localPlayer.hero.Equip(index, weapen);
    }
    public void Sell()
    {
        RoomController.instance.localPlayer.hero.UnEquip(index);
        RoomController.instance.localPlayer.property += cost;
        cost = 0;
    }
}