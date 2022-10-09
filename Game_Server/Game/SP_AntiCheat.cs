// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_AntiCheat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_AntiCheat : Packet
  {
    public SP_AntiCheat(Game_Server.User usr)
    {
      this.newPacket((ushort) 46723);
      this.addBlock((object) 0);
    }
  }
}
