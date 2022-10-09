// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.RoutineManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game_Server.Managers
{
  internal class RoutineManager
  {
    private static int ActualDay = 0;
    private static uint LastTick = 0;
    private static List<int> users = new List<int>();
    private static List<int> ResetRandomBoxIDs = new List<int>();
    private static object obj = new object();
    private static Thread DailyCheckThread;
    private static Thread UserRoutineThread;

    public static void Load()
    {
      RoutineManager.ActualDay = DateTime.Now.Day;
      RoutineManager.UserRoutineThread = new Thread(new ThreadStart(RoutineManager.UserRoutine));
      RoutineManager.UserRoutineThread.Start();
      RoutineManager.DailyCheckThread = new Thread(new ThreadStart(RoutineManager.DailyCheck));
      RoutineManager.DailyCheckThread.Start();
    }

    private static void UserRoutine()
    {
      while (true)
      {
        foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values)
        {
          try
          {
            if (Game_Server.Configs.Server.Player.CouponEvent)
            {
              if (user.room != null && user.room.gameactive)
                user.coupontime += 5;
              if (user.todaycoupons < 5 && user.coupontime >= 1800)
              {
                ++user.todaycoupons;
                ++user.coupons;
                user.coupontime = 0;
                DB.RunQuery(string.Format("UPDATE users SET coupons='{0}', todaycoupon='{1}' WHERE id='{2}'", (object) user.coupons, (object) user.todaycoupons, (object) user.userId));
                user.send((Packet) new SP_CouponEvent(user.todaycoupons, user.coupons));
              }
            }
            if (Game_Server.Configs.Server.RandomBoxEvent.Enabled && DateTime.Now.Hour == Game_Server.Configs.Server.RandomBoxEvent.hour)
            {
              if (!user.RandomBoxToday)
              {
                string boxCode = Game_Server.Configs.Server.RandomBoxEvent.BoxCode;
                user.RandomBoxToday = true;
                Inventory.AddItem(user, boxCode, -1);
                user.send((Packet) new SP_RandomBoxEvent(user, boxCode));
                if (!RoutineManager.ResetRandomBoxIDs.Contains(user.userId))
                  RoutineManager.ResetRandomBoxIDs.Add(user.userId);
              }
            }
            else if (Game_Server.Configs.Server.ChristmasBoxEvent.Enabled)
            {
              if (DateTime.Now.Hour == Game_Server.Configs.Server.ChristmasBoxEvent.hour)
              {
                if (!user.RandomBoxToday)
                {
                  string boxCode = Game_Server.Configs.Server.ChristmasBoxEvent.BoxCode;
                  user.RandomBoxToday = true;
                  Inventory.AddItem(user, boxCode, -1);
                  user.send((Packet) new SP_RandomBoxEvent(user, boxCode));
                  if (!RoutineManager.ResetRandomBoxIDs.Contains(user.userId))
                    RoutineManager.ResetRandomBoxIDs.Add(user.userId);
                }
              }
            }
          }
          catch
          {
          }
        }
        if (Game_Server.Configs.Server.RandomBoxEvent.Enabled)
        {
          if (RoutineManager.ResetRandomBoxIDs.Count > 0)
          {
            DB.RunQuery("UPDATE users SET randombox='1' WHERE id IN (" + string.Join(",", RoutineManager.ResetRandomBoxIDs.Select<int, string>((Func<int, string>) (x => x.ToString())).ToArray<string>()) + ")");
            RoutineManager.ResetRandomBoxIDs.Clear();
          }
        }
        Thread.Sleep(5000);
      }
    }

    private static void DailyCheck()
    {
      while (true)
      {
        if (RoutineManager.ActualDay != DateTime.Now.Day)
        {
          RoutineManager.ActualDay = DateTime.Now.Day;
          DB.RunQuery("DELETE FROM users_events WHERE permanent = '0'");
          DB.RunQuery("UPDATE users SET todaycoupon = '0', coupontime = '0', killcount='0', randombox = '0', loginEventToday = '0'");
          DB.RunQuery("UPDATE users SET loginEventProgress = '0' WHERE loginEventProgress = '7'");
          List<int> intList = new List<int>();
          foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values)
          {
            user.dailystats = false;
            user.todaycoupons = 0;
            user.coupontime = 0;
            user.eventcount = 0;
            user.rewardEvent.doneToday = false;
            if (user.rewardEvent.progress >= 7)
            {
              user.rewardEvent.progress = 0;
              intList.Add(user.userId);
              DB.RunQuery("UPDATE users SET loginEventProgress = '0' WHERE id = '" + (object) user.userId + "'");
            }
            user.RandomBoxToday = false;
          }
          if (intList.Count > 0)
            DB.RunQuery("UPDATE users SET loginEventProgress = '0' WHERE id IN (" + string.Join(",", RoutineManager.users.Select<int, string>((Func<int, string>) (x => x.ToString())).ToArray<string>()) + ")");
          Log.WriteError("Daily reset done at " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy"));
        }
        if ((int) RankingList.Hour != DateTime.Now.Hour)
          RankingList.Load();
        GC.Collect();
        if (RoutineManager.LastTick >= 10U)
        {
          RoutineManager.LastTick = 0U;
          int num = int.Parse(string.Format("{0:yyMMddHH}", (object) DateTime.Now));
          lock (RoutineManager.obj)
          {
            foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) UserManager.ServerUsers.Values)
              RoutineManager.users.Add(user.userId);
            if (RoutineManager.users.Count > 0)
              DB.RunQuery("UPDATE users SET online = '0', serverid = '-1' WHERE id NOT IN (" + string.Join(",", RoutineManager.users.Select<int, string>((Func<int, string>) (x => x.ToString())).ToArray<string>()) + ") AND serverid = '" + (object) Game_Server.Configs.Server.serverId + "'");
            RoutineManager.users.Clear();
          }
          DB.RunQuery("UPDATE users SET banned = '0', bantime = '-1' WHERE banned = '1' AND bantime <= " + (object) num + " AND bantime != '-1'");
          BanManager.Load();
          Log.WriteLine("10 minute routine done.");
        }
        ++RoutineManager.LastTick;
        DB.RunQuery("UPDATE serverinfo SET value = '" + Game_Server.Generic.runningSinceWeb + "' WHERE name = 'uptime'");
        DB.RunQuery("UPDATE serverinfo SET value = '" + (object) Game_Server.Generic.timestamp + "' WHERE name = 'Lastupdate'");
        DB.RunQuery("UPDATE users SET premium = '0' WHERE premiumExpire < " + (object) Game_Server.Generic.timestamp);
        Thread.Sleep(60000);
      }
    }
  }
}
