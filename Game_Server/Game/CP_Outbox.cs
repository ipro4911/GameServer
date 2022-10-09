// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_Outbox
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_Outbox : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      CP_Outbox.SubCodes subCodes = (CP_Outbox.SubCodes) int.Parse(this.getBlock(0));
      int outboxId = int.Parse(this.getBlock(1));
      switch (subCodes)
      {
        case CP_Outbox.SubCodes.Activate:
          if (usr.OutboxItems.Count <= 0)
            break;
          string block1 = this.getBlock(4);
          if (!Inventory.HasOutboxItem(usr, block1) || usr.OutboxItems.Values.Where<OutboxItem>((Func<OutboxItem, bool>) (r => r.id == outboxId)).Count<OutboxItem>() <= 0)
            break;
          OutboxItem outboxItem = usr.OutboxItems.Values.Where<OutboxItem>((Func<OutboxItem, bool>) (r => r.id == outboxId)).FirstOrDefault<OutboxItem>();
          int days = (int) outboxItem.days;
          if (Inventory.GetFreeItemSlotCount(usr) > 0)
          {
            if (ItemManager.GetItem(outboxItem.itemcode) == null)
              break;
            Inventory.PerformAddItem(usr, block1, days, (int) outboxItem.count);
            Inventory.RemoveOutBoxItem(usr, outboxId);
            usr.send((Packet) new SP_OutboxUse(usr, block1));
            usr.send((Packet) new SP_Outbox(usr));
            break;
          }
          usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.InventoryFull, new object[0]));
          break;
        case CP_Outbox.SubCodes.Delete:
          if (usr.OutboxItems.Count <= 0)
            break;
          string block2 = this.getBlock(4);
          if (!Inventory.HasOutboxItem(usr, block2))
            break;
          Inventory.RemoveOutBoxItem(usr, outboxId);
          usr.send((Packet) new SP_Outbox(usr));
          break;
      }
    }

    internal enum SubCodes
    {
      Activate = 1118, // 0x0000045E
      Delete = 1119, // 0x0000045F
    }
  }
}
