// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_ArtillerySupport
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_ArtillerySupport : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (room.channel != 2 || usr.Class != 2 || !usr.HasItem("DX01"))
        return;
      room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
      {
        (object) 1,
        (object) usr.roomslot,
        (object) room.id,
        (object) 2,
        (object) 159,
        (object) 0,
        (object) 1,
        (object) 0,
        (object) 1,
        (object) 1,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        this.sendBlocks[19],
        this.sendBlocks[20],
        this.sendBlocks[21],
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) "$"
      }));
      this.sendPacket = true;
    }
  }
}
