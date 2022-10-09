// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ZombieEndGameItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_ZombieEndGameItem : Packet
  {
    public SP_ZombieEndGameItem(int choose, string ItemCode, int rs, int days)
    {
      this.newPacket((ushort) 30053);
      this.addBlock((object) 6);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) choose);
      this.addBlock((object) ItemCode);
      this.addBlock((object) rs);
      this.addBlock((object) days);
    }
  }
}
