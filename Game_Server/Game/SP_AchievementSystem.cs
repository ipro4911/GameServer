// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_AchievementSystem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_AchievementSystem : Packet
  {
    public SP_AchievementSystem(Game_Server.User usr)
    {
      this.newPacket((ushort) 32257);
      this.addBlock((object) 1);
      this.addBlock((object) 1);
      this.addBlock((object) usr.nickname);
      this.addBlock((object) usr.level);
      if (usr.clan != null)
      {
        this.addBlock((object) usr.clan.name);
        this.addBlock((object) usr.clan.iconid);
      }
      else
      {
        this.addBlock((object) "NULL");
        this.addBlock((object) -1);
      }
      this.addBlock((object) usr.kills);
      this.addBlock((object) usr.deaths);
      this.addBlock((object) usr.headshots);
      this.addBlock((object) 0);
      this.addBlock((object) ((int) usr.wonMatchs + (int) usr.lostMatchs));
      this.addBlock((object) usr.medalid);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) -1);
      this.addBlock((object) usr.premium);
      for (int index = 1000; index < 1329; ++index)
      {
        this.addBlock((object) index);
        this.addBlock((object) 0);
      }
      this.addBlock((object) 1);
      this.addBlock((object) 0);
    }
  }
}
