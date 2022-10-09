//// Decompiled with JetBrains decompiler
//// Type: Game_Server.Game.SP_RankingList
//// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
//// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

//using System.Collections.Generic;

//namespace Game_Server.Game
//{
//  internal class SP_RankingList : Packet
//  {
//    public SP_RankingList(ushort tab, ushort page, ushort sortType)
//    {
//      this.newPacket((ushort) 30816);
//      this.addBlock((object) tab);
//      this.addBlock((object) page);
//      this.addBlock((object) sortType);
//      switch (tab)
//      {
//        case 0:
//          List<RankingList.User> userList = new List<RankingList.User>();
//          switch (sortType)
//          {
//            case 0:
//              userList = RankingList.UserByEXP;
//              break;
//            case 1:
//              userList = RankingList.UserByWins;
//              break;
//            case 2:
//              userList = RankingList.UserByKills;
//              break;
//          }
//          ushort num1 = (ushort) (userList.Count / 10);
//          if ((int) page >= (int) num1)
//            page = num1;
//          int num2 = (int) page * 10;
//          int num3 = userList.Count - num2 > 10 ? 10 : userList.Count - num2;
//          if (num3 < 0)
//            num3 = 0;
//          this.addBlock((object) num3);
//          for (int index = num2; index < num2 + num3; ++index)
//          {
//            RankingList.User user = userList[index];
//            if (user != null)
//            {
//              this.addBlock((object) (index + 1));
//              this.addBlock((object) 100);
//              this.addBlock((object) user.exp);
//              this.addBlock((object) user.kills);
//              this.addBlock((object) user.deaths);
//              this.addBlock((object) user.wins);
//              this.addBlock((object) user.loses);
//              this.addBlock((object) user.claniconid);
//              this.addBlock((object) user.nickname);
//              this.addBlock((object) user.clanname);
//            }
//          }
//          break;
//        case 1:
//          List<RankingList.Clan> clanList = new List<RankingList.Clan>();
//          switch (sortType)
//          {
//            case 0:
//              clanList = RankingList.ClanByEXP;
//              break;
//            case 1:
//              clanList = RankingList.ClanByWins;
//              break;
//            case 2:
//              clanList = RankingList.ClanByMembers;
//              break;
//          }
//          ushort num4 = (ushort) (clanList.Count / 10);
//          if ((int) page >= (int) num4)
//            page = num4;
//          int num5 = (int) page * 10;
//          int num6 = clanList.Count - num5 > 10 ? 10 : clanList.Count - num5;
//          if (num6 < 0)
//            num6 = 0;
//          this.addBlock((object) num6);
//          for (int index = num5; index < num5 + num6; ++index)
//          {
//            RankingList.Clan clan = clanList[index];
//            if (clan != null)
//            {
//              this.addBlock((object) (index + 1));
//              this.addBlock((object) clan.GetRank());
//              this.addBlock((object) clan.exp);
//              this.addBlock((object) clan.wins);
//              this.addBlock((object) clan.loses);
//              this.addBlock((object) clan.usercount);
//              this.addBlock((object) clan.claniconid);
//              this.addBlock((object) clan.name);
              
//            }
//          }
//          break;
//      }
//    }
//  }
//}
