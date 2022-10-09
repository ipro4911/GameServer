// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CostumeEquip
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_CostumeEquip : Packet
  {
    public SP_CostumeEquip(SP_CostumeEquip.ErrCode code)
    {
      this.newPacket((ushort) 29972);
      this.addBlock((object) (int) code);
    }

    public SP_CostumeEquip(int Class, string Code)
    {
      this.newPacket((ushort) 29971);
      this.addBlock((object) 1);
      this.addBlock((object) Class);
      this.addBlock((object) Code);
    }

    internal enum ErrCode
    {
      CannotDeleteDefaultItem = 97092, // 0x00017B44
    }
  }
}
