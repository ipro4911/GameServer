// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.EXPEventManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Linq;
using System.Threading;

namespace Game_Server.Managers
{
  internal class EXPEventManager
  {
    public static bool isRunning = false;
    public static int EventType = -1;
    public static int EventTime = -1;
    public static double EXPRate = -1.0;
    public static double DinarRate = -1.0;
    private static Thread EventThread = (Thread) null;

    public static void Load()
    {
      try
      {
        EXPEventManager.isRunning = false;
        EXPEventManager.EventThread = new Thread(new ThreadStart(EXPEventManager.EventLoop));
        EXPEventManager.EventThread.Priority = ThreadPriority.Lowest;
        EXPEventManager.EventThread.Start();
        DB.RunQuery("UPDATE serverinfo SET value='0' WHERE name='expevent'");
        Log.WriteLine("EXP/Dinar event manager is running properly!");
      }
      catch (Exception ex)
      {
        Log.WriteError("EXP/Dinar event manager didn't Load properly: " + ex.Message);
      }
    }

    private static void EventLoop()
    {
      while (true)
      {
        if (EXPEventManager.isRunning)
        {
          if (EXPEventManager.EventTime > 0)
            EXPEventManager.EventTime -= 5;
          else if (EXPEventManager.EventTime <= 0)
            EXPEventManager.StopEvent();
        }
        Thread.Sleep(5000);
      }
    }

    public static void StartEvent(int minute, double exp, double dinar)
    {
      EXPEventManager.isRunning = true;
      EXPEventManager.EventTime = minute;
      EXPEventManager.EXPRate = exp;
      EXPEventManager.DinarRate = dinar;
     // UserManager.sendToServer((Packet) new SP_ExpEvent(SP_ExpEvent.EventCodes.EXP_Activate));
      int num = (int) Math.Ceiling((Decimal) EXPEventManager.EventTime / new Decimal(60));
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.AddMinutes((double) num);
      DB.RunQuery("UPDATE serverinfo SET value='1' WHERE name='expevent'");
      DB.RunQuery("UPDATE serverinfo SET value='" + dateTime.ToString("HH:mm dd/MM/yy") + "' WHERE name='expexpire'");
      DB.RunQuery("UPDATE serverinfo SET value='" + (Math.Ceiling((Decimal) (exp * 100.0)).ToString() + "%, " + (object) (int) Math.Ceiling((Decimal) (dinar * 100.0)) + "%") + "' WHERE name='exprate'");
     // UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (u => u != null)).ToList<Game_Server.User>().ForEach((Action<Game_Server.User>) (usr => usr.send((Packet) new SP_PingInformation(usr))));
    }

    public static void StopEvent()
    {
      EXPEventManager.isRunning = false;
      EXPEventManager.EventTime = -1;
      EXPEventManager.EXPRate = 1.0;
      EXPEventManager.DinarRate = 1.0;
      UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice2, "A EXP Event" + " Ended", 999, "NULL"));
            //UserManager.sendToServer((Packet) new SP_ExpEvent(SP_ExpEvent.EventCodes.EXP_Deactivate));
            DB.RunQuery("INSERT INTO admincp_logs (adminid, log, date, timestamp) VALUES ('-1', 'EXP / Dinar event end!', '" + Generic.currentDate + "','" + (object) Generic.timestamp + "')");
      DB.RunQuery("UPDATE serverinfo SET value='0' WHERE name='expevent'");
    }
  }
}
