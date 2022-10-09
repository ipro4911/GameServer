// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.Subtype
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal enum Subtype
  {
    Start = 1,
    Explode = 3,
    ServerStart = 4,
    unkown2 = 300,
    ServerNewRound = 5,
    ServerPrepareNewRound = 6,
    NewRound = 7,
    BackToRoom = 9,
    VoteKickActive = 14, // 0x0000000E
    ReadyState = 50, // 0x00000032
    MapChange = 51, // 0x00000033
    ModeChange = 52, // 0x00000034
    KillLimitDeathmatchChange = 53, // 0x00000035
    TimeChange = 54, // 0x00000036
    KillLimitExplosiveChange = 55, // 0x00000037
    SwitchTeam = 56, // 0x00000038
    UserLimit = 58, // 0x0000003A
    PingChange = 59, // 0x0000003B
    VoteKick = 61, // 0x0000003D
    AutostartChange = 62, // 0x0000003E
    MagicSubtype1 = 100, // 0x00000064
    Heal = 101, // 0x00000065
    RepairVehicle = 102, // 0x00000066
    Damage = 103, // 0x00000067
    DamageVehicle = 104, // 0x00000068
    AmmoRecharge = 105, // 0x00000069
    Spawn = 150, // 0x00000096
    VehicleSpawn = 151, // 0x00000097
    ServerKill = 152, // 0x00000098
    VehicleKill = 153, // 0x00000099
    MagicSubtype2 = 154, // 0x0000009A
    WeaponSwitch = 155, // 0x0000009B
    Flag = 156, // 0x0000009C
    CaptureModeResponse = 157, // 0x0000009D
    Suicide = 157, // 0x0000009D
    ArtilleryRequest = 159, // 0x0000009F
    TotalWarFlag = 165, // 0x000000A5
    TotalWarSpawnVehicle = 166, // 0x000000A6
    CaptureModeRequest = 180, // 0x000000B4
    JoinVehicle = 200, // 0x000000C8
    ChangeVehicleSeat = 201, // 0x000000C9
    LeaveVehicle = 202, // 0x000000CA
    Place = 400, // 0x00000190
    PlaceUse = 401, // 0x00000191
    RoomReady = 402, // 0x00000192
    ServerRoomReady = 403, // 0x00000193
    WorldDamage = 500, // 0x000001F4
    DeathCam = 800, // 0x00000320
    ZombieExplode = 900, // 0x00000384
    ZombieDropUse = 902, // 0x00000386
    VehicleZombie = 30,
   UnkownZombie = 4,
   Repair = 3,
   Unkown = 2,
   Uknown2 = 1,
   unkown3 = 314,
   UnknownDie = 3,
   Uknown3 = 30, 
   Explodeonplayer = 3000,
   Respawn = 30,
   ZombieDeath = 300,
    }
}
