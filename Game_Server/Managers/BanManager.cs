// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.BanManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Game_Server.Managers
{
  internal class BanManager
  {
    private static List<string> BannedHWID = new List<string>();
    private static List<string> BannedMAC = new List<string>();

    public static void Load()
    {
      BanManager.LoadBannedMAC();
      BanManager.LoadBannedHWID();
    }

    public static void LoadBannedMAC()
    {
      BanManager.BannedMAC.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM macs_ban");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        BanManager.BannedMAC.Add(row["mac"].ToString());
      }
    }
        public static void unbanLoop()
        {
            while (true)
            {
                DB.RunQuery("UPDATE users SET bantime='-1', rank='1' WHERE rank ='0' AND bantime <= " + long.Parse(string.Format("{0:yyMMdd}", DateTime.Now)) + " AND bantime != '-1'");
                Thread.Sleep(10000);
            }
        }
    public static void LoadBannedHWID()
    {
      BanManager.BannedHWID.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM hwid_bans");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        BanManager.BannedHWID.Add(row["hwid"].ToString());
      }
    }

    public static bool isMacBanned(string mac)
    {
      return BanManager.BannedMAC.Cast<string>().Where<string>((Func<string, bool>) (r => r == mac)).Count<string>() > 0;
    }

    public static bool isHWIDBanned(string hwid)
    {
      return BanManager.BannedHWID.Cast<string>().Where<string>((Func<string, bool>) (r => r == hwid)).Count<string>() > 0;
    }
  }
}
