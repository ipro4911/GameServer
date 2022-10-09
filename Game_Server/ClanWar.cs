// Decompiled with JetBrains decompiler
// Type: Game_Server.ClanWar
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server
{
  internal class ClanWar
  {
    public int id;
    public string versusClan;
    public string score;
    public bool won;

    public ClanWar(int id, string vsclan, string score, bool won)
    {
      this.id = id;
      this.versusClan = vsclan;
      this.score = score;
      this.won = won;
    }
  }
}
