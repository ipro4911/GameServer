// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_ReadyState
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Linq;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_ReadyState : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (room.gameactive || usr.LastReadyTick >= (double) Generic.timestamp)
        return;
      if (!room.gameactive && room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r => !r.playing)).Count<Game_Server.User>() < room.users.Count)
      {
        usr.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> There is still someone in game, you must wait that everyone is back in lobby!", 999L, Game_Server.Configs.Server.SystemName));
      }
      else
      {
        usr.LastReadyTick = (double) Generic.timestamp + 0.1;
        usr.isReady = !usr.isReady;
        this.sendBlocks[6] = usr.isReady ? (object) "1" : (object) "0";
        this.sendPacket = true;
      }
    }
  }
}
