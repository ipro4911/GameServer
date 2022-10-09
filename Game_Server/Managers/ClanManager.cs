// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.ClanManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Game_Server.Managers
{
  internal class ClanManager
  {
    public static Dictionary<int, Game_Server.Clan> Clans = new Dictionary<int, Game_Server.Clan>();

    public static void loadClan(int id)
    {
      try
      {
        DataTable dataTable = DB.RunReader("SELECT name, maxusers, win, lose, exp, announcment, description, iconid FROM clans WHERE id=" + id.ToString());
        if (dataTable.Rows.Count <= 0)
          return;
        DataRow row = dataTable.Rows[0];
        string Announcment = row["announcment"].ToString();
        string Description = row["description"].ToString();
        string Master = "??";
        string MasterEXP = "0";
        Game_Server.Clan clan = new Game_Server.Clan(id, row["name"].ToString(), uint.Parse(row["iconid"].ToString()), int.Parse(row["maxusers"].ToString()), int.Parse(row["win"].ToString()), int.Parse(row["lose"].ToString()), int.Parse(row["exp"].ToString()), Announcment, Description, Master, MasterEXP, long.Parse(row["creationtime"].ToString()), false);
        ClanManager.Clans.Add(id, clan);
      }
      catch
      {
      }
    }

    public static void Load()
    {
      try
      {
        ClanManager.Clans.Clear();
        DataTable dataTable = DB.RunReader("SELECT * FROM clans");
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row = dataTable.Rows[index];
          int num = int.Parse(row["id"].ToString());
          string Announcment = row["announcment"].ToString();
          string Description = row["description"].ToString();
          string Master = "??";
          string MasterEXP = "0";
          Game_Server.Clan clan = new Game_Server.Clan(num, row["name"].ToString(), uint.Parse(row["iconid"].ToString()), int.Parse(row["maxusers"].ToString()), int.Parse(row["win"].ToString()), int.Parse(row["lose"].ToString()), int.Parse(row["exp"].ToString()), Announcment, Description, Master, MasterEXP, (long) Game_Server.Generic.timestamp, false);
          ClanManager.Clans.Add(num, clan);
        }
        Log.WriteLine("Successfully loaded [" + (object) dataTable.Rows.Count + "] Clans");
      }
      catch
      {
        Log.WriteError("Error while loading Clans");
      }
    }

    public static void CheckForDuplicate(Game_Server.User usr, string ClanName)
    {
      try
      {
        if (ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (v => string.Compare(v.name, ClanName, true) == 0)).Count<Game_Server.Clan>() > 0)
          usr.send((Packet) new SP_Clan.CheckClan(SP_Clan.CheckClan.ErrorCodes.Exist));
        else
          usr.send((Packet) new SP_Clan.CheckClan(SP_Clan.CheckClan.ErrorCodes.NotExist));
      }
      catch
      {
      }
    }

    public static int GetClanRank(int ID)
    {
      try
      {
        int exp = ClanManager.Clans[ID].exp;
        int num = 0;
        foreach (Game_Server.Clan clan in ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (c => c != null)))
        {
          ++num;
          if (clan.exp <= exp)
            return num;
        }
      }
      catch
      {
      }
      return 0;
    }

    public static Game_Server.Clan GetClan(int ID)
    {
      try
      {
        if (ClanManager.Clans.ContainsKey(ID))
          return ClanManager.Clans[ID];
        return (Game_Server.Clan) null;
      }
      catch
      {
        return (Game_Server.Clan) null;
      }
    }

    public static Game_Server.Clan GetClanByName(string cl)
    {
      try
      {
        return ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (v => string.Compare(v.name, cl, true) == 0)).First<Game_Server.Clan>();
      }
      catch
      {
        return (Game_Server.Clan) null;
      }
    }

    public static void AddClan(Game_Server.User usr, string Name)
    {
      try
      {
        if (ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (v => string.Compare(v.name, Name, true) == 0)).Count<Game_Server.Clan>() == 0)
        {
          uint creationCost = (uint) Game_Server.Configs.Server.Clan.CreationCost;
          if ((int) ((long) usr.dinar - (long) creationCost) > 0)
          {
            if (usr.clan == null)
            {
              string ClanJoinDate = DateTime.Now.ToString("dd/MM/yyyy");
              DB.RunQueryNotAsync("INSERT INTO clans (name, maxusers, count, win, lose, description, announcment, iconid, creationtime) VALUES ('" + DB.Stripslash(Name) + "', '20', '1', '0', '0', 'Welcome on Clan System!', 'Troopers, lets go to the attack!', '1001001', '" + (object) Game_Server.Generic.timestamp + "')");
              int num = int.Parse(DB.RunReaderOnce("id", "SELECT * FROM clans WHERE name='" + Name + "'").ToString());
              DB.RunQuery("UPDATE users SET clanid='-1' WHERE clanid='" + (object) num + "'");
              Game_Server.Clan clan = new Game_Server.Clan(num, Name, 1001001U, 20, 0, 0, 0, "Welcome on Clan System", "Troopers, lets go to the attack!", usr.nickname, usr.exp.ToString(), (long) Game_Server.Generic.timestamp, true);
              ClanManager.Clans.Add(num, clan);
              usr.clan = clan;
              usr.dinar -= (int) creationCost;
              clan.Users.TryAdd(usr.userId, usr);
              clan.ClanUsers.TryAdd(usr.userId, new ClanUsers(usr.userId, usr.nickname, usr.exp.ToString(), ClanJoinDate, 2));
              DB.RunQuery("UPDATE users SET dinar=" + (object) usr.dinar + ", clanid='" + (object) num + "', clanrank='2', clanjoindate='" + ClanJoinDate + "' WHERE id='" + (object) usr.userId + "'");
              usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> Successfully created the clan (" + Name + ")!", usr.sessionId, usr.nickname));
              usr.send((Packet) new SP_Clan.CreateClan(Name, num, (uint) usr.dinar));
            }
            else
            {
              string str = usr.clan.clanRank(usr) == 2 ? "own" : "are in";
              usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> Cannot create the clan because you " + str + " a clan!", usr.sessionId, usr.nickname));
            }
          }
          else
            usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> You havent enough dinars (" + creationCost.ToString("N0") + " needed!)", usr.sessionId, usr.nickname));
        }
        else
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> Cannot create the clan because this clan name is already in use!", usr.sessionId, usr.nickname));
      }
      catch
      {
      }
    }

    public static void RemoveClan(Game_Server.User usr)
    {
      try
      {
        if (usr.clan == null)
          return;
        if (usr.clan.clanRank(usr) == 2)
        {
          byte[] bytes = new SP_Chat("ClanSystem", SP_Chat.ChatType.Clan, "ClanSystem >> " + usr.nickname + " disbanded the clan :(", (uint) usr.clan.id, "NULL").GetBytes();
          Game_Server.Clan clan = usr.clan;
          ClanManager.Clans.Remove(clan.id);
          usr.send((Packet) new SP_Clan(SP_Clan.ClanCodes.DisbandClan));
          DB.RunQuery("DELETE FROM clans WHERE id='" + (object) usr.clan.id + "'");
          DB.RunQuery("DELETE FROM clans_clanwars WHERE clanid1='" + (object) usr.clan.id + "' OR clanid2='" + (object) usr.clan.id + "'");
          DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE clanid='" + (object) usr.clan.id + "'");
          foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) usr.clan.Users.Values)
          {
            user.sendBuffer(bytes);
            user.clan = (Game_Server.Clan) null;
          }
        }
        else
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> Cannot delete the clan because you're not the master!", usr.sessionId, usr.nickname));
      }
      catch
      {
      }
    }

    public static void UpgradeClan(Game_Server.User usr)
    {
      try
      {
        if (usr.clan == null)
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> You doesn't own a clan!", usr.sessionId, usr.nickname));
        else if (usr.clan.clanRank(usr) != 2)
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> You're not the owner of the clan!", usr.sessionId, usr.nickname));
        else if (usr.cash - 10000 < 0)
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> Not enough money!", usr.sessionId, usr.nickname));
        else if (usr.clan.maxUsers >= 100)
        {
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> Your clan cannot be extended more!!", usr.sessionId, usr.nickname));
        }
        else
        {
          int maxUsers = usr.clan.maxUsers;
          usr.clan.maxUsers += 20;
          byte[] bytes = new SP_Chat("ClanSystem", SP_Chat.ChatType.Clan, "ClanSystem >> " + usr.nickname + " has upgraded the clan slots from " + (object) maxUsers + " to " + (object) usr.clan.maxUsers + ":)!", (uint) usr.clan.id, "NULL").GetBytes();
          foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) usr.clan.Users.Values)
            user.sendBuffer(bytes);
          DB.RunQuery("UPDATE clans SET maxusers=maxusers+20 WHERE id='" + (object) usr.clan.id + "'");
        }
      }
      catch
      {
      }
    }
  }
}
