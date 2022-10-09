// Decompiled with JetBrains decompiler
// Type: Game_Server.Inventory
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Game_Server
{
  internal class Inventory
  {
    private static int[] periodDays = new int[7]
    {
      1,
      3,
      7,
      15,
      30,
      60,
      365
    };

    public static string Itemlist(User usr)
    {
      return string.Join(",", usr.inventory);
    }

    public static string Costumelist(User usr)
    {
      return string.Join(",", usr.costume);
    }

    public static string Storage(User usr)
    {
      return string.Join(",", usr.storageInventory);
    }

    public static string calculateInventory(int Id)
    {
      return string.Format("I{0:000}", (object) Id);
    }

    public static void PerformAddItem(User usr, string itemcode, int days, int count = 1)
    {
      Item obj = ItemManager.GetItem(itemcode);
      if (itemcode == null)
        return;
      if ((obj.accruable || obj.BuyType == 4) && usr.HasItem(itemcode))
      {
        Inventory.IncreaseEAItem(usr, itemcode, count);
      }
      else
      {
        if (PackageManager.AddItem(usr, itemcode))
          return;
        if (itemcode.StartsWith("B"))
          Inventory.AddCostume(usr, itemcode, days);
        else
          Inventory.AddItem(usr, itemcode, days);
        if (count <= 1 || !obj.accruable && obj.BuyType != 4)
          return;
        Inventory.IncreaseEAItem(usr, itemcode, count - 1);
      }
    }

    public static int GetDaysFromPeriod(int period)
    {
      if (Inventory.periodDays[period] >= 0)
        return Inventory.periodDays[period];
      return -1;
    }

    public static int GetFreeItemSlotCount(User usr)
    {
      return usr.inventory.Cast<string>().Where<string>((Func<string, bool>) (r => r == "^")).Count<string>();
    }

    public static int GetFreeCostumeSlotCount(User usr)
    {
      return usr.costume.Cast<string>().Where<string>((Func<string, bool>) (r => r == "^")).Count<string>();
    }

    public static bool AddOutBoxItem(User usr, string ItemCode, ushort Days, ushort count)
    {
      Item obj = ItemManager.GetItem(ItemCode);
      if (obj != null)
      {
        int timestamp = Game_Server.Generic.timestamp;
        DB.RunQueryNotAsync("INSERT INTO outbox (ownerid, itemcode, days, count, timestamp) VALUES ('" + (object) usr.userId + "', '" + ItemCode + "', '" + (object) Days + "', '" + (object) count + "','" + (object) timestamp + "')");
        DataTable dataTable = DB.RunReader("SELECT * FROM outbox WHERE ownerid='" + (object) usr.userId + "' AND itemcode='" + ItemCode + "' AND days='" + (object) Days + "' AND timestamp='" + (object) timestamp + "' ORDER BY id DESC");
        if (dataTable.Rows.Count > 0)
        {
          int num = int.Parse(dataTable.Rows[0]["id"].ToString());
          if (!usr.OutboxItems.ContainsKey(num))
          {
            OutboxItem outboxItem = new OutboxItem(num, ItemCode, Days, timestamp, count);
            usr.OutboxItems.TryAdd(num, outboxItem);
          }
          Log.WriteLine(usr.nickname + " -> Added outbox item " + obj.Name + " (ID: " + (object) num + ")");
        }
      }
      usr.send((Packet) new SP_Outbox(usr));
      return true;
    }

    public static void RemoveOutBoxItem(User usr, int Id)
    {
      if (!usr.OutboxItems.ContainsKey(Id))
        return;
      OutboxItem outboxItem;
      usr.OutboxItems.TryRemove(Id, out outboxItem);
      DB.RunQuery("DELETE FROM outbox WHERE id='" + (object) Id + "' AND ownerid='" + (object) usr.userId + "'");
    }

    public static bool HasOutboxItem(User usr, string strCode)
    {
      foreach (OutboxItem outboxItem in (IEnumerable<OutboxItem>) usr.OutboxItems.Values)
      {
        if (string.Compare(outboxItem.itemcode, strCode, true) == 0)
          return true;
      }
      return false;
    }

    public static bool AddItem(User usr, string item, int days)
    {
      try
      {
        if (days == -1)
          days = 3600;
        int itemIndex = usr.GetItemIndex(item);
        if (itemIndex != -1)
        {
          Item obj = ItemManager.GetItem(item);
          string[] strArray = usr.inventory[itemIndex].Split('-');
          DateTime dateTime = DateTime.ParseExact(strArray[3], "yyMMddHH", (IFormatProvider) null).AddDays((double) days);
          strArray[3] = string.Format("{0:yyMMddHH}", (object) dateTime);
          if (obj != null && (obj.accruable || obj.BuyType == 4))
          {
            int result = 1;
            int.TryParse(strArray[4], out result);
            int num = result + 1;
            if (num >= (int) obj.maxAccrueCount)
              num = (int) obj.maxAccrueCount;
            strArray[4] = num.ToString();
          }
          usr.inventory[itemIndex] = string.Join("-", strArray);
          DB.RunQuery("UPDATE equipment SET inventory='" + Inventory.Itemlist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
          return true;
        }
        int index = Array.IndexOf<string>(usr.inventory, "^");
        if (index != -1 && index <= Game_Server.Configs.Server.Player.MaxInventorySlot)
        {
          if (usr.inventory[index] == "^")
          {
            Item obj = ItemManager.GetItem(item);
            int num = 0;
            if (obj != null && (obj.accruable || obj.BuyType == 4))
            {
              num = 1;
              days = 3600;
            }
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays((double) days);
            dateTime = dateTime.AddHours(-4.0);
            usr.inventory[index] = item + "-1-1-" + string.Format("{0:yyMMddHH}", (object) dateTime) + "-" + (object) num;
            DB.RunQuery("UPDATE equipment SET inventory='" + Inventory.Itemlist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
            return true;
          }
        }
        else
        {
          DB.RunQuery("INSERT INTO inbox (ownerid, itemcode, days) VALUES ('" + (object) usr.userId + "', '" + item + "', '" + (object) days + "')");
          usr.send((Packet) new SP_CustomMessageBox("The item you bought has been added to inbox.\nYour inventory is full, delete a item and re-login to get it."));
          return true;
        }
      }
      catch (Exception ex)
      {
        Log.WriteError("Error at AddItem: " + ex.Message + " - " + ex.StackTrace);
      }
      return false;
    }

    public static bool AddCostume(User usr, string item, int days)
    {
      try
      {
        if (days == -1)
          days = 3600;
        int costumeIndex = usr.GetCostumeIndex(item);
        if (costumeIndex != -1)
        {
          Item obj = ItemManager.GetItem(item);
          string[] strArray = usr.costume[costumeIndex].Split('-');
          DateTime dateTime = DateTime.ParseExact(strArray[3], "yyMMddHH", (IFormatProvider) null).AddDays((double) days);
          strArray[3] = string.Format("{0:yyMMddHH}", (object) dateTime);
          usr.costume[costumeIndex] = string.Join("-", strArray);
          if (obj != null && (obj.accruable || obj.BuyType == 4))
          {
            int result = 1;
            int.TryParse(strArray[4], out result);
            if (result > (int) obj.maxAccrueCount)
              result = (int) obj.maxAccrueCount;
            strArray[4] = (result + 1).ToString();
          }
          DB.RunQuery("UPDATE users_costumes SET inventory='" + Inventory.Costumelist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
          return true;
        }
        int index = Array.IndexOf<string>(usr.costume, "^");
        if (index != -1 && index < Game_Server.Configs.Server.Player.MaxCostumeSlot)
        {
          if (!(usr.costume[index] == "^"))
            return false;
          DateTime dateTime = DateTime.Now;
          dateTime = dateTime.AddDays((double) days);
          dateTime = dateTime.AddHours(-4.0);
          usr.costume[index] = item + "-1-1-" + string.Format("{0:yyMMddHH}", (object) dateTime) + "-0-0-0-0-0";
          DB.RunQuery("UPDATE users_costumes SET inventory='" + Inventory.Costumelist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
          return true;
        }
        DB.RunQuery("INSERT INTO inbox (ownerid, itemcode, days) VALUES ('" + (object) usr.userId + "', '" + item + "', '" + (object) days + "')");
        usr.send((Packet) new SP_CustomMessageBox("The item you bought has been added to inbox.\nYour inventory is full, delete a item and relogin."));
        return true;
      }
      catch (Exception ex)
      {
        Log.WriteError("Error at AddCostume: " + ex.Message + " - " + ex.StackTrace);
      }
      return false;
    }

    public static void IncreaseEAItem(User usr, string item, int c = 1)
    {
      int itemIndex = usr.GetItemIndex(item);
      if (itemIndex == -1)
        return;
      string[] strArray = usr.inventory[itemIndex].Split('-');
      int result = 0;
      int.TryParse(strArray[4], out result);
      if (result <= 0)
        return;
      int num = result + c;
      if (num > 999)
        num = 999;
      strArray[4] = num.ToString();
      usr.inventory[itemIndex] = string.Join("-", strArray);
    }

    public static void DecreaseEAItem(User usr, string item, int c = 1)
    {
      int itemIndex = usr.GetItemIndex(item);
      if (itemIndex == -1)
        return;
      string[] strArray = usr.inventory[itemIndex].Split('-');
      int result = 0;
      int.TryParse(strArray[4], out result);
      int num = result - c;
      if (num >= 1)
      {
        strArray[4] = num.ToString();
        usr.inventory[itemIndex] = string.Join("-", strArray);
      }
      else
        usr.deleteItem(item);
    }

    public static int GetEAItem(User usr, string item)
    {
      int itemIndex = usr.GetItemIndex(item);
      if (itemIndex == -1)
        return 0;
      string[] strArray = usr.inventory[itemIndex].Split('-');
      int result = 0;
      int.TryParse(strArray[4], out result);
      return result;
    }

    public static int GetExpirationDate(User usr, string item)
    {
      int itemIndex = usr.GetItemIndex(item);
      int result = int.Parse(DateTime.Now.ToString("yyMMddHH"));
      if (itemIndex != -1)
        int.TryParse(usr.inventory[itemIndex].Split('-')[3], out result);
      return result;
    }

    public static int GetUserWeaponCount(User usr)
    {
      return ((IEnumerable<string>) usr.inventory).Where<string>((Func<string, bool>) (r => r.StartsWith("D"))).Count<string>();
    }

    public static bool isPXItem(string Weapon)
    {
      Item obj = ItemManager.GetItem(Weapon);
      if (obj != null)
        return obj.UseableSlot(5);
      return false;
    }
  }
}
