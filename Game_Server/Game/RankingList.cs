//// Decompiled with JetBrains decompiler
//// Type: Game_Server.Game.RankingList
//// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
//// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

//using Game_Server.Managers;
//using System;
//using System.Collections.Generic;
//using System.Data;

//namespace Game_Server.Game
//{
//  internal class RankingList
//  {
//    public static short hour = -1;
//    public static List<RankingList.User> UserByEXP = new List<RankingList.User>();
//    public static List<RankingList.User> UserByWins = new List<RankingList.User>();
//    public static List<RankingList.User> UserByKills = new List<RankingList.User>();
//    public static List<RankingList.Clan> ClanByEXP = new List<RankingList.Clan>();
//    public static List<RankingList.Clan> ClanByWins = new List<RankingList.Clan>();
//    public static List<RankingList.Clan> ClanByMembers = new List<RankingList.Clan>();

//    public static void Load()
//    {
//      if ((int) RankingList.hour == DateTime.Now.Hour)
//        return;
//      RankingList.hour = (short) DateTime.Now.Hour;
//      RankingList.UserByEXP.Clear();
//      RankingList.UserByWins.Clear();
//      RankingList.UserByKills.Clear();
//      RankingList.ClanByEXP.Clear();
//      RankingList.ClanByWins.Clear();
//      RankingList.ClanByMembers.Clear();
//      DataTable dataTable1 = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY exp DESC LIMIT 0, 100");
//      for (int index = 0; index < dataTable1.Rows.Count; ++index)
//      {
//        DataRow row = dataTable1.Rows[index];
//        if (row != null)
//        {
//          RankingList.User user = new RankingList.User();
//          user.nickname = row["nickname"].ToString();
//          int ID = int.Parse(row["clanid"].ToString());
//          Game_Server.Clan clan = ClanManager.GetClan(ID);
//          if (ID >= 0 && clan != null)
//          {
//            user.clanname = clan.name;
//            user.claniconid = (int) clan.iconid;
//          }
//          else
//          {
//            user.clanname = "NULL";
//            user.claniconid = -1;
//          }
//          user.exp = uint.Parse(row["exp"].ToString());
//          user.kills = uint.Parse(row["kills"].ToString());
//          user.deaths = uint.Parse(row["deaths"].ToString());
//          user.wins = uint.Parse(row["wonMatchs"].ToString());
//          user.loses = uint.Parse(row["lostMatchs"].ToString());
//          RankingList.UserByEXP.Add(user);
//        }
//      }
//      DataTable dataTable2 = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY wonMatchs DESC LIMIT 0, 100");
//      for (int index = 0; index < dataTable2.Rows.Count; ++index)
//      {
//        DataRow row = dataTable2.Rows[index];
//        if (row != null)
//        {
//          RankingList.User user = new RankingList.User();
//          user.nickname = row["nickname"].ToString();
//          int ID = int.Parse(row["clanid"].ToString());
//          Game_Server.Clan clan = ClanManager.GetClan(ID);
//          if (ID >= 0 && clan != null)
//          {
//            user.clanname = clan.name;
//            user.claniconid = (int) clan.iconid;
//          }
//          else
//          {
//            user.clanname = "NULL";
//            user.claniconid = -1;
//          }
//          user.exp = uint.Parse(row["exp"].ToString());
//          user.kills = uint.Parse(row["kills"].ToString());
//          user.deaths = uint.Parse(row["deaths"].ToString());
//          user.wins = uint.Parse(row["wonMatchs"].ToString());
//          user.loses = uint.Parse(row["lostMatchs"].ToString());
//          RankingList.UserByWins.Add(user);
//        }
//      }
//      DataTable dataTable3 = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY kills DESC LIMIT 0, 100");
//      for (int index = 0; index < dataTable3.Rows.Count; ++index)
//      {
//        DataRow row = dataTable3.Rows[index];
//        if (row != null)
//        {
//          RankingList.User user = new RankingList.User();
//          user.nickname = row["nickname"].ToString();
//          int ID = int.Parse(row["clanid"].ToString());
//          Game_Server.Clan clan = ClanManager.GetClan(ID);
//          if (ID >= 0 && clan != null)
//          {
//            user.clanname = clan.name;
//            user.claniconid = (int) clan.iconid;
//          }
//          else
//          {
//            user.clanname = "NULL";
//            user.claniconid = -1;
//          }
//          user.exp = uint.Parse(row["exp"].ToString());
//          user.kills = uint.Parse(row["kills"].ToString());
//          user.deaths = uint.Parse(row["deaths"].ToString());
//          user.wins = uint.Parse(row["wonMatchs"].ToString());
//          user.loses = uint.Parse(row["lostMatchs"].ToString());
//          RankingList.UserByKills.Add(user);
//        }
//      }
//      DataTable dataTable4 = DB.RunReader("SELECT * FROM clans ORDER BY exp DESC LIMIT 0, 100");
//      for (int index = 0; index < dataTable4.Rows.Count; ++index)
//      {
//        DataRow row = dataTable4.Rows[index];
//        if (row != null)
//          RankingList.ClanByEXP.Add(new RankingList.Clan()
//          {
//            id = uint.Parse(row["iconid"].ToString()),
//            name = row["name"].ToString(),
//            claniconid = int.Parse(row["iconid"].ToString()),
//            wins = uint.Parse(row["win"].ToString()),
//            loses = uint.Parse(row["lose"].ToString()),
//            exp = uint.Parse(row["exp"].ToString()),
//            usercount = uint.Parse(row["count"].ToString())
//          });
//      }
//      DataTable dataTable5 = DB.RunReader("SELECT * FROM clans ORDER BY win DESC LIMIT 0, 100");
//      for (int index = 0; index < dataTable5.Rows.Count; ++index)
//      {
//        DataRow row = dataTable5.Rows[index];
//        if (row != null)
//          RankingList.ClanByWins.Add(new RankingList.Clan()
//          {
//            id = uint.Parse(row["iconid"].ToString()),
//            name = row["name"].ToString(),
//            claniconid = int.Parse(row["iconid"].ToString()),
//            wins = uint.Parse(row["win"].ToString()),
//            loses = uint.Parse(row["lose"].ToString()),
//            exp = uint.Parse(row["exp"].ToString()),
//            usercount = uint.Parse(row["count"].ToString())
//          });
//      }
//      DataTable dataTable6 = DB.RunReader("SELECT * FROM clans ORDER BY count DESC LIMIT 0, 100");
//      for (int index = 0; index < dataTable6.Rows.Count; ++index)
//      {
//        DataRow row = dataTable6.Rows[index];
//        if (row != null)
//          RankingList.ClanByMembers.Add(new RankingList.Clan()
//          {
//            id = uint.Parse(row["iconid"].ToString()),
//            name = row["name"].ToString(),
//            claniconid = int.Parse(row["iconid"].ToString()),
//            wins = uint.Parse(row["win"].ToString()),
//            loses = uint.Parse(row["lose"].ToString()),
//            exp = uint.Parse(row["exp"].ToString()),
//            usercount = uint.Parse(row["count"].ToString())
//          });
//      }
//    }

