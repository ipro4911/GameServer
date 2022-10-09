// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.SP_RoomData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Room_Data
{
  internal class SP_RoomData : Packet
  {
    public SP_RoomData(params object[] Params)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      ((IEnumerable<object>) Params).ToList<object>().ForEach((Action<object>) (r => this.addBlock(r)));
    }
  }
}
