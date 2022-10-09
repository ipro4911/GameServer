// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ZombieChangeTarget
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_ZombieChangeTarget : Packet
  {
    public SP_ZombieChangeTarget(Room Room, int RoomSlot)
    {
      List<Zombie> zombieList = Room.ZombieFollowers(RoomSlot);
      this.newPacket((ushort) 13433);
      this.addBlock((object) RoomSlot);
      this.addBlock((object) zombieList.Count);
      foreach (Zombie zombie in zombieList)
      {
        this.addBlock((object) zombie.ID);
        this.addBlock((object) Room.master);
      }
    }
  }
}