//    internal class User
//    {
//      public uint id;
//      public uint kills;
//      public uint exp;
//      public uint deaths;
//      public uint wins;
//      public uint loses;
//      public int claniconid;
//      public string nickname;
//      public string clanname;
//    }

//    internal class Clan
//    {
//      public uint id;
//      public uint wins;
//      public uint loses;
//      public uint usercount;
//      public uint exp;
//      public int claniconid;
//      public string name;

//      public int GetRank()
//      {
//        if (this.usercount >= 81U)
//          return 9;
//        if (this.usercount >= 61U)
//          return 8;
//        if (this.usercount >= 51U)
//          return 7;
//        if (this.usercount >= 41U)
//          return 6;
//        if (this.usercount >= 31U)
//          return 5;
//        if (this.usercount >= 21U)
//          return 4;
//        if (this.usercount >= 11U)
//          return 3;
//        return this.usercount >= 6U ? 2 : 1;
//      }
//    }
//  }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Diagnostics;

namespace Game_Server.Game
{
    class RankingList
    {
        internal class User
        {
            private uint loses;
            private int claniconid;
            private string clanname;
            private uint id;
            private uint kills;
            private uint exp;
            private uint deaths;
            private uint wins;
            private string nickname;

            public uint Id { get => id; set => id = value; }
            public uint Kills { get => kills; set => kills = value; }
            public uint Exp { get => exp; set => exp = value; }
            public uint Deaths { get => deaths; set => deaths = value; }
            public uint Wins { get => wins; set => wins = value; }
            public uint Loses { get => loses; set => loses = value; }
            public int Claniconid { get => claniconid; set => claniconid = value; }
            public string Nickname { get => nickname; set => nickname = value; }
            public string Clanname { get => clanname; set => clanname = value; }
        }
        internal class Clan
        {
            private uint exp;
            private int claniconid;
            private string name;
            private uint id;
            private uint wins;
            private uint loses;
            private uint usercount;

