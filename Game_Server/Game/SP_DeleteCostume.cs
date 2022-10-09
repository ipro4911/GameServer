// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_DeleteCostume
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_DeleteCostume : Packet
  {
    public SP_DeleteCostume(Game_Server.User usr, string itemCode)
    {
      this.newPacket((ushort) 30225);
      this.addBlock((object) 1);
      this.addBlock((object) itemCode);
      this.addBlock((object) Inventory.Costumelist(usr));
      for (int index = 0; index < 5; ++index)
        this.addBlock((object) usr.costume[index]);
    }
  }
}
