// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_QuickJoinRoom
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class CP_QuickJoinRoom : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Channel channel = ChannelManager.channels[usr.channel];
      foreach (Room room in channel.GetRoomListByPage(new Random().Next(0, channel.roomToPageCount)))
      {
        if (room != null)
        {
          if (usr.room != null || room.users.Count >= room.maxusers || (room.enablepassword == 1 || room.type == 1) || (!room.isJoinable || room.voteKick.lockuser.IsLockedUser(usr)) || (int) usr.level < 10 * (room.levellimit - 1) + 1 && (usr.level > (byte) 10 || room.levellimit != 1) && room.levellimit != 0)
            break;
          if (room.JoinUser(usr, 2))
          {
            room.InitializeUDP(usr);
            room.ch.UpdateLobby(room);
            UserManager.UpdateUserlist(usr);
            break;
          }
        }
      }
    }
  }
}