            public uint Id { get => id; set => id = value; }
            public uint Wins { get => wins; set => wins = value; }
            public uint Loses { get => loses; set => loses = value; }
            public uint Usercount { get => usercount; set => usercount = value; }
            public uint Exp { get => exp; set => exp = value; }
            public int Claniconid { get => claniconid; set => claniconid = value; }
            public string Name { get => name; set => name = value; }

            public int GetRank()
            {
                if (Usercount >= 81) return (int)Game_Server.Clan.Rank.Corps;
                else if (Usercount >= 61) return (int)Game_Server.Clan.Rank.Division;
                else if (Usercount >= 51) return (int)Game_Server.Clan.Rank.Brigade;
                else if (Usercount >= 41) return (int)Game_Server.Clan.Rank.Regiment;
                else if (Usercount >= 31) return (int)Game_Server.Clan.Rank.Battalion;
                else if (Usercount >= 21) return (int)Game_Server.Clan.Rank.Company;
                else if (Usercount >= 11) return (int)Game_Server.Clan.Rank.Platoon;
                else if (Usercount >= 6) return (int)Game_Server.Clan.Rank.Squad;
                else return (int)Game_Server.Clan.Rank.Recon;
            }
        }
        private static short hour = -1;
        private static List<User> userByEXP = new List<User>();
        private static List<User> userByWins = new List<User>();
        private static List<User> userByKills = new List<User>();

        private static List<Clan> clanByEXP = new List<Clan>();
        private static List<Clan> clanByWins = new List<Clan>();
        private static List<Clan> clanByMembers = new List<Clan>();

        public static short Hour { get => hour; set => hour = value; }
        internal static List<User> UserByEXP { get => userByEXP; set => userByEXP = value; }
        internal static List<User> UserByWins { get => userByWins; set => userByWins = value; }
        internal static List<User> UserByKills { get => userByKills; set => userByKills = value; }
        internal static List<Clan> ClanByEXP { get => clanByEXP; set => clanByEXP = value; }
        internal static List<Clan> ClanByWins { get => clanByWins; set => clanByWins = value; }
        internal static List<Clan> ClanByMembers { get => clanByMembers; set => clanByMembers = value; }

        public static void Load()
        {
            if (Hour == DateTime.Now.Hour) return;
            Hour = (short)DateTime.Now.Hour;

            UserByEXP.Clear();
            UserByWins.Clear();
            UserByKills.Clear();

            ClanByEXP.Clear();
            ClanByWins.Clear();
            ClanByMembers.Clear();

            DataTable dt = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY exp DESC LIMIT 0, 100");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row != null)
                {
                    RankingList.User usr = new RankingList.User();
                    usr.Nickname = row["nickname"].ToString();
                    int clanId = int.Parse(row["clanid"].ToString());
                    Game_Server.Clan c = Managers.ClanManager.GetClan(clanId);
                    if (clanId >= 0 && c != null)
                    {
                        usr.Clanname = c.name;
                        usr.Claniconid = (int)c.iconid;
                    }
                    else
                    {
                        usr.Clanname = "NULL";
                        usr.Claniconid = -1;
                    }
                    usr.Exp = uint.Parse(row["exp"].ToString());
                    usr.Kills = uint.Parse(row["kills"].ToString());
                    usr.Deaths = uint.Parse(row["deaths"].ToString());
                    usr.Wins = uint.Parse(row["wonMatchs"].ToString());
                    usr.Loses = uint.Parse(row["lostMatchs"].ToString());

                    UserByEXP.Add(usr);
                }
            }

