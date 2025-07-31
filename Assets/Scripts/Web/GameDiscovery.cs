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
    public Dictionary<DiscoveryResponse, IPEndPoint> discoveredServers = new Dictionary<DiscoveryResponse, IPEndPoint>();
    public HallSubUI hall;
    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        if(hall == null && GameObject.Find("HallUI") != null)
            hall = GameObject.Find("HallUI").GetComponent<HallSubUI>();
    }
    public virtual void Discovery()
    {
        discoveredServers.Clear();
        StartDiscovery();
    }
    #region Server
    protected override void ProcessClientRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        base.ProcessClientRequest(request, endpoint);
    }

    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        try
        {
            return new DiscoveryResponse() { roomData = hall.roomData ,uri = transport.ServerUri()};
        }
        catch
        {
            Debug.LogError($"ProcessRequest send fail");
            throw;
        }

    }
    #endregion

    #region Client
    protected override DiscoveryRequest GetRequest()
    {
        return base.GetRequest();
    }
    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
    {
        discoveredServers.Add(response, endpoint);
        Debug.Log(discoveredServers.Count());
    }
    #endregion
}
public struct DiscoveryRequest : NetworkMessage
{
    public RoomData roomData;
}
public struct DiscoveryResponse : NetworkMessage
{
    public RoomData roomData;
    public Uri uri;
}
