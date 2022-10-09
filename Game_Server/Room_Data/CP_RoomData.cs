// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.CP_RoomData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Managers;
using System;
using System.Collections.Generic;

namespace Game_Server.Room_Data
{
  internal class CP_RoomData : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (this.blocks.Length < 6 || usr.room == null)
        return;
      Room room = usr.room;
      int num1 = int.Parse(this.getBlock(0));
      int num2 = int.Parse(this.getBlock(1));
      if (num1 != usr.roomslot || num2 != room.id)
        return;
      Subtype subtype = (Subtype) int.Parse(this.getBlock(3));
      object[] blocks = new object[this.blocks.Length - 1];
      Array.Copy((Array) this.blocks, (Array) blocks, blocks.Length);
      using (RoomDataHandler packet = RoomPacketManager.ParsePacket((int) subtype, blocks))
      {
        if (packet == null)
          return;
        packet.Handle(usr, room);
        object[] sendBlocks = packet.sendBlocks;
        int num3 = int.Parse(sendBlocks[3].ToString());
        if (!packet.sendPacket)
          return;
        switch (num3)
        {
          case 9:
            usr.send((Packet) new SP_RoomData(sendBlocks));
            break;
          case 61:
            byte[] bytes = new SP_RoomData(sendBlocks).GetBytes();
            int side = room.GetSide(usr);
            using (IEnumerator<Game_Server.User> enumerator = room.users.Values.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Game_Server.User current = enumerator.Current;
                if (room.GetSide(current) == side)
                  current.sendBuffer(bytes);
              }
              break;
            }
          case 403:
            if (!room.firstingame)
            {
              room.RespawnAllVehicles();
              room.firstingame = true;
            }
            if (room.MapData != null)
            {
              usr.send((Packet) new SP_RoomMapData(room));
              usr.send((Packet) new SP_RoomInitializeUsers(room));
            }
            usr.mapLoaded = true;
            usr.send((Packet) new SP_RoomData(sendBlocks));
            break;
          default:
            room.send((Packet) new SP_RoomData(sendBlocks));
            break;
        }
        if (!packet.lobbychanges)
          return;
        room.ch.UpdateLobby(room);
      }
    }

    internal enum DropType
    {
      Respawn,
      Medic,
      Ammo,
      Repair,
    }
  }
}
