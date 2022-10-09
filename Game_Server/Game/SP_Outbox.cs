// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Outbox
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_Outbox : Packet
  {
    public SP_Outbox(Game_Server.User usr)
    {
      this.newPacket((ushort) 30752);
      this.addBlock((object) 1117);
      this.addBlock((object) 1);
      this.addBlock((object) usr.dinar);
      this.addBlock((object) 0);
      this.addBlock((object) usr.cash);
      this.addBlock((object) "LIST");
      this.addBlock((object) usr.OutboxItems.Count);
      foreach (OutboxItem outboxItem in usr.OutboxItems.Values.OrderByDescending<OutboxItem, int>((Func<OutboxItem, int>) (i => i.timestamp)).ToList<OutboxItem>())
      {
        this.addBlock((object) outboxItem.id);
        this.addBlock((object) usr.userId);
        this.addBlock((object) outboxItem.itemcode);
        this.addBlock((object) (ushort) (outboxItem.count > (ushort) 1 ? (int) outboxItem.count : (int) outboxItem.days));
        this.addBlock((object) "NULL");
        this.addBlock((object) "NULL");
        this.addBlock((object) usr.nickname);
        this.addBlock((object) 0);
      }
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
    }
  }
}
