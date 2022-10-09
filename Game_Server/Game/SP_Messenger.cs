// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Messenger
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_Messenger : Packet
  {
    public SP_Messenger(SP_Messenger.Subtype Subtype)
    {
      this.newPacket((ushort) 32256);
      this.addBlock((object) (int) Subtype);
    }

    internal enum Subtype
    {
      InvalidNickname = -11, // 0xFFFFFFF5
    }
  }
}
