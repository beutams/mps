using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : DoubleClick
{
    public Image img;
    public TextMeshProUGUI titleObject;
    public TextMeshProUGUI descriptionObject;
    public TextMeshProUGUI gameModeObject;
    public TextMeshProUGUI playerObject;

    protected DiscoveryResponse server;
    protected RoomData roomData;
    public string number;
    public void Start()
    {
        onDoubleClick += OnJoin;
    }
    public void SetData(DiscoveryResponse server)
    {
        roomData = server.roomData;
        this.server = server;
    }
    protected virtual void Refresh()
    {
        titleObject.text = roomData.title; 
        descriptionObject.text = roomData.description;
        gameModeObject.text = roomData.gameMode;
        playerObject.text = number + "/" + roomData.maxNumber;
    }
    protected virtual void OnJoin()
    {
        FindAnyObjectByType<GameDiscovery>().StopDiscovery();
        NetworkManager.singleton.StartClient(server.uri);
    }
}
