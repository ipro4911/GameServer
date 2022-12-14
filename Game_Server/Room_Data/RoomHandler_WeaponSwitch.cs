// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_WeaponSwitch
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_WeaponSwitch : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (this.sendBlocks.Length < 6 || !room.gameactive || (!usr.IsAlive() || usr.Health <= 0) || room.mode == 16 && (room.mapid == 90 || room.mapid == 91) && (room.deathmatch != null)) // && room.deathmatch.isGunGame))
        return;
      usr.weapon = int.Parse(this.getBlock(6));
      this.sendPacket = true;
    }
  }
}
