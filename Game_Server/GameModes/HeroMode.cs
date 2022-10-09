//// Decompiled with JetBrains decompiler
//// Type: Game_Server.GameModes.HeroMode
//// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
//// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

//using Game_Server.Room_Data;
//using System;
//using System.Linq;

//namespace Game_Server.GameModes
//{
//  internal class HeroMode
//  {
//    private Room room;
//    private int sleepTick;

//    ~HeroMode()
//    {
//      GC.Collect();
//    }

//    public void CheckForNewRound()
//    {
//      if (this.room.derbHeroKill != 0 && this.room.niuHeroKill != 0 && this.room.timeleft > 0)
//        return;
//      int WinningTeam = this.room.timeleft <= 0 ? (this.room.derbHeroKill < this.room.niuHeroKill ? 1 : 0) : (this.room.derbHeroKill == 0 ? 1 : 0);
//      if (this.sleepTick >= 5) && !this.room.EndGamefreeze)
//      {
//        if (this.room.derbHeroKill == 3)
//          ++this.room.DerbRounds;
//        else if (this.room.niuHeroKill == 3)
//          ++this.room.NIURounds;
//        if (this.room.timeleft <= 0 && this.room.derbHeroKill < this.room.niuHeroKill)
//          ++this.room.NIURounds;
//        else if (this.room.timeleft <= 0 && this.room.niuHeroKill < this.room.derbHeroKill)
//          ++this.room.DerbRounds;
//        this.sleepTick = 0;
//        this.room.derbHeroKill = this.room.niuHeroKill = 3;
//        this.room.derbHeroUsr = this.room.niuHeroUsr = -1;
//        this.room.timeleft = 300000;
//        this.room.updateTime();
//        this.room.Placements.Clear();
//        this.room.send((Packet) new SP_RoomDataNewRound(this.room, WinningTeam, false));
//        this.room.send((Packet) new SP_InitializeNewRound(this.room));
//      }
//      else if ((WinningTeam == 0 ? this.room.DerbRounds : this.room.NIURounds) + 1 >= this.room.explosiveRounds)
//      {
//        if (WinningTeam == 0)
//          ++this.room.DerbRounds;
//        else
//          ++this.room.NIURounds;
//        this.room.EndGame();
//      }
//      else
//      {
//        if (this.sleepTick == 0)
//        {
//          this.room.send((Packet) new SP_RoomDataNewRound(this.room, WinningTeam, true));
//          foreach (User tempPlayer in this.room.tempPlayers)
//          {
//            tempPlayer.isSpawned = false;
//            tempPlayer.throwNades = tempPlayer.throwRockets = (ushort) 0;
//          }
//        }
//        ++this.sleepTick;
//      }
//    }

//    public void Update()
//    {
//      if (this.room == null || this.room.users.Count < 1 || !this.room.gameactive)
//        return;
//      if (this.room.AliveDerb > 0 && this.room.derbHeroUsr == -1)
//        this.room.derbHeroUsr = this.room.users.Values.Where<User>((Func<User, bool>) (r =>
//        {
//          if (r != null && this.room.GetSide(r) == 0)
//            return r.IsAlive();
//          return false;
//        })).OrderBy<User, Guid>((Func<User, Guid>) (qu => Guid.NewGuid())).FirstOrDefault<User>().roomslot;
//      if (this.room.AliveNIU > 0 && this.room.niuHeroUsr == -1)
//        this.room.niuHeroUsr = this.room.users.Values.Where<User>((Func<User, bool>) (r =>
//        {
//          if (r != null && this.room.GetSide(r) == 1)
//            return r.IsAlive();
//          return false;
//        })).OrderBy<User, Guid>((Func<User, Guid>) (qu => Guid.NewGuid())).FirstOrDefault<User>().roomslot;
//      if (this.room.NIURounds >= this.room.explosiveRounds || this.room.DerbRounds >= this.room.explosiveRounds)
//        this.room.EndGame();
//      else
//        this.CheckForNewRound();
//    }

//    public HeroMode(Room room)
//    {
//      this.room = room;
//    }
//  }
//}  // UP is Toxiic/Wrmontana version infinite never ending after 1 round like u could win unlimited times i think

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Game_Server.Game;
//using Game_Server.Room_Data;

//namespace Game_Server.GameModes
//{
//    class HeroMode
//    {
//        ~HeroMode()
//        {
//            GC.Collect();
//        }
//        Room room = null;

//        int sleepTick = 0;

//        public void CheckForNewRound()
//        {
//            if (room.derbHeroKill == 0 || room.niuHeroKill == 0 || room.timeleft <= 0)
//            {
//                int winningTeam = (room.timeleft <= 0 ? (room.derbHeroKill < room.niuHeroKill ? (int)Room.Side.NIU : (int)Room.Side.Derbaran) : (room.derbHeroKill == (int)Room.Side.Derbaran ? (int)Room.Side.NIU : (int)Room.Side.Derbaran));
//                if (sleepTick >= 5 && !room.EndGamefreeze)
//                {
//                    if (winningTeam == (int)Room.Side.Derbaran) room.DerbRounds++; else room.NIURounds++;
//                    sleepTick = 0;
//                    room.derbHeroKill = room.niuHeroKill = 3;
//                    room.derbHeroUsr = room.niuHeroUsr = -1;
//                    room.timeleft = 300000;
//                    room.updateTime();
//                    room.Placements.Clear();


