// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.Explosive
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.GameModes
    {
        internal class PACKET_ROOM_TICK : Packet
        {
            public PACKET_ROOM_TICK(Room Room)
            {
                try
                {
                    newPacket(30016);
                    if (Room.channel == 3)
                    {
                        if (Room.mode == 11)
                        {
                            addBlock(Room.TimeAttackTime);
                            addBlock(Room.RoundTimeSpent);
                            addBlock(0);
                            addBlock(2);
                            addBlock(0);
                            addBlock(30);
                        }
                        else
                        {
                            addBlock(-1);
                            addBlock(Room.RoundTimeSpent);
                            addBlock(Room.ZombiePoints);
                            addBlock(Room.ZombiePoints);
                            addBlock(30);
                        }
                    }
                    else
                    {
                        addBlock(Room.RoundTimeSpent);
                        addBlock(Room.RoundTimeLeft);
                        if (Room.mode == 2 || Room.mode == 3)
                        {
                            addBlock(0);
                            addBlock(0);
                            addBlock(Room.KillsDerbaranLeft);
                            addBlock(Room.KillsNIULeft);
                        }
                        else
                        {
                            addBlock(Room.cDerbRounds);
                            addBlock(Room.cNiuRounds);
                            addBlock(Room.ffakillpoints);
                            addBlock(Room.highestkills);
                        }
                        addBlock(2);
                        addBlock(0);
                    }
                    addBlock(30);
                }
                catch (Exception ex)
                {
                    Log.WriteError("Error @ room tick: " + ex.Message);
                }
            }
        }
    }


