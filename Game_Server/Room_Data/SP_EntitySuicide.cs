// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.SP_EntitySuicide
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class SP_EntitySuicide : Packet
  {
    public SP_EntitySuicide(int slotId, SP_EntitySuicide.SuicideType type = SP_EntitySuicide.SuicideType.Suicide, bool outofworld = false)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) slotId);
      this.addBlock((object) -1);
      this.addBlock((object) 2);
            addBlock((int)Subtype.Suicide);
      //this.addBlock((object) Subtype.WorldDamage);
      //this.addBlock((object)Subtype.ServerKill);
      this.addBlock((object) 0);
      this.addBlock((object) (int) type);
      this.addBlock((object) (outofworld ? 2 : slotId));
      this.Fill((object) 0, 7);
    }

    internal enum SuicideType
    {
      Suicide,
      KilledByNotHavinHealTreatment,
    }
  }
}
