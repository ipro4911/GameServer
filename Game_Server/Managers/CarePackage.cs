// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.CarePackage
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;
using System.Data;

namespace Game_Server.Managers
{
  internal class CarePackage
  {
    public static Dictionary<int, CarePackageItem> items = new Dictionary<int, CarePackageItem>();

    public static CarePackageItem GetItem(int ID)
    {
      if (CarePackage.items.ContainsKey(ID))
        return CarePackage.items[ID];
      return (CarePackageItem) null;
    }

    public static void Load()
    {
      CarePackage.items.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM carepackage");
      for (int key = 0; key < dataTable.Rows.Count; ++key)
      {
        DataRow row = dataTable.Rows[key];
        CarePackage.items.Add(key, new CarePackageItem()
        {
          Item = row["itemcode"].ToString(),
          Price = int.Parse(row["price"].ToString()),
          Method = int.Parse(row["method"].ToString()),
          days = int.Parse(row["itemdays"].ToString()),
          Item1 = row["loseitem1"].ToString(),
          days1 = int.Parse(row["loseitemdays1"].ToString()),
          Item2 = row["loseitem2"].ToString(),
          days2 = int.Parse(row["loseitemdays2"].ToString()),
          Item3 = row["loseitem3"].ToString(),
          days3 = int.Parse(row["loseitemdays3"].ToString()),
          Item4 = row["loseitem4"].ToString(),
          days4 = int.Parse(row["loseitemdays4"].ToString())
        });
      }
    }
  }
}
