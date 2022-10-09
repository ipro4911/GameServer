// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.RoomPacketManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Room_Data;
using System.Collections.Generic;

namespace Game_Server.Managers
{
  internal class RoomPacketManager
  {
    private static Dictionary<int, RoomDataHandler> packets = new Dictionary<int, RoomDataHandler>();

    public static void setup()
    {
      RoomPacketManager.addPacket(1, (RoomDataHandler) new RoomHandler_RoomStart());
      RoomPacketManager.addPacket(51, (RoomDataHandler) new RoomHandler_MapChange());
      RoomPacketManager.addPacket(62, (RoomDataHandler) new RoomHandler_AutostartChange());
      RoomPacketManager.addPacket(53, (RoomDataHandler) new RoomHandler_KillLimitDeathmatchChange());
      RoomPacketManager.addPacket(55, (RoomDataHandler) new RoomHandler_KillLimitExplosiveChange());
      RoomPacketManager.addPacket(59, (RoomDataHandler) new RoomHandler_PingChange());
      RoomPacketManager.addPacket(50, (RoomDataHandler) new RoomHandler_ReadyState());
      RoomPacketManager.addPacket(157, (RoomDataHandler) new RoomHandler_Suicide());
      RoomPacketManager.addPacket(56, (RoomDataHandler) new RoomHandler_SwitchTeam());
      RoomPacketManager.addPacket(402, (RoomDataHandler) new RoomHandler_RoomReady());
      RoomPacketManager.addPacket(9, (RoomDataHandler) new RoomHandler_BackToRoom());
      RoomPacketManager.addPacket(150, (RoomDataHandler) new RoomHandler_Spawn());
      RoomPacketManager.addPacket(500, (RoomDataHandler) new RoomHandler_WorldDamage());
      RoomPacketManager.addPacket(58, (RoomDataHandler) new RoomHandler_UserLimit());
      RoomPacketManager.addPacket(155, (RoomDataHandler) new RoomHandler_WeaponSwitch());
      RoomPacketManager.addPacket(101, (RoomDataHandler) new RoomHandler_Heal());
      RoomPacketManager.addPacket(400, (RoomDataHandler) new RoomHandler_Place());
      RoomPacketManager.addPacket(401, (RoomDataHandler) new RoomHandler_PlacementUse());
      RoomPacketManager.addPacket(902, (RoomDataHandler) new RoomHandler_ZombieDropUse());
      RoomPacketManager.addPacket(159, (RoomDataHandler) new RoomHandler_ArtillerySupport());
      RoomPacketManager.addPacket(200, (RoomDataHandler) new RoomHandler_JoinVehicle());
      RoomPacketManager.addPacket(201, (RoomDataHandler) new RoomHandler_ChangeVehicleSeat());
      RoomPacketManager.addPacket(202, (RoomDataHandler) new RoomHandler_LeaveVehicle());
      RoomPacketManager.addPacket(102, (RoomDataHandler) new RoomHandler_RepairVehicle());
      RoomPacketManager.addPacket(166, (RoomDataHandler) new RoomHandler_TotalWarSpawnVehicle());
      RoomPacketManager.addPacket(61, (RoomDataHandler) new RoomHandler_VoteKick());
      RoomPacketManager.addPacket(156, (RoomDataHandler) new RoomHandler_Flag());
      RoomPacketManager.addPacket(165, (RoomDataHandler) new RoomHandler_Flag());
      RoomPacketManager.addPacket(103, (RoomDataHandler) new RoomHandler_Damage());
      RoomPacketManager.addPacket(104, (RoomDataHandler) new RoomHandler_DamageVehicle());
      RoomPacketManager.addPacket(900, (RoomDataHandler) new RoomHandler_ZombieExplode());
      RoomPacketManager.addPacket(105, (RoomDataHandler) new RoomHandler_AmmoRecharge());
      RoomPacketManager.addPacket(180, (RoomDataHandler) new RoomHandler_CaptureModeRequest());
      RoomPacketManager.addPacket(100, (RoomDataHandler)new RoomHandler_ZombieExplode());
      RoomPacketManager.addPacket(3, (RoomDataHandler)new RoomHandler_Damage());
      RoomPacketManager.addPacket(300, (RoomDataHandler)new RoomHandler_Damage());
      RoomPacketManager.addPacket(3, (RoomDataHandler)new RoomHandler_ZombieExplode());
            RoomPacketManager.addPacket(69, (RoomDataHandler)new RoomHandler_WorldDamage());
            RoomPacketManager.addPacket(3, (RoomDataHandler)new RoomHandler_ZombieDropUse());
            RoomPacketManager.addPacket(3000, (RoomDataHandler)new RoomHandler_WorldDamage());


            Log.WriteLine("Loaded " + (object) RoomPacketManager.packets.Count + " room data handlers");
    }

    public static RoomDataHandler ParsePacket(int subtype, object[] blocks)
    {
      if (!RoomPacketManager.packets.ContainsKey(subtype))
        return (RoomDataHandler) null;
      RoomDataHandler packet = RoomPacketManager.packets[subtype];
      packet.FillData(subtype, blocks);
      return packet;
    }

    private static void addPacket(int id, RoomDataHandler handler)
    {
      if (!RoomPacketManager.packets.ContainsKey(id))
        RoomPacketManager.packets.Add(id, handler);
      else
        Log.WriteError("Packet Manager already contains packetID: " + (object) id);
    }
  }
}
