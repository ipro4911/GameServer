using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class CP_ScoreBoard : Handler
    {
        public override void Handle(User usr)
        {
            if (usr.room == null) { usr.disconnect(); return; }
            usr.send(new SP_ScoreBoard(usr.room));
        }
    }

    class SP_ScoreBoard : Packet
    {
        public SP_ScoreBoard(Room Room)
        {
            newPacket(30032);
            addBlock(1);
            addBlock((Room.mode == (int)RoomMode.Explosive || Room.mode == (int)RoomMode.HeroMode ? Room.DerbRounds : 0));
            addBlock((Room.mode == (int)RoomMode.Explosive || Room.mode == (int)RoomMode.HeroMode ? Room.NIURounds : 0));

            RoomMode mode = (RoomMode)Room.mode;

            switch (mode)
            {
                case RoomMode.Explosive:
                case RoomMode.HeroMode:
                    {
                        addBlock(Room.derbHeroKill);
                        addBlock(Room.niuHeroKill);
                        break;
                    }
                case RoomMode.FFA:
                    {
                        addBlock(Room.ffakillpoints);
                        addBlock(Room.highestkills);
                        break;
                    }
                case RoomMode.FourVersusFour:
                case RoomMode.TDM:
                case RoomMode.Conquest:
                case RoomMode.TotalWar:
                case RoomMode.BGExplosive:
                    {
                        addBlock(Room.KillsDerbaranLeft);
                        addBlock(Room.KillsNIULeft);
                        break;
                    }
                default:
                    {
                        addBlock(0);
                        addBlock(0);
                        break;
                    }
            }

            // 30032 1 0 0 3 4 8 0 0 0 0 0 0 0 1 0 0 0 0 0 0 2 0 0 0 0 0 0 3 0 0 0 0 0 0 4 0 0 0 0 0 0 5 0 0 0 0 0 0 6 0 0 0 0 0 0 7 0 0 0 0 0 0
            addBlock(Room.users.Count);
            foreach (User RoomUser in Room.users.Values)
            {
                //5 0 0 0 0 0 0
                //0 2 0 0 2 0 0 0 <-- AI
                //0 1 0 0 1 0 0 0 <-- AI
                addBlock(RoomUser.roomslot);
                addBlock(RoomUser.rKills);
                addBlock(RoomUser.rDeaths);
                addBlock(RoomUser.rFlags);
                addBlock(RoomUser.rPoints);
                addBlock(1); // Assist in chapter 1
            }
        }
    }
}
