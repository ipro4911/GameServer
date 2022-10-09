// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CustomSound
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CustomSound : Packet
  {
    public SP_CustomSound(SP_CustomSound.Sounds soundIndex)
    {
      this.newPacket((ushort) 46725);
      this.addBlock((object) (int) soundIndex);
    }

    public SP_CustomSound(int soundIndex)
    {
      this.newPacket((ushort) 46725);
      this.addBlock((object) soundIndex);
    }

    public enum Sounds
    {
      FirstBlood,
      HeadShot,
    }
  }
}
