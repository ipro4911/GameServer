// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_JoinVehicle
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_JoinVehicle : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive)
        return;
      int ID = int.Parse(this.getBlock(6));
      Vehicle vehicleById = room.GetVehicleByID(ID);
      if (vehicleById == null || usr.currentVehicle != null || vehicleById.Seats.Count < 1 || (vehicleById.Side != room.GetSide(usr) && vehicleById.Side != -1 || (vehicleById.Health <= 0 || usr.Health <= 0)) || (!usr.IsAlive() || !vehicleById.isJoinable))
        return;
      usr.currentVehicle = vehicleById;
      vehicleById.TimeWithoutOwner = 0;
      vehicleById.Join(usr);
      this.sendBlocks[6] = (object) vehicleById.ID;
      this.sendBlocks[7] = (object) vehicleById.GetSeatByUser(usr).ID;
      this.sendBlocks[8] = (object) vehicleById.Health;
      this.sendBlocks[9] = (object) vehicleById.MaxHealth;
      this.sendBlocks[10] = (object) usr.currentSeat.MainCT;
      this.sendBlocks[11] = (object) usr.currentSeat.MainCTMag;
      this.sendBlocks[12] = (object) usr.currentSeat.SubCT;
      this.sendBlocks[13] = (object) usr.currentSeat.SubCTMag;
      this.sendPacket = true;
    }
  }
}
