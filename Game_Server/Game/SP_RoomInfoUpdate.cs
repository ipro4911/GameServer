// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomInfoUpdate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomInfoUpdate : Packet
  {
    public SP_RoomInfoUpdate(Room Room)
    {
      this.newPacket((ushort) 29201);
      this.addBlock((object) Room.id);
      this.addBlock((object) Room.name);
      this.addBlock((object) Room.enablepassword);
      this.addBlock((object) Room.password);
      this.addBlock((object) Room.maxusers);
      this.addBlock((object) 0);
      this.addBlock((object) Room.levellimit);
      this.addBlock((object) Room.rounds);
      this.addBlock((object) Room.zombiedifficulty);
      this.addBlock((object) Room.rounds);
      this.addBlock((object) Room.timelimit);
      this.addBlock((object) 0);
      this.addBlock((object) Room.autostart);
      this.addBlock((object) Room.mapid);
      this.addBlock((object) Room.mode);
      this.addBlock((object) Room.new_mode);
      this.addBlock((object) Room.new_mode_sub);
    }
  }
}
