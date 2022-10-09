// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_PlayerInfo
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_PlayerInfo : Packet
  {
    public SP_PlayerInfo(List<Game_Server.User> users)
    {
      this.newPacket((ushort) 29952);
      this.addBlock((object) users.Count);
      foreach (Game_Server.User user in users)
      {
        this.addBlock((object) user.userId);
        this.addBlock((object) user.sessionId);
        this.addBlock((object) user.roomslot);
        this.addBlock((object) (user.isReady ? 1 : 0));
        this.addBlock((object) user.room.GetSide(user));
        this.addBlock((object) user.weapon);
        this.addBlock((object) user.Health);
        this.addBlock((object) user.Class);
        this.addBlock((object) user.Health);
        this.addBlock((object) user.nickname);
        this.addBlock((object) (user.clan != null ? user.clan.id : -1));
        if (user.clan != null && !user.clanPending)
          this.addBlock((object) user.clan.iconid);
        else
          this.addBlock((object) -1);
        this.addBlock(user.clan != null ? (object) user.clan.name : (object) "NULL");
        this.addBlock((object) (user.clan == null || user.clanPending ? 0 : user.clan.clanRank(user)));
        this.addBlock((object) 1);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) (!user.room.supermaster || user.roomslot != user.room.master ? (int) user.premium : 0));
        this.addBlock((object) 0);
        this.addBlock((object) user.HasSmileBadge);
        this.addBlock((object) user.kills);
        this.addBlock((object) user.deaths);
        this.addBlock((object) 0);
        this.addBlock((object) user.exp);
        this.addBlock((object) (user.currentVehicle == null ? -1 : user.currentVehicle.ID));
        this.addBlock((object) (user.currentSeat == null ? -1 : user.currentSeat.ID));
        this.addBlock((object) user.classCode);
        addBlock(0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
      }
    }
  }
}
