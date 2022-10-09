// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomListUpdate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomListUpdate : Packet
  {
    public SP_RoomListUpdate(Room room, int exist = 1)
    {
      int num1 = room.ch.roomToPageCount - 1;
      int num2 = num1 < 0 ? 0 : num1;
      this.newPacket((ushort) 29200);
      this.addBlock((object) room.id);
      this.addBlock((object) exist);
      this.addBlock((object) num2);
      if (exist == 2)
        return;
      this.addRoomInfo(room);
    }
  }
}
