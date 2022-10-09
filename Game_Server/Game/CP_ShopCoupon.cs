// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_ShopCoupon
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_ShopCoupon : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      string str1 = this.getBlock(0).Replace("-", "");
      DataTable dataTable = DB.RunReader("SELECT * FROM ingame_coupons WHERE code='" + str1 + "'");
      if (dataTable.Rows.Count > 0)
      {
        DataRow row = dataTable.Rows[0];
        bool flag = row["used"].ToString() == "1";
        int num1 = int.Parse(row["userId"].ToString());
        if (!flag)
        {
          if (Inventory.GetFreeItemSlotCount(usr) > 0)
          {
            uint num2 = uint.Parse(row["dinars"].ToString());
            if (num2 > 0U)
              usr.dinar += (int) num2;
            uint num3 = uint.Parse(row["cashs"].ToString());
            if (num3 > 0U)
            {
              usr.cash += (int) num3;
              usr.send((Packet) new SP_CashItemBuy(usr));
            }
            if (num2 > 0U || num3 > 0U)
              DB.RunQuery("UPDATE users SET cash='" + (object) usr.cash + "', dinar='" + (object) usr.dinar + "' WHERE id='" + (object) usr.userId + "'");
            if (row["items"].ToString().Length > 0)
            {
              List<string> list = ((IEnumerable<string>) row["items"].ToString().Split('|')).ToList<string>();
              if (list.Count > 0)
              {
                foreach (string str2 in list)
                {
                  char[] chArray = new char[1]{ ',' };
                  string[] strArray = str2.Split(chArray);
                  string itemcode = strArray[0];
                  int result = 0;
                  int.TryParse(strArray[1], out result);
                  if (result != 0)
                    Inventory.PerformAddItem(usr, itemcode, result, 1);
                }
              }
            }
            DB.RunQuery("UPDATE ingame_coupons SET used='1', userId='" + (object) usr.userId + "', time='" + (object) Game_Server.Generic.timestamp + "' WHERE code='" + str1 + "'");
            usr.send((Packet) new SP_ShopCoupon(usr));
          }
          else
            usr.send((Packet) new SP_ShopCoupon(SP_ShopCoupon.Subtype.InventoryFull));
        }
        else
          usr.send((Packet) new SP_ShopCoupon(num1 == usr.userId ? SP_ShopCoupon.Subtype.AlreadyUsedCouponByHimself : SP_ShopCoupon.Subtype.AlreadyUsedCouponByOther));
      }
      else
        usr.send((Packet) new SP_ShopCoupon(SP_ShopCoupon.Subtype.WrongCoupon));
    }
  }
}
