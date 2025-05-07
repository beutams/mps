using Mirror;
using System;

#region Server
public struct ServerMessage : NetworkMessage
{
    public ServerMessageOption option;
    public Guid roomId;

}
public enum ServerMessageOption
{
    None,
    Create,
    Cancel,
    Start,
    Match,
    Join,
    Leave,
    Ready
}
#endregion

#region Client
public struct ClientMessage : NetworkMessage
{
    public ClientMessageOption option;
    public Guid roomId;
    public RoomInfo[] rooms;
    public PlayerInfo[] playerInfos;
}
public enum ClientMessageOption
{
    None,
    List,
    Created,
    Cancelled,
    Joined,
    Departed,
    UpdateRoom,
    Started
}
[Serializable]
public struct PlayerInfo
{
    public int playerIndex;
    public byte playerSite;
    public bool ready;
    public Guid roomId;
}
[Serializable]
public struct RoomInfo
{
    public Guid roomId;
    public byte players;
    public byte maxPlayers;
}
#endregion