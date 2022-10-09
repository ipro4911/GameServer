using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Room_Data
{
    class RoomHandler_AmmoRecharge : RoomDataHandler
    {
        public override void Handle(User usr, Room room)
        {
            if (usr.LastAmmoRechargeTick > Generic.timestamp) return;
            usr.LastAmmoRechargeTick = Generic.timestamp + 4;

            usr.throwNades = 0;
            usr.throwRockets = 0;

            /* Important */
            sendPacket = true;
        }
    }
}
