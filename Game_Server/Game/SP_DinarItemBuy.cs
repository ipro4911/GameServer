// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_DinarItemBuy
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_DinarItemBuy : Packet
  {
    public SP_DinarItemBuy(Game_Server.User usr, string item)
    {
      this.newPacket((ushort) 30208);
      this.addBlock((object) 1);
      this.addBlock((object) 1110);
      this.addBlock((object) -1);
      this.addBlock((object) 3);
      this.addBlock((object) 4);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.dinar);
      this.addBlock((object) usr.AvailableSlots);
    }

    public SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes err, params object[] Params)
    {
      this.newPacket((ushort) 30208);
      this.addBlock((object) (int) err);
      foreach (object block in Params)
        this.addBlock(block);
    }

    internal enum ErrorCodes
    {
      Success = 1,
      NoLongerValid = 97010, // 0x00017AF2
      PurchaseFifthSlotFirst = 97012, // 0x00017AF4
      CannotBeBougth = 97020, // 0x00017AFC
      NotEnoughDinar = 97040, // 0x00017B10
      LevelLow = 97050, // 0x00017B1A
      InventoryFull = 97070, // 0x00017B2E
      NotEnoughCash = 97092, // 0x00017B44
      MaximumTimeLimit = 97100, // 0x00017B4C
      PremiumUsersOnly = 98010, // 0x00017EDA
      GoldUsersOnly = 98020, // 0x00017EE4
      FifthSlotFreeForGoldUsers = 98030, // 0x00017EEE
    }
  }
}
