using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class SP_ScoreboardInformations : Packet
    {
        private Room room;

        public SP_ScoreboardInformations(Room room)
        {
            this.room = room;
        }

        public SP_ScoreboardInformations(Room r, long milliSec)
        {
            newPacket(30053);
            addBlock(3);
            if (r.timeattack != null)
            {
                DateTime dt = DateTime.Now;

                dt = dt.AddMilliseconds(r.timeattack.time.ElapsedMilliseconds);

                TimeSpan span = dt - DateTime.Now;

                addBlock(span.TotalMilliseconds);
                switch (r.timeattack.Stage)
                {
                    case 0:
                        {
                            var v = r.users.Values.OrderByDescending(u => u.kills).Take(2);
                            foreach (User usr in v)
                            {
                                addBlock(usr.roomslot);
                                addBlock((usr.rKills > r.timeattack.stage1ZombieCount ? r.timeattack.stage1ZombieCount : usr.rKills));
                            }
                            if (v.Count() == 1)
                            {
                                addBlock(-1);
                                addBlock(0);
                            }
                            break;
                        }
                    case 1:
                        {
                            var v = r.users.Values.OrderByDescending(u => u.kills).Take(2);
                            foreach (User usr in v)
                            {
                                addBlock(usr.roomslot);
                                addBlock(usr.hackPercentage);
                            }
                            if (v.Count() == 1)
                            {
                                addBlock(-1);
                                addBlock(0);
                            }
                            break;
                        }
                    case 2:
                        {
                            var v = r.users.Values.OrderByDescending(u => u.kills).Take(2);
                            foreach (User usr in v)
                            {
                                addBlock(usr.roomslot);
                                addBlock(usr.timeattackDamagedDoor);
                            }
                            if (v.Count() == 1)
                            {
                                addBlock(-1);
                                addBlock(0);
                            }
                            break;
                        }
                    default:
                        {
                            addBlock(0);
                            addBlock(0);
                            addBlock(0);
                            addBlock(0);
                            break;
                        }
                }
            }
        }
    }
}
