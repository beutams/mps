using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : UIBase
{
    protected HallSubUI hallSubUI;
    [SerializeField] protected TextMeshProUGUI roomName;
    [SerializeField] protected TextMeshProUGUI description;
    [SerializeField] protected TMP_Dropdown chapter;
    [SerializeField] protected TMP_Dropdown gameMode;
    [SerializeField] protected TMP_Dropdown maxPlayers;
    [SerializeField] protected Button createButton;
    [SerializeField] protected Button cancelButton;
    public override void Init()
    {
        base.Init();
        hallSubUI = FindAnyObjectByType<HallSubUI>();
        createButton.onClick.AddListener(OnCreateClick);
        cancelButton.onClick.AddListener(OnCancelClick);
    }
    protected void OnCancelClick()
    {
        GameEntry.UIComponent.CloseUI(this);
    }
    protected void OnCreateClick()
    {
        RoomData room = new RoomData("owner", chapter.itemText.text, roomName.text, description.text, gameMode.itemText.text, maxPlayers.itemText.text);
        GameEntry.EventComponent.Notify(GameEvent.CreateRoomEvent, room);
        Debug.Log($"Create Room {room}");
        Clear();
    }
    protected void Clear()
    {
        GameEntry.UIComponent.CloseUI(this);
    }
}