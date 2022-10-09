// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_RoomInfoUpdate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_RoomInfoUpdate : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Room room = usr.room;
      if (room == null || room.gameactive)
        return;
      room.name = this.getBlock(1);
      
      room.enablepassword = int.Parse(this.getBlock(2));
      room.password = this.getBlock(3);
      room.maxusers = int.Parse(this.getBlock(6));
      room.zombiedifficulty = int.Parse(this.getBlock(8));
      room.rounds = room.mode == 0 || room.mode == 7 ? int.Parse(this.getBlock(7)) : int.Parse(this.getBlock(9));
      room.timelimit = int.Parse(this.getBlock(10));
     // room.ping = int.Parse(this.getBlock(11));
      room.mapid = int.Parse(this.getBlock(13));
      room.levellimit = (int) byte.Parse(this.getBlock(5));
      room.new_mode = (int) byte.Parse(this.getBlock(15));
      room.new_mode_sub = int.Parse(this.getBlock(16));
      if (room.new_mode > 6)
        room.new_mode = 6;
      
      room.send((Packet) new SP_RoomInfoUpdate(usr.room));
      room.ch.UpdateLobby(room);
    }
  }
}
