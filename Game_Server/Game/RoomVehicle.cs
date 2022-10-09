using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class SP_RoomVehicleExplode : Packet
    {
        public SP_RoomVehicleExplode(int RoomID, int TargetID, int shooterId)
        {
            //16 20 2 153 0 1 16 21 21 580 15 27 134217728 0 0 0 0 0 0 0 0 0 0 0 0 0 0 FFFF
            //0 3 2 153 0 1 0 4 0 2 0 49 0 1 1 100 -300 400 15 6112.0200 1344.3668 4490.7837 0 0 0 0 0 EJ05 
            newPacket(30000);
            addBlock(1);
            addBlock(shooterId); // usr
            addBlock(RoomID); // Room id
            addBlock(2);
            addBlock(153);
            addBlock(0);
            addBlock(1);
            addBlock(0);
            addBlock(TargetID); // Target
            addBlock(shooterId); // Target
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(100);
            addBlock(0);
            addBlock(0);
            // Coords //
            addBlock(0);
            addBlock(0);
            addBlock(0);
            // End Coords //
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0); // Remove for chapter 1
            addBlock("FFFF");
        }
    }

    class SP_RoomRespawnVehicle : Packet
    {
        public SP_RoomRespawnVehicle(int ID, Room room)
        {
            newPacket(30000);
            addBlock(1);
            addBlock(-1);
            addBlock(13);
            addBlock(2);
            addBlock(151);
            addBlock(0);
            addBlock(1);
            addBlock(ID);
            addBlock(0);
            addBlock(0);
            addBlock(room.kills);
            addBlock(20);
            addBlock(1);
            addBlock(0);
            addBlock(0);
            addBlock(1200000);
            addBlock(-1036745);
            addBlock(1200000);
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock("0.0000");
            addBlock(0);
            addBlock(0);
            addBlock("DV01");
        }
    }

    class CP_RoomVehicleUpdate : Handler
    {
        public override void Handle(User usr)
        {
            // 0 190.4736 5.6864 189.3861 -0.5000 -0.5000 0.5000 0.5000 @ 407 0 0 0 41 0 0 0
            Vehicle Vehicle = usr.room.GetVehicleByID(int.Parse(getBlock(0)));
            if (Vehicle != null)
            {
                Vehicle.X = getBlock(1);
                Vehicle.Y = getBlock(2);
                Vehicle.Z = getBlock(3);
                Vehicle.PosX = getBlock(4);
                Vehicle.PosY = getBlock(5);
                Vehicle.PosZ = getBlock(6);
                Vehicle.ChangedCode = getBlock(8);
            }
        }
    }
}
