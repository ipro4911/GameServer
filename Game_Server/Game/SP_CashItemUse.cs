// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CashItemUse
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

/*namespace Game_Server.Game
{
  internal class SP_CashItemUse : Packet
  {
    public SP_CashItemUse(SP_CashItemUse.ErrCode ErrCode, Game_Server.User usr, string ItemCode)
    {
      this.newPacket((ushort) 30720);
      this.addBlock((object) 1111);
      this.addBlock((object) (int) ErrCode);
      this.addBlock((object) ItemCode);
      this.addBlock((object) Inventory.Itemlist(usr));
    }

    public SP_CashItemUse(Game_Server.User usr, string ItemCode)
    {
      this.newPacket((ushort) 30720);
      this.addBlock((object) 1111);
      this.addBlock((object) 1);
      this.addBlock((object) ItemCode);
      this.addBlock((object) Inventory.Itemlist(usr));
      if (ItemCode == "CB03")
      {
        this.addBlock((object) usr.AvailableSlots);
        this.addBlock((object) 0);
        this.addBlock((object) 0);
        this.addBlock((object) usr.dinar);
      }
      else
      {
        if (!(ItemCode == "CB01"))
          return;
        this.addBlock((object) usr.AvailableSlots);
        this.addBlock((object) usr.nickname);
      }
    }

    internal enum ErrCode
    {
      NeedSupplyBox = -3, // 0xFFFFFFFD
    }
  }
}*/
