// Decompiled with JetBrains decompiler
// Type: Game_Server.ClanRanking
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System.Collections.Concurrent;
using System.Data;

namespace Game_Server
{
  internal class ClanRanking
  {
    public static ConcurrentDictionary<int, Clan> clans = new ConcurrentDictionary<int, Clan>();
    public static int LastUpdate = -1;

    public static void refreshclans()
    {
      ClanRanking.clans.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM clans ORDER BY exp DESC LIMIT 0, 30");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        int num = int.Parse(dataTable.Rows[index]["id]"].ToString());
        Clan clan = ClanManager.GetClan(num);
        if (clan != null)
          ClanRanking.clans.TryAdd(num, clan);
      }
    }
  }
}
