// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_LoginEvent
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_LoginEvent : Packet
  {
    public SP_LoginEvent(SP_LoginEvent.ErrorCodes err)
    {
      this.newPacket((ushort) 30933);
      this.addBlock((object) (int) err);
    }

    public SP_LoginEvent(Game_Server.User usr, string item, int days)
    {
      this.newPacket((ushort) 30993);
      this.addBlock((object) 1);
      this.addBlock((object) 1);
      this.addBlock((object) usr.rewardEvent.progress);
      this.addBlock((object) item);
      this.addBlock((object) days);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) Inventory.Costumelist(usr));
      this.addBlock((object) usr.dinar);
    }

    public enum ErrorCodes
    {
      AlreadyChecked = -1, // 0xFFFFFFFF
      Success = 1,
    }
  }
}
