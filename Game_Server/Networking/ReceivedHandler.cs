using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;

namespace Game_Server.Networking
{
    class ReceivedHandler
    {
        public static void HandlePacket(User usr, string packet)
        {
            try
            {
                Handler handler = Managers.Packet_Manager.ParsePacket(packet);
                if (handler != null)
                {
                    handler.Handle(usr);
                }
            }
            catch
            { }
        }
    }
}
