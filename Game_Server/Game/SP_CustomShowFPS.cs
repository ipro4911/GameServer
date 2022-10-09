// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CustomShowFPS
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CustomShowFPS : Packet
  {
    public SP_CustomShowFPS()
    {
      this.newPacket((ushort) 46543);
      this.addBlock((object) 0);
    }
  }
}
