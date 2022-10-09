// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_SwitchTeam
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Linq;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_SwitchTeam : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (usr.spectating || room.gameactive)
        return;
      int roomslot = usr.roomslot;
      int key = room.SwitchSide(usr);
      if (roomslot == key || room.type == 1)
        return;
      if (!room.gameactive && room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r => !r.playing)).Count<Game_Server.User>() < room.users.Count)
      {
        usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> There is still someone in game, you must wait that everyone is back in lobby!", 999L, Game_Server.Configs.Server.SystemName));
      }
      else
      {
        usr.roomslot = key;
        Game_Server.User user = (Game_Server.User) null;
        room.users.TryRemove(roomslot, out user);
        room.users.TryAdd(key, usr);
        this.sendBlocks[7] = (object) usr.roomslot;
        this.sendBlocks[8] = (object) room.master;
        this.sendPacket = true;
      }
    }
  }
}
