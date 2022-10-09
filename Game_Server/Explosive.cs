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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game_Server.Game;
using Game_Server.Room_Data;

namespace Game_Server.GameModes
{
    class Explosive
    {
        ~Explosive()
        {
            GC.Collect();
        }
        Room room = null;

        public void sendNewRound(int WinningTeam)
        {
            if (room.waitExplosiveTime >= 5 && !room.EndGamefreeze)
            {
                List<User> tempPlayers = new List<User>();
                tempPlayers.AddRange(room.users.Values);
                tempPlayers.AddRange(room.spectators.Values);

                WinningTeam = (room.bombPlanted && !room.bombDefused ? (int)Room.Side.Derbaran : (int)Room.Side.NIU);

                room.send(new SP_RoomDataNewRound(room, WinningTeam, false));
                room.send(new SP_InitializeNewRound(room));
                

                room.updateTime();
                room.timespent = 0;
                room.timeleft = room.mode != (int)RoomMode.Annihilation ? 180000 : 180000;
                room.bombDefused = false;
                room.bombPlanted = false;
                room.sleep = false;
                room.isNewRound = false;
                room.waitExplosiveTime = 0;
                room.Placements.Clear();
            }
        }

        public void prepareRound(int WinningTeam)
        {
            if (room.isNewRound == false)
            {
                switch (WinningTeam)
                {
                    case (int)Room.Side.Derbaran: room.DerbRounds++; break;
                    case (int)Room.Side.NIU: room.NIURounds++; break;
                }

                room.isNewRound = true;
            }

            if ((WinningTeam == (int)Room.Side.Derbaran && room.DerbRounds >= room.explosiveRounds || WinningTeam == (int)Room.Side.NIU && room.NIURounds >= room.explosiveRounds) && !room.EndGamefreeze)
            {
                room.EndGame();
            }
            else
            {
                room.sleep = true;
                room.waitExplosiveTime = 0;

                room.send(new SP_RoomDataNewRound(room, WinningTeam, true));
                room.send (new SP_Unknown(29985, 0, 0, 1, 4, 0, 100, 0, 0));

                foreach (User usr in room.tempPlayers)
                {
                    usr.isSpawned = false;
                    usr.throwNades = usr.throwRockets = 0;
                }
            }
        }

        public void CheckForNewRound()
        {
            if (room.mode == 0 && room.channel == 1 && !room.sleep)
            {
                if (room.AliveDerb == 0 && room.AliveNIU > 0 && room.bombPlanted == false)
                {
                    prepareRound((int)Room.Side.NIU);
                }
                else if (room.AliveNIU == 0 && room.AliveDerb > 0)
                {
                    prepareRound((int)Room.Side.Derbaran);
                }
                else if (room.AliveNIU == 0 && room.AliveDerb == 0)
                {
                    prepareRound((room.bombPlanted ? (int)Room.Side.Derbaran : (int)Room.Side.NIU));
                }
            }
        }

        public void Update()
        {
            try
            {
                if (room != null)
                {
                    if (room.users.Count > 1 && room.gameactive)
                    {
                        if (room.isNewRound)
                        {
                            room.waitExplosiveTime++;

                            if (room.waitExplosiveTime >= 5)
                            {
                                if (room.AliveDerb == 0 && room.AliveNIU > 0 && room.bombPlanted == false)
                                {
                                    sendNewRound((int)Room.Side.NIU);
                                }
                                else if (room.AliveNIU == 0 && room.AliveDerb > 0)
                                {
                                    sendNewRound((int)Room.Side.Derbaran);
                                }
                                else
                                {
                                    sendNewRound((room.bombPlanted ? (int)Room.Side.Derbaran : (int)Room.Side.NIU));
                                }
                            }
                        }
                        else
                        {
                            if (room.NIURounds >= room.explosiveRounds || room.DerbRounds >= room.explosiveRounds) { room.EndGame(); return; }
                            if (room.timeleft <= 0) { prepareRound((room.bombPlanted ? (int)Room.Side.Derbaran : (int)Room.Side.NIU)); }
                            //if (!room.sleep)
                            //    CheckForNewRound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);

            }
        }

        public Explosive(Room room)
        {
            this.room = room;
        }
    }
}