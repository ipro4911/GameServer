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

namespace Game_Server.Room_Data
{
    class RoomHandler_Suicide : RoomDataHandler
    {
        public override void Handle(User usr, Room room)
        {
            if (!room.gameactive || room.mode == (int)RoomMode.Explosive || room.mode == (int)RoomMode.FFA || room.channel == 3 || usr.Health <= 0) return;

            if (usr.LastSuicideTick + 5 > Generic.timestamp || usr.currentVehicle != null) return;
            usr.LastSuicideTick = Generic.timestamp;

            bool OutOfWorldSuicide = int.Parse(getBlock(7)) == 5;

            if (OutOfWorldSuicide)
            {
                room.send(new SP_EntitySuicide(usr.roomslot, SP_EntitySuicide.SuicideType.Suicide, true));
                usr.OnDie();
                return;
            }

            if (usr.Health > 0)
            {
                usr.OnDie();
                room.updateTime();
            }

            /* Important */

            sendPacket = true;
        }
    }
}
