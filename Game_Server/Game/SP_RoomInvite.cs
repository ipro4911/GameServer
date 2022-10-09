// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomInvite
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomInvite : Packet
  {
    public SP_RoomInvite(SP_RoomInvite.ErrorCodes ErrCode)
    {
      this.newPacket((ushort) 29520);
      this.addBlock((object) (int) ErrCode);
    }

    public SP_RoomInvite(Game_Server.User usr, string Message)
    {
      this.newPacket((ushort) 29520);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) -1);
      this.addBlock((object) usr.userId);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) usr.nickname);
      this.addBlock((object) (usr.clan != null ? usr.clan.id : 0));
      this.addBlock((object) (uint) (usr.clan == null || usr.clanPending ? 0 : (int) usr.clan.iconid));
      this.addBlock(usr.clan != null ? (object) usr.clan.name : (object) "NULL");
      this.addBlock((object) -1);
      this.addBlock((object) (usr.clan == null || usr.clanPending ? 0 : usr.clan.clanRank(usr)));
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) usr.exp);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) -1);
      this.addBlock((object) Message);
      this.addBlock((object) 1);
      this.addBlock((object) usr.room.id);
      this.addBlock((object) usr.room.password);
    }

    internal enum ErrorCodes
    {
      GenericError = 93020, // 0x00016B5C
      IsPlaying = 93030, // 0x00016B66
    }
  }
}
