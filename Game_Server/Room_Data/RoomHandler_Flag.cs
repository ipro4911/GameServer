// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_Flag
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_Flag : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive)
        return;
      int index = int.Parse(this.getBlock(6));
      if (room.MapData != null && (index == room.MapData.derb || index == room.MapData.niu))
        return;
      int flag1 = room.flags[index];
      int side = room.GetSide(usr);
      if (flag1 == side)
        return;
      bool flag2 = room.rounds > 2;
      if (flag1 == -1)
      {
        room.flags[index] = side;
        if (usr.rFlags < Game_Server.Configs.Server.Experience.MaxFlags)
          usr.rPoints += Game_Server.Configs.Server.Experience.OnTakeFlag;
        ++usr.rFlags;
      }
      else
        room.flags[index] = -1;
      if (room.mode != 8)
      {
        if (flag2)
        {
          switch (side)
          {
            case 0:
              --room.KillsNIULeft;
              break;
            case 1:
              --room.KillsDerbaranLeft;
              break;
          }
        }
      }
      else
      {
        int num = index == 8 ? 30 : 15;
        usr.TotalWarPoint += num;
      }
      this.sendBlocks[6] = (object) index;
      this.sendBlocks[7] = (object) room.flags[index];
      this.sendBlocks[8] = (object) flag1;
      this.sendBlocks[9] = (object) index;
      this.sendBlocks[11] = (object) (room.mode == 8 ? usr.TotalWarPoint : 0);
      room.updateTime();
      this.sendPacket = true;
    }
  }
}
