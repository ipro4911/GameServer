// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ZombieDrop
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_ZombieDrop : Packet
  {
    public SP_ZombieDrop(Game_Server.User usr, int ID, int UsementID, int Type)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) ID);
      this.addBlock((object) 17);
      this.addBlock((object) 2);
      this.addBlock((object) 901);
      this.addBlock((object) UsementID);
      this.addBlock((object) 0);
      this.addBlock((object) -1);
      this.addBlock((object) Type);
      this.addBlock((object) ((long) usr.sessionId + (long) UsementID));
      this.addBlock((object) ID);
      this.addBlock((object) UsementID);
      this.addBlock((object) 13);
      this.addBlock((object) UsementID);
      this.addBlock((object) 13);
      this.addBlock((object) UsementID);
    }
  }
}
