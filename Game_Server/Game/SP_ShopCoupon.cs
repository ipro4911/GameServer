// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ShopCoupon
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_ShopCoupon : Packet
  {
    public SP_ShopCoupon(SP_ShopCoupon.Subtype Subtype)
    {
      this.newPacket((ushort) 30992);
      this.addBlock((object) (int) Subtype);
    }

    public SP_ShopCoupon(Game_Server.User usr)
    {
      this.newPacket((ushort) 30992);
      this.addBlock((object) 1);
      this.addBlock((object) "#toxiic");
      this.addBlock((object) 0);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) Inventory.Costumelist(usr));
      this.addBlock((object) usr.dinar);
      this.addBlock((object) usr.exp);
    }

    public enum Subtype
    {
      InvalidCoupon = -11, // 0xFFFFFFF5
      Unknown = -10, // 0xFFFFFFF6
      InventoryFull = -9, // 0xFFFFFFF7
      CouponIsExpired = -8, // 0xFFFFFFF8
      AlreadyUsedCouponByHimself = -7, // 0xFFFFFFF9
      UserDinarIsToHigh = -6, // 0xFFFFFFFA
      CouponCanNotBeUsedUnder7Days = -5, // 0xFFFFFFFB
      UnknownError = -4, // 0xFFFFFFFC
      CouponRegistrationError = -3, // 0xFFFFFFFD
      WrongCoupon = -2, // 0xFFFFFFFE
      AlreadyUsedCouponByOther = -1, // 0xFFFFFFFF
    }
  }
}
