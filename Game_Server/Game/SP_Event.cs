// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Event
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_Event : Packet
  {
    public SP_Event(Game_Server.User usr, string ItemCode, int Days)
    {
      this.newPacket((ushort) 30977);
      this.addBlock((object) 1);
      for (int index = 0; index < 5; ++index)
        this.addBlock((object) usr.costumes_char[index]);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) 0);
      this.addBlock((object) 1005);
      this.addBlock((object) ItemCode);
      this.addBlock((object) Days);
      this.addBlock((object) usr.eventcount);
    }

    internal enum ErrorCodes
    {
      ItemNotAvailable = -1, // 0xFFFFFFFF
      InventoryFull = 97070, // 0x00017B2E
    }
  }
}
