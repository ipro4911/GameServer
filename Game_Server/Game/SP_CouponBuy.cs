// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CouponBuy
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CouponBuy : Packet
  {
    public SP_CouponBuy(SP_CouponBuy.ErrorCode ErrCode)
    {
      this.newPacket((ushort) 25606);
      this.addBlock((object) (int) ErrCode);
    }

    public SP_CouponBuy(string WeaponCode, Game_Server.User usr)
    {
      this.newPacket((ushort) 25606);
      this.addBlock((object) 0);
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.coupontime);
      this.addBlock((object) usr.dinar);
      this.addBlock((object) usr.todaycoupons);
      this.addBlock((object) usr.coupons);
      this.addBlock((object) Inventory.Costumelist(usr));
    }

    internal enum ErrorCode
    {
      NotEnoughCoupons = 1,
    }
  }
}
