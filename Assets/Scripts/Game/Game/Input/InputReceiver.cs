using Mirror;
using UnityEngine;

public class InputReceiver : SingletonMonoBehaviour<InputReceiver> 
{
    private void Update()
    {
        if (!RoomController.instance.gameReady) return;
        InputManager.instance.CameraMove();
        ReceiverShop();
        ReceiverEsc();
        if (GameEntry.UIComponent.GetTopUI() is GameUI)
        {
            ReceiverGather();
            ReceiverMove();
            ReceiverFire();
            ReceiverSkill();
        }
    }
    public void ReceiverGather()
    {
        if (InputManager.instance.GetGather() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit))
        {
            if(hit.collider != null)
            {
                GroupMovementManager.instance.CaclTargetPoint(RoomController.instance.localPlayer.unitList, hit.point);
            }
        }
    }
    public void ReceiverEsc()
    {
        if (InputManager.instance.GetEsc())
        {
            if (GameEntry.UIComponent.GetTopUI() is GameUI)
            {
                GameEntry.UIComponent.ShowUI("EscUI");
            }
            else if(GameEntry.UIComponent.GetTopUI() is EscUI)
            {
                GameEntry.UIComponent.CloseUI("EscUI");
            }
        }
    }
    public void ReceiverShop()
    {
        if (InputManager.instance.GetShop())
        {
            if (GameEntry.UIComponent.GetTopUI() is GameUI)
            {
                GameEntry.UIComponent.ShowUI("ShopUI");
            }
            else if (GameEntry.UIComponent.GetTopUI() is ShopUI)
            {
                GameEntry.UIComponent.CloseUI("ShopUI");
            }
        }
    }
    public void ReceiverExit()
    {
        if (InputManager.instance.GetShop())
        {
            GameEntry.UIComponent.ShowUI("ExitUI");
        }
    }
    public void ReceiverMove()
    {
        if (RoomController.instance.localPlayer.hero == null) return;
        if (InputManager.instance.GetMove() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.CompareTag("GameObject"))
            {
                RoomController.instance.localPlayer.hero.SetMoveTarget(hit.transform.GetComponent<GameObjectController>(), hit.point);
            }
            else if (hit.collider != null)
            {
                RoomController.instance.localPlayer.hero.SetMoveTarget(null, hit.point);
            }
        }
    }
    public void ReceiverFire()
    {
        if (RoomController.instance.localPlayer.hero == null) return;
        if (InputManager.instance.GetFire() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.CompareTag("GameObject"))
            {
                RoomController.instance.localPlayer.hero.SetTarget(hit.transform.GetComponent<GameObjectController>(), hit.point);
            }
            else if (hit.collider != null)
            {
                RoomController.instance.localPlayer.hero.SetTarget(null, hit.point);
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
    public void ReceiverWeapenSwitchAuto()
    {
        int index = 0;
        index = InputManager.instance.GetWeapenSwitch();
        if (index != 0)
            RoomController.instance.localPlayer.hero.ChangeAutoStatu(index);
    }
}