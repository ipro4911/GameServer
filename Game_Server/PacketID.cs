// Decompiled with JetBrains decompiler
// Type: Game_Server.PacketID
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server
{
  internal enum PacketID : ushort
  {
    Disconnect = 24576, // 0x6000
    WelcomePacket = 24832, // 0x6100
    CharacterInfo = 25088, // 0x6200
    PingInformation = 25600, // 0x6400
    CouponOpen = 25605, // 0x6405
    CouponBuy = 25606, // 0x6406
    Clan = 26384, // 0x6710
    ClanRanking = 26464, // 0x6760
    SwitchChannel = 28673, // 0x7001
    UserList = 28928, // 0x7100
    RoomList = 29184, // 0x7200
    RoomInfoUpdate = 29201, // 0x7211
    CreateRoom = 29440, // 0x7300
    JoinRoom = 29456, // 0x7310
    RoomInviteOrQuickJoin = 29457, // 0x7311
    QuickJoinRoom = 29472, // 0x7320
    SpectateRoom = 29488, // 0x7330
    LeaveRoom = 29504, // 0x7340
    RoomKick = 29505, // 0x7341
    RoomInvite = 29520, // 0x7350
    Chat = 29696, // 0x7400
    RoomUDP = 29952,
    RoomVehicleUpdate = 29969, // 0x7511
    Itemequipment = 29970, // 0x7512
    CostumeEquip = 29971, // 0x7513
    RoomPlantData = 29984, // 0x7520
    RoomHackMission = 29985, // 0x7521
    RoomData = 30000, // 0x7530
    ScoreBoard = 30032, // 0x7550
    NewZombieStage = 30053, // 0x7565
    DinarItemBuy = 30208, // 0x7600
    CostumeBuy = 30209, // 0x7601
    DeleteItem = 30224, // 0x7610
    DeleteCostume = 30225, // 0x7611
    CarePackageOpen = 30272, // 0x7640
    CarePackageSendItem = 30273, // 0x7641
    CashItemBuy = 30720, // 0x7800
    Outbox = 30752, // 0x7820
    RankingList = 30816, // 0x7860
    ShopCoupon = 30992, // 0x7910
    DailyLoginEvent = 30993, // 0x7911
    GunSmith = 30995, // 0x7913
    ZombieMultiplayerUpdate = 31490, // 0x7B02
    ZombieSkillPointRequest = 31492, // 0x7B04
    Messenger = 32256, // 0x7E00
    AchievementSystem = 32257, // 0x7E01
    AntiCheat = 46723, // 0xB683
    Emblem = 30013,
  }
}
