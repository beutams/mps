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
    public PlayerInfo[] data;
    public override string ToString()
    {
        string result = "";
        foreach (var item in data)
            result += item.ToString();
        return result;
    }
}
public enum ClientMessageOption
{
    None,
    UpdateRoom,
    Started
}
#endregion
#region Data
[Serializable]
public struct PlayerInfo
{
    public string name;
    public bool ready;
    public int index;
    public override string ToString()
    {
        return $"name:{name},ready:{ready},index:{index}";
    }
}
#endregion