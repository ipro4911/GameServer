// Decompiled with JetBrains decompiler
// Type: Game_Server.ClanPendingUsers
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server
{
  public class ClanPendingUsers
  {
    public int id;
    public string nickname;
    public string EXP;
    public string ClanJoinDate;

    public ClanPendingUsers(int id, string nickname, string exp, string ClanJoinDate)
    {
      this.id = id;
      this.nickname = nickname;
      this.EXP = exp;
      this.ClanJoinDate = ClanJoinDate;
    }
  }
}
