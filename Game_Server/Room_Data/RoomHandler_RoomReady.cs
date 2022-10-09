// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_RoomReady
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_RoomReady : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      this.sendBlocks[3] = (object) 403;
      this.sendBlocks[6] = (object) "3";
      this.sendBlocks[7] = (object) "882";
      this.sendBlocks[8] = (object) "0";
      this.sendBlocks[9] = (object) "1";
      if (room.mode == 8 && room.channel == 2)
      {
        this.sendBlocks[10] = (object) room.kills;
        this.sendBlocks[11] = (object) "20";
        usr.TotalWarPoint = 20;
      }
      usr.send((Packet) new SP_CustomCRCCheck());
      this.sendPacket = true;
    }
  }
}
