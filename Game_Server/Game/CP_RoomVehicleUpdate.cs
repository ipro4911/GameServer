// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_RoomVehicleUpdate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_RoomVehicleUpdate : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Vehicle vehicleById = usr.room.GetVehicleByID(int.Parse(this.getBlock(0)));
      if (vehicleById == null)
        return;
      vehicleById.X = this.getBlock(1);
      vehicleById.Y = this.getBlock(2);
      vehicleById.Z = this.getBlock(3);
      vehicleById.PosX = this.getBlock(4);
      vehicleById.PosY = this.getBlock(5);
      vehicleById.PosZ = this.getBlock(6);
      vehicleById.ChangedCode = this.getBlock(8);
    }
  }
}
