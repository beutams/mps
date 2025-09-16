using Mirror.BouncyCastle.Asn1.X500;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AddCommand", menuName = "ScriptableObject/Universal/AddCommand")]
public class AddCommand : AutoAbility
{
    public int command;
    public override void OnTimerComplete()
    {
        base.OnTimerComplete();
        RoomController.instance.localPlayer.AddCommand(command);
    }
}
