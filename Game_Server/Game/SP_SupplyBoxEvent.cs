﻿// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_SupplyBoxEvent
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_SupplyBoxEvent : Packet
  {
    public SP_SupplyBoxEvent(Game_Server.User usr)
    {
      this.newPacket((ushort) 32258);
      this.addBlock((object) 18);
      this.addBlock((object) 17);
      this.addBlock((object) 0);
      this.addBlock((object) 2);
    }
  }
}
