// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.NoticeManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Data;
using System.Threading;

namespace Game_Server.Managers
{
  internal class NoticeManager
  {
    private static Thread NoticeThread = (Thread) null;
    private static int rn = 0;
    private static string[] Messages;

    public static bool Load()
    {
      try
      {
        NoticeManager.LoadMessages();
        if (NoticeManager.NoticeThread == null)
        {
          NoticeManager.NoticeThread = new Thread(new ThreadStart(NoticeManager.noticeLoop));
          NoticeManager.NoticeThread.Priority = ThreadPriority.Lowest;
          NoticeManager.NoticeThread.Start();
        }
        else
          NoticeManager.NoticeThread.Start();
        Log.WriteLine("Successfully loaded [" + (object) NoticeManager.Messages.Length + "] notices");
      }
      catch
      {
      }
      return true;
    }

    private static void LoadMessages()
    {
      DataTable dataTable = DB.RunReader("SELECT * FROM notices WHERE deleted='0'");
      NoticeManager.Messages = new string[dataTable.Rows.Count];
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        NoticeManager.Messages[index] = row["message"].ToString();
      }
    }

    private static void noticeLoop()
    {
      while (true)
      {
        try
        {
          ++NoticeManager.rn;
          if (NoticeManager.rn >= 3)
          {
            NoticeManager.rn = 0;
            UserManager.sendToServer((Packet) new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, UserManager.ServerUsers.Count.ToString() + " online players. Server " + Generic.runningSince.ToLower() + " Server Time: " + DateTime.Now.ToString("HH:mm") + " - " + DateTime.Now.ToString("dd/MM/yy"), 0U, "NULL"));
          }
          else
          {
            NoticeManager.LoadMessages();
            if (NoticeManager.Messages.Length > 0)
            {
              int index = new Random().Next(0, NoticeManager.Messages.Length - 1);
              //UserManager.sendToServer((Packet) new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, NoticeManager.Messages[index], 0U, "NULL"));
            }
          }
        }
        catch
        {
        }
        Thread.Sleep(300000);
      }
    }
  }
}
