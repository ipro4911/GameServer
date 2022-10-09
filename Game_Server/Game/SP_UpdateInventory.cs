// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_UpdateInventory
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_UpdateInventory : Packet
  {
    public SP_UpdateInventory(Game_Server.User usr, List<string> items)
    {
      this.newPacket((ushort) 30976);
      this.addBlock((object) 1);
      this.addBlock((object) usr.AvailableSlots);
      for (int c = 0; c < 5; ++c)
        this.addBlock((object) usr.GetEquipment(c));
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) (items != null ? items.Count : 0));
      if (items == null)
        return;
      foreach (object block in items)
        this.addBlock(block);
    }
  }
}
