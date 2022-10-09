// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_RepairVehicle
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_RepairVehicle : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive)
        return;
      int ID = int.Parse(this.getBlock(6));
      int.Parse(this.getBlock(7));
      Vehicle vehicleById = room.GetVehicleByID(ID);
      if (vehicleById == null || vehicleById.Side != room.GetSide(usr) && vehicleById.Side != -1 || (vehicleById.Health >= vehicleById.MaxHealth || usr.LastRepairTick > Generic.timestamp))
        return;
      double num1 = 0.075;
      switch (ItemManager.GetItemCodeByID(usr.weapon))
      {
        case "DR01":
          num1 = 0.1;
          break;
        case "DR02":
          num1 = 0.15;
          break;
        case "DU51":
          num1 = 0.25;
          break;
      }
      int num2 = (int) Math.Truncate((double) vehicleById.MaxHealth * num1);
      vehicleById.Health += num2;
      if (vehicleById.Health > vehicleById.MaxHealth)
        vehicleById.Health = vehicleById.MaxHealth;
      usr.LastRepairTick = Generic.timestamp + 2;
      this.sendBlocks[7] = (object) vehicleById.Health;
      this.sendBlocks[8] = (object) vehicleById.MaxHealth;
      this.sendPacket = true;
    }
  }
}
