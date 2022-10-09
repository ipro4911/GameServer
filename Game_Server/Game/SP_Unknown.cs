// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Unknown
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_Unknown : Packet
  {
        /*public SP_Unknown(ushort packetId, params object[] Params)
        {
          this.newPacket(packetId);
          ((IEnumerable<object>) Params).ToList<object>().ForEach((Action<object>) (p => this.addBlock(p)));
        }*/  // Old one

        public SP_Unknown(ushort packetId, params object[] Params)
        {
            newPacket(packetId);
            Params.ToList().ForEach(p => { addBlock(p); });
        } // New one testing
    }
}
