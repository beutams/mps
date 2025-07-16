using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Server
public struct ServerMessage : NetworkMessage
{
    public ServerMessageOption option;
    public object data;
}
public enum ServerMessageOption
{
    None,
    Start,
    Ready
}
#endregion

#region Client
public struct ClientMessage : NetworkMessage
{
    public ClientMessageOption option;
    public object data;
}
public enum ClientMessageOption
{
    None,
    UpdateRoom,
    Started
}
#endregion