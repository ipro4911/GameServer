// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Spectate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_Spectate : Packet
  {
    public SP_Spectate()
    {
      this.newPacket((ushort) 29488);
      this.addBlock((object) 0);
    }

    public SP_Spectate(Game_Server.User usr, Room room)
    {
      this.newPacket((ushort) 29488);
      this.addBlock((object) 1);
      this.addBlock((object) 1);
      this.addBlock((object) usr.roomslot);
      this.addRoomInfo(room);
    }
  }
}
