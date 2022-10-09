// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_LeaveVehicle
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_LeaveVehicle : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive || usr.currentVehicle == null)
        return;
      int ID = int.Parse(this.getBlock(6));
      Vehicle vehicleById = room.GetVehicleByID(ID);
      if (vehicleById == null || usr.currentVehicle != vehicleById)
        return;
      vehicleById.TimeWithoutOwner = 0;
      usr.currentSeat.MainCT = int.Parse(this.getBlock(8));
      usr.currentSeat.MainCTMag = int.Parse(this.getBlock(9));
      usr.currentSeat.SubCT = int.Parse(this.getBlock(10));
      usr.currentSeat.SubCTMag = int.Parse(this.getBlock(11));
      this.sendBlocks[6] = (object) ID;
      this.sendBlocks[7] = (object) usr.currentSeat.ID;
      vehicleById.Leave(usr);
      this.sendPacket = true;
    }
  }
}
