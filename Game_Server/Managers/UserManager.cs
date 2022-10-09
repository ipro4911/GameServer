// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.UserManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game_Server.Managers
{
  internal class UserManager
  {
    private static Thread UserRoutineThread = (Thread) null;
    public static readonly ConcurrentDictionary<uint, Game_Server.User> ServerUsers = new ConcurrentDictionary<uint, Game_Server.User>();
    private static Random r = new Random();

    public static void setup()
    {
      UserManager.UserRoutineThread = new Thread(new ThreadStart(UserManager.UserRoutine));
      UserManager.UserRoutineThread.Start();
    }

    private static void UserRoutine()
    {
      while (true)
      {
        foreach (Game_Server.User usr in (IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values)
        {
          if (!usr.IsConnectionAlive)
          {
            Log.WriteLine("Kick request due to no received packet anymore for " + usr.nickname);
            usr.disconnect();
          }
          else if (usr.lastShoxTick + 15 < Game_Server.Generic.timestamp && usr.lastShoxTick > 0)
          {
            Log.WriteError(usr.nickname + " has been kicked out reason: No ShoxGuard Packet");
            usr.disconnect();
          }
          else
          {
            usr.RetrievePing();
            usr.send((Packet) new SP_PingInformation(usr));
          }
        }
        GC.Collect();
        Thread.Sleep(Game_Server.Configs.Server.PingRequestTick);
      }
    }

    public static List<Game_Server.User> getAllUsers()
    {
      return new List<Game_Server.User>((IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values.ToArray<Game_Server.User>());
    }

    public static List<Game_Server.User> getAllUsers(int c)
    {
      return new List<Game_Server.User>((IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values.Take<Game_Server.User>(c).ToArray<Game_Server.User>());
    }

    public static Game_Server.User GetUser(int id)
    {
      if (UserManager.ServerUsers.Count > 0)
        return UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (u => u.userId == id)).FirstOrDefault<Game_Server.User>();
      return (Game_Server.User) null;
    }

    public static Game_Server.User GetUser(ushort connectionId)
    {
      if (UserManager.ServerUsers.Count > 0)
        return UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (u => (int) u.connectionId == (int) connectionId)).FirstOrDefault<Game_Server.User>();
      return (Game_Server.User) null;
    }

    public static List<Game_Server.User> GetUserByClan(int clanid)
    {
      return UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (p =>
      {
        if (p.clan != null)
          return p.clan.id == clanid;
        return false;
      })).ToList<Game_Server.User>();
    }

    public static Game_Server.User GetRandomUser()
    {
      return UserManager.ServerUsers.Values.OrderBy<Game_Server.User, Guid>((Func<Game_Server.User, Guid>) (u => Guid.NewGuid())).FirstOrDefault<Game_Server.User>();
    }

    public static int GetUsersWithIP(string ip)
    {
      return UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r => r.IP == ip)).Count<Game_Server.User>();
    }

    public static Game_Server.User GetUserByRoomSlot(Room Room, int Roomslot)
    {
      if (UserManager.ServerUsers.Count > 0)
        return Room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (p =>
        {
          if (p != null)
            return p.roomslot == Roomslot;
          return false;
        })).FirstOrDefault<Game_Server.User>();
      return (Game_Server.User) null;
    }

    public static Game_Server.User GetUser(string nickname)
    {
      if (UserManager.ServerUsers.Count > 0)
        return UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (p => string.Compare(p.nickname, nickname, true) == 0)).FirstOrDefault<Game_Server.User>();
      return (Game_Server.User) null;
    }

    public static Game_Server.User getTarGetUser(uint sessionId)
    {
      if (UserManager.ServerUsers.ContainsKey(sessionId))
        return UserManager.ServerUsers[sessionId];
      return (Game_Server.User) null;
    }

    public static void SetOnlineToFriends(Game_Server.User usr, bool status)
    {
      foreach (Messenger messenger in (IEnumerable<Messenger>) usr.Friends.Values)
      {
        if (messenger != null)
        {
          Game_Server.User user = UserManager.GetUser(messenger.id);
          if (user != null)
          {
            Messenger friend = user.GetFriend(usr.userId);
            if (friend != null)
              friend.isOnline = status;
            messenger.isOnline = true;
            user.RefreshFriends();
          }
        }
      }
    }

    public static bool addUser(Game_Server.User usr)
    {
      for (uint key = 1; (long) key <= (long) Game_Server.Configs.Server.MaxSessions; ++key)
      {
        if (!UserManager.ServerUsers.ContainsKey(key))
        {
          usr.sessionId = key;
          break;
        }
      }
      if (usr.sessionId > 0U)
      {
        DB.RunQuery("UPDATE users SET online = '1', serverid = '" + (object) Game_Server.Configs.Server.serverId + "' WHERE id=" + (object) usr.userId);
        UserManager.ServerUsers.TryAdd(usr.sessionId, usr);
        Log.WriteLine(usr.nickname + " logged in!");
        return true;
      }
      Log.WriteError("Cannot add user " + usr.nickname + " to the stuck!");
      return false;
    }

    public static bool RemoveUser(Game_Server.User usr)
    {
      if (usr == null || !UserManager.ServerUsers.ContainsKey(usr.sessionId))
        return false;
      Game_Server.User user = (Game_Server.User) null;
      DB.RunQuery("UPDATE users SET online = '0', Lastmac='" + usr.macAddress + "', country='" + usr.country + "', coupons='" + (object) usr.coupons + "', todaycoupon='" + (object) usr.todaycoupons + "', coupontime='" + (object) usr.coupontime + "', Lasthwid='" + usr.hwid + "', lastjoin='" + (object) Game_Server.Generic.timestamp + "', Lastipaddress='" + (usr.rank < 5 ? (object) usr.IP : (object) "U mad bro?") + "',  serverid='" + (object) Game_Server.Configs.Server.serverId + "' WHERE id='" + (object) usr.userId + "'");
      UserManager.SetOnlineToFriends(usr, false);
      Log.WriteLine(usr.nickname + " logged out.");
      UserManager.ServerUsers.TryRemove(usr.sessionId, out user);
      return true;
    }

    public static List<Game_Server.User> GetUsersInChannel(int ChannelID, bool inRoom)
    {
      return UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (p =>
      {
        if (p.channel == ChannelID && !inRoom)
          return p.room == null;
        return false;
      })).ToList<Game_Server.User>();
    }

    public static void sendToServer(Packet p)
    {
      byte[] bytes = p.GetBytes();
      foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values)
        user.sendBuffer(bytes);
    }

    public static void sendToChannel(int channelId, bool inRoom, Packet p)
    {
      byte[] pBuffer = p.GetBytes();
      UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r =>
      {
        if (r.channel == channelId && !inRoom)
          return r.room == null;
        return false;
      })).ToList<Game_Server.User>().ForEach((Action<Game_Server.User>) (u => u.sendBuffer(pBuffer)));
    }

    public static void UpdateUserlist(Game_Server.User u)
    {
      byte[] bytes1 = new SP_UserList(SP_UserList.Type.Wait, UserManager.GetUsersInChannel(u.channel, false)).GetBytes();
      foreach (Game_Server.User user in UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r => r.channel == u.channel)))
      {
        if ((user.room == null || !user.room.gameactive) && (user.actualUserlistType == 2 && user.userId != u.userId))
          user.sendBuffer(bytes1);
        if (user.actualUserlistType == 0 && u.GetFriend(user.userId) != null)
          user.RefreshFriends();
      }
      u.sendBuffer(bytes1);
      if (u.clan == null)
        return;
      List<Game_Server.User> list = u.clan.Users.Values.ToList<Game_Server.User>();
      byte[] bytes2 = new SP_UserList(SP_UserList.Type.Clan, list).GetBytes();
      foreach (Game_Server.User user in list)
      {
        if (user.actualUserlistType == 1)
          user.sendBuffer(bytes2);
      }
    }
  }
}
