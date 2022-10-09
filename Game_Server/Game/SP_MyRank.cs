// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_MyRank
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_MyRank : Packet
  {
    public SP_MyRank(Game_Server.User usr)
    {
      this.newPacket((ushort) 30816);
      this.addBlock((object) 2);
      this.addBlock((object) (usr.clan == null ? 1 : 2));
      this.addBlock((object) 1);
      this.addBlock((object) 1);
      this.addBlock((object) usr.exp);
      this.addBlock((object) usr.kills);
      this.addBlock((object) usr.deaths);
      this.addBlock((object) usr.wonMatchs);
      this.addBlock((object) usr.lostMatchs);
      this.addBlock((object) (usr.clan != null ? (int) usr.clan.iconid : -1));
      this.addBlock((object) usr.nickname);
      this.addBlock(usr.clan != null ? (object) usr.clan.name : (object) "NULL");
      if (usr.clan == null)
        return;
      this.addBlock((object) 4);
      this.addBlock((object) (usr.clan.maxUsers / 20 - 1));
      this.addBlock((object) usr.clan.exp);
      this.addBlock((object) usr.clan.win);
      this.addBlock((object) usr.clan.lose);
      this.addBlock((object) usr.clan.ClanUsers.Count);
      this.addBlock((object) usr.clan.iconid);
      this.addBlock((object) usr.clan.name);
      this.addBlock((object) usr.clan.GetRank());
    }
  }
}
