// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.CP_RoomInviteOrQuickJoin
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Managers
{
  internal class CP_RoomInviteOrQuickJoin : Handler
  {
    public override void Handle(User usr)
    {
      int.Parse(this.getBlock(0));
      int roomId = int.Parse(this.getBlock(1));
      string str = "NULL";
      if (this.getAllBlocks.Length >= 2)
        str = this.getBlock(2);
      Room room = ChannelManager.channels[usr.channel].GetRoom(roomId);
      if (room == null || usr.room != null || (room.users.Count >= room.maxusers || room.type == 1) || (!room.isJoinable || room.voteKick.lockuser.IsLockedUser(usr)) || room.enablepassword != 0 && (room.enablepassword != 1 || !(room.password == str)) || ((int) usr.level < 10 * (room.levellimit - 1) + 1 && (usr.level > (byte) 10 || room.levellimit != 1) && room.levellimit != 0 || !room.JoinUser(usr, 2)))
        return;
      room.InitializeUDP(usr);
      room.ch.UpdateLobby(room);
      UserManager.UpdateUserlist(usr);
    }
  }
}
