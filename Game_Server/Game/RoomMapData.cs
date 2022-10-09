using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class SP_RoomMapData : Packet
    {
        public SP_RoomMapData(Room room)
        {
            newPacket(29968);
            addBlock(1);
            addBlock(room.MapData.flags);
            for (int i = 0; i < room.MapData.flags; i++)
            {
                addBlock(room.flags[i]);
            }

            addBlock(0);

            addBlock(room.users.Values.Count); // Players
            foreach (User usr in room.users.Values)
            {
                //0 -1 0 0 2 1 1000 6 0 0 
                addBlock(usr.roomslot); // Slot
                addBlock(-1);
                addBlock(usr.rKills);
                addBlock(usr.rDeaths);
                addBlock(usr.Class); // Class
                addBlock(usr.weapon);
                addBlock(usr.Health); // Health
                addBlock((usr.currentVehicle == null ? -1 : usr.currentVehicle.ID));
                addBlock((usr.currentSeat == null ? -1 : usr.currentSeat.ID));
                addBlock(0); // <- To remove if Chapter 1
            }

            List<Vehicle> changedVehicles = room.Vehicles.Values.Where(r => r.ChangedCode != string.Empty).ToList();
            
            addBlock(changedVehicles.Count);
            if (changedVehicles.Count > 0)
            {
                addBlock(" ");
                foreach (Vehicle Object in changedVehicles)
                {
                    addBlock(Object.ID);
                    addBlock(Object.Health);
                    addBlock(Object.X); // X
                    addBlock(Object.Y); // Y
                    addBlock(Object.Z); // Z
                    addBlock(Object.PosX); // Axis X
                    addBlock(Object.PosY); // Axis Y
                    addBlock(Object.PosZ); // Axis Z
                    addBlock(Object.PosZ); // Axis Z
                    addBlock(Object.ChangedCode);
                    addBlock(0);
                    addBlock(0);
                    addBlock(-75);
                    addBlock(0);
                    addBlock(0);
                    addBlock(0);
                    addBlock(53);
                    addBlock(0);
                }
            }
        }
    }
}
