using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public InputActionAsset asset;
    private InputAction gatherAction;
    private InputAction shopAction;
    private InputAction exitAction;
    private InputAction skill1Action;
    private InputAction skill2Action;
    private InputAction skill3Action;
    private InputAction skill4Action;
    private InputAction fireAction;
    private InputAction moveAction;
    private InputAction lockAction;
    private InputAction weapen1Action;
    private InputAction weapen2Action;
    private InputAction weapen3Action;
    private InputAction weapen4Action;
    private InputAction weapen5Action;
    private InputAction weapen6Action;
    private InputAction weapen7Action;
    private InputAction weapen8Action;
    private InputAction weapen9Action;
    private InputAction weapenSwitchAuto1Action;
    private InputAction weapenSwitchAuto2Action;
    private InputAction weapenSwitchAuto3Action;
    private InputAction weapenSwitchAuto4Action;
    private InputAction weapenSwitchAuto5Action;
    private InputAction weapenSwitchAuto6Action;
    private InputAction weapenSwitchAuto7Action;
    private InputAction weapenSwitchAuto8Action;
    private InputAction weapenSwitchAuto9Action;
    private InputAction exit;
    private void Start()
    {
        gatherAction = asset.FindAction("Gather");
        shopAction = asset.FindAction("Shop");
        exitAction = asset.FindAction("Exit");
        skill1Action = asset.FindAction("Skill1");
        skill2Action = asset.FindAction("Skill2");
        skill3Action = asset.FindAction("Skill3");
        skill4Action = asset.FindAction("Skill4");
        fireAction = asset.FindAction("Fire");
        moveAction = asset.FindAction("Move");
        lockAction = asset.FindAction("Lock");
        weapen1Action = asset.FindAction("Weapen1");
        weapen2Action = asset.FindAction("Weapen2");
        weapen3Action = asset.FindAction("Weapen3");
        weapen4Action = asset.FindAction("Weapen4");
        weapen5Action = asset.FindAction("Weapen5");
        weapen6Action = asset.FindAction("Weapen6");
        weapen7Action = asset.FindAction("Weapen7");
        weapen8Action = asset.FindAction("Weapen8");
        weapen9Action = asset.FindAction("Weapen9");
        weapenSwitchAuto1Action = asset.FindAction("WeapenSwitchAuto1");
        weapenSwitchAuto2Action = asset.FindAction("WeapenSwitchAuto2");
        weapenSwitchAuto3Action = asset.FindAction("WeapenSwitchAuto3");
        weapenSwitchAuto4Action = asset.FindAction("WeapenSwitchAuto4");
        weapenSwitchAuto5Action = asset.FindAction("WeapenSwitchAuto5");
        weapenSwitchAuto6Action = asset.FindAction("WeapenSwitchAuto6");
        weapenSwitchAuto7Action = asset.FindAction("WeapenSwitchAuto7");
        weapenSwitchAuto8Action = asset.FindAction("WeapenSwitchAuto8");
        weapenSwitchAuto9Action = asset.FindAction("WeapenSwitchAuto9");
        exit = asset.FindAction("Exit");
    }
    private HeroController hero => RoomController.instance.localPlayer.hero;

    public bool GetGather() => gatherAction.WasPerformedThisFrame();
    public bool GetShop() => shopAction.WasPerformedThisFrame();
    public bool GetExit() => exitAction.IsPressed();
    public byte GetSkill()
    {
        byte index = 0;
        if (skill1Action.IsPressed())
            index += 1;
        if (skill2Action.IsPressed())
            index += 2;
        if (skill3Action.IsPressed())
            index += 4;
        if (skill4Action.IsPressed())
            index += 8;
        return index;
    }
    public bool GetFire() => fireAction.IsPressed();
    public bool GetMove() => moveAction.WasPressedThisFrame();
    public bool GetLock() => lockAction.IsPressed();
    public bool GetEsc() => exit.WasPerformedThisFrame();
    public int GetWeapen()
    {
        if (weapen1Action.IsPressed()) return 1;
        if (weapen2Action.IsPressed()) return 2;
        if (weapen3Action.IsPressed()) return 3;
        if (weapen4Action.IsPressed()) return 4;
        if (weapen5Action.IsPressed()) return 5;
        if (weapen6Action.IsPressed()) return 6;
        if (weapen7Action.IsPressed()) return 7;
        if (weapen8Action.IsPressed()) return 8;
        if (weapen9Action.IsPressed()) return 9;
        return -1;
    }
    public int GetWeapenSwitch()
    {
        if (weapenSwitchAuto1Action.WasPerformedThisFrame()) return 1;
        if (weapenSwitchAuto2Action.WasPerformedThisFrame()) return 2;
        if (weapenSwitchAuto3Action.WasPerformedThisFrame()) return 3;
        if (weapenSwitchAuto4Action.WasPerformedThisFrame()) return 4;
        if (weapenSwitchAuto5Action.WasPerformedThisFrame()) return 5;
        if (weapenSwitchAuto6Action.WasPerformedThisFrame()) return 6;
        if (weapenSwitchAuto7Action.WasPerformedThisFrame()) return 7;
        if (weapenSwitchAuto8Action.WasPerformedThisFrame()) return 8;
        if (weapenSwitchAuto9Action.WasPerformedThisFrame()) return 9;
        return -1;
    }
    public void CameraMove()
    {
        if (!GameEntry.UIComponent.IsGameUI()) return;
        float x = Input.mousePosition.x / Screen.width;
        float y = Input.mousePosition.y / Screen.height;
        float xDir = x < 0.05f && x > -0.1 ? -1 : 0;
        xDir = x > 0.95f && x < 1.1f ? 1 : xDir;
        float yDir = y < 0.05f && y > -0.1 ? -1 : 0;
        yDir = y > 0.95f && y < 1.1f ? 1 : yDir;
        Vector2 direction = new Vector2(xDir, yDir);
        Camera.main.transform.position = Camera.main.transform.position + Tools.V2ToV3(direction) * GameEntry.SettingComponent.settingData.CameraMoveSpeed * Time.deltaTime;
    }
}