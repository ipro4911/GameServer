// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.VehicleManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;
using System.Data;

namespace Game_Server.Managers
{
  internal class VehicleManager
  {
    public static List<VehicleManager> CollectedVehicles = new List<VehicleManager>();
    public string Code;
    public string Name;
    public int MaxHealth;
    public int RespawnTime;
    public string Seats;
    public bool isJoinable;

    public static void Load()
    {
      VehicleManager.CollectedVehicles.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM vehicles");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        string Code = row["code"].ToString();
        string Name = row["name"].ToString();
        int MaxHealth = int.Parse(row["maxhealth"].ToString());
        int RespawnTime = int.Parse(row["respawntime"].ToString());
        string Seats = (string) null;
        if (row["seats"].ToString() != "-1")
          Seats = row["seats"].ToString();
        bool isJoinable = row["joinable"].ToString() == "1";
        VehicleManager vehicleManager = new VehicleManager(Code, Name, MaxHealth, RespawnTime, Seats, isJoinable);
        VehicleManager.CollectedVehicles.Add(vehicleManager);
      }
      Log.WriteLine("Successfully loaded [" + (object) dataTable.Rows.Count + "] Vehicle Informations");
    }

    public static VehicleManager GetVehicleInfoByCode(string Code)
    {
      foreach (VehicleManager collectedVehicle in VehicleManager.CollectedVehicles)
      {
        if (collectedVehicle.Code == Code)
          return collectedVehicle;
      }
      return (VehicleManager) null;
    }

    public VehicleManager(
      string Code,
      string Name,
      int MaxHealth,
      int RespawnTime,
      string Seats,
      bool isJoinable)
    {
      this.Code = Code;
      this.Name = Name;
      this.MaxHealth = MaxHealth;
      this.RespawnTime = RespawnTime;
      this.Seats = Seats;
      this.isJoinable = isJoinable;
    }
  }
}
