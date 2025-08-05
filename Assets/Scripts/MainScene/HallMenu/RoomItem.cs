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
    public string number;
    public void Start()
    {
        onDoubleClick += OnJoin;
    }
    public void SetData(DiscoveryResponse server)
    {
        this.server = server;
    }
    protected virtual void Refresh()
    {
        titleObject.text = server.roomData.title; 
        descriptionObject.text = server.roomData.description;
        gameModeObject.text = server.roomData.gameMode;
        playerObject.text = number + "/" + server.roomData.maxNumber;
    }
    public virtual void OnJoin()
    {
        Debug.Log($"Join {server.uri}");
        FindAnyObjectByType<GameDiscovery>().StopDiscovery();
        GameEntry.EventComponent.Notify(GameEvent.ClientReadyConnectEvent, server);
    }
}
