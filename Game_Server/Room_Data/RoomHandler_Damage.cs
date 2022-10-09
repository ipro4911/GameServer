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

using Game_Server.Managers;
using Game_Server.Game;

namespace Game_Server.Room_Data
{
    class RoomHandler_Damage : RoomDataHandler
    {
        public override void Handle(User usr, Room room)
        {
            // 27 = NX / 26 = G1
            if (!room.gameactive || room.sleep && room.bombDefused || room.firstInGameTS > Generic.timestamp) return;

            int targetId = int.Parse(getBlock(7));
            uint HSCalculation = uint.Parse(getBlock(15)) - usr.sessionId;
            string shootenPart = getBlock(22);
            int slotId = int.Parse(getBlock(8));
            int weaponSlot = int.Parse(getBlock(13)); // Weapon slot (example if you shoot with knuckles the slot is 0, if K1, slot is 2, etc...)

            bool useRadius = getBlock(14) == "1";
            string weapon = getBlock(27).Substring(0, 4);

            Item Item = ItemManager.GetItem(weapon);
            if (Item == null) return;

            //22 = Nexon - 21 = Older clients
            //27 = Nexon - 26 = Older clients <- Weapon

            int points = Configs.Server.Experience.OnKillPoints;

            int Type = 1;

            switch (HSCalculation)
            {
                case 1237: Type = 0; break;
                case 1239: Type = 1; break;
                case 1241: Type = 2; break;
            }

            /* Avoid nades as hs etc*/

            if (weapon.StartsWith("DM") || weapon.StartsWith("DN") || ((weapon.StartsWith("DJ") || weapon.StartsWith("DK")) && HSCalculation >= 1237)) // Weapon hit user at 100%
                Type = 0;

            int Damage = ItemManager.GetDamage(weapon, Type);
            //int CharProctection = ItemManager.GetDamage()

            if (usr.currentVehicle != null)
            {
                bool MainCT = int.Parse(getBlock(13)) == 0;
                if (weapon == usr.currentVehicle.Code)
                {
                    points += Configs.Server.Experience.OnVehicleKillAdditional;
                    if (MainCT)
                    {
                        Damage = ItemManager.GetDamage(usr.currentSeat.MainCTCode);
                    }
                    else
                    {
                        Damage = ItemManager.GetDamage(usr.currentSeat.SubCTCode);
                    }
                }
            }
            else
            {
                if (room.new_mode == 5) // Kamikaze mode -> 1 hit
                {
                    Damage = 1000; // One hit
                }
            }

            bool isNade = weapon.StartsWith("DN") || weapon.StartsWith("DM");

            int[] calculations = new int[] { 75, 100 };

            int min = calculations[isNade ? 0 : 1];

            if (HSCalculation > 0 && HSCalculation < min)
            {
                // Calculate range and remove the percentage from the damage
                Damage = (int)Math.Ceiling((double)(Damage * HSCalculation) / 100);
            }

            if ((usr.Health <= 0 || !usr.IsAlive()) && (HSCalculation < 0 || HSCalculation > 100)) return;

            if (!usr.IsWhitelistedWeapon(weapon) && (usr.currentVehicle != null && weapon != usr.currentVehicle.Code)) return;

            bool hs = false;

            if (room.channel == 3)
            {
                if (targetId >= 4 && slotId < 4) // Target > 4 (Zombie) && Shooter < 4 (Player) || Target
                {
                    if (Type == 0)
                    {
                        if (!weapon.StartsWith("DA") && !weapon.StartsWith("DN") && !weapon.StartsWith("DJ") && !weapon.StartsWith("DM") && !weapon.StartsWith("DK"))
                        {
                            shootenPart = "99.0000";
                            hs = true;
                        }
                    }

                    Zombie zombie = room.GetZombieByID(targetId); // targetID -> Who we are shooting

                    if (zombie != null && zombie.Health > 0)
                    {
                        if (zombie.timestamp > Generic.timestamp) return;
                        points = zombie.Points;

                        if (hs)
                        {
                            points *= 2;
                        }

                        zombie.Health -= Damage;

                        if (zombie.Health <= 0)
                        {
                            room.KillsBeforeDrop++;

                            int b = (room.zombie != null ? room.zombie.Wave : room.timeattack.Stage);
                            int c = (room.zombie != null ? 3 : 0);

                            if (b >= c && room.DropID < 30 && room.KillsBeforeDrop >= 7)
                            {
                                int DropType = room.RandomDrop();

                                room.send(new SP_ZombieDrop(usr, targetId, room.DropID, DropType));
                                room.DropID++;
                                room.KillsBeforeDrop = 0;
                            }

                            if (room.zombie != null)
                            {
                                if (zombie.Type == 7 && usr.skillPoints < 20)
                                {
                                    usr.skillPoints++;
                                    if (usr.skillPoints == 5 || usr.skillPoints == 10 || usr.skillPoints == 20)
                                        room.send(new SP_ZombieSkillpointUpdate(usr));
                                }
                            }

                            usr.rPoints += points;
                            usr.rKills++;

                            zombie.Health = 0;
                            zombie.respawn = Generic.timestamp + 4;

                            room.KilledZombies++;
                            room.ZombiePoints += points;
                            sendBlocks[3] = (int)Subtype.ServerKill;
                            sendBlocks[8] = slotId;
                            sendBlocks[22] = shootenPart;
                        }
                    }
                }
                else if (targetId <= 3 && slotId >= 4)
                {
                    Zombie Zombie = room.GetZombieByID(slotId); // SlotID -> From who getting shotted

                    if (slotId == usr.roomslot) return; // Cant shoot yourself

                    if (Zombie == null || Zombie.Health <= 0 || usr.Health <= 0 || Zombie.ID == -1) return;

                    usr.Health -= Zombie.Damage;

                    sendBlocks[8] = slotId;
                    sendBlocks[16] = usr.Health; //
                    sendBlocks[17] = (usr.Health + Zombie.Damage); // 
                    sendBlocks[27] = weapon; // Weapon (NX removes the other 4 chars at the send ( Splitted full is ) - DF01299 = Weapon[4]|Class[1]

                    if (usr.Health <= 0)
                    {
                        if (room.timeattack != null)
                        {
                            usr.timeAttackSpawns--;
                            if (usr.timeAttackSpawns <= 0)
                            {
                                sendBlocks[3] = (int)Subtype.ServerKill; // Switch to 152
                                usr.OnDie();
                                room.EndGame();
                            }
                        }
                        else
                        {
                            sendBlocks[3] = (int)Subtype.ServerKill; // Switch to 152
                            usr.OnDie();
                        }
                    }
                }
            }
            else
            {
                User target = room.users[targetId];
                if (target == null || target.spawnprotection > 0 || room.GetSide(usr) == room.GetSide(target) && room.mode != 1) return;

                if (HSCalculation == 1237) // HS = x2
                {
                    if (!weapon.StartsWith("DA") && !weapon.StartsWith("DN") && !weapon.StartsWith("DJ") && !weapon.StartsWith("DM") && !weapon.StartsWith("DK"))
                    {
                        Damage += 200;
                        shootenPart = "99.0000";
                        hs = true;
                    }
                }

                if (target.Health >= 0)
                {
                    target.Health -= Damage;

                    sendBlocks[8] = slotId;
                    sendBlocks[16] = target.Health; //
                    sendBlocks[17] = (target.Health + Damage); // 
                    sendBlocks[18] = points; // UPDATE 24.07.2013 - Showen points in the center of the screen when you kill
                    sendBlocks[22] = shootenPart; // Headshot or other shit handled from SessionID [ TODO: Costume calculate damage ]
                    sendBlocks[27] = weapon; // Weapon (NX removes the other 4 chars at the send ( Splitted full is ) - DF01299 = Weapon[4]|Class[1]

                    if (target.Health <= 0)
                    {
                        if (room.heromode != null && (target.roomslot == room.derbHeroUsr || target.roomslot == room.niuHeroUsr))
                        {
                            points += 20;
                        }

                        target.OnDie();
                        sendBlocks[3] = (int)Subtype.ServerKill;

                        if (room.mode == 8)
                        {
                            usr.TotalWarPoint = 0;
                            target.TotalWarSupport += 2;

                            sendBlocks[19] = usr.TotalWarPoint;
                            sendBlocks[20] = usr.TotalWarSupport;

                            switch (room.GetSide(usr))
                            {
                                case 0: room.TotalWarDerb += 5; room.TotalWarNIU += 2; break;
                                case 1: room.TotalWarNIU += 5; room.TotalWarDerb += 2; break;
                            }
                        }

                        if (hs)
                        {
                            usr.rPoints += Configs.Server.Experience.OnHSKillPoints;
                            usr.rHeadShots++;
                            if (room.new_mode != 6 && room.new_mode_sub != 0)
                            {
                                usr.send(new SP_CustomSound(SP_CustomSound.Sounds.HeadShot));
                            }
                        }

                        usr.rKillSinceSpawn++;

                        if (!room.firstblood)
                        {
                            room.firstblood = true;
                            usr.rPoints += 3;
                            usr.send(new SP_KillAnimation(SP_KillAnimation.Type.FirstKill));
                        }

                        if (usr.lastKillUser == target.roomslot)
                        {
                            usr.send(new SP_KillAnimation(SP_KillAnimation.Type.RevengeKill));
                            usr.rPoints++;
                        }

                        if (isNade)
                        {
                            usr.send(new SP_KillAnimation(SP_KillAnimation.Type.GrenadeKill));
                            usr.rPoints++;
                        }
                        else if (hs)
                        {
                            usr.send(new SP_KillAnimation(SP_KillAnimation.Type.HeadShot));
                            usr.rPoints++;
                        }

                        switch (usr.rKillSinceSpawn)
                        {
                            case 1: // Do nothing
                                break;
                            case 2:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.DoubleKill));
                                usr.rPoints++;
                                break;
                            case 3:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.TripleKill));
                                usr.rPoints++;
                                break;
                            case 4:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.QuadraKill));
                                usr.rPoints += 2;
                                break;
                            case 5:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.PentaKill));
                                usr.rPoints += 2;
                                break;
                            case 6:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.HexaKill));
                                usr.rPoints += 3;
                                break;
                            case 7:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.UltraKill));
                                usr.rPoints += 3;
                                break;
                            case 8:
                                usr.send(new SP_KillAnimation(SP_KillAnimation.Type.Assasin));
                                usr.rPoints += 2;
                                break;
                        }

                        target.lastKillUser = usr.roomslot;


                        /* Snow Fight */

                        /*if(room.mapid == 72 && room.new_mode == 6 && room.new_mode_sub == 2)
                        {
                            usr.KillEventCheck();
                        }*/

                        /* GunSmith Materials Earn part */

                        int g_perc = Generic.random(0, 500);

                        if (g_perc < 20)
                        {
                            usr.RandomGunsmithResource();
                        }



                        if (usr.rKills > room.highestkills) { room.highestkills = usr.rKills; }

                        if (room.KillsDerbaranLeft == 30 || room.KillsNIULeft == 30 || room.NIURounds >= 6 || room.DerbRounds >= 6)
                        {
                            lobbychanges = true;
                        }

                        if (target.currentVehicle != null) points += 5;

                        //sendBlocks[4] = (int)Subtype.Explode;
                        sendBlocks[3] = (int)Subtype.ServerKill;
                        usr.rKills++;
                        usr.rPoints += points;

                        if (room.explosive != null)
                        {
                            sendPacket = false;
                            //room.send(new SP_EntitySuicide(usr.roomslot, SP_EntitySuicide.SuicideType.Suicide, true));
                            room.send(new SP_RoomData(sendBlocks));
                            room.explosive.CheckForNewRound();  
                            return;
                        }
                        else if (room.heromode != null)
                        {
                            sendPacket = false;
                            room.send(new SP_RoomData(sendBlocks));
                            room.heromode.CheckForNewRound();
                            return;
                        }
                        if (room.mode == (int)RoomMode.TDM)
                        if (room.mode == (int)RoomMode.FFA)
                        {
                            //room.send(new SP_EntitySuicide(usr.roomslot, SP_EntitySuicide.SuicideType.Suicide, true));
                            usr.isSpawned = true;
                            sendPacket = true;
                            room.send(new SP_RoomData(sendBlocks));
                        }
                        
                        //    // Max 30 kills at FFA
                        //    if (room.ffa.gunGameScores[usr.roomslot] < 30)
                        //    {
                        //        room.ffa.UpdateGunGameScore(usr.roomslot);
                        //        room.ffa.GunGameUpdate(usr);
                        //        usr.SwitchWeapon(room.ffa.GetGunGameWeapon(usr));
                        //    }

                        //    room.ffa.UpdateGunGameScore(usr.roomslot);
                        //    target.SwitchWeapon("DA01");
                        //    return;
                        //}
                    }
                }
            }

            /* Important */

            sendPacket = true;
        }
    }
}
