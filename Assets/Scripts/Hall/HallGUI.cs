using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HallGUI : MonoBehaviour
{
    public GameObject roomPerfab;
    [Header("MenuUI")]
    public GameObject menuUI;
    public Button matchButton;
    public Button hallButton;
    [Header("HallUI")]
    public GameObject hallUI;
    public Button createButton;
    public Button joinButton;
    public Button exitButton;
    public GameObject content;
    [Header("RoomUI")]
    public GameObject roomUI;
    public GameObject players;
    public Button startReadyButton;
    public Button cancelLeaveButton;
    private TextMeshProUGUI startReadyText;
    private TextMeshProUGUI cancelLeaveText;

    private Dictionary<PlayerSite, Transform> playerUIContent;
    private List<GameObject> objs = new List<GameObject>();
    private PlayerSite me;
    private HallManager hallManager;
    private void Awake()
    {
        hallManager = FindAnyObjectByType<HallManager>();
        //matchButton.onClick.AddListener(OnMatchClick);
        createButton.onClick.AddListener(OnCreateClick);
        joinButton.onClick.AddListener(OnJoinClick);
        hallButton.onClick.AddListener(OnHallClick);
        exitButton.onClick.AddListener(OnExitClick);
        startReadyButton.onClick.AddListener(OnStartReadyClick);
        cancelLeaveButton.onClick.AddListener(OnCancelLeaveClick);
        menuUI.SetActive(true);
        hallUI.SetActive(false);
        roomUI.SetActive(false);
        startReadyText = startReadyButton.GetComponentInChildren<TextMeshProUGUI>();
        cancelLeaveText = cancelLeaveButton.GetComponentInChildren<TextMeshProUGUI>();
        playerUIContent = new Dictionary<PlayerSite, Transform>();
        for(int i = 0;i < players.transform.childCount; i++)
        {
            playerUIContent.Add((PlayerSite)(i + 1), players.transform.GetChild(i));
        }
    }
    #region Button
    private void OnHallClick()
    {
        menuUI.SetActive(false);
        hallUI.SetActive(true);
        RefreshRoomList();
    }
    private void OnExitClick()
    {
        menuUI.SetActive(true);
        hallUI.SetActive(false);
    }
    private void OnCreateClick()
    {
        hallManager.RequestCreateRoom();
    }
    private void OnJoinClick()
    {
        hallManager.RequestJoinRoom();
    }
    private void OnCancelLeaveClick()
    {
        if (hallManager.isOwner)
        {
            hallManager.RequestCancelRoom();
        }
        else
        {
            hallManager.RequestLeaveRoom();
        }
    }
    private void OnStartReadyClick()
    {
        if (hallManager.isOwner)
        {
            hallManager.RequestStartRoom();
        }
        else
        {
            hallManager.RequestReadyChange();
        }
    }
    #endregion

    #region CallBack
    [ClientCallback]
    public void RefreshRoomList()
    {
        foreach(var obj in objs)
        {
            Destroy(obj);
        }
        objs.Clear();
        foreach (var item in HallManager.openRooms)
        {
            GameObject room = Instantiate(roomPerfab);
            objs.Add(room);
            room.transform.SetParent(content.transform);
            RoomGUI roomGUI = room.GetComponent<RoomGUI>();
            roomGUI.Init($"Room--{item.Value.players}/{item.Value.maxPlayers}", item.Key);
        }
    }
    [ClientCallback]
    public void OnJoinRoom(PlayerInfo[] infos)
    {
        roomUI.SetActive(true);
        hallUI.SetActive(false);
        if (hallManager.isOwner)
        {
            startReadyText.text = "Start";
            cancelLeaveText.text = "Cancel";
        }
        else
        {
            startReadyText.text = "Ready";
            cancelLeaveText.text = "Leave";
        }
        foreach(var item in infos)
        {
            if (item.playerSite == 0) continue;
            me = (PlayerSite)item.playerSite;
            playerUIContent[me].GetComponentInChildren<TextMeshProUGUI>().text = item.playerIndex.ToString();
        }
    }
    [ClientCallback]
    public void OnLeaveRoom()
    {
        ResetRoom();
        roomUI.SetActive(false);
        hallUI.SetActive(true);
        RefreshRoomList();
    }
    [ClientCallback]
    public void RefreshRoom(PlayerInfo[] infos)
    {
        ResetRoom();
        foreach(var item in playerUIContent)
        {
            foreach(var info in infos)
            {
                if(info.playerSite == (byte)item.Key)
                {
                    item.Value.GetComponentInChildren<TextMeshProUGUI>().text = info.playerIndex.ToString();
                    item.Value.GetComponent<Image>().color = info.ready ? Color.green : Color.gray;
                    break;
                }
            }
        }
        foreach (var item in infos)
        {
            playerUIContent[(PlayerSite)item.playerSite].GetComponentInChildren<TextMeshProUGUI>().text = item.playerIndex.ToString();
            playerUIContent[(PlayerSite)item.playerSite].GetComponent<Image>().color = item.ready ? Color.green : Color.gray;
        }
    }
    [ClientCallback]
    public void ResetRoom()
    {
        foreach(var item in playerUIContent.Values)
        {
            item.GetComponentInChildren<TextMeshProUGUI>().text = "";
            item.GetComponent<Image>().color = Color.gray;
        }
    }
    [ClientCallback]
    public void OnStartGame()
    {
        roomUI.gameObject.SetActive(false);
    }
    #endregion
}
public enum PlayerSite : byte
{
    NoCamp = 0,
    Left = 1,
    Right = 2,
}
