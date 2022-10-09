// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CouponBuy
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Game
{
  internal class CP_CouponBuy : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null || !Game_Server.Configs.Server.Player.CouponEvent)
        return;
      int result = -1;
      int.TryParse(this.getBlock(0), out result);
      if (result >= 0 && result <= 8)
      {
        int num1 = 0;
        int num2 = 0;
        string WeaponCode = (string) null;
        switch (result)
        {
          case 0:
            WeaponCode = "CC41";
            num1 = 3;
            num2 = 15;
            break;
          case 1:
            WeaponCode = "CI01";
            num1 = 3;
            num2 = 10;
            break;
          case 2:
            WeaponCode = "DF96";
            num1 = 3;
            num2 = 20;
            break;
          case 3:
            WeaponCode = "BS12";
            num1 = 3;
            num2 = 10;
            break;
          case 4:
            WeaponCode = "DF14";
            num1 = 3;
            num2 = 15;
            break;
          case 5:
            WeaponCode = "DC40";
            num1 = 3;
            num2 = 15;
            break;
          case 6:
            WeaponCode = "DF18";
            num1 = 3;
            num2 = 15;
            break;
          case 7:
            WeaponCode = "DF12";
            num1 = 3;
            num2 = 10;
            break;
          case 8:
            WeaponCode = "DG44";
            num1 = 3;
            num2 = 10;
            break;
        }
        if (usr.coupons >= num2)
        {
          if (Inventory.GetFreeItemSlotCount(usr) > 0)
          {
            usr.coupons -= num2;
            DB.RunQuery("UPDATE users SET coupons='" + (object) usr.coupons + "' WHERE id='" + (object) usr.userId + "'");
            if (WeaponCode != null)
            {
              if (WeaponCode == "CC41")
              {
                int days = new Random().Next(1, num1);
                if (usr.premium == (byte) 3)
                  usr.premiumExpire += (uint) (86400 * days);
                else
                  usr.premiumExpire = (uint) (Generic.timestamp + 86400 * days);
                usr.premium = (byte) 3;
                Inventory.AddItem(usr, "DB33", days);
                Inventory.AddItem(usr, "CD01", days);
                Inventory.AddItem(usr, "CD02", days);
                DB.RunQuery("UPDATE users SET premium='3', premiumExpire='" + (object) usr.premiumExpire + "' WHERE id='" + (object) usr.userId + "'");
                usr.send((Packet) new SP_PingInformation(usr));
              }
              else if (WeaponCode.StartsWith("B"))
                Inventory.AddCostume(usr, WeaponCode, num1);
              else
                Inventory.AddItem(usr, WeaponCode, num1);
              usr.send((Packet) new SP_CouponBuy(WeaponCode, usr));
            }
            else
              usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[1]
              {
                (object) "err"
              }));
          }
          else
            usr.send((Packet) new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.InventoryFull, new object[1]
            {
              (object) "NULL"
            }));
        }
        else
          usr.send((Packet) new SP_CouponBuy(SP_CouponBuy.ErrorCode.NotEnoughCoupons));
      }
      else
        usr.disconnect();
    }
  }
}
