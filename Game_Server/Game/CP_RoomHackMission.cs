//// Decompiled with JetBrains decompiler
//// Type: Game_Server.Game.CP_RoomHackMission
//// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
//// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

//namespace Game_Server.Game
//{
//  internal class CP_RoomHackMission : Handler
//  {
//    public override void Handle(Game_Server.User usr)
//    {
//      Room room = usr.room;
//      int num1 = room.HackPercentage.BaseA + room.HackPercentage.BaseB;
//      if (!room.gameactive || num1 >= 100)
//        return;
//      usr.hackingBase = int.Parse(this.getBlock(3));
//      int Type1 = 2;
//      if (this.getBlock(1) == "0" && this.getBlock(2) == "0" && (this.getBlock(3) == "0" || this.getBlock(3) == "1"))
//        usr.send((Packet) new SP_RoomHackMission(usr.roomslot, usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB, 0, usr.hackingBase));
//      else if (this.getBlock(1) == "1" && this.getBlock(2) == "6" && (this.getBlock(3) == "0" && !room.PickuppedC4))
//      {
//        if (room.mapid == 60)
//        {
//          if (room.GetSide(usr) != 0)
//            return;
//          room.PickuppedC4 = true;
//          usr.hasC4 = true;
//          room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
//          {
//            (object) 0,
//            (object) 0,
//            (object) 1,
//            (object) 6,
//            (object) 0,
//            (object) 0,
//            (object) -1,
//            (object) 0
//          }));
//          room.send((Packet) new SP_Unknown((ushort) 30000, new object[15]
//          {
//            (object) 1,
//            (object) usr.roomslot,
//            (object) usr.room.id,
//            (object) 2,
//            (object) 155,
//            (object) 0,
//            (object) 0,
//            (object) 92,
//            (object) 0,
//            (object) 0,
//            (object) 0,
//            (object) 0,
//            (object) 0,
//            (object) 0,
//            (object) 0
//          }));
//        }
//        else
//          Log.WriteError("Tried to pickup C4 in room " + (object) room.mapid);
//      }
//      else if (this.getBlock(1) == "1" && this.getBlock(2) == "0" && this.getBlock(3) == "0")
//      {
//        if (room.mapid == 60)
//        {
//          room.PickuppedC4 = false;
//          usr.hasC4 = false;
//          room.SiegeWarC4User = usr;
//          room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
//          {
//            (object) 0,
//            (object) 0,
//            (object) 1,
//            (object) 0,
//            (object) 0,
//            (object) 0,
//            (object) 0,
//            (object) 0
//          }));
//          room.SiegeWarTime = 3;
//        }
//        else
//          Log.WriteError("Tried to use C4 in room " + (object) room.mapid);
//      }
//      else if (int.Parse(this.getBlock(2)) == 3)
//      {
//        usr.isHacking = false;
//        int Type2 = 3;
//        room.send((Packet) new SP_RoomHackMission(usr.roomslot, usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB, Type2, usr.hackingBase));
//      }
//      else
//      {
//        if (room == null || usr.LastHackTick > Generic.timestamp && usr.LastHackTick > 0)
//          return;
//        if (usr.hackingBase != usr.LastHackBase)
//          usr.hackTick = 0;
//        if (usr.LastHackTick < Generic.timestamp || usr.LastHackTick == 0)
//        {
//          usr.LastHackTick = Generic.timestamp + 1;
//          ++usr.hackTick;
//          ++usr.rPoints;
//          usr.hackTick = 0;
//          if (usr.hackingBase == 0)
//            ++room.HackPercentage.BaseA;
//          else
//            ++room.HackPercentage.BaseB;
//        }
//        int num2 = room.HackPercentage.BaseA + room.HackPercentage.BaseB;
//        if (usr.GMMode)
//          num2 = 100;
//        usr.LastHackBase = usr.hackingBase;
//        ++usr.hackPercentage;
//        usr.isHacking = true;
//        if (num2 >= 100)
//        {
//          if (room.mapid == 60)
//          {
//            if (room.Mission1 == null)
//            {
//              room.Mission1 = usr.nickname;
//              usr.rPoints += Game_Server.Configs.Server.Experience.OnMissionHack;
//              room.send((Packet) new SP_Unknown((ushort) 29985, new object[6]
//              {
//                (object) 0,
//                (object) 0,
//                (object) 1,
//                (object) 1,
//                (object) 99,
//                (object) 0
//              }));
//              room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
//              {
//                (object) 0,
//                (object) 0,
//                (object) 0,
//                (object) 4,
//                (object) 1,
//                (object) 100,
//                (object) -1,
//                (object) 0
//              }));
//              room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
//              {
//                (object) 0,
//                (object) -1,
//                (object) 1,
//                (object) 5,
//                (object) -1,
//                (object) 0,
//                (object) -1,
//                (object) 0
//              }));
//              room.flags[2] = 1;
//              room.flags[3] = 0;
//              room.flags[0] = room.flags[1] = -1;
//              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
//              {
//                (object) 1,
//                (object) -1,
//                (object) room.id,
//                (object) 2,
//                (object) 156,
//                (object) 0,
//                (object) 1,
//                (object) 2,
//                (object) 1,
//                (object) -1,
//                (object) 2,
//                (object) 0,
//                (object) 92,
//                (object) -1,
//                (object) 0,
//                (object) 0,
//                (object) 1333333,
//                (object) -1166666,
//                (object) 1333333,
//                (object) 0,
//                (object) 3689.667,
//                (object) 969.9617,
//                (object) 4332.0752,
//                (object) 64.4469,
//                (object) 37.4174,
//                (object) -290.5969,
//                (object) 0,
//                (object) 0,
//                (object) "DU02"
//              }));
//              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
//              {
//                (object) 1,
//                (object) -1,
//                (object) room.id,
//                (object) 2,
//                (object) 156,
//                (object) 0,
//                (object) 1,
//                (object) 3,
//                (object) 0,
//                (object) -1,
//                (object) 2,
//                (object) 0,
//                (object) 92,
//                (object) -1,
//                (object) 0,
//                (object) 0,
//                (object) 1333333,
//                (object) -1166666,
//                (object) 1333333,
//                (object) 0,
//                (object) 3689.667,
//                (object) 969.9617,
//                (object) 4332.0752,
//                (object) 64.4469,
//                (object) 37.4174,
//                (object) -290.5969,
//                (object) 0,
//                (object) 0,
//                (object) "DU02"
//              }));
//              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
//              {
//                (object) 1,
//                (object) -1,
//                (object) room.id,
//                (object) 2,
//                (object) 156,
//                (object) 0,
//                (object) 1,
//                (object) 0,
//                (object) -1,
//                (object) 0,
//                (object) 0,
//                (object) 0,
//                (object) 92,
//                (object) -1,
//                (object) 0,
//                (object) 0,
//                (object) 1333333,
//                (object) -1166666,
//                (object) 1333333,
//                (object) 0,
//                (object) 3689.667,
//                (object) 969.9617,
//                (object) 4332.0752,
//                (object) 64.4469,
//                (object) 37.4174,
//                (object) -290.5969,
//                (object) 0,
//                (object) 0,
//                (object) "DU02"
//              }));
//              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
//              {
//                (object) 1,
//                (object) -1,
//                (object) room.id,
//                (object) 2,
//                (object) 156,
//                (object) 0,
//                (object) 1,
//                (object) 1,
//                (object) -1,
//                (object) 1,
//                (object) 1,
//                (object) 0,
//                (object) 92,
//                (object) -1,
//                (object) 0,
//                (object) 0,
//                (object) 1333333,
//                (object) -1166666,
//                (object) 1333333,
//                (object) 0,
//                (object) 3689.667,
//                (object) 969.9617,
//                (object) 4332.0752,
//                (object) 64.4469,
//                (object) 37.4174,
//                (object) -290.5969,
//                (object) 0,
//                (object) 0,
//                (object) "DU02"
//              }));
//              room.PickuppedC4 = false;
//            }
//          }
//          else if (room.timeattack != null)
//          {
//            room.timeattack.Stage2.Stop();
//            room.timeattack.milliSec = room.timeattack.Stage2.ElapsedMilliseconds;
//            room.timeleft += 360000;
//            room.send((Packet) new SP_ScoreboardInformations(room, room.timeattack.milliSec));
//            room.timeattack.milliSec = 0L;
//            room.timeattack.PreparingStage = true;
//          }
//        }
//        room.send((Packet) new SP_RoomHackMission(usr.roomslot, usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB, Type1, usr.hackingBase));
//      }
//    }
//  }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    class SP_RoomHackMission : Packet
    {
        public SP_RoomHackMission(int rs, int Percentage, int Type, int Base)
        {
            //29985 0 0 0 2 0 14 -1 0 
            newPacket(29985);
            addBlock(0);
            addBlock(rs);
            addBlock(0);
            addBlock(Type);
            addBlock(Base);
            addBlock(Percentage);
            addBlock(-1);
            addBlock(0);
        }
    }

    class CP_RoomHackMission : Handler
    {
        public override void Handle(User usr)
        {
            Room room = usr.room;

            int totPerc = (room.HackPercentage.BaseA + room.HackPercentage.BaseB);
            if (!room.gameactive || totPerc >= 100) return;

            //0 1 6 0 99 0
            usr.hackingBase = int.Parse(getBlock(3));
            int sType = 2;

            if (getBlock(1) == "0" && getBlock(2) == "0" && (getBlock(3) == "0" || getBlock(3) == "1")) // Starting to hack send animation
            {
                usr.send(new SP_RoomHackMission(usr.roomslot, (usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB), 0, usr.hackingBase));
                return;
            }
            else if (getBlock(1) == "1" && getBlock(2) == "6" && getBlock(3) == "0" && room.PickuppedC4 == false) // Pickup the C4
            {
                if (room.mapid == 56)
                {
                    if (room.GetSide(usr) != (int)Room.Side.Derbaran) return;
                    room.PickuppedC4 = true;
                    usr.hasC4 = true;
                    room.send(new SP_Unknown(29985, 0, 0, 1, 6, 0, 0, -1, 0)); // Remove the C4 from table
                    room.send(new SP_Unknown(30000, 1, usr.roomslot, usr.room.id, 2, 155, 0, 0, 92, 0, 0, 0, 0, 0, 0, 0)); // Switch to C4
                }
                else
                {
                    Log.WriteError("Tried to pickup C4 in room " + room.mapid);
                }
                return;
            }
            else if (getBlock(1) == "1" && getBlock(2) == "0" && getBlock(3) == "0") // Use the C4
            {
                if (room.mapid == 56)
                {
                    //30000 1 -1 4 2 104 0 1 1 0 0 92 0 92 -1 0 0 1333333 -1166666 1333333 0 2845.7510 205.0797 3374.0964 -70.9974 45.4165 -287.9179 0 0 DP05
                    room.PickuppedC4 = false;
                    usr.hasC4 = false;
                    room.SiegeWarC4User = usr;
                    room.send(new SP_Unknown(29985, 0, 0, 1, 0, 0, 0, 0, 0)); // Remove C4 from the user 
                    room.SiegeWarTime = 5;
                }
                else
                {
                    Log.WriteError("Tried to use C4 in room " + room.mapid);
                }
                return;
            }
            else if (int.Parse(getBlock(2)) == 3) // Stop to hack
            {
                usr.isHacking = false;
                sType = 3;
                room.send(new SP_RoomHackMission(usr.roomslot, (usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB), sType, usr.hackingBase));
                return;
            }

            if (room == null || usr.LastHackTick > Generic.timestamp && usr.LastHackTick > 0) return;

            if (usr.hackingBase != usr.LastHackBase)
            {
                usr.hackTick = 0;
            }

            if ((usr.LastHackTick < Generic.timestamp) || (usr.LastHackTick == 0))
            {
                usr.LastHackTick = Generic.timestamp + 1;
                usr.hackTick++;
                usr.rPoints++;
                usr.hackTick = 0;

                if (usr.hackingBase == 0)
                {
                    room.HackPercentage.BaseA++;
                }
                else
                {
                    room.HackPercentage.BaseB++;
                }
            }

            int totalPercentage = (room.HackPercentage.BaseA + room.HackPercentage.BaseB);

            usr.LastHackBase = usr.hackingBase;

            usr.hackPercentage++;
            usr.isHacking = true;

            if (totalPercentage >= 100)
            {
                if (room.mapid == 56)
                {
                    if (room.Mission1 == null)
                    {
                        room.Mission1 = usr.nickname;
                        usr.rPoints += Configs.Server.Experience.OnMissionHack;
                        room.send(new SP_Unknown(29985, 0, 0, 1, 1, 99, 0)); // Go to mission 2
                        room.send(new SP_Unknown(29985, 0, 0, 0, 4, 1, 100, -1, 0)); // Go to mission 2 
                        room.send(new SP_Unknown(29985, 0, -1, 1, 5, -1, 0, -1, 0)); // Spawn the C4
                        room.flags[2] = 1;
                        room.flags[3] = 0;
                        room.flags[0] = room.flags[1] = -1;
                        room.send(new SP_Unknown(30000, 1, -1, room.id, 2, 156, 0, 1, 2, 1, -1, 2, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        room.send(new SP_Unknown(30000, 1, -1, room.id, 2, 156, 0, 1, 3, 0, -1, 2, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        room.send(new SP_Unknown(30000, 1, -1, room.id, 2, 156, 0, 1, 0, -1, 0, 0, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        room.send(new SP_Unknown(30000, 1, -1, room.id, 2, 156, 0, 1, 1, -1, 1, 1, 0, 92, -1, 0, 0, 1333333, -1166666, 1333333, 0, 3689.6670, 969.9617, 4332.0752, 64.4469, 37.4174, -290.5969, 0, 0, "DU02"));
                        room.PickuppedC4 = false;
                    }
                }
                else if (room.timeattack != null)
                {
                    room.timelimit += 360000;
                    room.timeattack.PrepareNewStage(3);
                }
            }

            room.send(new SP_RoomHackMission(usr.roomslot, (usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB), sType, usr.hackingBase));
        }
    }
}