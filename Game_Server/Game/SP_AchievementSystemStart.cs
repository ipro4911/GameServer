// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_AchievementSystemStart
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_AchievementSystemStart : Packet
  {
    public SP_AchievementSystemStart(int Value)
    {
      this.newPacket((ushort) 32257);
      this.addBlock((object) 1);
      this.addBlock((object) Value);
      this.addBlock((object) 0);
    }
  }
}
