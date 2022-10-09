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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game_Server.Game;

namespace Game_Server.Room_Data
{
    class CP_RoomData : Handler
    {
        internal enum DropType
        {
            Respawn = 0,
            Medic = 1,
            Ammo = 2,
            Repair = 3
        }

        public override void Handle(User usr)
        {

            if (blocks.Length >= 6 && usr.room != null)
            {
                Room room = usr.room;
                int roomSlot = int.Parse(getBlock(0));
                int roomId = int.Parse(getBlock(1));
                if (roomSlot == usr.roomslot && roomId == room.id)
                {
                    Subtype type = (Subtype)int.Parse(getBlock(3));

                    object[] sendBlocks = new object[blocks.Length - 1];
                    Array.Copy(blocks, sendBlocks, sendBlocks.Length);

                    /* Handle each subtype (if handler is available) */
                   // Log.WriteDebug(string.Join(" ", this.getAllBlocks));

                    using (RoomDataHandler handler = Managers.RoomPacketManager.ParsePacket((int)type, sendBlocks))
                    {
                        if (handler != null)
                        {
                            handler.Handle(usr, room);

                            /* Send out packet to the room*/

                            sendBlocks = handler.sendBlocks;

                            int subtypeId = int.Parse(sendBlocks[3].ToString());

                            if (handler.sendPacket)
                            {
                                if (subtypeId == (int)Subtype.ServerRoomReady)
                                {
                                    if (!room.firstingame)
                                    {
                                        room.RespawnAllVehicles();
                                        room.firstingame = true;
                                    }

                                    if (room.MapData != null)
                                    {
                                        usr.send(new SP_RoomMapData(room));
                                        usr.send(new SP_RoomInitializeUsers(room));
                                    }

                                    usr.isSpawned = false;

                                    usr.mapLoaded = true;

                                    usr.send(new SP_RoomData(sendBlocks));
                                }
                                else if (subtypeId == (int)Subtype.BackToRoom)
                                {
                                    usr.send(new SP_RoomData(sendBlocks));
                                }
                                else if (subtypeId == (int)Subtype.VoteKick)
                                {
                                    byte[] buffer = (new SP_RoomData(sendBlocks)).GetBytes();
                                    int usrside = room.GetSide(usr);
                                    foreach (User u in room.users.Values)
                                    {
                                        if (room.GetSide(u) == usrside)
                                        {
                                            u.sendBuffer(buffer);
                                        }
                                    }
                                }
                                else
                                {
                                    room.send(new SP_RoomData(sendBlocks));
                                }

                                if (handler.lobbychanges)
                                {
                                    room.ch.UpdateLobby(room);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class SP_RoomData : Packet
    {
        public SP_RoomData(params object[] Params)
        {

            newPacket(30000);
            addBlock(1); // Success
            Params.ToList().ForEach(action: r => { addBlock(r); });
        }
    }

    class SP_RoomDataNewRound : Packet
    {
        public SP_RoomDataNewRound(Room Room, int WinningTeam, bool Prepare)
        {
            int Code = (Prepare == true ? 6 : 5);
            newPacket(30000);
            addBlock(1);
            addBlock(-1);
            addBlock(Room.id);
            addBlock(1);
            addBlock(Code);
            addBlock(0);
            addBlock(1);
            addBlock(WinningTeam);
            addBlock(Room.DerbRounds);
            addBlock(Room.NIURounds);
        }
    }

    class SP_InitializeNewRound : Packet
    {
        public SP_InitializeNewRound(Room r)
        {
          

            newPacket(30000);
            addBlock(1);
            addBlock(-1);
            addBlock(r.id);
            addBlock(1);
            addBlock(403);
            addBlock(0);
            addBlock(1);
            addBlock(r.WinningTeam);
            addBlock(r.DerbRounds);
            addBlock(r.NIURounds);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(-1);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock(0);
            addBlock("DS05");
        }
    }
}
