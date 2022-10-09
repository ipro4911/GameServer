// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ZombieNewWave
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_ZombieNewWave : Packet
  {
    public SP_ZombieNewWave(int Wave)
    {
      this.newPacket((ushort) 13431);
      this.addBlock((object) 1);
      this.addBlock((object) 13);
      this.addBlock((object) Wave);
      this.addBlock((object) (Wave == 20 ? 1 : 0));
    }
  }
}
