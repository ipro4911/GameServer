// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RandomBoxEvent
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RandomBoxEvent : Packet
  {
    public SP_RandomBoxEvent(Game_Server.User u, string item)
    {
      this.newPacket((ushort) 21281);
      this.addBlock((object) 0);
      this.addBlock((object) item);
      this.addBlock((object) Inventory.Itemlist(u));
      this.addBlock((object) Inventory.Costumelist(u));
    }
  }
}