//                    room.send(new SP_RoomDataNewRound(room, winningTeam, false));
//                    room.send(new SP_InitializeNewRound(room));


//                }
//                else
//                {
//                    int winningTeamRounds = winningTeam == (int)Room.Side.Derbaran ? room.DerbRounds : room.NIURounds;
//                    if (winningTeamRounds + 1 >= room.explosiveRounds)
//                    {
//                        if (winningTeam == (int)Room.Side.Derbaran) room.DerbRounds++; else room.NIURounds++;
//                        room.EndGame();
//                    }
//                    else
//                    {
//                        if (sleepTick == 0)
//                        {
//                            room.send(new SP_RoomDataNewRound(room, winningTeam, true));

//                            foreach (User usr in room.tempPlayers)
//                            {
//                                usr.isSpawned = false;
//                                usr.throwNades = usr.throwRockets = 0;
//                            }
//                        }
//                        sleepTick++;
//                    }
//                }
//            }
//        }

//        public void Update()
//        {
//            if (room != null)
//            {
//                if (room.users.Count >= 1 && room.gameactive)
//                {
//                    if (room.AliveDerb > 0 && room.derbHeroUsr == -1)
//                    {
//                        room.derbHeroUsr = room.users.Values.Where(r => r != null && room.GetSide(r) == (int)Room.Side.Derbaran && r.IsAlive()).OrderBy(qu => Guid.NewGuid()).FirstOrDefault().roomslot;
//                    }

//                    if (room.AliveNIU > 0 && room.niuHeroUsr == -1)
//                    {
//                        room.niuHeroUsr = room.users.Values.Where(r => r != null && room.GetSide(r) == (int)Room.Side.NIU && r.IsAlive()).OrderBy(qu => Guid.NewGuid()).FirstOrDefault().roomslot;
//                    }

//                    if (room.NIURounds >= room.explosiveRounds || room.DerbRounds >= room.explosiveRounds) { room.EndGame(); return; }
//                    CheckForNewRound();
//                }
//            }
//        }

//        public HeroMode(Room room)
//        {
//            this.room = room;
//        }
//    }
//} // works 90% but 1 round is glitchy


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game_Server.Game;
using Game_Server.Room_Data;

namespace Game_Server.GameModes
{
    class HeroMode
    {
        ~HeroMode()
        {
            GC.Collect();
        }
        Room room = null;

        int sleepTick = 0;

        public void CheckForNewRound()
        {
            if (room.derbHeroKill == 0 || room.niuHeroKill == 0 || room.timeleft <= 0)
            {
                int winningTeam = (room.timeleft <= 0 ? (room.derbHeroKill < room.niuHeroKill ? (int)Room.Side.NIU : (int)Room.Side.Derbaran) : (room.derbHeroKill == (int)Room.Side.Derbaran ? (int)Room.Side.NIU : (int)Room.Side.Derbaran));
                if (sleepTick >= 5 && !room.EndGamefreeze)
                {
                    if (winningTeam == (int)Room.Side.Derbaran) room.DerbRounds++; else room.NIURounds++;
                    sleepTick = 0;
                    room.derbHeroKill = room.niuHeroKill = 3;
                    room.derbHeroUsr = room.niuHeroUsr = -1;
                    room.timeleft = 300000;
                    room.updateTime();
                    room.Placements.Clear();

                    room.send(new SP_RoomDataNewRound(room, winningTeam, false));
                    room.send(new SP_InitializeNewRound(room));
                }
                else
                {
                    int winningTeamRounds = winningTeam == (int)Room.Side.Derbaran ? room.DerbRounds : room.NIURounds;
                    if (winningTeamRounds + 1 >= room.explosiveRounds)
                    {
                        if (winningTeam == (int)Room.Side.Derbaran) room.DerbRounds++; else room.NIURounds++;
                        room.EndGame();
                    }
                    else
                    {
                        if (sleepTick == 0)
                        {
                            room.send(new SP_RoomDataNewRound(room, winningTeam, true));

                            foreach (User usr in room.tempPlayers)
                            {
                                usr.isSpawned = false;
                                usr.throwNades = usr.throwRockets = 0;
                            }
                        }
                        sleepTick++;
                    }
                }
            }
        }

        public void Update()
        {
            if (room != null)
            {
                if (room.users.Count >= 1 && room.gameactive)
                {
                    if (room.AliveDerb > 0 && room.derbHeroUsr == -1)
                    {
                        room.derbHeroUsr = room.users.Values.Where(r => r != null && room.GetSide(r) == (int)Room.Side.Derbaran && r.IsAlive()).OrderBy(qu => Guid.NewGuid()).FirstOrDefault().roomslot;
                    }

                    if (room.AliveNIU > 0 && room.niuHeroUsr == -1)
                    {
                        room.niuHeroUsr = room.users.Values.Where(r => r != null && room.GetSide(r) == (int)Room.Side.NIU && r.IsAlive()).OrderBy(qu => Guid.NewGuid()).FirstOrDefault().roomslot;
                    }

                    if (room.NIURounds >= room.explosiveRounds || room.DerbRounds >= room.explosiveRounds) { room.EndGame(); return; }
                    CheckForNewRound();
                }
            }
        }

        public HeroMode(Room room)
        {
            this.room = room;
        }
    }
}
