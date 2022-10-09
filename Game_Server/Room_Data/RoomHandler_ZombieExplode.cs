// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_ZombieExplode
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_ZombieExplode : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      int num = int.Parse(this.getBlock(8));
      Zombie zombieById = room.GetZombieByID(num);
      if (zombieById.Name != "Breaker")
        room.send((Packet) new SP_EntitySuicide(num, SP_EntitySuicide.SuicideType.Suicide, false));
      if (zombieById != null && zombieById.Health > 0 && zombieById.Name != "Breaker")
      {
        zombieById.Health = 0;
        zombieById.respawn = Generic.timestamp + 4;
        if (room.mode != 12)
          ++room.KilledZombies;
      }
      this.sendPacket = true;
    }
  }
}
