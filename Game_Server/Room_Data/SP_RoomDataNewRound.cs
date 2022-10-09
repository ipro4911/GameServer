// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.SP_RoomDataNewRound
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class SP_RoomDataNewRound : Packet
  {
    public SP_RoomDataNewRound(Room Room, int WinningTeam, bool Prepare)
    {
      int num = Prepare ? 6 : 5;
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) -1);
      this.addBlock((object) Room.id);
      this.addBlock((object) 1);
      this.addBlock((object) num);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) WinningTeam);
      this.addBlock((object) Room.DerbRounds);
      this.addBlock((object) Room.NIURounds);
      this.addBlock((object) 5);
      this.addBlock((object) 0);
      this.addBlock((object) 92);
      this.addBlock((object) -1);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 1200000);
      this.addBlock((object) -900000);
      this.addBlock((object) 1200000);
      this.addBlock((object) "778.0000");
      this.addBlock((object) "32.0000");
      this.addBlock((object) "1438.0000");
      this.addBlock((object) "49.0000");
      this.addBlock((object) "-275.0000");
      this.addBlock((object) "-108.0000");
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) "DS05");
    }
  }
}
