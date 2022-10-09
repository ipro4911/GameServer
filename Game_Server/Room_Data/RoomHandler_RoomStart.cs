// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_RoomStart
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Linq;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_RoomStart : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (usr.LastStartTick >= (double) Generic.timestamp || room.gameactive)
        return;
      if (!room.gameactive && room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r => !r.playing)).Count<Game_Server.User>() < room.users.Count)
      {
        usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> There is still someone in game, you must wait that everyone is back in lobby!", 999L, Game_Server.Configs.Server.SystemName));
      }
      else
      {
        usr.LastStartTick = (double) Generic.timestamp + 0.1;
        if (room.master == usr.roomslot)
        {
          int sideCountDerb = room.SideCountDerb;
          int sideCountNiu = room.SideCountNIU;
          if (room.isPremMap(room.mapid) && usr.premium < (byte) 1)
          {
            usr.send((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Game_Server.Configs.Server.SystemName + " >> You cannot start a premium map as free user!", 999U, "NULL"));
            return;
          }
          if (room.type == 1)
          {
            if (room.GetSideCount(0) != room.GetSideCount(1))
            {
              room.send((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> Teams need to be balanced.", 998U, "NULL"));
              return;
            }
          }
          else if ((room.users.Count <= 1 || sideCountDerb > sideCountNiu + 1 || (sideCountNiu > sideCountDerb + 1 || sideCountDerb == 0) || sideCountNiu == 0) && (usr.channel != 3 && room.mode != 1 && !Game_Server.Configs.Server.Debug))
            return;
          if (!room.Start())
            return;
          this.sendBlocks[3] = (object) 4;
          this.sendBlocks[6] = (object) room.mapid;
          room.status = 2;
          this.lobbychanges = true;
          room.isSpawned = false;
        }
        else
          usr.disconnect(); // if its bugged
        this.sendPacket = true;
      }
    }
  }
}
