// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_WorldDamage
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Generic;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_WorldDamage : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      int num1 = int.Parse(this.getBlock(6));
      int num2 = int.Parse(this.getBlock(9));
      if (usr.Health <= 0 || !usr.IsAlive())
        return;
      if (!room.gameactive || num2 <= 0)
      {
        this.sendPacket = false;
      }
      else
      {
        if (num1 == 1)
        {
          if (room.mode == 16 && (room.mapid == 90 || room.mapid == 91))
            usr.Health -= usr.Health;
          else
            usr.Health -= num2;
          if (usr.Health <= 0)
          {
            this.sendBlocks[3] = (object) 157;
            this.sendBlocks[6] = (object) usr.roomslot;
            usr.OnDie();
          }
          else
          {
            this.sendBlocks[11] = (object) num2;
            this.sendBlocks[12] = (object) usr.Health;
          }
        }
        else
        {
          int num3 = int.Parse(this.getBlock(8));
          bool flag = int.Parse(this.getBlock(10)) == 1;
          Vehicle vehicleById = room.GetVehicleByID(num3);
          if (vehicleById == null)
          {
            this.sendPacket = false;
            return;
          }
          int num4 = (int) Math.Ceiling((Decimal) (vehicleById.MaxHealth * (int.Parse(this.getBlock(9)) / int.Parse(this.getBlock(10)))) / new Decimal(100));
          if (flag)
            num4 = (int) Math.Truncate((double) (vehicleById.MaxHealth * 60) / 100.0);
          vehicleById.Health -= num4;
          this.sendBlocks[9] = (object) num4;
          this.sendBlocks[11] = (object) num4;
          this.sendBlocks[12] = (object) vehicleById.Health;
          if (vehicleById.Health <= 0)
          {
            foreach (VehicleSeat vehicleSeat in (IEnumerable<VehicleSeat>) vehicleById.Seats.Values)
            {
              if (vehicleSeat.seatOwner != null)
              {
                if (vehicleSeat.seatOwner.Health <= 0)
                {
                  this.sendPacket = false;
                  return;
                }
                vehicleSeat.seatOwner.OnDie();
                room.send((Packet) new SP_RoomData(new object[28]
                {
                  (object) usr.roomslot,
                  (object) room.id,
                  (object) 2,
                  (object) 157,
                  (object) 0,
                  (object) 1,
                  (object) usr.roomslot,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) 0,
                  (object) "$"
                }));
              }
            }
            room.updateTime();
            room.send((Packet) new SP_RoomVehicleExplode(room.id, num3, usr.roomslot));
            vehicleById.Health = 0;
            return;
          }
        }
        this.sendPacket = true;
      }
    }
  }
}
