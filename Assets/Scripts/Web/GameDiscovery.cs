using Mirror;
using Mirror.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class GameDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    public HashSet<DiscoveryResponse> discoveredServers = new HashSet<DiscoveryResponse>();
    protected List<DiscoveryResponse> currentFindList = new List<DiscoveryResponse>();
    public RoomData roomData;
    public override void Start()
    {
        base.Start();
        GameEntry.EventComponent.Subscribe(GameEvent.CreateRoomEvent, SetRoomData);
        GameEntry.EventComponent.Subscribe(GameEvent.ServerStartEvent, OnServerStart);
        GameEntry.EventComponent.Subscribe(GameEvent.ClientReadyConnectEvent, Connect);
        OnServerFound.AddListener(OnDiscoveredServer);
    }
    private void LateUpdate()
    {
        foreach(var entry in discoveredServers)
        {
            if(!currentFindList.Contains(entry))
            {
                discoveredServers.Remove(entry);
            }
        }
        foreach(var entry in currentFindList) 
        {
            if (!discoveredServers.Contains(entry))
            {
                discoveredServers.Add(entry);
            }
        }
        currentFindList.Clear();
    }
    protected void SetRoomData(object roomData)
    {
        if(roomData is RoomData)
        {
            this.roomData = (RoomData)roomData;
        }
    }
    public virtual void Discovery()
    {
        StartDiscovery();
        Debug.Log("Start Discovery");
    }
    #region Server
    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        try
        {
            return new DiscoveryResponse() { roomData = roomData ,uri = transport.ServerUri(), serverId = ServerId};
        }
        catch
        {
            Debug.LogError($"ProcessRequest send fail");
            throw;
        }
    }
    protected void OnServerStart(object data)
    {
        AdvertiseServer();
    }
    #endregion

    #region Client
    protected override DiscoveryRequest GetRequest()
    {
        return new DiscoveryRequest();
    }
    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
    {
        response.endPoint = endpoint;
        UriBuilder realUri = new UriBuilder(response.uri)
        {
            Host = response.endPoint.Address.ToString()
        };
        response.uri = realUri.Uri;
        OnServerFound.Invoke(response);
    }
    public void OnDiscoveredServer(DiscoveryResponse response)
    {
        currentFindList.Add(response);
        Debug.Log($"Discovered Server: {response.uri}");
        Debug.Log($"{currentFindList.Count}");
    }
    public void Connect(object response)
    {
        if (NetworkClient.isConnecting || NetworkClient.isConnected) return;
        Debug.Log($"Client try connect {((DiscoveryResponse)response).uri}");
        NetworkClient.Connect(((DiscoveryResponse)response).uri);
    }
    #endregion
}
public struct DiscoveryRequest : NetworkMessage { }
public struct DiscoveryResponse : NetworkMessage
{
    public RoomData roomData;
    public Uri uri;
    public long serverId;
    public IPEndPoint endPoint { get; set; }
}
