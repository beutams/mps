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
    public Dictionary<DiscoveryResponse, int> discoveredServers = new Dictionary<DiscoveryResponse, int>();
    public Dictionary<DiscoveryResponse, int> curDiscoveredServers = new Dictionary<DiscoveryResponse, int>();
    protected List<DiscoveryResponse> waitDelete = new List<DiscoveryResponse>();
    protected List<DiscoveryResponse> waitAdd = new List<DiscoveryResponse>();
    protected bool isSearching;
    public RoomData roomData;
    float timer;
    float maxTime = 10f;
    public override void Start()
    {
        base.Start();
        GameEntry.EventComponent.Subscribe(GameEvent.CreateRoomEvent, SetRoomData);
        GameEntry.EventComponent.Subscribe(GameEvent.ServerStartEvent, OnServerStart);
        GameEntry.EventComponent.Subscribe(GameEvent.ClientReadyConnectEvent, Connect);
        OnServerFound.AddListener(OnDiscoveredServer);
        GameEntry.WebComponent.gameDiscover.Discovery();
    }
    private void Update()
    {
        KeepServerActive();
    }
    protected void KeepServerActive()
    {
        if (timer < 1)
            timer += Time.deltaTime;
        else
        {

            timer = 0;
            waitDelete.Clear();
            waitAdd.Clear();
            discoveredServers.Clear();
            foreach(var entry in curDiscoveredServers)
            {
                discoveredServers.Add(entry.Key, entry.Value);
            }
            foreach (var entry in discoveredServers.Keys)
            {
                waitAdd.Add(entry);
                if (discoveredServers[entry] > maxTime)
                {
                    waitDelete.Add(entry);
                }
            }
            if (waitAdd.Count > 0)
            {
                foreach (var entry in waitAdd)
                {
                    if (discoveredServers.ContainsKey(entry))
                    {
                        discoveredServers[entry] += 1;
                    }
                }
            }
            if (waitDelete.Count > 0)
            {
                foreach(var entry in waitDelete)
                {
                    if(discoveredServers.ContainsKey(entry))
                    {
                        discoveredServers.Remove(entry);
                    }
                }
            }
        }

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
        isSearching = false;
        curDiscoveredServers[response] = 0;
        Debug.Log($"Discovered Server: {response.uri},{response.roomData}");
    }
    public void Connect(object response)
    {
        if (NetworkClient.isConnecting || NetworkClient.isConnected) return;
        NetworkManager.singleton.StartClient(((DiscoveryResponse)response).uri);
        Debug.Log($"Client try connect {((DiscoveryResponse)response).uri}");
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
