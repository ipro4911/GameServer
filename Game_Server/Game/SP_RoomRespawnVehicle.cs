// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomRespawnVehicle
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomRespawnVehicle : Packet
  {
    public SP_RoomRespawnVehicle(int ID, Room room)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) -1);
      this.addBlock((object) 13);
      this.addBlock((object) 2);
      this.addBlock((object) 151);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) ID);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) room.kills);
      this.addBlock((object) 20);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 1200000);
      this.addBlock((object) -1036745);
      this.addBlock((object) 1200000);
      this.addBlock((object) "0.0000");
      this.addBlock((object) "0.0000");
      this.addBlock((object) "0.0000");
      this.addBlock((object) "0.0000");
      this.addBlock((object) "0.0000");
      this.addBlock((object) "0.0000");
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) "DV01");
    }
  }
}
