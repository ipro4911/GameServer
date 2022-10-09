// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_UserList
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_UserList : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      SP_UserList.Type type = (SP_UserList.Type) int.Parse(this.getBlock(0));
      usr.actualUserlistType = (int) type;
      List<Game_Server.User> users = new List<Game_Server.User>();
      switch (type)
      {
        case SP_UserList.Type.Friends:
          usr.RefreshFriends();
          return;
        case SP_UserList.Type.Clan:
          if (usr.clan != null)
          {
            users = usr.clan.Users.Values.ToList<Game_Server.User>();
            break;
          }
          break;
        case SP_UserList.Type.Wait:
          users = UserManager.ServerUsers.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (u =>
          {
            if (u.channel == usr.channel)
              return u.room == null;
            return false;
          })).ToList<Game_Server.User>();
          break;
        default:
          usr.disconnect();
          break;
      }
      usr.send((Packet) new SP_UserList(type, users));
    }
  }
}
