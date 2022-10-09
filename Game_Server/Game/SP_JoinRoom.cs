// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_JoinRoom
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_JoinRoom : Packet
  {
    public SP_JoinRoom(Game_Server.User usr, Room r)
    {
      this.newPacket((ushort) 29456);
      this.addBlock((object) 1);
      this.addBlock((object) usr.roomslot);
      //addBlock(usr.ping);
      this.addRoomInfo(r);
    }

    public SP_JoinRoom(SP_JoinRoom.ErrorCodes ErrCode)
    {
      this.newPacket((ushort) 29456);
      this.addBlock((object) (int) ErrCode);
    }

    internal enum ErrorCodes
    {
      GenericError = 94010, // 0x00016F3A
      InvalidPassword = 94030, // 0x00016F4E
      GameIsEnding = 94120, // 0x00016FA8
      BadLevel = 94300, // 0x0001705C
      OnlyPremium = 94301, // 0x0001705D
    }
  }
}