            dt = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY wonMatchs DESC LIMIT 0, 100");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row != null)
                {
                    RankingList.User usr = new RankingList.User();
                    usr.Nickname = row["nickname"].ToString();
                    int clanId = int.Parse(row["clanid"].ToString());
                    Game_Server.Clan c = Managers.ClanManager.GetClan(clanId);
                    if (clanId >= 0 && c != null)
                    {
                        usr.Clanname = c.name;
                        usr.Claniconid = (int)c.iconid;
                    }
                    else
                    {
                        usr.Clanname = "NULL";
                        usr.Claniconid = -1;
                    }
                    usr.Exp = uint.Parse(row["exp"].ToString());
                    usr.Kills = uint.Parse(row["kills"].ToString());
                    usr.Deaths = uint.Parse(row["deaths"].ToString());
                    usr.Wins = uint.Parse(row["wonMatchs"].ToString());
                    usr.Loses = uint.Parse(row["lostMatchs"].ToString());

                    UserByWins.Add(usr);
                }
            }

            dt = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY kills DESC LIMIT 0, 100");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row != null)
                {
                    RankingList.User usr = new RankingList.User();
                    usr.Nickname = row["nickname"].ToString();
                    int clanId = int.Parse(row["clanid"].ToString());
                    Game_Server.Clan c = Managers.ClanManager.GetClan(clanId);
                    if (clanId >= 0 && c != null)
                    {
                        usr.Clanname = c.name;
                        usr.Claniconid = (int)c.iconid;
                    }
                    else
                    {
                        usr.Clanname = "NULL";
                        usr.Claniconid = -1;
                    }
                    usr.Exp = uint.Parse(row["exp"].ToString());
                    usr.Kills = uint.Parse(row["kills"].ToString());
                    usr.Deaths = uint.Parse(row["deaths"].ToString());
                    usr.Wins = uint.Parse(row["wonMatchs"].ToString());
                    usr.Loses = uint.Parse(row["lostMatchs"].ToString());

                    UserByKills.Add(usr);
                }
            }


            dt = DB.RunReader("SELECT * FROM clans ORDER BY exp DESC LIMIT 0, 100");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row != null)
                {
                    RankingList.Clan clan = new RankingList.Clan();
                    clan.Id = uint.Parse(row["iconid"].ToString());
                    clan.Name = row["name"].ToString();
                    clan.Claniconid = int.Parse(row["iconid"].ToString());
                    clan.Wins = uint.Parse(row["win"].ToString());
                    clan.Loses = uint.Parse(row["lose"].ToString());
                    clan.Exp = uint.Parse(row["exp"].ToString());
                    clan.Usercount = uint.Parse(row["count"].ToString());

                    ClanByEXP.Add(clan);
                }
            }


            dt = DB.RunReader("SELECT * FROM clans ORDER BY win DESC LIMIT 0, 100");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row != null)
                {
                    Clan clan = new Clan();
                    clan.Id = uint.Parse(row["iconid"].ToString());
                    clan.Name = row["name"].ToString();
                    clan.Claniconid = int.Parse(row["iconid"].ToString());
                    clan.Wins = uint.Parse(row["win"].ToString());
                    clan.Loses = uint.Parse(row["lose"].ToString());
                    clan.Exp = uint.Parse(row["exp"].ToString());
                    clan.Usercount = uint.Parse(row["count"].ToString());

                    ClanByWins.Add(clan);
                }
            }


            dt = DB.RunReader("SELECT * FROM clans ORDER BY count DESC LIMIT 0, 100");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row != null)
                {
                    RankingList.Clan clan = new RankingList.Clan();
                    clan.Id = uint.Parse(row["iconid"].ToString());
                    clan.Name = row["name"].ToString();
                    clan.Claniconid = int.Parse(row["iconid"].ToString());
                    clan.Wins = uint.Parse(row["win"].ToString());
                    clan.Loses = uint.Parse(row["lose"].ToString());
                    clan.Exp = uint.Parse(row["exp"].ToString());
                    clan.Usercount = uint.Parse(row["count"].ToString());

                    ClanByMembers.Add(clan);
                }
            }
        }
    }



    class SP_RankingList : Packet
    {
        public SP_RankingList(ushort tab, ushort page, ushort sortType)
        {
            this.newPacket((ushort)30816);
            this.addBlock((object)tab);
            this.addBlock((object)page);
            this.addBlock((object)sortType);
            switch (tab)
            {
                case 0:
                    List<RankingList.User> userList = new List<RankingList.User>();
                    switch (sortType)
                    {
                        case 0:
                            userList = RankingList.UserByEXP;
                            break;
                        case 1:
                            userList = RankingList.UserByWins;
                            break;
                        case 2:
                            userList = RankingList.UserByKills;
                            break;
                    }
                    ushort num1 = (ushort)(userList.Count / 10);
                    if ((int)page >= (int)num1)
                        page = num1;
                    int num2 = (int)page * 10;
                    int num3 = userList.Count - num2 > 10 ? 10 : userList.Count - num2;
                    if (num3 < 0)
                        num3 = 0;
                    this.addBlock((object)num3);
                    for (int index = num2; index < num2 + num3; ++index)
                    {
                        RankingList.User user = userList[index];
                        if (user != null)
                        {
                            this.addBlock((object)(index + 1));
                            this.addBlock((object)100);
                            this.addBlock((object)user.Exp);
                            this.addBlock((object)user.Kills);
                            this.addBlock((object)user.Deaths);
                            this.addBlock((object)user.Wins);
                            this.addBlock((object)user.Loses);
                            this.addBlock((object)user.Claniconid);
                            this.addBlock((object)user.Nickname);
                            this.addBlock((object)user.Clanname);
                        }
                    }
                    break;
                case 1:
                    List<RankingList.Clan> clanList = new List<RankingList.Clan>();
                    switch (sortType)
                    {
                        case 0:
                            clanList = RankingList.ClanByEXP;
                            break;
                        case 1:
                            clanList = RankingList.ClanByWins;
                            break;
                        case 2:
                            clanList = RankingList.ClanByMembers;
                            break;
                    }
                    ushort num4 = (ushort)(clanList.Count / 10);
                    if ((int)page >= (int)num4)
                        page = num4;
                    int num5 = (int)page * 10;
                    int num6 = clanList.Count - num5 > 10 ? 10 : clanList.Count - num5;
                    if (num6 < 0)
                        num6 = 0;
                    this.addBlock((object)num6);
                    for (int index = num5; index < num5 + num6; ++index)
                    {
                        RankingList.Clan clan = clanList[index];
                        if (clan != null)
                        {
                            this.addBlock((object)(index + 1));
                            this.addBlock((object)clan.GetRank());
                            this.addBlock((object)clan.Exp);
                            this.addBlock((object)clan.Wins);
                            this.addBlock((object)clan.Loses);
                            this.addBlock((object)clan.Usercount);
                            this.addBlock((object)clan.Claniconid);
                            this.addBlock((object)clan.Name);
                        }
                    }
                    break;
            }
        }
    }

    
    internal class CP_RankingList : Handler
    {
        public override void Handle(Game_Server.User usr)
        {
            usr.send(new SP_MyRank(usr));
            ushort tab = ushort.Parse(this.getBlock(0));
            ushort page = ushort.Parse(this.getBlock(1));
            ushort sortType = ushort.Parse(this.getBlock(2));
            usr.send(new SP_RankingList(tab, page, sortType));
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
