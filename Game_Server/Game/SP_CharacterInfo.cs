// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CharacterInfo
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

/*namespace Game_Server.Game
{
  internal class SP_CharacterInfo : Packet
  {
    public SP_CharacterInfo(SP_CharacterInfo.ErrCodes ErrCode)
    {
      this.newPacket((ushort) 25088);
      this.addBlock((object) (int) ErrCode);
    }

    public SP_CharacterInfo(Game_Server.User usr)
    {
      this.newPacket((ushort) 25088);
      this.addBlock((object) 1);
      this.addBlock((object) "WrMontana V1");
      this.addBlock((object) "#madcuzbad?");
      this.addBlock((object) usr.userId);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) usr.nickname);
      if (usr.clan != null)
      {
        this.addBlock((object) usr.clan.id);
        this.addBlock((object) usr.clan.iconid);
        this.addBlock((object) usr.clan.name);
        this.addBlock((object) -1);
        this.addBlock((object) usr.clan.clanRank(usr));
      }
      else
        this.Fill((object) -1, 5);
      this.addBlock((object) 0);
      this.addBlock((object) usr.level);
      this.addBlock((object) usr.exp);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) usr.dinar);
      this.addBlock((object) usr.kills);
      this.addBlock((object) usr.deaths);
      this.addBlock((object) usr.wonMatchs);
      this.addBlock((object) usr.lostMatchs);
      this.addBlock((object) usr.AvailableSlots);
      for (int c = 0; c < 5; ++c)
        this.addBlock((object) usr.GetEquipment(c));
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) Game_Server.Configs.Server.Player.MaxInventorySlot);
      for (int index = 0; index < 5; ++index)
        this.addBlock((object) usr.costumes_char[index]);
      this.addBlock((object) Inventory.Costumelist(usr));
      this.addBlock((object) usr.premium);
      this.addBlock((object) 0);
      this.addBlock((object) usr.HasSmileBadge);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) usr.medalid);
    }

    internal enum ErrCodes
    {
      NormalProcedure = 73030, // 0x00011D46
      InvalidPacket = 90100, // 0x00015FF4
      UnregisteredUser = 90101, // 0x00015FF5
      ServerInaccessible = 90105, // 0x00015FF9
      TraineeServer = 90106, // 0x00015FFA
      ServerFull = 91020, // 0x0001638C
      UpdateFailed = 91040, // 0x000163A0
      SyncFail = 91050, // 0x000163AA
      IDUsed = 92040, // 0x00016788
      PremiumOnly = 98010, // 0x00017EDA
    }
  }
}
*/