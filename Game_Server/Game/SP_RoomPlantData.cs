// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomPlantData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_RoomPlantData : Packet
  {
    public SP_RoomPlantData(params object[] Params)
    {
      this.newPacket((ushort) 29984);
      this.addBlock((object) 1);
      ((IEnumerable<object>) Params).ToList<object>().ForEach((Action<object>) (obj => this.addBlock(obj)));
    }
  }
}
