// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CarePackageSendItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class CP_CarePackageSendItem : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (!Game_Server.Configs.Server.Player.CarePackage)
        return;
      try
      {
        CarePackageItem carePackageItem = CarePackage.GetItem(int.Parse(this.getBlock(0)));
        if (carePackageItem == null)
          return;
        string str = carePackageItem.Item;
        int days = carePackageItem.days;
        bool isdinar = carePackageItem.Method == 0;
        uint price = (uint) carePackageItem.Price;
        int num1 = (isdinar ? usr.dinar : usr.cash) - (int) price;
        bool win = true;
        int num2 = new Random().Next(0, 4);
        if (num2 != 0)
        {
          win = false;
          switch (num2 - 1)
          {
            case 0:
              str = carePackageItem.Item1;
              days = carePackageItem.days1;
              break;
            case 1:
              str = carePackageItem.Item2;
              days = carePackageItem.days2;
              break;
            case 2:
              str = carePackageItem.Item3;
              days = carePackageItem.days3;
              break;
            case 3:
              str = carePackageItem.Item4;
              days = carePackageItem.days4;
              break;
          }
        }
        if (num1 < 0)
          return;
        if (isdinar)
        {
          usr.dinar = num1;
          DB.RunQuery("UPDATE users SET dinar='" + (object) num1 + "' WHERE id='" + (object) usr.userId + "'");
          Inventory.AddItem(usr, str, days);
        }
        else
        {
          usr.cash = num1;
          DB.RunQuery("UPDATE users SET cash='" + (object) num1 + "' WHERE id='" + (object) usr.userId + "'");
          Inventory.AddOutBoxItem(usr, str, (ushort) days, (ushort) 1);
        }
        usr.send((Packet) new SP_CARE_PACKAGE.SendItem(usr, str, days, isdinar, win));
      }
      catch
      {
      }
    }
  }
}
