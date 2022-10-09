// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.GunSmithManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System.Collections.Generic;
using System.Data;

namespace Game_Server.Managers
{
  internal class GunSmithManager
  {
    public static Dictionary<int, GunSmith> items = new Dictionary<int, GunSmith>();

    public static void Load()
    {
      GunSmithManager.items.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM gunsmith");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        int num = int.Parse(row["gameid"].ToString());
        int result = 0;
        string str = row["item"].ToString();
        string rare = row["rare"].ToString();
        string[] required_materials = row["required_materials"].ToString().Split(',');
        int.TryParse(row["cost"].ToString(), out result);
        string[] required_items = row["required_items"].ToString().Split(',');
        string[] lose_items = row["lose_items"].ToString().Split(',');
        GunSmith gunSmith = new GunSmith(num, result, str, rare, required_materials, required_items, lose_items);
        if (!GunSmithManager.items.ContainsKey(num))
          GunSmithManager.items.Add(num, gunSmith);
        else
          Log.WriteError("Couldn't add GunSmith ID " + (object) num + " [DUPLICATE]");
      }
      Log.WriteLine("Successfully loaded [" + (object) GunSmithManager.items.Count + "] GunSmith Items");
    }

    public static GunSmith GetGunSmithByGameId(int id)
    {
      if (GunSmithManager.items.ContainsKey(id))
        return GunSmithManager.items[id];
      return (GunSmith) null;
    }
  }
}
