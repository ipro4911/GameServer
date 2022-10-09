// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_ChangeVehicleSeat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_ChangeVehicleSeat : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive || usr.currentVehicle == null)
        return;
      int ID = int.Parse(this.getBlock(6));
      int num = int.Parse(this.getBlock(7));
      Vehicle vehicleById = room.GetVehicleByID(ID);
      if (vehicleById == null || usr.currentVehicle != vehicleById || (usr.currentSeat.ID == num || vehicleById.Side != room.GetSide(usr)) || !vehicleById.FreeSeat(num))
        return;
      int id = usr.currentSeat.ID;
      usr.currentSeat.MainCT = int.Parse(this.getBlock(8));
      usr.currentSeat.MainCTMag = int.Parse(this.getBlock(9));
      usr.currentSeat.SubCT = int.Parse(this.getBlock(10));
      usr.currentSeat.SubCTMag = int.Parse(this.getBlock(11));
      vehicleById.SwitchSeat(num, usr);
      this.sendBlocks[6] = (object) ID;
      this.sendBlocks[7] = (object) num;
      this.sendBlocks[8] = (object) id;
      this.sendBlocks[9] = (object) usr.currentSeat.MainCT;
      this.sendBlocks[10] = (object) usr.currentSeat.MainCTMag;
      this.sendBlocks[11] = (object) usr.currentSeat.SubCT;
      this.sendBlocks[12] = (object) usr.currentSeat.SubCTMag;
      this.sendPacket = true;
    }
  }
}
