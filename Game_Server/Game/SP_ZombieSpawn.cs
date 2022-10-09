// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ZombieSpawn
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_ZombieSpawn : Packet
  {
    public SP_ZombieSpawn(int Slot, int FollowUser, int Place, int ZombieType, int health)
    {
      this.newPacket((ushort) 13432);
      this.addBlock((object) Slot);
      this.addBlock((object) FollowUser);
      this.addBlock((object) ZombieType);
      this.addBlock((object) Place);
      this.addBlock((object) health);
    }
  }
}
