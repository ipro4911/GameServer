// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomVehicleExplode
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomVehicleExplode : Packet
  {
    public SP_RoomVehicleExplode(int RoomID, int TargetID, int shooterId)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) shooterId);
      this.addBlock((object) RoomID);
      this.addBlock((object) 2);
      this.addBlock((object) 153);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) TargetID);
      this.addBlock((object) shooterId);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 100);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) "FFFF");
    }
  }
}
