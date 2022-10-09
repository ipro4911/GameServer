// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_DinarItemBuy
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_DinarItemBuy : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null)
        return;
      string upper = this.getBlock(1).ToUpper();
      int num1 = int.Parse(this.getBlock(4));
      int days = Inventory.GetDaysFromPeriod(num1);
      Item obj = ItemManager.GetItem(upper);
      if (obj == null)
        return;
      if (days > 0)
      {
        if (obj.Buyable || ((IEnumerable<string>) Game_Server.Configs.Server.ItemShop.hiddenItems).Contains<string>(upper))
        {
          if (Inventory.GetFreeItemSlotCount(usr) > 0)
          {
            uint price = (uint) obj.GetPrice(num1);
            double num2 = 1.0;
            if (usr.HasItem(upper))
              num2 = (DateTime.ParseExact(Inventory.GetExpirationDate(usr, upper).ToString(), "yyMMddHH", (IFormatProvider) null) - DateTime.Now).TotalDays;
            if (price > 0U)
            {
              if (num2 < 60.0)
              {
                bool flag = upper.ToLower().StartsWith("cz");
                int num3 = (int) ((long) usr.dinar - (long) price);
                if (obj.Premium && usr.premium < (byte) 1)
                  usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.PremiumUsersOnly, new object[0]));
                else if ((int) usr.level < obj.Level && usr.rank < 2)
                  usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.LevelLow, new object[0]));
                else if (num3 < 0)
                {
                  usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NotEnoughDinar, new object[0]));
                }
                else
                {
                  if (flag)
                    days = 3600;
                  if (Inventory.AddItem(usr, upper, days))
                  {
                    usr.dinar = num3;
                    usr.send((Packet) new SP_DinarItemBuy(usr, upper));
                    DB.RunQuery("UPDATE users SET dinar=" + (object) num3 + " WHERE id='" + (object) usr.userId + "'");
                  }
                  else
                    usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[0]));
                }
              }
              else
                usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.MaximumTimeLimit, new object[0]));
            }
            else
              usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[0]));
          }
          else
            usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.InventoryFull, new object[0]));
        }
        else
          usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NoLongerValid, new object[0]));
      }
      else
        usr.disconnect();
    }
  }
}
