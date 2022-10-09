// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.SP_RoomData_VoteKick
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class SP_RoomData_VoteKick : Packet
  {
    public SP_RoomData_VoteKick(int Target, bool KickedOut, int roomId)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) -1);
      this.addBlock((object) roomId);
      this.addBlock((object) 2);
      this.addBlock((object) 61);
      this.addBlock((object) (KickedOut ? 1 : 0));
      this.addBlock((object) (KickedOut ? 0 : 1));
      this.addBlock((object) 2);
      this.addBlock((object) Target);
      this.addBlock((object) (KickedOut ? 1 : 0));
      this.addBlock((object) (KickedOut ? 75 : 0));
      this.Fill((object) 0, 4);
    }

    public SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes code)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) (int) code);
    }

    internal enum ErrCodes
    {
      NotEnoughPlayers = 96140, // 0x0001778C
      VoteKickInProgress = 96150, // 0x00017796
      CannotKickError = 96151, // 0x00017797
      InvalidCandidate = 96152, // 0x00017798
      CannotKickOpposingTeam = 96153, // 0x00017799
      Need4Players = 96154, // 0x0001779A
      CannotKickError2 = 96155, // 0x0001779B
    }
  }
}
