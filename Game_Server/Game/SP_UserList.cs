// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_UserList
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_UserList : Packet
  {
    public SP_UserList(SP_UserList.Type type, List<Game_Server.User> users)
    {
      this.newPacket((ushort) 28928);
      this.addBlock((object) 1);
      this.addBlock((object) (int) type);
      this.addBlock((object) users.Count);
      foreach (Game_Server.User user in users)
      {
        this.addBlock((object) user.userId);
        this.addBlock((object) user.sessionId);
        this.addBlock((object) (user.clan != null ? user.clan.id : -1));
        if (user.clan != null)
          this.addBlock((object) user.clan.iconid);
        else
          this.addBlock((object) -1);
        this.addBlock(user.clan != null ? (object) user.clan.name : (object) "NULL");
        this.addBlock((object) 4);
        this.addBlock((object) user.nickname);
        this.addBlock((object) user.level);
        this.addBlock((object) user.medalid);
        this.addBlock((object) user.HasSmileBadge);
        this.addBlock((object) user.premium);
        this.addBlock((object) 1);
        this.addBlock((object) user.channel);
        this.addBlock((object) (user.room != null ? user.room.id : -1));
      }
    }

    internal enum Type
    {
      Friends,
      Clan,
      Wait,
    }
  }
}
