// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CostumeBuy
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_CostumeBuy : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null)
        return;
      int.Parse(this.getBlock(0));
      int[] numArray = new int[7]
        {
          1,
          3,
          7,
          15,
          30,
          365,
          -1,
        };
            int num1 = int.Parse(this.getBlock(4));
      string block = this.getBlock(1);
      Item obj = ItemManager.GetItem(block);
      if (obj != null)
      {
        int cashPrice = obj.GetCashPrice(num1);
        if (usr.cash - cashPrice < 0)
          usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NotEnoughDinar, new object[1]
          {
            (object) "NULL"
          }));
        else if (usr.cash >= cashPrice && obj.Buyable && cashPrice > 0)
        {
          if (Inventory.GetFreeCostumeSlotCount(usr) >= 0)
          {
            int num2 = usr.cash - cashPrice;
            if (num2 > 0)
            {
              ushort daysFromPeriod = (ushort) Inventory.GetDaysFromPeriod(num1);
              Inventory.AddOutBoxItem(usr, block, daysFromPeriod, (ushort) 1);
              usr.cash = num2;
              usr.send((Packet) new SP_OutboxSend(usr));
              usr.send((Packet) new SP_CashItemBuy(usr));
              DB.RunQuery("UPDATE users SET cash='" + (object) num2 + "' WHERE id='" + (object) usr.userId + "'");
            }
            else
              usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NotEnoughCash, new object[1]
              {
                (object) "NULL"
              }));
          }
          else
            usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.InventoryFull, new object[1]
            {
              (object) "NULL"
            }));
        }
        else
          usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[1]
          {
            (object) "NULL"
          }));
      }
      else
        usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NoLongerValid, new object[1]
        {
          (object) "NULL"
        }));
    }
  }
}
