// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_PlacementUse
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_PlacementUse : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive || !usr.IsAlive() || usr.Health <= 0)
        return;
      int num = int.Parse(this.getBlock(8));
      string block = this.getBlock(27);
      if (!room.Placements.ContainsKey(num) || room.getPlacement(num).Used)
        return;
      User placementOwner = room.getPlacementOwner(num);
      if (placementOwner == null)
        return;
      int side1 = room.GetSide(placementOwner);
      int side2 = room.GetSide(usr);
      switch (block)
      {
        case "DV01":
          usr.Health += 500;
          if (placementOwner != null && !usr.Equals((object) placementOwner) && (side2 == side1 && placementOwner.droppedMedicBox < 10))
          {
            ++placementOwner.droppedMedicBox;
            placementOwner.rPoints += Game_Server.Configs.Server.Experience.OnNormalPlaceUse;
            break;
          }
                    break;
                case "DU01":
          if (placementOwner != null && !usr.Equals((object) placementOwner) && (side2 == side1 && placementOwner.droppedAmmo < 10))
          {
            ++placementOwner.droppedAmmo;
            placementOwner.rPoints += Game_Server.Configs.Server.Experience.OnLandPlaceUse;
            switch (usr.Class)
            {
              case 3:
                usr.throwNades = (ushort) 0;
                break;
              case 4:
                usr.throwRockets = (ushort) 0;
                break;
            }
          }
                    break;


                case "DU02":
          usr.Health -= 100;
          if (usr.Health < 1)
            usr.Health = 1;
          if (placementOwner != null && !usr.Equals((object) placementOwner) && (side2 != side1 && placementOwner.droppedM14 < 8))
          {
            ++placementOwner.droppedM14;
            placementOwner.rPoints += Game_Server.Configs.Server.Experience.OnLandPlaceUse;
            break;
          }
          break;
        case "DS05":
          if (placementOwner != null && !usr.Equals((object) placementOwner) && (side2 != side1 && placementOwner.droppedFlash < 6))
          {
            ++placementOwner.droppedFlash;
            placementOwner.rPoints += Game_Server.Configs.Server.Experience.OnNormalPlaceUse;
            break;
          }
          break;
        case "DZ01":
          if (usr.Class == 4)
          {
            usr.throwRockets = (ushort) 0;
            break;
          }
          break;
      }
      if (usr.Health > 1000)
        usr.Health = 1000;
      this.sendBlocks[10] = (object) usr.Health;
      room.RemovePlacement(num);
      this.sendPacket = true;
    }
  }
}
