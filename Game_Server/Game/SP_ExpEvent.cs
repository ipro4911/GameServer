// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ExpEvent
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_ExpEvent : Packet
  {
    public SP_ExpEvent(SP_ExpEvent.EventCodes eCode)
    {
      this.newPacket((ushort) 25344);
      this.addBlock((object) (int) eCode);
      this.addBlock((object) 0);
    }

    internal enum EventCodes
    {
      EXP_Activate = 810, // 0x0000032A
      EXP_Deactivate = 820, // 0x00000334
    }
  }
}
