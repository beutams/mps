using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    public HallSubUI hallSubUI;
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI description;

    protected string owner;
    protected string chapter;
    protected string gameMode;
    protected string maxPlayer;
    public void Init()
    {

    }
    public void Create()
    {
        hallSubUI.OnCreateRoom(new RoomData(owner, chapter, roomName.text, description.text, gameMode, maxPlayer));
        Clear();
    }
    protected void Clear()
    {

    }
}
