// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_KillCount
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_KillCount : Packet
  {
    public SP_KillCount(SP_KillCount.ActionType t)
    {
      this.newPacket((ushort) 45656);
      this.addBlock((object) (int) t);
      this.addBlock((object) 0);
    }

    public SP_KillCount(SP_KillCount.ActionType t, int kills)
    {
      this.newPacket((ushort) 45656);
      this.addBlock((object) (int) t);
      this.addBlock((object) kills);
    }

    internal enum ActionType
    {
      Hide,
      Show,
    }
  }
}
