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
        ReceiverSkill();
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
        if (IRoomController.Instance().localPlayer.hero == null) return;
        if (InputManager.instance.GetMove() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.CompareTag("GameObject"))
            {
                IRoomController.Instance().localPlayer.hero.SetMoveTarget(hit.transform.GetComponent<GameObjectController>(), hit.point);
            }
            else if (hit.collider != null)
            {
                IRoomController.Instance().localPlayer.hero.SetMoveTarget(null, hit.point);
            }
        }
    }
    public void ReceiverFire()
    {
        if (IRoomController.Instance().localPlayer.hero == null) return;
        if (InputManager.instance.GetFire() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.CompareTag("GameObject"))
            {
                IRoomController.Instance().localPlayer.hero.SetTarget(hit.transform.GetComponent<GameObjectController>(), hit.point);
            }
            else if (hit.collider != null)
            {
                IRoomController.Instance().localPlayer.hero.SetTarget(null, hit.point);
            }
        }
    }
    public void ReceiverSkill()
    {
        byte skills = InputManager.instance.GetSkill();
        if ((skills & 1) == 1) GlobalSkill.instance.DoSkill(1);
        if ((skills & 2) == 2) GlobalSkill.instance.DoSkill(2);
        if ((skills & 4) == 4) GlobalSkill.instance.DoSkill(3);
    }
}