// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CustomPremium
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CustomPremium : Packet
  {
    public SP_CustomPremium(int premiumId)
    {
      this.newPacket((ushort) 25023);
      this.addBlock((object) 0);
      this.addBlock((object) premiumId);
    }
  }
}
