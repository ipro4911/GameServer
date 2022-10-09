// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_UpdateChristmasEquipment
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_UpdateChristmasEquipment : Packet
  {
    public SP_UpdateChristmasEquipment(Game_Server.User usr, List<string> items)
    {
      this.newPacket((ushort) 30976);
      this.addBlock((object) 1);
      this.addBlock((object) usr.AvailableSlots);
      string str = "D201," + (usr.premium >= (byte) 3 ? "D204" : "^") + ",^,^,^,^,^,^";
      for (int index = 0; index < 5; ++index)
        this.addBlock((object) str);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) items.Count);
      foreach (object block in items)
        this.addBlock(block);
    }
  }
}
