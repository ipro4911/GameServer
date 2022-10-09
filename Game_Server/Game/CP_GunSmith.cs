// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_GunSmith
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_GunSmith : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      int id = int.Parse(this.getBlock(0));
      CP_GunSmith.Type type = (CP_GunSmith.Type) int.Parse(this.getBlock(1));
      if (Inventory.GetFreeItemSlotCount(usr) == 0)
        return;
      if (type == CP_GunSmith.Type.Ticket && usr.HasItem("CZ75") || type == CP_GunSmith.Type.Dinar)
      {
        GunSmith gunSmithByGameId = GunSmithManager.GetGunSmithByGameId(id);
        if (gunSmithByGameId == null)
          return;
        string itemcode = gunSmithByGameId.item;
        string rare = gunSmithByGameId.rare;
        int eaItem1 = Inventory.GetEAItem(usr, "CZ85");
        int eaItem2 = Inventory.GetEAItem(usr, "CZ84");
        int eaItem3 = Inventory.GetEAItem(usr, "CZ83");
        string[] requiredMaterials = gunSmithByGameId.required_materials;
        uint num1 = (uint) gunSmithByGameId.cost;
        if (type != CP_GunSmith.Type.Dinar)
          num1 = 0U;
        if (num1 < 0U)
          return;
        int result1 = 0;
        int result2 = 0;
        int result3 = 0;
        int.TryParse(requiredMaterials[0].ToString(), out result1);
        int.TryParse(requiredMaterials[1].ToString(), out result2);
        int.TryParse(requiredMaterials[2].ToString(), out result3);
        if (eaItem1 < result1 || eaItem2 < result2 || eaItem3 < result3)
          return;
        string[] requiredItems = gunSmithByGameId.required_items;
        foreach (string strCode in requiredItems)
        {
          if (!usr.HasItem(strCode))
          {
            Log.WriteError("User " + usr.nickname + " hasn't " + strCode);
            usr.disconnect();
            return;
          }
        }
        foreach (string str in requiredItems)
          usr.deleteItem(str);
        if (result1 > 0)
          Inventory.DecreaseEAItem(usr, "CZ85", result1);
        if (result2 > 0)
          Inventory.DecreaseEAItem(usr, "CZ84", result2);
        if (result3 > 0)
          Inventory.DecreaseEAItem(usr, "CZ83", result3);
        string[] loseItems = gunSmithByGameId.lose_items;
        int num2 = Generic.random(0, 50);
        int num3 = type == CP_GunSmith.Type.Dinar ? 10 : 25;
        int days = 30;
        CP_GunSmith.WonType wonType = CP_GunSmith.WonType.Win;
        if (num2 > num3)
        {
          wonType = CP_GunSmith.WonType.Lose;
          int index = Generic.random(0, loseItems.Length - 1);
          itemcode = loseItems[index];
          days = Generic.random(7, 30);
        }
        else if (num2 == 17)
        {
          itemcode = rare;
          wonType = CP_GunSmith.WonType.RareWin;
        }
        if (type == CP_GunSmith.Type.Dinar)
          usr.dinar -= (int) num1;
        else
          Inventory.DecreaseEAItem(usr, "CZ75", 1);
        Inventory.AddItem(usr, itemcode, days);
        usr.send((Packet) new SP_GunSmith(usr, itemcode, wonType));
      }
      else
        usr.disconnect();
    }

    internal enum WonType : byte
    {
      Lose,
      Win,
      RareWin,
    }

    private enum Type : byte
    {
      Dinar,
      Ticket,
    }
  }
}
