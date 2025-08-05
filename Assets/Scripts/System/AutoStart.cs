using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoStart : MonoBehaviour
{
    public ArmorySubUI armory;
    public bool single;
    public bool createRoom;
    public bool joinRoomFirst;
    protected bool isdo;
    protected float timer;
    void Update()
    {
        timer+= Time.deltaTime;
        if (isdo)
            return;
        else if (single)
            AutoStartSingle();
        else if (createRoom)
            AutoStartCreateRoom();
        else if (joinRoomFirst)
            AutoStartJoinRoom();
    }
    public void AutoStartSingle()
    {
        armory.SetData(ArmorySubUI.ArmoryType.Hero, 0);
        armory.SetData(ArmorySubUI.ArmoryType.GlobalSkillsAdd, 0);
        armory.SetData(ArmorySubUI.ArmoryType.GlobalSkillsAdd, 1);
        armory.SetData(ArmorySubUI.ArmoryType.GlobalSkillsAdd, 2);
        SingleSubUI obj = FindAnyObjectByType<SingleSubUI>();
        if (obj.GetComponent<SingleSubUI>())
        {
            obj.GetComponent<SingleSubUI>().OnEnterClick();
            Destroy(this);
        }
        isdo = true;
    }
    public void AutoStartCreateRoom()
    {
        if (timer < 1f) return;
        HallSubUI obj = SubUIBase.allTitles[UIGroup.globalDic["TitleMenu"].Find(s => s.name == "Hall")] as HallSubUI;
        UIGroup.globalDic["TitleMenu"].Find(s => s.name == "Hall").GetComponent<MainPanelButton>()?.OnPointerClick(null);
        GameEntry.EventComponent.Notify(GameEvent.CreateRoomEvent, new RoomData("test", "test", "test", "test", "test", "test"));
        isdo = true;
    }
    public void AutoStartJoinRoom()
    {
        if (timer < 1f) return;
        HallSubUI obj = SubUIBase.allTitles[UIGroup.globalDic["TitleMenu"].Find(s => s.name == "Hall")] as HallSubUI;
        UIGroup.globalDic["TitleMenu"].Find(s => s.name == "Hall").GetComponent<MainPanelButton>()?.OnPointerClick(null);
        if (obj.rooms.Count > 0)
        {
            obj.rooms.First().Key.OnJoin();
            isdo = true;
        }
    }
}
