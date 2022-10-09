// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_CaptureModeRequest
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_CaptureModeRequest : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive || room.capturemode == null || (usr.currentVehicle == null || !(usr.currentVehicle.Code == "ED13")))
        return;
      this.sendBlocks[3] = (object) 157;
      this.sendBlocks[8] = (object) 7;
      int num = int.Parse(this.getBlock(9));
      if (room.GetSide(usr) == 1 && num == 1)
        room.capturemode.NIUPoints += 20;
      else
        room.capturemode.DerbaranPoints += 20;
      usr.rPoints += 50;
      ++usr.rAssist;
    }
  }
}
