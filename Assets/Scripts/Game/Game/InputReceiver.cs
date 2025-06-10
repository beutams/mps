using Mirror;
using UnityEngine;

public class InputReceiver : SingletonMonoBehaviour<InputReceiver> 
{
    private void Update()
    {
        InputManager.instance.CameraMove();
        ReceiverGather();
        ReceiverShop();
        ReceiverMove();
        ReceiverFire();
    }
    public void ReceiverGather()
    {
        if (InputManager.instance.GetGather() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit))
        {
            if(hit.collider != null)
            {
                foreach (var unit in IRoomController.Instance().localPlayer.unitList)
                {
                    unit.SetMoveTarget(null, hit.point);
                }
            }
        }
    }
    public void ReceiverShop()
    {
        if (InputManager.instance.GetShop())
        {

        }
    }
    public void ReceiverMove()
    {
        if (InputManager.instance.GetMove())
        {

        }
    }
    public void ReceiverFire()
    {
        if (InputManager.instance.GetFire())
        {

        }
    }
}