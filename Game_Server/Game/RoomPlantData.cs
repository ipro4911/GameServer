using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class CP_RoomPlantData : Handler
    {
        public override void Handle(User usr)
        {
         
            int roomslot = int.Parse(getBlock(0));
            int Type = int.Parse(getBlock(3));
            Room Room = usr.room;
            if (Room == null || !Room.gameactive || usr.roomslot != roomslot) return;
            //Log.WriteLine(string.Join(" ", getAllBlocks()));
            if (Room.GetSide(usr) == (int)Game_Server.Room.Side.NIU && usr.weapon == 118 && Type == 0 && !Room.sleep && usr.IsAlive()) // NIU Defuse
            {
                if (Room.explosive == null) return;
                if (Room.timeleft <= 0) return;
                if (Room.bombPlanted == false) { usr.disconnect(); return; }
                Room.bombPlanted = false;
                Room.bombDefused = false;
                Room.explosive.prepareRound((int)Game_Server.Room.Side.NIU);
                Room.NIUExplosivePoints += Configs.Server.Experience.OnBombDefuse;
                Room.send(new SP_RoomPlantData(getAllBlocks));
            }
            else if (Room.GetSide(usr) == (int)Game_Server.Room.Side.Derbaran && usr.weapon == 91 && Type == 91 && !Room.sleep && usr.IsAlive()) // Derb Plant
            {

                if (Room.explosive == null) return;
                if (Room.timeleft <= 0) return;
                if (Room.bombPlanted == true) { return; }
                Room.bombPlanted = true;
                Room.DerbExplosivePoints += Configs.Server.Experience.OnBombPlant;
                Room.timeleft = 45000;
                Room.send(new SP_RoomPlantData(getAllBlocks));
            }
            else if ((Type == 80 || Type == 88 || Type == 163) && !Room.sleep && usr.IsAlive())
            {
                Room.send(new SP_RoomPlantData(getAllBlocks));
            }

        }
    
    }

    class SP_RoomPlantData : Packet
    {
        public SP_RoomPlantData(params object[] Params)
        {
            newPacket(29984);
            addBlock(1);
            for (int index = 0; index < Params.Length; ++index)
                addBlock(Params[index]);
        }
    }
}
