// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_LevelUp
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_LevelUp : Packet
  {
    public SP_LevelUp(Game_Server.User usr, int Dinar, List<LevelUPItem> Items)
    {
      this.newPacket((ushort) 31008);
      this.addBlock((object) usr.roomslot);
      this.addBlock((object) usr.exp);
      this.addBlock((object) Items.Count);
      foreach (LevelUPItem levelUpItem in Items)
      {
        this.addBlock((object) levelUpItem.Code);
        this.addBlock((object) levelUpItem.Days);
      }
      this.addBlock((object) Dinar);
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) Inventory.Costumelist(usr));
      this.addBlock((object) 0);
    }
  }
}
