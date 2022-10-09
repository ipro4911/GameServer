// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CouponEvent
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CouponEvent : Packet
  {
    public SP_CouponEvent(int TodayCoupon, int Coupons)
    {
      this.newPacket((ushort) 25605);
      this.addBlock((object) 1);
      this.addBlock((object) TodayCoupon);
      this.addBlock((object) Coupons);
      this.addBlock((object) 1);
      this.addBlock((object) 1);
      this.addBlock((object) "0-0-0-0-0-0-0-0-0");
    }

    public SP_CouponEvent(Game_Server.User usr)
    {
      this.newPacket((ushort) 25605);
      this.addBlock((object) 0);
      this.addBlock((object) usr.todaycoupons);
      this.addBlock((object) usr.coupons);
      this.addBlock((object) usr.coupontime);
      this.addBlock((object) 0);
      this.addBlock((object) "0-0-0-0-0-0-0-0-0");
    }
  }
}
