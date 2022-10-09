using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game_Server.Room_Data;

namespace Game_Server.Game
{
    class SP_ZombieDrop : Packet
    {
        public SP_ZombieDrop(User usr, int ID, int UsementID, int Type)
        {
            newPacket(30000);
            addBlock(1);
            addBlock(ID);
            addBlock(17);
            addBlock(2);
            addBlock(901);
            addBlock(UsementID);
            addBlock(0);
            addBlock(-1); // Useless
            addBlock(Type); // (0 = Respawn, 1 = Medic, 2 = Ammo, 3 = Repair)
            addBlock(usr.sessionId + UsementID);
            addBlock(ID);
            addBlock(UsementID);
            addBlock(13);
            addBlock(UsementID);
            addBlock(13);
            addBlock(UsementID);
        }
    }

    class SP_ZombieNewWave : Packet
    {
        public SP_ZombieNewWave(int Wave, bool LastWave = false)
        {
            newPacket(13431);
            addBlock(1);
            addBlock(13);
            addBlock(Wave);
            addBlock(LastWave ? 1 : 0);
        }
    }

    class SP_ZombieEndGameItem : Packet
    {
        public SP_ZombieEndGameItem(int choose, int rs)
        {
            newPacket(30053);
            addBlock(6);
            addBlock(0);
            addBlock(choose);
            addBlock(rs);
        }
    }

    class CP_ZombieNewStage : Handler
    {
        public override void Handle(User usr)
        {
            Log.WriteLine(string.Join(" ", getAllBlocks));
            int v = int.Parse(getBlock(0));
            Room room = usr.room;
            if (room != null && room.timeattack != null)
            {
                Zombie zombie;
                if (v == 4)
                {
                    int doorNumber = int.Parse(getBlock(1));
                    room.timeattack.waitForPrepare = 1;
                    room.timeattack.time.Stop();
                    for (int i = 4; i < 32; i++)
                    {
                        zombie = room.GetZombieByID(i);
                        room.send(new SP_EntitySuicide(i));
                        zombie.Health = 0;
                        zombie.respawn = Generic.timestamp + 4;
                    }
                }
                else if (v == 6)
                {
                    for (int i = 4; i < 32; i++)
                    {
                        zombie = room.GetZombieByID(i);
                        room.send(new SP_EntitySuicide(i));
                        zombie.Health = 0;
                        zombie.respawn = Generic.timestamp + 4;
                    }

                    if (usr.RandomSupplyBoxSelected) return;
                    usr.RandomSupplyBoxSelected = true;

                    int choose = int.Parse(getBlock(1));
                    usr.timeattackBoxChoose = choose;
                    room.send(new SP_ZombieEndGameItem(choose, usr.roomslot));
                    room.timeattack.waitBeforeSupplyBoxItemsOut = 20;
                }
            }
        }
    }

    class SP_ZombieNewStage : Packet
    {
        public SP_ZombieNewStage(Room Room, int stage)
        {
            newPacket(30053);
            addBlock(stage); // 1 - Choose your prize
            if ((stage - 1) == 1)
                addBlock(Room.timeattack.zombieForStage);
        }
    }

    class SP_ZombieSpawn : Packet
    {
        public SP_ZombieSpawn(int Slot, int FollowUser, int Place, int ZombieType, int health)
        {
            newPacket(13432);
            addBlock(Slot);
            addBlock(FollowUser);
            addBlock(ZombieType); // ZombieType
            addBlock(Place);
            addBlock(health);
        }
    }

    class SP_ZombieChangeTarget : Packet
    {
        public SP_ZombieChangeTarget(Room Room, int RoomSlot)
        {
            List<Zombie> list = Room.ZombieFollowers(RoomSlot);
            newPacket(13433);
            addBlock(RoomSlot);
            addBlock(list.Count);
            foreach (Zombie z in list)
            {
                addBlock(z.ID);
                addBlock(Room.master);
            }
        }
    }

    class SP_ZombieSkillpointUpdate : Packet
    {
        public SP_ZombieSkillpointUpdate(User usr)
        {
            newPacket(31495);
            addBlock(usr.roomslot);
            addBlock(usr.skillPoints);
        }
    }


    
    class CP_ZombieSkillPointRequest : Handler
    {
        public override void Handle(User usr)
        {
            Room Room = usr.room;
            if (Room != null && Room.gameactive && Room.channel == 3 && Room.mode == (int)RoomMode.Defence)
            {
                int Type = int.Parse(getBlock(1));
                bool send = true;
                switch (Type)
                {
                    case 1:
                        if (usr.skillPoints < 5) { usr.disconnect(); send = false; }
                        break;
                    case 2:
                        if (usr.skillPoints < 10) { usr.disconnect(); send = false; }
                        break;
                    case 3:
                        if (usr.skillPoints < 20) { usr.disconnect(); send = false; }
                        break;
                    default:
                        {
                            send = false;
                        }
                        break;
                }
                if (send)
                {
                    usr.skillPoints = 0;
                    Room.send(new SP_Unknown(31492, getAllBlocks));
                }
            }
        }
    }

    class CP_ZombieMultiPlayer : Handler
    {
        public override void Handle(User usr)
        {
            if (usr.channel == 3)
            {
                Room Room = usr.room;
                if (Room != null)
                {
                    if (Room.users.Count > 1)
                    {
                        Room.send(new SP_Unknown(31490, getAllBlocks));
                    }
                }
            }
        }


    }
    class CP_ZOMBIE_UPDATE : Handler
    {
        public override void Handle(User usr)
        {
            try
            {

                Room Room = usr.room;
                if (Room == null || Room.channel != 3) return;
                if (Room != null)
                {
                    usr.room.send(new SP_Unknown(31492, getAllBlocks));
                    usr.room.send(new SP_RoomData(3000, getAllBlocks));

                }
            }
            catch(Exception e) {Console.WriteLine(e); }
        }
    }
}
