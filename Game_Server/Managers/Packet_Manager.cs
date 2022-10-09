// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.Packet_Manager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Room_Data;
using System;
using System.Collections.Generic;

namespace Game_Server.Managers
{
  internal class Packet_Manager
  {
    private static Dictionary<ushort, Handler> packets = new Dictionary<ushort, Handler>();

    public static void setup()
    {
      Packet_Manager.addPacket((ushort) 24832, (Handler) new CP_WelcomePacket());
      Packet_Manager.addPacket((ushort) 25088, (Handler) new CP_CharacterInfo());
      Packet_Manager.addPacket((ushort) 28673, (Handler) new CP_SwitchChannel());
      Packet_Manager.addPacket((ushort) 29184, (Handler) new CP_RoomList());
      Packet_Manager.addPacket((ushort) 29505, (Handler) new CP_RoomKick());
      Packet_Manager.addPacket((ushort) 29970, (Handler) new CP_ItemEquipment());
      Packet_Manager.addPacket((ushort) 29440, (Handler) new CP_CreateRoom());
      Packet_Manager.addPacket((ushort) 29696, (Handler) new CP_Chat());
      Packet_Manager.addPacket((ushort) 30208, (Handler) new CP_DinarItemBuy());
      Packet_Manager.addPacket((ushort) 30720, (Handler) new CP_CashItemBuy());
      Packet_Manager.addPacket((ushort) 30752, (Handler) new CP_Outbox());
      Packet_Manager.addPacket((ushort) 30000, (Handler) new CP_RoomData());
      Packet_Manager.addPacket((ushort) 29985, (Handler) new CP_RoomHackMission());
      Packet_Manager.addPacket((ushort) 29969, (Handler) new CP_RoomVehicleUpdate());
      Packet_Manager.addPacket((ushort) 30224, (Handler) new CP_DeleteItem());
      Packet_Manager.addPacket((ushort) 30225, (Handler) new CP_DeleteCostume());
      Packet_Manager.addPacket((ushort) 28928, (Handler) new CP_UserList());
      Packet_Manager.addPacket((ushort) 29456, (Handler) new CP_JoinRoom());
      Packet_Manager.addPacket((ushort) 30032, (Handler) new CP_ScoreBoard());
      Packet_Manager.addPacket((ushort) 29504, (Handler) new CP_LeaveRoom());
      Packet_Manager.addPacket((ushort) 29984, (Handler) new CP_RoomPlantData());
      Packet_Manager.addPacket((ushort) 31490, (Handler) new CP_ZombieMultiPlayer());
      Packet_Manager.addPacket((ushort) 31492, (Handler) new CP_ZombieSkillPointRequest());
      Packet_Manager.addPacket((ushort) 29472, (Handler) new CP_QuickJoinRoom());
      Packet_Manager.addPacket((ushort) 29201, (Handler) new CP_RoomInfoUpdate());
      Packet_Manager.addPacket((ushort) 29520, (Handler) new CP_RoomInvite());
      Packet_Manager.addPacket((ushort) 26464, (Handler) new CP_ClanRanking());
      Packet_Manager.addPacket((ushort) 25605, (Handler) new CP_CouponEvent());
      Packet_Manager.addPacket((ushort) 25606, (Handler) new CP_CouponBuy());
      Packet_Manager.addPacket((ushort) 30209, (Handler) new CP_CostumeBuy());
      Packet_Manager.addPacket((ushort) 29971, (Handler) new CP_CostumeEquip());
      Packet_Manager.addPacket((ushort) 32256, (Handler) new CP_Messenger());
      Packet_Manager.addPacket((ushort) 29488, (Handler) new CP_Spectate());
      Packet_Manager.addPacket((ushort) 25600, (Handler) new CP_PingInformation());
      Packet_Manager.addPacket((ushort) 26384, (Handler) new CP_Clan());
      Packet_Manager.addPacket((ushort) 30272, (Handler) new CP_CarePackage());
      Packet_Manager.addPacket((ushort) 30273, (Handler) new CP_CarePackageSendItem());
      Packet_Manager.addPacket((ushort) 46723, (Handler) new CP_AntiCheat());
      Packet_Manager.addPacket((ushort) 30992, (Handler) new CP_ShopCoupon());
      Packet_Manager.addPacket((ushort) 30053, (Handler) new CP_ZombieNewStage());
      Packet_Manager.addPacket((ushort) 30995, (Handler) new CP_GunSmith());
      Packet_Manager.addPacket((ushort) 24576, (Handler) new CP_Disconnect());
      Packet_Manager.addPacket((ushort) 30993, (Handler) new CP_LoginEvent());
      Packet_Manager.addPacket((ushort) 29457, (Handler) new CP_RoomInviteOrQuickJoin());
      Packet_Manager.addPacket((ushort) 30816, (Handler) new CP_RankingList());
      // NEW
     addPacket((ushort)30113, (Handler)new CP_Emblem());
     addPacket((ushort)32257, (Handler)new CP_AchievementSystem());
     Log.WriteLine("Loaded " + (object) Packet_Manager.packets.Count + " packet handlers");
    }

    public static Handler ParsePacket(string packetStr)
    {
      string[] strArray = packetStr.Split(' ');
      uint result1;
      uint.TryParse(strArray[0], out result1);
      ushort result2;
      ushort.TryParse(strArray[1], out result2);
      if (result1 > 0U && result2 > (ushort) 0)
      {
        if (Packet_Manager.packets.ContainsKey(result2))
        {
          string[] blocks = new string[strArray.Length - 2];
          Array.Copy((Array) strArray, 2, (Array) blocks, 0, strArray.Length - 2);
          Handler packet = Packet_Manager.packets[result2];
          packet.FillData(result1, (int) result2, blocks);
          return packet;
        }
        if (Game_Server.Configs.Server.Debug)
        {
          Log.WriteError("Unhandled Packet ID " + (object) result2);
          Log.WriteError("Packet -> " + packetStr);
        }
      }
      return (Handler) null;
    }

    private static void addPacket(ushort id, Handler handler)
    {
      if (!Packet_Manager.packets.ContainsKey(id))
        Packet_Manager.packets.Add(id, handler);
      else
        Log.WriteError("Packet Manager already contains packetID: " + (object) id);
    }
  }
}
