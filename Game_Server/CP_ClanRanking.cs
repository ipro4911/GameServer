// Decompiled with JetBrains decompiler
// Type: Game_Server.CP_ClanRanking
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server
{
  internal class CP_ClanRanking : Handler
  {
    public override void Handle(User usr)
    {
      if (usr.room != null)
        return;
      if (ClanRanking.LastUpdate != DateTime.Now.Hour)
        ClanRanking.refreshclans();
      usr.send((Packet) new SP_ClanRanking());
    }
  }
}
