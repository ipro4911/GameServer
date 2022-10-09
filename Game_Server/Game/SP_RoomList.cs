// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomList
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_RoomList : Packet
  {
    public SP_RoomList(Game_Server.User usr, int page, bool waiting = false, int pageIdx = 0, int type = 1)
    {
      SP_RoomList spRoomList = this;
      this.newPacket((ushort) 29184);
      List<Room> roomList = new List<Room>();
      Channel channel = ChannelManager.channels[usr.channel];
      int num = !waiting ? channel.roomToPageCount : channel.availableRoomToPageCount;
      if (page > num)
        page = num;
      List<Room> source = waiting ? (pageIdx > 13 ? (type != 2 ? channel.GetAvailableRoomList().Where<Room>((Func<Room, bool>) (r => r.id <= pageIdx)).Skip<Room>(pageIdx - 13).Take<Room>(13).ToList<Room>() : channel.GetAvailableRoomList().Where<Room>((Func<Room, bool>) (r => r.id >= pageIdx)).Take<Room>(13).ToList<Room>()) : channel.GetAvailableRoomListByPage(0)) : channel.GetRoomListByPage(page);
      this.addBlock((object) type);
      this.addBlock((object) page);
      this.addBlock((object) (num - 1));
      this.addBlock((object) source.Count);
      source.Cast<Room>().ToList<Room>().ForEach((Action<Room>) (r => this.addRoomInfo(r)));
    }
  }
}
