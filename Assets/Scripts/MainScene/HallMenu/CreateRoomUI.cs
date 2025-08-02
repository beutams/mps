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
    [SerializeField] protected TextMeshProUGUI gameMode;
    [SerializeField] protected TextMeshProUGUI maxPlayers;
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
        GameEntry.EventComponent.Notify(GameEvent.CreateRoomEvent, new RoomData("owner", chapter.itemText.text, roomName.text, description.text, gameMode.text, maxPlayers.text));
        Clear();
    }
    protected void Clear()
    {

    }
}