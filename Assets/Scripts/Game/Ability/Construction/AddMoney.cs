using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AddMoney", menuName = "ScriptableObject/Universal/AddMoney")]
public class AddMoney : AutoAbility
{
    public int money;
    public override void OnTimerComplete()
    {
        base.OnTimerComplete();
        RoomController.instance.localPlayer.property += money;
    }
}
