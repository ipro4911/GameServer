// Decompiled with JetBrains decompiler
// Type: Game_Server.Clan
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Game_Server
{
  internal class Clan
  {
    public ConcurrentDictionary<int, User> Users = new ConcurrentDictionary<int, User>();
    public ConcurrentDictionary<int, ClanPendingUsers> pendingUsers = new ConcurrentDictionary<int, ClanPendingUsers>();
    public ConcurrentDictionary<int, Game_Server.ClanUsers> ClanUsers = new ConcurrentDictionary<int, Game_Server.ClanUsers>();
    public ConcurrentDictionary<int, ClanWar> ClanWars = new ConcurrentDictionary<int, ClanWar>();
    public int id;
    public string name;
    public uint iconid;
    public int maxUsers;
    public int win;
    public int lose;
    public int exp;
    public string Announcment;
    public string Description;
    public string Master;
    public string MasterEXP;
    public long creation;

    public int GetRank()
    {
      return 8;
    }

    public string GetCreationDate()
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) this.creation).ToString("yyyy.mm.dd");
    }

    public Clan(
      int id,
      string name,
      uint iconId,
      int maxusers,
      int win,
      int lose,
      int exp,
      string Announcment,
      string Description,
      string Master,
      string MasterEXP,
      long creation,
      bool created = false)
    {
      this.id = id;
      this.name = name;
      this.iconid = iconId;
      this.maxUsers = maxusers;
      this.win = win;
      this.lose = lose;
      this.exp = exp;
      this.Announcment = Announcment;
      this.Description = Description;
      this.Master = Master;
      this.MasterEXP = MasterEXP;
      this.creation = creation;
      this.loadUsers(id, created);
      this.loadClanWar(id);
    }

    public void loadUsers(int clanId, bool created)
    {
      DataTable dataTable = DB.RunReader("SELECT * FROM users WHERE clanid='" + (object) clanId + "'");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        int clanrank = int.Parse(row["clanrank"].ToString());
        int num = int.Parse(row["id"].ToString());
        string nickname = row["nickname"].ToString();
        string exp = row["exp"].ToString();
        string ClanJoinDate = row["clanjoindate"].ToString();
        if (clanrank == -1 || clanrank == 9)
        {
          ClanPendingUsers clanPendingUsers = new ClanPendingUsers(num, nickname, exp, ClanJoinDate);
          this.pendingUsers.TryAdd(num, clanPendingUsers);
        }
        else
        {
          Game_Server.ClanUsers clanUsers = new Game_Server.ClanUsers(num, nickname, exp, ClanJoinDate, clanrank);
          this.ClanUsers.TryAdd(num, clanUsers);
        }
        if (clanrank == 2)
        {
          this.Master = nickname;
          this.MasterEXP = exp.ToString();
        }
      }
      if (dataTable.Rows.Count > 0 || created)
        return;
      DB.RunQuery("DELETE FROM clans WHERE id='" + (object) clanId + "'");
      DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE clanid='" + (object) clanId + "'");
      ClanManager.Clans.Remove(this.id);
    }

    public int clanRank(User u)
    {
      foreach (Game_Server.ClanUsers clanUsers in (IEnumerable<Game_Server.ClanUsers>) this.ClanUsers.Values)
      {
        if (clanUsers.id == u.userId)
          return clanUsers.clanrank;
      }
      foreach (ClanPendingUsers clanPendingUsers in (IEnumerable<ClanPendingUsers>) this.pendingUsers.Values)
      {
        if (clanPendingUsers.id == u.userId)
          return 9;
      }
      return 0;
    }

    public void loadClanWar(int clanId)
    {
      DataTable dataTable = DB.RunReader("SELECT * FROM clans_clanwars WHERE clanid1='" + (object) clanId + "' OR clanid2='" + (object) clanId + "' ORDER BY timestamp DESC LIMIT 0, 3");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        try
        {
          DataRow row = dataTable.Rows[index];
          string vsclan = DB.RunReaderOnce("name", "SELECT * FROM clans WHERE id='" + (row["clanid1"].ToString() == this.id.ToString() ? row["clanid2"] : row["clanid1"]).ToString() + "'").ToString();
          ClanWar clanWar = new ClanWar(index, vsclan, row["score"].ToString(), row["clanwon"].ToString() == this.id.ToString());
          this.ClanWars.TryAdd(index, clanWar);
        }
        catch
        {
        }
      }
    }

    public void AddClanWar(string name, string score, bool won)
    {
      int count = this.ClanWars.Count;
      ClanWar[] array = this.ClanWars.Values.ToArray<ClanWar>();
      this.ClanWars.Clear();
      ClanWar clanWar = new ClanWar(0, name, score, won);
      for (int index = 0; index < (count > 1 ? 1 : count); ++index)
        this.ClanWars.TryAdd(index + 1, array[index]);
    }

    public void sendToClan(Packet p)
    {
      byte[] bytes = p.GetBytes();
      foreach (User user in (IEnumerable<User>) this.Users.Values)
        user?.sendBuffer(bytes);
    }

    public ClanPendingUsers getPendingUser(int id)
    {
      if (this.pendingUsers.ContainsKey(id))
        return this.pendingUsers[id];
      return (ClanPendingUsers) null;
    }

    public ClanPendingUsers getPendingUser(string nickname)
    {
      return this.pendingUsers.Values.Where<ClanPendingUsers>((Func<ClanPendingUsers, bool>) (x => string.Compare(x.nickname, nickname, true) == 0)).First<ClanPendingUsers>();
    }

    public Game_Server.ClanUsers GetUser(int id)
    {
      if (this.Users.ContainsKey(id))
        return this.ClanUsers[id];
      return (Game_Server.ClanUsers) null;
    }

    public Game_Server.ClanUsers GetUser(string nickname)
    {
      return this.ClanUsers.Values.Where<Game_Server.ClanUsers>((Func<Game_Server.ClanUsers, bool>) (x => string.Compare(x.nickname, nickname, true) == 0)).First<Game_Server.ClanUsers>();
    }

    public List<ClanPendingUsers> pusers()
    {
      return new List<ClanPendingUsers>((IEnumerable<ClanPendingUsers>) this.pendingUsers.Values);
    }

    public List<User> GetUsers()
    {
      return new List<User>((IEnumerable<User>) this.Users.Values);
    }

    public List<Game_Server.ClanUsers> getAllUsers()
    {
      return new List<Game_Server.ClanUsers>((IEnumerable<Game_Server.ClanUsers>) this.ClanUsers.Values);
    }

    public enum Rank
    {
      Recon = 1,
      Squad = 2,
      Platoon = 3,
      Company = 4,
      Battalion = 5,
      Regiment = 6,
      Brigade = 7,
      Division = 8,
      Corps = 9,
    }
  }
}
