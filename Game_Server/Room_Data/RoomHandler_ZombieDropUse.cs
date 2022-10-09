// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_ZombieDropUse
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

/*namespace Game_Server.Room_Data
{
  internal class RoomHandler_ZombieDropUse : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive && room.channel != 3)
        return;
      switch (int.Parse(this.getBlock(7)))
      {
        case 0:
        case 2:
          --room.DropID;
          this.sendPacket = true;
          break;
        case 1:
          usr.Health = 1000;
          this.sendBlocks[10] = (object) usr.Health;
          goto case 0;
        case 3:
          int incubatorVehicleId = room.GetIncubatorVehicleId();
          Vehicle vehicleById = room.GetVehicleByID(incubatorVehicleId);
          if (vehicleById != null)
          {
            vehicleById.Health += 10000;
            if (vehicleById.Health > vehicleById.MaxHealth)
              vehicleById.Health = vehicleById.MaxHealth + 1;
            this.sendBlocks[10] = (object) vehicleById.Health;
            this.sendBlocks[11] = (object) vehicleById.MaxHealth;
            goto case 0;
          }
          else
            goto case 0;
        default:
          Log.WriteError("Unknown Zombie Drop ID: " + (object) int.Parse(this.getBlock(7)));
          goto case 0;
      }
    }

    internal enum DropType
    {
      Respawn,
      Medic,
      Ammo,
      Repair,
    }
  }
}
/*
 _____   ___ __  __  _____ _____   ___  _  __              ___   ___   __    __ 
/__   \ /___\\ \/ /  \_   \\_   \ / __\( )/ _\            / __\ /___\ /__\  /__\
  / /\///  // \  /    / /\/ / /\// /   |/ \ \            / /   //  /// \// /_\  
 / /  / \_//  /  \ /\/ /_/\/ /_ / /___    _\ \          / /___/ \_/// _  \//__  
 \/   \___/  /_/\_\\____/\____/ \____/    \__/          \____/\___/ \/ \_/\__/  
__________________________________________________________________________________

Created by: ToXiiC
Thanks to: CodeDragon, Kill1212, CodeDragon

*/

namespace Game_Server.Room_Data
{
    class RoomHandler_ZombieDropUse : RoomDataHandler
    {
        internal enum DropType
        {
            Respawn = 0,
            Medic = 1,
            Ammo = 2,
            Repair = 3
        }

        public override void Handle(User usr, Room room)
        {
            if (!room.gameactive && room.channel != 3) return;

            DropType DropType = (DropType)(int.Parse(getBlock(7)));

            switch (DropType)
            {
                case DropType.Respawn:
                    {
                        break;
                    }
                case DropType.Medic:
                    {
                        usr.Health = 1000;
                        sendBlocks[10] = usr.Health;
                        break;
                    }
                case DropType.Ammo:
                    {
                        break;
                    }
                case DropType.Repair:
                    {
                        int vehicleId = room.GetIncubatorVehicleId();
                        Vehicle Vehicle = room.GetVehicleByID(vehicleId);
                        if (Vehicle != null)
                        {
                            Vehicle.Health += 10000;

                            if (Vehicle.Health > Vehicle.MaxHealth)
                                Vehicle.Health = Vehicle.MaxHealth + 1;

                            sendBlocks[10] = Vehicle.Health;
                            sendBlocks[11] = Vehicle.MaxHealth;
                        }
                        break;
                    }
                default:
                    {
                        Log.WriteError("Unknown Zombie Drop ID: " + int.Parse(getBlock(7)));
                        break; // Unknown
                    }
            }

            room.DropID--;

            /* Important */

            sendPacket = true;
        }
    }
}

