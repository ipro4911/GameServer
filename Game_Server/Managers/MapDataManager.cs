// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.MapDataManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Concurrent;
using System.Data;

namespace Game_Server.Managers
{
  internal class MapDataManager
  {
    public static ConcurrentDictionary<int, MapData> datas = new ConcurrentDictionary<int, MapData>();

    public static void Load()
    {
      MapDataManager.datas.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM maps");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        int num = int.Parse(row["mapid"].ToString());
        if (!MapDataManager.datas.ContainsKey(num))
        {
          try
          {
            string name = row["name"].ToString();
            int flags = int.Parse(row["flags"].ToString());
            string[] strArray = row["defaultflags"].ToString().Split('|');
            int derb = int.Parse(strArray[0]);
            int niu = int.Parse(strArray[1]);
            string vehicleString = row["vehicles"].ToString();
            MapData mapData = new MapData(num, name, flags, derb, niu, vehicleString);
            MapDataManager.datas.TryAdd(num, mapData);
          }
          catch (Exception ex)
          {
            Log.WriteError("Coudln't Load map id " + (object) num + "[" + ex.Message + "]");
          }
        }
        else
          Log.WriteError("Map ID [" + (object) num + "] its already in the dictionary, maybe some duplicate (?)");
      }
      Log.WriteLine("Successfully loaded [" + (object) MapDataManager.datas.Count + "] MapDatas");
    }

    public static MapData GetMapByID(int MapID)
    {
      if (MapDataManager.datas.ContainsKey(MapID))
        return MapDataManager.datas[MapID];
      return (MapData) null;
    }
  }
}
