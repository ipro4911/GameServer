// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_PingInformation
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class SP_PingInformation : Packet
  {
    public SP_PingInformation(Game_Server.User usr)
    {
      this.newPacket((ushort) 25600);
      this.addBlock((object) 5000);
      this.addBlock((object) usr.ping);
      this.addBlock((object) 0);
      this.addBlock((object) EXPEventManager.EventTime);
      if (Game_Server.Configs.Server.RandomBoxEvent.hour == DateTime.Now.Hour)
        this.addBlock((object) 16);
      else if (Game_Server.Configs.Server.Christmas.IsChristmas && Game_Server.Configs.Server.Christmas.enabled)
        this.addBlock((object) 64);
      else
        this.addBlock((object) (EXPEventManager.isRunning ? EXPEventManager.EventType : 0));
      this.addBlock((object) EXPEventManager.EXPRate);
      this.addBlock((object) EXPEventManager.DinarRate);
      this.addBlock((object) usr.PremiumTimeLeft());
    }

    private enum EventType
    {
      Default = -1, // 0xFFFFFFFF
      ExpDinarEvent = 4,
      RandomHotTime = 16, // 0x00000010
      WhiteChristmas = 64, // 0x00000040
    }
  }
}
