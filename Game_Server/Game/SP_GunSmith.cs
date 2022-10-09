// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_GunSmith
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_GunSmith : Packet
  {
    public SP_GunSmith(Game_Server.User usr, string itemcode, CP_GunSmith.WonType wonType)
    {
      this.newPacket((ushort) 30995);
      this.addBlock((object) 1);
      this.addBlock((object) itemcode);
      this.addBlock((object) (byte) wonType);
      this.addBlock((object) usr.dinar);
      this.addBlock((object) usr.cash);
      this.addBlock((object) Inventory.Itemlist(usr));
      for (int c = 0; c < 5; ++c)
        this.addBlock((object) usr.GetEquipment(c));
    }
  }
}
