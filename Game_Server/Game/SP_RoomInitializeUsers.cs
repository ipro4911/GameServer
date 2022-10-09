// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomInitializeUsers
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_RoomInitializeUsers : Packet
  {
    public SP_RoomInitializeUsers(Room room)
    {
      this.newPacket((ushort) 30017);
      this.addBlock((object) room.users.Count);
      foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) room.users.Values)
      {
        this.addBlock((object) user.roomslot);
        this.addBlock((object) user.Health);
        this.addBlock((object) (user.currentVehicle == null ? -1 : user.currentVehicle.ID));
        this.addBlock((object) (user.currentSeat == null ? -1 : user.currentSeat.ID));
        this.addBlock((object) -1);
      }
      this.addBlock((object) room.Vehicles.Count);
      foreach (Vehicle vehicle in (IEnumerable<Vehicle>) room.Vehicles.Values)
      {
        this.addBlock((object) vehicle.ID);
        this.addBlock((object) vehicle.Health);
        this.addBlock((object) vehicle.MaxHealth);
        this.addBlock((object) "NULL");
      }
    }
  }
}
