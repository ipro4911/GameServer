// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_MessengerMessage
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_MessengerMessage : Packet
  {
    public SP_MessengerMessage(string User, string Friend, string Message)
    {
      this.newPacket((ushort) 32256);
      this.addBlock((object) 1);
      this.addBlock((object) 5608);
      this.addBlock((object) User);
      this.addBlock((object) Friend);
      this.addBlock((object) Message);
    }
  }
}
