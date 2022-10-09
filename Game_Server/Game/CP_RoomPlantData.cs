// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_RoomPlantData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_RoomPlantData : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      int num1 = int.Parse(this.getBlock(0));
      int num2 = int.Parse(this.getBlock(3));
      Room room = usr.room;
      if (room == null || !room.gameactive || usr.roomslot != num1)
        return;
      if (room.GetSide(usr) == 1 && usr.weapon == 118 && (num2 == 0 && !room.sleep) && usr.IsAlive())
      {
        if (room.explosive == null || room.timeleft <= 0)
          return;
        if (!room.bombPlanted)
        {
          usr.disconnect();
        }
        else
        {
          room.bombPlanted = false;
          room.bombDefused = true;
          room.explosive.prepareRound(1);
          room.NIUExplosivePoints += Game_Server.Configs.Server.Experience.OnBombDefuse;
          room.send((Packet) new SP_RoomPlantData((object[]) this.getAllBlocks));
        }
      }
      else if (room.GetSide(usr) == 0 && usr.weapon == 91 && (num2 == 91 && !room.sleep) && usr.IsAlive())
      {
        if (room.explosive == null || room.timeleft <= 0 || room.bombPlanted)
          return;
        room.bombPlanted = true;
        room.DerbExplosivePoints += Game_Server.Configs.Server.Experience.OnBombPlant;
        room.timeleft = 45000;
        room.send((Packet) new SP_RoomPlantData((object[]) this.getAllBlocks));
      }
      else
      {
        if (num2 != 80 && num2 != 88 && num2 != 163 || (room.sleep || !usr.IsAlive()))
          return;
        room.send((Packet) new SP_RoomPlantData((object[]) this.getAllBlocks));
      }
    }
  }
}
