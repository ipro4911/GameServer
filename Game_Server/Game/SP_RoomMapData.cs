// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomMapData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_RoomMapData : Packet
  {
    public SP_RoomMapData(Room room)
    {
      this.newPacket((ushort) 29968);
      this.addBlock((object) 1);
      this.addBlock((object) room.MapData.flags);
      for (int index = 0; index < room.MapData.flags; ++index)
        this.addBlock((object) room.flags[index]);
      this.addBlock((object) 0);
      this.addBlock((object) room.users.Values.Count);
      foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) room.users.Values)
      {
        this.addBlock((object) user.roomslot);
        this.addBlock((object) -1);
        this.addBlock((object) user.rKills);
        this.addBlock((object) user.rDeaths);
        this.addBlock((object) user.Class);
        this.addBlock((object) user.weapon);
        this.addBlock((object) user.Health);
        this.addBlock((object) (user.currentVehicle == null ? -1 : user.currentVehicle.ID));
        this.addBlock((object) (user.currentSeat == null ? -1 : user.currentSeat.ID));
        this.addBlock((object) 0);
      }
      List<Vehicle> list = room.Vehicles.Values.Where<Vehicle>((Func<Vehicle, bool>) (r => r.ChangedCode != string.Empty)).ToList<Vehicle>();
      this.addBlock((object) list.Count);
      if (list.Count <= 0)
        return;
      this.addBlock((object) " ");
      foreach (Vehicle vehicle in list)
      {
        this.addBlock((object) vehicle.ID);
        this.addBlock((object) vehicle.Health);
        this.addBlock((object) vehicle.X);
        this.addBlock((object) vehicle.Y);
        this.addBlock((object) vehicle.Z);
        this.addBlock((object) vehicle.PosX);
        this.addBlock((object) vehicle.PosY);
        this.addBlock((object) vehicle.PosZ);
        this.addBlock((object) vehicle.PosZ);
        this.addBlock((object) vehicle.ChangedCode);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) -75);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 53);
        this.addBlock((object) 0);
      }
    }
  }
}
