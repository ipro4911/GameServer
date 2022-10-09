// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.Channel
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Managers
{
  internal class Channel
  {
    public int maxRooms = 500;
    public ConcurrentDictionary<int, Room> rooms = new ConcurrentDictionary<int, Room>();
    public int channelId;

    public Room GetRoom(int roomId)
    {
      if (this.rooms.ContainsKey(roomId))
        return this.rooms[roomId];
      return (Room) null;
    }

    public int roomToPageCount
    {
      get
      {
        return (int) Math.Ceiling((Decimal) this.rooms.Values.Where<Room>((Func<Room, bool>) (u => u != null)).Count<Room>() / new Decimal(13));
      }
    }

    public int availableRoomToPageCount
    {
      get
      {
        return (int) Math.Ceiling((Decimal) this.rooms.Values.Where<Room>((Func<Room, bool>) (u =>
        {
          if (u != null)
            return u.isJoinable;
          return false;
        })).Count<Room>() / new Decimal(13));
      }
    }

    public int GetOpenID
    {
      get
      {
        for (int key = 0; key < this.maxRooms; ++key)
        {
          if (!this.rooms.ContainsKey(key))
            return key;
        }
        return -1;
      }
    }

    public List<Room> GetRoomListByPage(int p)
    {
      return this.rooms.Values.OrderBy<Room, int>((Func<Room, int>) (r => r.id)).Skip<Room>(p * 13).Take<Room>(13).ToList<Room>();
    }

    public List<Room> GetAvailableRoomListByPage(int p)
    {
      return this.rooms.Values.Where<Room>((Func<Room, bool>) (r => r.isJoinable)).OrderBy<Room, int>((Func<Room, int>) (r => r.id)).Skip<Room>(p * 13).Take<Room>(13).ToList<Room>();
    }

    public List<Room> GetAvailableRoomList()
    {
      return this.rooms.Values.Where<Room>((Func<Room, bool>) (r => r.isJoinable)).OrderBy<Room, int>((Func<Room, int>) (r => r.id)).ToList<Room>();
    }

    public void UpdateLobby(Room room)
    {
      byte[] bytes = new SP_RoomListUpdate(room, 1).GetBytes();
      foreach (Game_Server.User user in UserManager.GetUsersInChannel(room.channel, false))
      {
        if ((Decimal) user.lobbypage == Math.Floor((Decimal) (room.id / 13)))
          user.sendBuffer(bytes);
      }
    }

    public bool AddRoom(int roomId, Room r)
    {
      if (!this.rooms.ContainsKey(roomId))
        return this.rooms.TryAdd(roomId, r);
      return false;
    }

    public bool RemoveRoom(int roomId)
    {
      if (!this.rooms.ContainsKey(roomId))
        return false;
      Room room = this.rooms[roomId];
      return this.rooms.TryRemove(roomId, out room);
    }
  }
}
