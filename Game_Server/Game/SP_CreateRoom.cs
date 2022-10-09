// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CreateRoom
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CreateRoom : Packet
  {
    public SP_CreateRoom(SP_CreateRoom.ErrorCode Code)
    {
      this.newPacket((ushort) 29440);
      this.addBlock((object) (int) Code);
    }

    public SP_CreateRoom(Room Room)
    {
      this.newPacket((ushort) 29440);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addRoomInfo(Room);
    }

    internal enum ErrorCode
    {
      FailedToCreate = 94501, // 0x00017125
    }
  }
}
