// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_OutboxUse
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_OutboxUse : Packet
  {
    public SP_OutboxUse(Game_Server.User usr, string itemcode)
    {
      this.newPacket((ushort) 30752);
      this.addBlock((object) 1118);
      this.addBlock((object) 1);
      this.addBlock((object) usr.dinar);
      this.addBlock((object) 0);
      this.addBlock((object) usr.cash);
      this.addBlock((object) itemcode);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) Inventory.Costumelist(usr));
    }
  }
}
