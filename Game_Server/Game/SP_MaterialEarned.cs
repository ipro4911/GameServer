// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_MaterialEarned
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_MaterialEarned : Packet
  {
    public SP_MaterialEarned(Game_Server.User usr, int type)
    {
      this.newPacket((ushort) 30996);
      this.addBlock((object) type);
      this.addBlock((object) Inventory.Itemlist(usr));
    }
  }
}
