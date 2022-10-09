// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomKick
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomKick : Packet
  {
    public SP_RoomKick(int slot)
    {
      this.newPacket((ushort) 29505);
      this.addBlock((object) 1);
      this.addBlock((object) slot);
    }
  }
}
