// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_OutboxSend
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_OutboxSend : Packet
  {
    public SP_OutboxSend(Game_Server.User usr)
    {
      this.newPacket((ushort) 30752);
      this.addBlock((object) 1117);
      this.addBlock((object) 1);
      this.addBlock((object) usr.dinar);
      this.addBlock((object) 0);
      this.addBlock((object) usr.cash);
      this.addBlock((object) "LIST");
      this.addBlock((object) usr.OutboxItems.Count);
      foreach (KeyValuePair<int, OutboxItem> keyValuePair in usr.OutboxItems.OrderByDescending<KeyValuePair<int, OutboxItem>, int>((Func<KeyValuePair<int, OutboxItem>, int>) (i => i.Key)).ToList<KeyValuePair<int, OutboxItem>>())
      {
        OutboxItem outboxItem = keyValuePair.Value;
        this.addBlock((object) outboxItem.id);
        this.addBlock((object) usr.userId);
        this.addBlock((object) outboxItem.itemcode);
        this.addBlock((object) (ushort) (outboxItem.count > (ushort) 1 ? (int) outboxItem.count : (int) outboxItem.days));
        this.addBlock((object) "NULL");
        this.addBlock((object) "NULL");
        this.addBlock((object) usr.nickname);
        this.addBlock((object) 0);
      }
      this.addBlock((object) 1);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) Inventory.Costumelist(usr));
      this.addBlock((object) 0);
      this.addBlock((object) 1);
    }
  }
}
