// Decompiled with JetBrains decompiler
// Type: Game_Server.SP_ClanRanking
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Linq;

namespace Game_Server
{
  internal class SP_ClanRanking : Packet
  {
    public SP_ClanRanking()
    {
      this.newPacket((ushort) 26464);
      this.addBlock((object) 1);
      this.addBlock((object) (DateTime.Now.Hour + 1));
      this.addBlock((object) ClanRanking.clans.Count);
      foreach (Clan clan in ClanRanking.clans.Values.Take<Clan>(30))
      {
        if (clan != null)
        {
          this.addBlock((object) clan.iconid);
          this.addBlock((object) clan.name);
          this.addBlock((object) clan.exp);
          this.addBlock((object) clan.ClanUsers.Count);
          this.addBlock((object) clan.maxUsers);
        }
      }
    }
  }
}
