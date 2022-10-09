// Decompiled with JetBrains decompiler
// Type: Game_Server.Anti_Cheat.Data.SP_Connect
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Anti_Cheat.Data
{
  internal class SP_Connect : Game_Server.Anti_Cheat.Structure.Packet
  {
    public SP_Connect(Client usr)
    {
      this.newPacket(10040);
      this.addBlock((object) ((int) Math.Ceiling((double) usr.sessionId * 3.35) * Game_Server.Configs.Server.ClientVersion));
    }
  }
}
