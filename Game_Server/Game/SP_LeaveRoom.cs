using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class CP_LeaveRoom : Handler
    {
        public override void Handle(User usr)
        {

            Room room = usr.room;
            if (room != null)
            {
                if (room.users.ContainsKey(usr.roomslot))
                {
                    if (room.users[usr.roomslot].userId == usr.userId)
                    {
                        room.RemoveUser(usr.roomslot);
                    }
                    else
                    {
                        /* Fake packet (?) */
                        Log.WriteError("Something went wrong while leaving room");
                        usr.disconnect();
                    }
                }

                usr.lobbypage = 0;
                usr.send(new SP_RoomList(usr, usr.lobbypage, false));
            }
        }
    }

    class SP_LeaveRoom : Packet
    {
        public SP_LeaveRoom(int sessionId, Room r, int oldSlot, int newMaster)
        {
            // Used for crashed people
            newPacket(29504);
            addBlock(1);
            addBlock(sessionId); // SessionID
            addBlock(oldSlot); // Position in Room
            addBlock(1); // ?
            addBlock(newMaster); // Master Slot
            addBlock(0); // Exp
            addBlock(0); // Dinar
        }

        public SP_LeaveRoom(User usr, Room r, int oldSlot, int newMaster)
        {
            //29504 1 814 8 1 0 5375 70887
            newPacket(29504);
            addBlock(1);
            addBlock(usr.sessionId); // SessionID
            addBlock(oldSlot); // Position in Room
            addBlock(1); // ?
            addBlock(newMaster); // Master Slot
            addBlock(usr.exp); // Exp
            addBlock(usr.dinar); // Dinar
        }
    }
}
