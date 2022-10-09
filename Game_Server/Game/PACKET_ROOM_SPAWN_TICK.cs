//using Game_Server;

//namespace GameServer.Networking.Packets
//{
//    class PACKET_ROOM_SPAWN_TICK : Packet
//    {
//        public PACKET_ROOM_SPAWN_TICK(Room Room)
//        {
//            newPacket(30016);
//            if (Room.channel == 4)
//            {
//                addBlock(-1);
//                addBlock(Room.timespent);
//                addBlock(Room.ZombiePoints); // Points
//                addBlock(Room.ZombiePoints); // Total Points
//                addBlock(30);
//            }
//            else if (Room.channel == 1)
//            {
//                addBlock(Room.timespent); // Spawn Counter
//                addBlock(Room.timeleft); // Time Left
//                addBlock(((Room.mode == 0) ? Room.cDerbRounds : -1)); //Current Round
//                addBlock(((Room.mode == 0) ? Room.cNiuRounds : -1)); //Current Round
//                if (Room.mode == 1)
//                {
//                    addBlock(Room.ffakillpoints); // Mission Kills (FFA)
//                    addBlock(Room.highestkills); // Current Leader Kills (FFA)
//                }
//                else
//                {
//                    if (Room.mode == 2)
//                    {
//                        addBlock(Room.KillsDerbaranLeft); // Score DERBERAN
//                        addBlock(Room.KillsNIULeft); // Score NIU
//                    }
//                    else if (Room.mode == 0)
//                    {
//                        addBlock(Room.AliveUsers(0)); // Players a life DERB
//                        addBlock(Room.AliveUsers(1));  // Players a life NIU
//                    }
//                }

//                if (Room.mode == 0)
//                {
//                    addBlock(Room.cDerbRounds);
//                    addBlock(Room.cNiuRounds);
//                }
//                addBlock(30);
//            }
//        }
//    }
//}

using Game_Server;
using System;
using Game_Server.GameModes;



namespace GameServer.Networking.Packets
{
    class PACKET_ROOM_SPAWN_TICK : Packet
    {
        public PACKET_ROOM_SPAWN_TICK(Room Room)
        {
            newPacket(30016);
            if (Room.channel == 4)
            {
                addBlock(-1);
                addBlock(Room.RoundTimeSpent);
                addBlock(Room.ZombiePoints); // Points
                addBlock(Room.ZombiePoints); // Total Points
                addBlock(30);
            }
            else if (Room.channel == 1)
            {
                addBlock(Room.RoundTimeSpent); // Spawn Counter
                addBlock(Room.timeleft); // Time Left
                addBlock(((Room.mode == 0) ? Room.cDerbRounds : -1)); //Current Round
                addBlock(((Room.mode == 0) ? Room.cNiuRounds : -1)); //Current Round
                if (Room.mode == 1)
                {
                    addBlock(Room.ffakillpoints); // Mission Kills (FFA)
                    addBlock(Room.highestkills); // Current Leader Kills (FFA)
                }
                else
                {
                    if (Room.mode == 2)
                    {
                        addBlock(Room.KillsDerbaranLeft); // Score DERBERAN
                        addBlock(Room.KillsNIULeft); // Score NIU
                    }
                    else if (Room.mode == 0)
                    {
                        addBlock(Room.AliveUsers(0)); // Players a life DERB
                        addBlock(Room.AliveUsers(1));  // Players a life NIU
                    }
                }

                if (Room.mode == 0)
                {
                    addBlock(Room.cDerbRounds);
                    addBlock(Room.cNiuRounds);
                }
                addBlock(30);
            }
        }
    }
}

