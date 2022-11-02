// Decompiled with JetBrains decompiler
// Type: Game_Server.Room
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.GameModes;
using Game_Server.Managers;
using Game_Server.Room_Data;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game_Server
{
  internal class Room : IDisposable
  {
    public int kills = 2;
    public int status = 1;
    public int rounds = 3;
    public int ffakillpoints = 10;
    public int timeleft = 180000;
    public int[] flags = new int[32];
    public int SpawnLocation = -1;
    public int KillsNIULeft = 10;
    public int KillsDerbaranLeft = 10;
    public int timelimit = 4;
    public int RespawnVehicleCount = 120;
        private static Hashtable[] _Rooms = new Hashtable[5];
        public ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>();
    public ConcurrentDictionary<int, User> spectators = new ConcurrentDictionary<int, User>();
    public ConcurrentDictionary<int, Vehicle> Vehicles = new ConcurrentDictionary<int, Vehicle>();
    public Dictionary<int, Zombie> Zombies = new Dictionary<int, Zombie>();
    public int SleepTime = 15;
    public int ZombieID = 3;
    public int SiegeWarTime = -1;
    public HackPercentage HackPercentage = new HackPercentage();
    public int ConquestCountdown = 30;
    public int WinningTeam = -1;
        public int TimeAttackTime;
        public int RoundTimeLeft;
        private Hashtable Users = new Hashtable();
        public ConcurrentDictionary<int, Placement> Placements = new ConcurrentDictionary<int, Placement>();
    public int derbHeroUsr = -1;
    public int niuHeroUsr = -1;
    public Thread updateThread;
    public int id;
    public Channel ch;
    public int channel;
    public string name;
    public int enablepassword;
    public string password;
    public bool supermaster;
    public bool autostart;
    public bool userlimit;
    public int mapid;
    public int premiumonly;
    public int mode;
    public int type;
    public int levellimit;
    public int ping;
    public int zombiedifficulty;
    public int votekickOption;
    public bool sleep;
        public int RoundTimeSpent;
        public int timespent;
        public int emblemid;
    public bool cwcheck;
    public bool gameactive;
    public bool bombPlanted;
    public bool bombDefused;
    public int explosiveRounds;
    public int dmRounds;
    public int new_mode;
    public int new_mode_sub;
    public int highestkills;
    public int master;
    public int maxusers;
    public int waitExplosiveTime;
    public bool isNewRound;
    public int SpawnedZombieplayers;
    public bool EndGamefreeze;
    public bool firstingame;
    public bool firstspawn;
    public int DerbRounds;
    public int NIURounds;
    public int firstInGameTS;
    public MapData MapData;
    public int NIUExplosivePoints;
    public int DerbExplosivePoints;
    public bool firstblood;
    public Room.VoteKick voteKick;
    public int TotalWarDerb;
    public int TotalWarNIU;
    public bool SendFirstWave;
    public bool FirstWaveSent;
    public bool zombieRunning;
    public int ZombiePoints;
    public int SpawnedZombies;
    public int spawnedZombieToMap1;
    public int spawnedZombieToMap2;
    public int spawnedZombieToMap3;
    public int spawnedZombieToMap4;
    public int ZombieToMap1;
    public int ZombieToMap2;
    public int ZombieToMap3;
    public int ZombieToMap4;
    public int spawnedMadmans;
    public int spawnedManiacs;
    public int spawnedGrinders;
    public int spawnedGrounders;
    public int spawnedHeavys;
    public int spawnedGrowlers;
    public int spawnedLovers;
    public int spawnedHandgemans;
    public int spawnedChariots;
    public int spawnedCrushers;
    public int spawnedBusters;
    public int spawnedCrashers;
    public int spawnedEnvys;
    public int spawnedClaws;
    public int spawnedBombers;
    public int spawnedDefeders;
    public int spawnedBreakers;
    public int spawnedMadSoldiers;
    public int spawnedMadPrisoners;
    public int spawnedSuperHeavYs;
    public int spawnedLadYs;
    public int spawnedMidgets;
    public int KilledZombies;
    public int KillsBeforeDrop;
    public int DropID;
        public int RoomType;
    public int ZombieSpawnPlace;
    public bool PickuppedC4;
    public User SiegeWarC4User;
    public string Mission1;
    public string Mission2;
    public string Mission3;
    public bool runningCountdown;
    public int Lasttick;
    public Explosive explosive;
    public FreeForAll ffa;
    public DeathMatch deathmatch;
    public TotalWar totalwar;
    public ZombieMode zombie;
    public TimeAttack timeattack;
    public CaptureMode capturemode;
    public HeroMode heromode;
    public EscapeMode escapemode;
    public bool disposed;
    public int derbHeroKill;
    public int niuHeroKill;
        public int cDerbRounds;
        public int cNiuRounds;

        ~Room()
    {
      GC.Collect();
    }

        public int AliveUsers(int Side)
        {
            int num = 0;
            foreach (User Client in this.tempPlayers)
            {
                if (Client.Health > 0 && this.GetSide(Client) == Side && Client.ExplosiveAlive)
                    ++num;
            }
            return num;
        }

        public Room()
    {
      this.disposed = false;
      this.voteKick = new Room.VoteKick(this);
      this.updateThread = new Thread(new ThreadStart(this.update));
      this.updateThread.Start();
    }

    public void send(Packet p)
    {
      try
      {
        byte[] bytes = p.GetBytes();
        foreach (User user in (IEnumerable<User>) this.users.Values)
          user.sendBuffer(bytes);
        foreach (User user in (IEnumerable<User>) this.spectators.Values)
          user.sendBuffer(bytes);
      }
      catch
      {
      }
    }

       
        public bool RemoveUser(int slotId)
        {
            if (slotId >= 0 && users.ContainsKey(slotId))
            {
                User usr = GetUser(slotId);
                if (usr != null)
                {
                    int usrSide = GetSide(usr);
                    if (usr.currentVehicle != null)
                    {
                        usr.currentVehicle.Leave(usr);
                    }

                    if (usr.channel == 2 || usr.channel == 3)
                    {
                        if (usr.isHacking)
                        {
                            usr.isHacking = false;
                            send(new SP_RoomHackMission(usr.roomslot, (usr.hackingBase == 0 ? HackPercentage.BaseA : HackPercentage.BaseB), 3, usr.hackingBase));
                        }

                        if (usr.hasC4)
                        {
                            send(new SP_Unknown(29985, 0, 0, 1, 0, 0, 0, 0, 0)); // Remove C4 from the user 
                            usr.hasC4 = false;
                            PickuppedC4 = false;
                        }
                    }

                    if (channel == 3)
                    {
                        usr.rKills = usr.rDeaths = usr.rHeadShots = -1;
                    }
                    else
                    {
                        usr.kills += usr.rKills;
                        usr.deaths += usr.rDeaths;
                        usr.headshots += usr.rHeadShots;
                    }

                    usr.SaveStats(); // Runs the query for update the user row

                    int roomslot = usr.roomslot;

                    foreach (User u in users.Values)
                    {
                        if (u.lastKillUser == roomslot)
                        {
                            u.lastKillUser = -1;
                        }
                    }

                    usr.room = null;

                    usr.roomslot = -1;

                    users[slotId] = null;

                    User ur;

                    users.TryRemove(slotId, out ur);

                    if (slotId == master)
                    {
                        supermaster = false; /* Remove EXP Buff */
                        foreach (User vu in users.Values.OrderByDescending(r => r.premium).ThenByDescending(r => r.exp).ToArray())
                        {
                            if (master != vu.roomslot)
                            {
                                master = vu.roomslot;
                                break;
                            }
                        }
                    }

                    if (channel == 3 && users.Count >= 1)
                    {
                        send(new SP_ZombieChangeTarget(this, roomslot));
                    }
                 /*   if (channel == 3 && users.Count <= 2)
                    {
                        send(new SP_ZombieChangeTarget(this, roomslot));
                    }
                   */

                    send(new SP_LeaveRoom(usr, this, slotId, master)); // Send to the room about the user left

                    usr.send(new SP_LeaveRoom(usr, this, slotId, master));

                    usr.send(new SP_LobbyInfoUpdate(usr));

                    //usr.send(new SP_KillCount(SP_KillCount.ActionType.Hide));

                    if (status != 1 && users.Values.Where(u => u != null).Count() <= 1 && channel != 3)
                    {
                        EndGame();
                    }
                    else if (users.Values.Where(u => u != null).Count() <= 0)
                    {
                        disposed = true;
                        remove();
                    }

                    Managers.UserManager.UpdateUserlist(usr);

                    #region ClanWar

                    if (type == 1 && GetSideCount(usrSide) < 4 && cwcheck && gameactive && (DerbRounds >= 3 || NIURounds >= 3 || KillsDerbaranLeft <= kills - 5 || KillsNIULeft <= kills - 5))
                    {
                        cwcheck = false;
                        int vsClanSide = (usrSide == 1 ? 0 : 1);

                        Clan myClan = usr.clan;
                        Clan vsClan = GetClan(vsClanSide);

                        myClan.lose++;
                        DB.RunQuery("UPDATE clans SET lose=lose+1 WHERE id='" + vsClan.id + "'");
                        myClan.AddClanWar(vsClan.name, "0-0", false);

                        vsClan.win++;
                        vsClan.exp += 250;
                        DB.RunQuery("UPDATE clans SET win=win+1, exp=exp+250 WHERE id='" + vsClan.id + "'");
                        vsClan.AddClanWar(myClan.name, "0-0", true);

                        DB.RunQuery("INSERT INTO clans_clanwars (clanid1, clanid2, score, clanwon, timestamp) VALUES ('" + GetClan((int)Side.Derbaran).id + "', '" + GetClan((int)Side.NIU).id + "', '" + "0-0" + "', '" + vsClan.id + "', '" + Generic.timestamp + "')");
                    }
                    #endregion

                    ch.UpdateLobby(this);

                    return true;
                }
            }

            send(new SP_LeaveRoom(0, this, slotId, master)); // Send to the room about the user left

            return false;
        }
        /// <summary>
        /// Get side of a user
        /// </summary>
        /// <param name="usr">User</param>
        /// <returns></returns>

        public int GetSide(User usr)
    {
      return usr.spectating || usr.channel == 3 && (this.mode == 10 || this.mode == 11 || this.mode == 12) || usr.roomslot < this.maxusers / 2 ? 0 : 1;
    }

    public int GetSideCount(int side)
    {
      return this.users.Values.Where<User>((Func<User, bool>) (r => this.GetSide(r) == side)).Count<User>();
    }

    public List<User> tempPlayers
    {
      get
      {
        List<User> userList = new List<User>();
        userList.AddRange((IEnumerable<User>) this.users.Values);
        userList.AddRange((IEnumerable<User>) this.spectators.Values);
        return userList;
      }
    }

    public void EndGame()
    {

            if (EndGamefreeze || !gameactive) return;
            EndGamefreeze = true;
            gameactive = false;
            bombPlanted = false;
            bombDefused = false;
      this.timeleft = 180000;
      List<User> userList = new List<User>();
      userList.AddRange((IEnumerable<User>) this.users.Values);
      userList.AddRange((IEnumerable<User>) this.spectators.Values);
      int num1 = this.mode == 0 || this.mode == 7 || this.mode == 15 ? this.DerbRounds : this.KillsDerbaranLeft;
      int num2 = this.mode == 0 || this.mode == 7 || this.mode == 15 ? this.NIURounds : this.KillsNIULeft;
      int num3 = num1 > num2 ? 0 : (num1 < num2 ? 1 : -1);
      foreach (User user1 in userList)
      {
        int num4 = this.GetSide(user1) == 0 ? this.DerbExplosivePoints : this.NIUExplosivePoints;
        double[] numArray = new double[5]
        {
          0.0,
          0.2,
          0.3,
          0.5,
          0.75
        };
        bool flag1 = user1.HasItem("CC05");
        bool flag2 = user1.HasItem("CC72");
        bool flag3 = user1.HasItem("CC76");
        bool flag4 = user1.HasItem("CD02");
        bool flag5 = user1.HasItem("CD05");
        bool flag6 = user1.HasItem("CD01");
        bool flag7 = user1.HasItem("CD06");
        bool flag8 = user1.HasItem("CD03");
        bool flag9 = user1.HasItem("CD04");
        bool flag10 = user1.HasItem("CD07");
        bool flag11 = user1.HasItem("CE01");
        bool flag12 = user1.HasItem("CE02");
        double num5 = !this.supermaster || this.master != user1.roomslot ? 1.0 : 1.1;
        double num6 = (this.supermaster ? 1.05 : 1.0) + numArray[(int) user1.premium];
        if (flag1)
        {
          num6 += 0.25;
          num5 += 0.25;
        }
        if (flag2)
        {
          num6 += 0.3;
          num5 += 0.3;
        }
        if (flag3)
        {
          num6 += 0.4;
          num5 += 0.4;
        }
        if (flag4)
          num6 += 0.2;
        if (flag5)
          num6 += 0.25;
        if (flag6)
          num6 += 0.3;
        if (flag7)
          num6 += 0.35;
        if (flag8)
          num6 += 0.5;
        if (flag9)
          num6 += 0.6;
        if (flag10)
          num6 += 0.8;
        if (flag11)
          num5 += 0.2;
        if (flag12)
          num5 += 0.3;
        if (Configs.Server.Christmas.enabled)
        {
          num6 += Configs.Server.Christmas.ExpRate;
          num5 += Configs.Server.Christmas.DinarRate;
        }
        double num7 = num6 + Configs.Server.Experience.ExpRate;
        double num8 = num5 + Configs.Server.Experience.DinarRate;
        if (EXPEventManager.isRunning)
        {
          num7 += EXPEventManager.EXPRate;
          num8 += EXPEventManager.DinarRate;
        }
        if (this.mode == 0)
        {
          num7 += 2.0;
          num8 += 2.0;
        }
        else if (this.mode == 16 && (this.mapid == 90 || this.mapid == 91) && (this.deathmatch != null)) // && this.deathmatch.isGunGame))
        {
          //this.deathmatch.GunGameLeave(user1);
          ++user1.PlayedEventMap;
        }
        if (user1.PlayedEventMap >= 3)
        {
          switch (new Random().Next(1, 4))
          {
            case 1:
              Inventory.AddOutBoxItem(user1, "DJ27", (ushort) 3, (ushort) 1);
              break;
            case 2:
              Inventory.AddOutBoxItem(user1, "D202", (ushort) 3, (ushort) 1);
              break;
            case 3:
              Inventory.AddOutBoxItem(user1, "DA06", (ushort) 3, (ushort) 1);
              break;
            case 4:
              Inventory.AddOutBoxItem(user1, "D201", (ushort) 3, (ushort) 1);
              break;
          }
          user1.PlayedEventMap = 0;
        }
        if (user1.clan != null)
        {
          switch (user1.clan.GetRank())
          {
            case 2:
              num7 += 0.01;
              num8 += 0.01;
              break;
            case 3:
              num7 += 0.02;
              num8 += 0.02;
              break;
            case 4:
              num7 += 0.03;
              num8 += 0.03;
              break;
            case 5:
              num7 += 0.04;
              num8 += 0.04;
              break;
            case 6:
              num7 += 0.05;
              num8 += 0.05;
              break;
            case 7:
              num7 += 0.06;
              num8 += 0.06;
              break;
            case 8:
              num7 += 0.07;
              num8 += 0.07;
              break;
            case 9:
              num7 += 0.08;
              num8 += 0.08;
              break;
          }
        }
        int num9 = user1.rPoints + num4;
        if (this.channel != 3)
        {
          user1.ExpEarned = 1 + (int) Math.Round((double) num9 * num7);
          user1.DinarEarned = 50 + (int) Math.Round((double) num9 / 2.5 * num8);
        }
        else
        {
          user1.ExpEarned = 1 + (int) Math.Round(1.0 + (double) num9 / 2.5 * num7);
          user1.DinarEarned = 50 + (int) Math.Round((double) (num9 / 5) * num8);
        }
        if (user1.ExpEarned > Game_Server.Configs.Server.Experience.MaxExperience)
          user1.ExpEarned = Game_Server.Configs.Server.Experience.MaxExperience;
        if (user1.DinarEarned > Game_Server.Configs.Server.Experience.MaxDinars)
          user1.DinarEarned = Game_Server.Configs.Server.Experience.MaxDinars;
        int level1 = (int) user1.level;
        user1.exp += user1.ExpEarned;
        user1.dinar += user1.DinarEarned;
        int level2 = (int) user1.level;
        if (this.channel != 3)
        {
          user1.kills += user1.rKills;
          user1.deaths += user1.rDeaths;
          user1.headshots += user1.rHeadShots;
        }
        if (!user1.spectating && this.channel != 3)
        {
          if (this.mode != 1)
          {
            if (this.GetSide(user1) == num3)
              ++user1.wonMatchs;
            else if (num3 != -1)
              ++user1.lostMatchs;
          }
          else if (user1.rKills >= this.highestkills)
            ++user1.wonMatchs;
          else
            ++user1.lostMatchs;
        }
        if ((long) user1.exp >= (long) LevelCalculator.getExpForLevel(level1 + 1) && level1 < 101)
        {
          List<LevelUPItem> Items = new List<LevelUPItem>();
          int Dinar = Game_Server.Configs.Server.Player.LevelupDinar * level2;
          switch (level2)
          {
             case 2:
             Items.Add(new LevelUPItem("CI01", 3));
              break;
                        case 3:
                            Items.Add(new LevelUPItem("CI01", 3));
                            break;
                        case 4:
                            Items.Add(new LevelUPItem("DV01", 3));
                            break;
                        case 5:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 6:
                            Items.Add(new LevelUPItem("DS10", 3));
                            break;
                        case 7:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 8:
                            Items.Add(new LevelUPItem("CA01", 5));
                            break;
                        case 9:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 10:
                            Items.Add(new LevelUPItem("DF18", 5));
                            break;
                        case 11:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 12:
                            Items.Add(new LevelUPItem("DB10", 7));
                            break;
                        case 13:
                            Items.Add(new LevelUPItem("CC73", 3));
                            break;
                        case 14:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 15:
                            Items.Add(new LevelUPItem("DG01", 7));
                            break;
                        case 16:
                           Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 17:
                            Items.Add(new LevelUPItem("DS01", 7));
                            break;
                        case 18:
                            Items.Add(new LevelUPItem("DC04", 7));
                            break;
                        case 19:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 20:
                            Items.Add(new LevelUPItem("CA04", 7));
                            break;
                        case 21:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 22:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 23:
                            Items.Add(new LevelUPItem("DF36", 7));
                            break;
                        case 24:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 25:
                            Items.Add(new LevelUPItem("DS03", 10));
                            break;
                        case 26:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 27:
                            Items.Add(new LevelUPItem("BA1A", 10));
                            break;
                        case 28:
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 29:
                            Items.Add(new LevelUPItem("CC74", 7));
                            break;
                        case 30:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 31:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 32:
                            Items.Add(new LevelUPItem("CC05", 10));
                            break;
                        case 33:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 34:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 35:
                            Items.Add(new LevelUPItem("DA08", 10));
                            break;
                        case 36:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 37:
                            Items.Add(new LevelUPItem("DG28", 10));
                            break;
                        case 38:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 39:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 40:
                            Items.Add(new LevelUPItem("DJ05", 10));
                            break;
                        case 41:
                            Items.Add(new LevelUPItem("CA04", 10));
                            break;
                        case 42:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 43:
                            Items.Add(new LevelUPItem("CC72", 10));
                           
                            break;
                        case 44:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 45:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 46:
                            Items.Add(new LevelUPItem("DF18", 10));
                            break;
                        case 47:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 48:
                            Items.Add(new LevelUPItem("CC74", 7));
                            break;
                        case 49:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 50:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 51:
                            Items.Add(new LevelUPItem("DJ04", 10));
                            break;
                        case 52:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 53:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 54:
                            Items.Add(new LevelUPItem("DC80", 10));
                            break;
                        case 55:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 56:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 57:
                            Items.Add(new LevelUPItem("DG33", 10));
                            break;
                        case 58:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 59:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 60:
                            Items.Add(new LevelUPItem("CD07", 10));
                            break;
                        case 61:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 62:
                            Items.Add(new LevelUPItem("DF25", 10));
                            break;
                        case 63:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 64:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 65:
                            Items.Add(new LevelUPItem("BA44", 10));
                            break;
                        case 66:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 67:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 68:
                            Items.Add(new LevelUPItem("CI01", 10));
                            break;
                        case 69:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 70:
                            Items.Add(new LevelUPItem("CC75", 15));
                            break;
                        case 71:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 72:
                            Items.Add(new LevelUPItem("CH06", 15));
                            break;
                        case 73:
                            Items.Add(new LevelUPItem("CC76", 15));
                            break;
                        case 74:
                            Items.Add(new LevelUPItem("DB10", 15));
                            break;
                        case 75:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 76:
                            Items.Add(new LevelUPItem("CA04", 15));
                            break;
                        case 77:
                            Items.Add(new LevelUPItem("DF25", 10));
                            break;
                        case 78:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 79:
                            Items.Add(new LevelUPItem("CH05", 20));
                            break;
                        case 80:
                            Items.Add(new LevelUPItem("DJ04", 20));
                            break;
                        case 81:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 82:
                            Items.Add(new LevelUPItem("CH06", 20));
                            break;
                        case 83:
                            Items.Add(new LevelUPItem("CA04", 20));
                            break;
                        case 84:
                            Items.Add(new LevelUPItem("DG88", 20));
                            break;
                        case 85:
                            Items.Add(new LevelUPItem("CD06", 20));
                            break;
                        case 86:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 87:
                            Items.Add(new LevelUPItem("DC73", 30));
                            break;
                        case 88:
                            Items.Add(new LevelUPItem("CH05", 30));
                            break;
                        case 89:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 90:
                            Items.Add(new LevelUPItem("CD05", 30));
                            break;
                        case 91:
                            Items.Add(new LevelUPItem("CI01", 30));
                            break;
                        case 92:
                            Items.Add(new LevelUPItem("DB10", 30));
                            break;
                        case 93:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 94:
                            Items.Add(new LevelUPItem("CA04", 30));
                            break;
                        case 95:
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            Items.Add(new LevelUPItem("CZ81", 1));
                            break;
                        case 96:
                            Items.Add(new LevelUPItem("CH06", 30));
                            break;
                        case 97:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));                 
                            break;
                        case 98:
                            Items.Add(new LevelUPItem("CD06", 30));
                            break;
                        case 99:
                            Items.Add(new LevelUPItem("DF25", 30));
                            break;
                        case 100:
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            Items.Add(new LevelUPItem("CB09", 1));
                            break;
                        case 101:
                            Items.Add(new LevelUPItem("CD07", 60));
                            Items.Add(new LevelUPItem("DA85", -1));
                            break;
          }
          foreach (LevelUPItem levelUpItem in Items)
            Inventory.AddItem(user1, levelUpItem.Code, levelUpItem.Days);
          if (user1.clan != null)
          {
            switch (user1.clan.clanRank(user1))
            {
              case 2:
                user1.clan.MasterEXP = user1.exp.ToString();
                break;
              case 9:
                ClanPendingUsers pendingUser = user1.clan.getPendingUser(user1.userId);
                if (pendingUser != null)
                {
                  pendingUser.EXP = user1.clan.ToString();
                  break;
                }
                break;
              default:
                ClanUsers user2 = user1.clan.GetUser(user1.userId);
                if (user2 != null)
                {
                  user2.EXP = user1.exp.ToString();
                  break;
                }
                break;
            }
          }
          user1.dinar += 10000;
          user1.cash += 25000;
          user1.send((Packet) new SP_LevelUp(user1, Dinar, Items));
          DB.RunQuery("INSERT INTO levelups (userid, oldlevel, newlevel, premium, timestamp) VALUES ('" + (object) user1.userId + "', '" + (object) level1 + "', '" + (object) level2 + "', '" + (object) user1.premium + "', '" + (object) Game_Server.Generic.timestamp + "')");
          Log.WriteLine("[---- " + user1.nickname + " leveled up to " + (object) level2 + " ----]");
        }
       // Inventory.AddItem(user1, "CB08", 5000);
        user1.actualUserlistType = 0;
        user1.RefreshFriends();
        DB.RunQuery("UPDATE users SET dinar = '" + (object) user1.dinar + "', cash = '" + (object) user1.cash + "' WHERE id = '" + (object) user1.userId + "'");
        user1.send((Packet) new SP_LobbyInfoUpdate(user1));
        user1.AddDailyStats(this.channel != 3 ? user1.rKills : 0, user1.channel != 3 ? user1.rDeaths : 0, this.channel != 3 ? user1.rHeadShots : 0, user1.ExpEarned, user1.DinarEarned);
        user1.SaveStats();
        //if (mode == 1 || mode == 2 || mode == 3 || mode == 4 || mode == 5 || mode == 6 || mode == 7 || mode == 8 || mode == 9 || mode == 15 || mode == 16) ;
        user1.send((Packet) new SP_EndGame(user1));
        Inventory.AddItem(user1, "CB08", 5000);
            }
            this.NIUExplosivePoints = this.DerbExplosivePoints = 0;
      if (this.type == 1 && this.users.Count >= 8 && this.cwcheck && (this.DerbRounds >= 3 || this.NIURounds >= 3 || (this.KillsDerbaranLeft <= this.kills - 5 || this.KillsNIULeft <= this.kills - 5)))
      {
        this.cwcheck = false;
        int num4 = -1;
        int num5 = -1;
        DateTime.Now.ToString("dd/MM/yyyy");
        Clan clan1 = this.GetClan(0);
        Clan clan2 = this.GetClan(1);
        if (clan1 != null && clan2 != null)
        {
          if (num1 != num2)
            num4 = num1 > num2 ? 0 : 1;
          num5 = num1 > num2 ? clan1.id : clan2.id;
          for (int index = 0; index < 1; ++index)
          {
            Clan clan3 = index == 0 ? clan1 : clan2;
            Clan clan4 = index == 0 ? clan2 : clan1;
            if (num4 != -1)
            {
              if (num5 == clan3.id)
              {
                ++clan3.win;
                clan3.exp += 1000;
                DB.RunQuery("UPDATE clans SET win=win+1, exp=exp+1000 WHERE id='" + (object) clan3.id + "'");
              }
              else
              {
                ++clan3.lose;
                clan3.exp += 500;
                DB.RunQuery("UPDATE clans SET lose=lose+1, exp=exp+500 WHERE id='" + (object) clan3.id + "'");
              }
              clan3.AddClanWar(clan4.name, num1.ToString() + "-" + (object) num2, num5 == clan3.id);
            }
            else
            {
              clan3.exp += 250;
              DB.RunQuery("UPDATE clans SET exp=exp+250 WHERE id='" + (object) clan3.id + "'");
            }
          }
        }
        DB.RunQuery("INSERT INTO clans_clanwars (clanid1, clanid2, score, clanwon, timestamp) VALUES ('" + (object) clan1.id + "', '" + (object) clan2.id + "', '" + (object) num1 + "-" + (object) num2 + "', '" + (object) num5 + "', '" + (object) Game_Server.Generic.timestamp + "')");
      }
      this.send((Packet) new SP_ScoreBoard(this));
      this.send((Packet) new SP_Unknown((ushort) 30000, new object[15]
      {
        (object) 1,
        (object) 51,
        (object) this.master,
        (object) this.id,
        (object) 2,
        (object) 1,
        (object) 0,
        (object) this.mapid,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0
      }));
      this.highestkills = this.NIURounds = this.DerbRounds = this.KillsNIULeft = this.KillsDerbaranLeft = 0;
      this.status = 1;
      this.EndGamefreeze = false;
    }

    public int FreeRoomSlotBySide(int side)
    {
      lock (this)
      {
        for (int key = side == 1 ? this.maxusers / 2 : 0; key < (side == 1 ? this.maxusers : this.maxusers / 2); ++key)
        {
          if (!this.users.ContainsKey(key))
            return key;
        }
        return -1;
      }
    }

    public void ResetUserStats(User usr)
    {
      usr.isReady = usr.isSpawned = usr.isHacking = usr.hasC4 = usr.ExplosiveAlive = usr.RandomSupplyBoxSelected = false;
      usr.Health = 1000;
      usr.mapLoaded = false;
      usr.rKillSinceSpawn = 0;
      usr.rKills = usr.rDeaths = usr.rHeadShots = usr.rPoints = usr.rAssist = usr.weapon = usr.Class = usr.skillPoints = 0;
      usr.droppedAmmo = usr.droppedFlash = usr.droppedM14 = usr.droppedMedicBox = 0;
      usr.LastRepairTick = 0;
      usr.timeattackBoxChoose = -1;
      usr.LastHackTick = 0;
      usr.HPLossTick = 0;
      usr.classCode = "-1";
      usr.TotalWarPoint = 0;
      usr.currentVehicle = (Vehicle) null;
      usr.currentSeat = (VehicleSeat) null;
      usr.playing = false;
      usr.lastKillUser = -1;
      usr.DinarEarned = usr.ExpEarned = 0;
      
      if (this.mode != 16 || this.mapid != 90 && this.mapid != 91 || (!this.gameactive || this.deathmatch == null))// || !this.deathmatch.isGunGame))
        return;
      //this.deathmatch.GunGameJoin(usr);
    }

    public int SwitchSide(User usr)
    {
      if (this.status == 2 || usr == null || (usr.room == null || usr.room.id != this.id))
        return -1;
      int roomslot = usr.roomslot;
      int side = this.GetSide(usr);
      for (int key = side == 0 ? this.maxusers / 2 : 0; key < (side == 0 ? this.maxusers : this.maxusers / 2); ++key)
      {
        if (!this.users.ContainsKey(key))
        {
          if (usr.roomslot == this.master)
            this.master = key;
          return key;
        }
      }
      return -1;
    }

    private void SpawnVehicles()
    {
      if (this.MapData == null)
        return;
      int num = 0;
      this.Vehicles.Clear();
      if (this.MapData.vehicleString == null || !(this.MapData.vehicleString != string.Empty) || this.MapData == null)
        return;
      string vehicleString = this.MapData.vehicleString;
      char[] chArray = new char[1]{ ';' };
      foreach (string Code in vehicleString.Split(chArray))
      {
        VehicleManager vehicleInfoByCode = VehicleManager.GetVehicleInfoByCode(Code);
        if (vehicleInfoByCode != null)
        {
          Vehicle vehicle = new Vehicle(num, Code, vehicleInfoByCode.Name, vehicleInfoByCode.MaxHealth, vehicleInfoByCode.MaxHealth, vehicleInfoByCode.RespawnTime, vehicleInfoByCode.Seats, vehicleInfoByCode.isJoinable);
          this.Vehicles.TryAdd(num, vehicle);
          ++num;
        }
        else
          Log.WriteError("Could not find the vehicle with the code " + Code + "!");
      }
    }

    public void RespawnAllVehicles()
    {
      for (int ID = 0; ID < this.Vehicles.Count; ++ID)
        this.RespawnVehicle(ID);
    }

    public Vehicle GetVehicleByID(int ID)
    {
      if (this.Vehicles.ContainsKey(ID))
        return this.Vehicles[ID];
      return (Vehicle) null;
    }

    public bool HasChristmasMap
    {
      get
      {
        if (this.mapid != 91 && this.mapid != 92 && this.mapid != 93)
          return this.mapid == 94;
        return true;
      }
    }

    public void RespawnVehicle(int ID)
    {
      Vehicle vehicleById = this.GetVehicleByID(ID);
      if (vehicleById.RespawnTime == -1 || vehicleById == null || (vehicleById == null || vehicleById.Users.Count != 0))
        return;
      vehicleById.RespawnTick = 0;
      vehicleById.SpawnProtection = 5;
      vehicleById.Health = vehicleById.MaxHealth;
      vehicleById.LoadSeats(vehicleById.SeatString);
      vehicleById.ChangedCode = string.Empty;
      vehicleById.TimeWithoutOwner = 0;
      this.send((Packet) new SP_RoomRespawnVehicle(ID, this));
    }

        public int GetIncubatorVehicleId()
        {
            if (channel == 3 && mode == (int)RoomMode.Defence && mode == (int)RoomMode.Escape)
            {
                var vehicle = Vehicles.Values.Where(r => r.Code == "EN16");
                if (vehicle.Count() > 0)
                {
                    return vehicle.FirstOrDefault().ID;
                }
            }
            return -1;
        }

        public Zombie GetAvailableZombie()
        {
            foreach (Zombie z in Zombies.Values)
            {
                if (z.respawn < Generic.timestamp && z.Health == 0)
                {
                    return z;
                }
            }
            return null;
        }

        public void SpawnZombie(int Type)
    {
      lock (this)
      {
        if (this.Zombies.Where<KeyValuePair<int, Zombie>>((Func<KeyValuePair<int, Zombie>, bool>) (r => r.Value.Health > 0)).Count<KeyValuePair<int, Zombie>>() >= 20 || Type == 8 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 8)) >= 3 || (Type == 9 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 9)) >= 2 || Type == 10 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 10)) >= 2) || (Type == 11 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 11)) >= 1 || Type == 14 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 14)) >= 2 || (Type == 15 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 15)) >= 1 || Type == 16 && this.Zombies.Values.Count<Zombie>((Func<Zombie, bool>) (r => r.Type == 16)) >= 1)))
          return;
        Zombie availableZombie = this.GetAvailableZombie();
        if (availableZombie == null)
          return;
                if (this.mapid == 53)
                    this.ZombieSpawnPlace = new Random().Next(1, 8);
                    if (this.mapid == 95)
          this.ZombieSpawnPlace = new Random().Next(4, 8);
        else if (this.ZombieSpawnPlace >= 24)
          this.ZombieSpawnPlace = 0;
        if (Type == 14)
          this.ZombieSpawnPlace = 4;
        if (Type == 15)
          this.ZombieSpawnPlace = 5;
        if (this.mapid == 55 && Type == 16)
          this.ZombieSpawnPlace = 1;
        if (Type == 10 && this.spawnedBusters % 2 != 0)
          this.ZombieSpawnPlace = 25;
        if (Type == 10 && this.spawnedBusters % 2 == 0)
          this.ZombieSpawnPlace = 26;
        if (Type < 0 || Type > 22)
          return;
        int num = this.Zombies.Count < 32 % this.users.Count || this.users.Count <= 1 ? this.master : this.RandomTargetRoomSlot;
        availableZombie.FollowUser = num;
        availableZombie.timestamp = Game_Server.Generic.timestamp + 1;
        availableZombie.Type = Type;
        availableZombie.Reset();
        ++this.SpawnedZombies;
        ++this.ZombieSpawnPlace;
        this.send((Packet) new SP_ZombieSpawn(availableZombie.ID, availableZombie.FollowUser, this.ZombieSpawnPlace, Type, availableZombie.Health));
        switch (Type)
        {
          case 0:
            ++this.spawnedMadmans;
            ++this.spawnedZombieToMap1;
            break;
          case 1:
            ++this.spawnedManiacs;
            ++this.spawnedZombieToMap2;
            break;
          case 2:
            ++this.spawnedGrinders;
            break;
          case 3:
            ++this.spawnedGrounders;
            break;
          case 4:
            ++this.spawnedHeavys;
            break;
          case 5:
            ++this.spawnedGrowlers;
            break;
          case 6:
            ++this.spawnedLovers;
            ++this.spawnedZombieToMap3;
            break;
          case 7:
            ++this.spawnedHandgemans;
            ++this.spawnedZombieToMap4;
            break;
          case 8:
            ++this.spawnedChariots;
            break;
          case 9:
            ++this.spawnedCrushers;
            break;
          case 10:
            ++this.spawnedBusters;
            break;
          case 11:
            ++this.spawnedCrashers;
            break;
          case 12:
            ++this.spawnedEnvys;
            ++this.spawnedZombieToMap3;
            break;
          case 13:
            ++this.spawnedClaws;
            ++this.spawnedZombieToMap4;
            break;
          case 14:
            ++this.spawnedBombers;
            break;
          case 15:
            ++this.spawnedDefeders;
            break;
          case 16:
            ++this.spawnedBreakers;
            break;
          case 17:
            ++this.spawnedMadSoldiers;
            ++this.spawnedZombieToMap2;
            break;
          case 18:
            ++this.spawnedMadPrisoners;
            ++this.spawnedZombieToMap1;
            break;
          case 20:
            ++this.spawnedSuperHeavYs;
            break;
          case 21:
            ++this.spawnedLadYs;
            break;
          case 22:
            ++this.spawnedMidgets;
            break;
        }
      }
    }

    public Zombie GetZombieByID(int id)
    {
      if (this.Zombies.ContainsKey(id))
        return this.Zombies[id];
      return (Zombie) null;
    }

    public int RandomDrop()
    {
      Random random = new Random();
      int num1 = random.Next(1, 2);
      int num2 = random.Next(0, 400);
      if (num2 >= 300 && this.mode == 11)
        num1 = 3;
      else if (num2 >= 200)
        num1 = 2;
      else if (num2 >= 100)
        num1 = 1;
      else if (num2 >= 0 && this.mode == 10 && (this.zombie.Wave >= 6 && !this.zombie.respawnThisWave))
      {
        this.zombie.respawnThisWave = true;
        num1 = 0;
      }
      return num1;
    }

    public List<Zombie> ZombieFollowers(int SlotID)
    {
      return this.Zombies.Values.Where<Zombie>((Func<Zombie, bool>) (r =>
      {
        if (r != null)
          return r.FollowUser == SlotID;
        return false;
      })).ToList<Zombie>();
    }

        public static Room getRoom(int Channel, int ID)
        {
            try
            {
                if (Room._Rooms[Channel].ContainsKey(ID))
                    return (Room)Room._Rooms[Channel][ID];
                else
                    return (Room)null;
            }
            catch
            {
                return (Room)null;
            }
        }

        public int RandomTargetRoomSlot
    {
      get
      {
        for (int key = 0; key < this.maxusers; ++key)
        {
          if (this.users.ContainsKey(key) && key != this.master)
            return key;
        }
        return -1;
      }
    }

    public void CheckForMission(User usr, int VehicleID)
    {
      if (VehicleID == this.GetIncubatorVehicleId() && this.channel == 3 && this.mode == 11)
        this.EndGame();
      else if (this.mapid == 42)
      {
        switch (VehicleID)
        {
          case 7:
            usr.rPoints += 30;
            if (this.Mission2 == null)
              this.Mission2 = usr.nickname;
            this.flags[1] = 0;
            this.flags[3] = 1;
            this.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
            {
              (object) 1,
              (object) -1,
              (object) this.id,
              (object) 2,
              (object) 156,
              (object) 0,
              (object) 1,
              (object) 1,
              (object) 0,
              (object) -1,
              (object) 1,
              (object) 0,
              (object) 20,
              (object) 0,
              (object) 0,
              (object) 0,
              (object) 705882,
              (object) 637900,
              (object) 705882,
              (object) 0,
              (object) 5600.8521,
              (object) 287.8355,
              (object) 5443.2065,
              (object) 267.1544,
              (object) -90.9612,
              (object) -101.7575,
              (object) 0,
              (object) 0,
              (object) "DS05"
            }));
            this.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
            {
              (object) 1,
              (object) -1,
              (object) this.id,
              (object) 2,
              (object) 156,
              (object) 0,
              (object) 1,
              (object) 3,
              (object) 1,
              (object) -1,
              (object) -1,
              (object) 0,
              (object) 20,
              (object) 0,
              (object) 0,
              (object) 0,
              (object) 705882,
              (object) 637900,
              (object) 705882,
              (object) 0,
              (object) 5600.8521,
              (object) 287.8355,
              (object) 5443.2065,
              (object) 267.1544,
              (object) -90.9612,
              (object) -101.7575,
              (object) 0,
              (object) 0,
              (object) "DS05"
            }));
            break;
          case 8:
          case 9:
            usr.rPoints += 25;
            if (this.Mission1 == null)
              this.Mission1 = usr.nickname;
            this.flags[2] = 0;
            this.flags[1] = 1;
            this.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
            {
              (object) 1,
              (object) -1,
              (object) this.id,
              (object) 2,
              (object) 156,
              (object) 0,
              (object) 1,
              (object) 2,
              (object) 0,
              (object) 1,
              (object) 1,
              (object) 0,
              (object) 20,
              (object) 0,
              (object) 0,
              (object) 0,
              (object) 705882,
              (object) 637900,
              (object) 705882,
              (object) 0,
              (object) 5600.8521,
              (object) 287.8355,
              (object) 5443.2065,
              (object) 267.1544,
              (object) -90.9612,
              (object) -101.7575,
              (object) 0,
              (object) 0,
              (object) "DS05"
            }));
            this.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
            {
              (object) 1,
              (object) -1,
              (object) this.id,
              (object) 2,
              (object) 156,
              (object) 0,
              (object) 1,
              (object) 1,
              (object) 1,
              (object) -1,
              (object) -1,
              (object) 0,
              (object) 20,
              (object) 0,
              (object) 0,
              (object) 0,
              (object) 705882,
              (object) 637900,
              (object) 705882,
              (object) 0,
              (object) 5600.8521,
              (object) 287.8355,
              (object) 5443.2065,
              (object) 267.1544,
              (object) -90.9612,
              (object) -101.7575,
              (object) 0,
              (object) 0,
              (object) "DS05"
            }));
            break;
          case 23:
          case 24:
          case 25:
            usr.rPoints += 50;
            if (this.Mission3 != null)
              break;
            this.Mission3 = usr.nickname;
            break;
        }
      }
      else
      {
        if (this.mapid != 60 || VehicleID != 0)
          return;
        usr.rPoints += 50;
        if (this.Mission2 != null)
          return;
        this.Mission2 = usr.nickname;
      }
    }

    public void SiegeWar2Explosion()
    {
      try
      {
        this.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
        {
          (object) 0,
          (object) 0,
          (object) 1,
          (object) 4,
          (object) 0,
          (object) 100,
          (object) 0,
          (object) 0
        }));
        Vehicle vehicleById = this.GetVehicleByID(0);
        if (vehicleById == null)
          return;
        int num = int.Parse((Math.Truncate((double) (vehicleById.MaxHealth * 100)) / 100.0).ToString());
        vehicleById.Health -= num;
        this.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
        {
          (object) 1,
          (object) -1,
          (object) this.id,
          (object) 2,
          (object) 104,
          (object) 0,
          (object) 1,
          (object) 1,
          (object) 0,
          (object) 0,
          (object) 92,
          (object) 0,
          (object) 92,
          (object) -1,
          (object) 0,
          (object) 0,
          (object) vehicleById.Health,
          (object) vehicleById.Health,
          (object) (vehicleById.Health + num),
          (object) 0,
          (object) 2845.751,
          (object) 205.0797,
          (object) 3374.0964,
          (object) -70.9974,
          (object) 45.4165,
          (object) -287.9179,
          (object) 0,
          (object) 0,
          (object) "DP05"
        }));
        this.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
        {
          (object) 0,
          (object) -1,
          (object) 1,
          (object) 5,
          (object) -1,
          (object) 0,
          (object) -1,
          (object) 0
        }));
        if (vehicleById.Health <= 0)
        {
          if (this.Mission2 == null)
            this.Mission2 = this.SiegeWarC4User.nickname;
          this.SiegeWarC4User.rPoints += 50;
          this.EndGame();
          this.SiegeWarC4User = (User) null;
        }
        this.SiegeWarTime = -1;
      }
      catch
      {
      }
    }

    public int GetActualMission
    {
      get
      {
        int num = 0;
        if (this.mapid == 42 || this.mapid == 60)
        {
          if (this.Mission1 != null)
            ++num;
          if (this.Mission2 != null)
            ++num;
          if (this.Mission3 != null)
            ++num;
        }
        return num;
      }
    }

        public bool isSpawned;
        /*public bool Start()
        {
          if (this.gameactive || this.status == 2 || this.users.Count < 2 && this.channel != 3 && !Game_Server.Configs.Server.Debug)
            return false;
                foreach (User usr in users.Values)
          {
            if (!usr.isReady && usr.roomslot != this.master)
            {
              this.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> All Players must be ready for start the game!!", 999L, Game_Server.Configs.Server.SystemName));
              return false;
            }
            if (this.type == 1 && this.users.Count < 4 && !Game_Server.Configs.Server.Debug)
            {
              this.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> Not enough Players to start a clanwar (Min: 4)!!", 999L, Game_Server.Configs.Server.SystemName));
              return false;
            }
            if (this.type == 1 && this.rounds < 2)
            {
              string str = this.mode == 0 || this.mode == 7 ? "3+ rounds (Hero Mode) / 5+ rounds (Explosive)" : "100+ kills";
              this.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> Need at least " + str + " for start a clanwar!!", 999L, Game_Server.Configs.Server.SystemName));
              return false;
            }
            if(this.mode == 12)
            {
              this.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> [ENG] This Mode isn't finished yet, but we're working on it!!", 999L, Game_Server.Configs.Server.SystemName));
              return false;
            }
            if (!Game_Server.Configs.Server.Debug && this.channel != 3 && this.users.Count <= 1)
              return false;
          }

          this.Vehicles.Clear();
          this.MapData = MapDataManager.GetMapByID(this.mapid);
          this.SpawnLocation = 0;
          this.kills = this.highestkills = this.NIURounds = this.DerbRounds = this.KillsNIULeft = this.KillsDerbaranLeft = this.DerbExplosivePoints = this.NIUExplosivePoints = this.TotalWarDerb = this.TotalWarNIU = 0;
          this.bombPlanted = this.bombDefused = this.firstblood = this.isNewRound = this.EndGamefreeze = this.firstingame = this.firstspawn = this.sleep = false;
          this.HackPercentage.BaseA = this.HackPercentage.BaseB = 0;
          this.Mission1 = this.Mission2 = this.Mission3 = (string) null;
          this.SiegeWarC4User = (User) null;
          this.SiegeWarTime = -1;
          this.timespent = 0;
          this.Placements.Clear();
          this.gameactive = this.cwcheck = true;
          RoomMode mode = (RoomMode) this.mode;
          this.explosive = (Explosive) null;
          this.ffa = (FreeForAll) null;
          this.deathmatch = (DeathMatch) null;
          this.totalwar = (TotalWar) null;
          this.zombie = (ZombieMode) null;
          this.timeattack = (TimeAttack) null;
          this.capturemode = (CaptureMode) null;
          this.heromode = (HeroMode) null;
          switch (mode)
          {
            case RoomMode.Explosive:
            case RoomMode.Annihilation:
              this.explosive = new Explosive(this);
              break;
            case RoomMode.FFA:
              this.ffa = new FreeForAll(this);
              break;
            case RoomMode.FourVersusFour:
            case RoomMode.TDM:
            case RoomMode.Conquest:
            case RoomMode.BGExplosive:
            case RoomMode.SpecialMode:
              this.deathmatch = new DeathMatch(this);
              break;
            case RoomMode.HeroMode:
              this.heromode = new HeroMode(this);
              break;
            case RoomMode.TotalWar:
              this.totalwar = new TotalWar(this);
              break;
            case RoomMode.CaptureMode:
              this.capturemode = new CaptureMode(this);
              break;
            case RoomMode.Survival:
            case RoomMode.Defence:
              this.zombie = new ZombieMode(this);
              break;
            case RoomMode.TimeAttack:
              this.timeattack = new TimeAttack(this);
              break;
          }
          if (this.mode == 0 || this.mode == 7 || this.mode == 15)
          {
            switch (this.rounds)
            {
              case 0:
                this.explosiveRounds = 1;
                break;
              case 1:
                this.explosiveRounds = 3;
                break;
              case 2:
                this.explosiveRounds = 5;
                break;
              case 3:
                this.explosiveRounds = 7;
                break;
              case 4:
                this.explosiveRounds = 9;
                break;
            }
          }
          else if (this.mode == 1)
            this.ffakillpoints = 10 + 5 * this.rounds;
          else if (this.mode == 2 || this.mode == 3)
          {
            switch (this.rounds)
            {
              case -1:
              case 0:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 30;
                break;
              case 1:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 50;
                break;
              case 2:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 100;
                break;
              case 3:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 150;
                break;
              case 4:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 200;
                break;
              case 5:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 300;
                break;
              case 6:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 500;
                break;
              case 7:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 999;
                break;
            }
          }
          else if (this.mode == 16)
          {
            switch (this.rounds)
            {
              case -1:
              case 0:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 30;
                break;
              case 1:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 50;
                break;
              case 2:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 100;
                break;
              case 3:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 150;
                break;
              case 4:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 200;
                break;
              case 5:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 300;
                break;
            }
          }
          else if (this.mode == 4 || this.mode == 5)
          {
            switch (this.rounds)
            {
              case -1:
              case 0:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 100;
                break;
              case 1:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 200;
                break;
              case 2:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 300;
                break;
              case 3:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 500;
                break;
              case 4:
                this.kills = this.KillsDerbaranLeft = this.KillsNIULeft = 999;
                break;
            }
          }
          else if (this.mode == 8)
          {
            switch (this.rounds)
            {
              case -1:
              case 0:
                this.kills = 100;
                break;
              case 1:
                this.kills = 200;
                break;
              case 2:
                this.kills = 300;
                break;
              case 3:
                this.kills = 500;
                break;
              case 4:
                this.kills = 999;
                break;
            }
          }
          if (this.mode != 0 && this.mode != 7 && this.mode != 15)
          {
            switch (this.timelimit)
            {
              case 1:
                this.timeleft = 599000;
                break;
              case 2:
                this.timeleft = 1199000;
                break;
              case 3:
                this.timeleft = 1799000;
                break;
              case 4:
                this.timeleft = 2399000;
                break;
              case 5:
                this.timeleft = 2399000;
                break;
              case 6:
                this.timeleft = -1000;
                break;
            }
          }
          else
            this.timeleft = this.mode == 15 ? 90000 : (this.mode == 0 ? 180000 : 300000);
          if (this.mapid == 42 || this.mapid == 60)
          {
            if (this.mapid == 60)
            {
              this.KillsDerbaranLeft = this.KillsNIULeft = 400;
              this.timeleft = 600000;
            }
            else
            {
              this.KillsDerbaranLeft = this.KillsNIULeft = 300;
              this.timeleft = 1800000;
            }
          }
          else if (this.timeattack != null)
            this.timeleft = 720000;
          for (int index = 0; index < 32; ++index)
            this.flags[index] = -1;
          if (this.MapData != null)
          {
            this.flags[this.MapData.derb] = 0;
            this.flags[this.MapData.niu] = 1;
            this.SpawnVehicles();
          }
          if (this.mode == 16 && (this.mapid == 90 || this.mapid == 91) && (this.deathmatch != null)) //&& this.deathmatch.isGunGame))
            //this.deathmatch.InitializeGunGame();
          foreach (User usr in (IEnumerable<User>) this.users.Values)
          {
            this.ResetUserStats(usr);
            usr.ExplosiveAlive = true;
            usr.playing = true;
          }
          this.send((Packet) new SP_RoomInitializeUsers(this));
          this.firstInGameTS = Game_Server.Generic.timestamp + 15;
          return true;
        }*/
        public bool Start()
        {
            if (gameactive || status == 2 || users.Count < 2 && channel != 3)  //&& !Configs.Server.Debug) return false;
            foreach (User usr in users.Values)
            {
                if (usr.isReady == false && usr.roomslot != master)
                {
                    send(new Game.SP_Chat(usr, Game.SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> All Players must be ready for start the game!!", 999, Configs.Server.SystemName));
                    return false;
                }
                else if (type == 1 && users.Count < 4 && !Configs.Server.Debug)
                {
                    send(new Game.SP_Chat(usr, Game.SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> Not enough Players to start a clanwar (Min: 4)!!", 999, Configs.Server.SystemName));
                    return false;
                }
                else if (type == 1 && rounds < 2)
                {
                    string Need = ((mode == (int)RoomMode.Explosive || mode == (int)RoomMode.HeroMode) ? "3+ rounds (Hero Mode) / 5+ rounds (Explosive)" : "100+ kills");
                    send(new Game.SP_Chat(usr, Game.SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> Need at least " + Need + " for start a clanwar!!", 999, Configs.Server.SystemName));
                    return false;
                }

                if (Configs.Server.Debug1 == 0)
                {
                    if (this.channel != 3 && users.Count <= 1)
                    send(new Game.SP_Chat(usr, Game.SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> Cant Start Solo Games!!", 999, Configs.Server.SystemName));

                        return false;
                }
            }
            // Reset Room //
            isSpawned = false;
            Vehicles.Clear();
            MapData = Managers.MapDataManager.GetMapByID(mapid);
            SpawnLocation = 0;
            kills = highestkills = NIURounds = DerbRounds = KillsNIULeft = KillsDerbaranLeft = DerbExplosivePoints = NIUExplosivePoints = TotalWarDerb = TotalWarNIU = 0;
            bombPlanted = bombDefused = firstblood = isNewRound = EndGamefreeze = firstingame = firstspawn = sleep = false;
            HackPercentage.BaseA = HackPercentage.BaseB = 0;
            Mission1 = Mission2 = Mission3 = null;
            SiegeWarC4User = null;
            SiegeWarTime = -1;
            timespent = 0;
            Placements.Clear();

            gameactive = cwcheck = true;

            #region Modes

            RoomMode tmode = (RoomMode)this.mode;

            this.explosive = null;
            this.ffa = null;
            this.deathmatch = null;
            this.totalwar = null;
            this.zombie = null;
            this.timeattack = null;
            this.capturemode = null;
            this.heromode = null;
            this.escapemode = null;

            switch (tmode)
            {
                case RoomMode.Explosive:
                    {
                        this.explosive = new Explosive(this);
                        break;
                    }
                case RoomMode.Annihilation:
                    {
                        this.explosive = new Explosive(this);
                        break;
                    }
                case RoomMode.FFA:
                    {
                        this.ffa = new FreeForAll(this);
                        break;
                    }
                case RoomMode.FourVersusFour:
                case RoomMode.TDM:
                case RoomMode.Conquest:
                case RoomMode.BGExplosive:
                    {
                        this.deathmatch = new DeathMatch(this);
                        break;
                    }
                case RoomMode.TotalWar:
                    {
                        this.totalwar = new TotalWar(this);
                        break;
                    }
                case RoomMode.Survival:
                    {
                        this.zombie = new ZombieMode(this);
                        break;
                    }
                case RoomMode.Defence:
                    {
                        this.zombie = new ZombieMode(this);
                        break;
                    }
                case RoomMode.CaptureMode:
                    {
                        this.capturemode = new CaptureMode(this);
                        break;
                    }
                case RoomMode.TimeAttack:
                    {
                        this.timeattack = new TimeAttack(this);
                        break;
                    }
                case RoomMode.HeroMode:
                    {
                        this.heromode = new HeroMode(this);
                        break;
                    }
                case RoomMode.Escape: // WIP
                    {
                        this.escapemode = new EscapeMode(this);
                        break;
                    }
            }

            #endregion

            if (mode == (int)RoomMode.Explosive || mode == (int)RoomMode.HeroMode || mode == (int)RoomMode.Annihilation || mode == (int)RoomMode.Escape)
            {
                switch (rounds)
                {
                    case 0: explosiveRounds = 1; break;
                    case 1: explosiveRounds = 3; break;
                    case 2: explosiveRounds = 5; break;
                    case 3: explosiveRounds = 7; break;
                    case 4: explosiveRounds = 9; break;
                }
            }
            else if (mode == 1)
            {
                ffakillpoints = 10 + (5 * rounds);
            }
            else if (mode == 2 || mode == 3)
            {
                switch (rounds)
                {
                    case 0: kills = KillsDerbaranLeft = KillsNIULeft = 30; break;
                    case 1: kills = KillsDerbaranLeft = KillsNIULeft = 50; break;
                 //   case -1:
                    case 2: kills = KillsDerbaranLeft = KillsNIULeft = 100; break;
                    case 3: kills = KillsDerbaranLeft = KillsNIULeft = 150; break;
                    case 4: kills = KillsDerbaranLeft = KillsNIULeft = 200; break;
                    case 5: kills = KillsDerbaranLeft = KillsNIULeft = 300; break;
                    case 6: kills = KillsDerbaranLeft = KillsNIULeft = 500; break;
                    case 7: kills = KillsDerbaranLeft = KillsNIULeft = 999; break;
                }
            }
            else if (mode == 4 || mode == 5)
            {
                switch (rounds)
                {
                    case -1:
                    case 0: kills = KillsDerbaranLeft = KillsNIULeft = 100; break;
                    case 1: kills = KillsDerbaranLeft = KillsNIULeft = 200; break;
                    case 2: kills = KillsDerbaranLeft = KillsNIULeft = 300; break;
                    case 3: kills = KillsDerbaranLeft = KillsNIULeft = 500; break;
                    case 4: kills = KillsDerbaranLeft = KillsNIULeft = 999; break;
                }
            }
            else if (mode == 8)
            {
                switch (rounds)
                {
                    case -1:
                    case 0: kills = 100; break;
                    case 1: kills = 200; break;
                    case 2: kills = 300; break;
                    case 3: kills = 500; break;
                    case 4: kills = 999; break;
                }
            }

            if (mode != (int)RoomMode.Explosive && mode != (int)RoomMode.HeroMode && mode != (int)RoomMode.Annihilation)
            {
                switch (timelimit)
                {
                    case 1:
                        timeleft = 599000;
                        break;
                    case 2:
                        timeleft = 1199000;
                        break;
                    case 3:
                        timeleft = 1799000;
                        break;
                    case 4:
                        timeleft = 2399000;
                        break;
                    case 5:
                        timeleft = 2399000;
                        break;
                    case 6:
                        timeleft = -1000;
                        break;
                }
            }
            else
            {
                if (mode != (int)RoomMode.Annihilation)
                {
                    timeleft = mode == (int)RoomMode.Explosive ? 180000 : 300000; // Round Timer
                }
                else { timeleft = 90000; }
            }

            if (mapid == 42 || mapid == 56)
            {
                if (mapid == 56)
                {
                    KillsDerbaranLeft = KillsNIULeft = 400;
                    timeleft = 600000; // Round Timer
                }
                else
                {
                    KillsDerbaranLeft = KillsNIULeft = 300;
                    timeleft = 1800000; // Round Timer
                }
            }
            else if (timeattack != null)
            {
                timeleft = 1200000;
            }

            for (int I = 0; I < 32; I++)
            {
                flags[I] = (int)Side.Neutral;
            }

            if (MapData != null)
            {
                flags[MapData.derb] = (int)Side.Derbaran;
                flags[MapData.niu] = (int)Side.NIU;
                SpawnVehicles();
            }

           

            foreach (User usr in users.Values)
            {
                ResetUserStats(usr);
                usr.ExplosiveAlive = true;
                usr.playing = true;
            }

            send(new SP_RoomInitializeUsers(this));

            firstInGameTS = Generic.timestamp + 15;

            return true;
        }

        public int GetClanSide(User usr)
    {
      foreach (User usr1 in (IEnumerable<User>) this.users.Values)
      {
        if (usr1.clanId == usr.clanId)
          return this.GetSide(usr1);
      }
      return -1;
    }

    public bool JoinClanWar(User usr)
    {
      if (usr.clan == null)
        return false;
      switch (usr.clan.clanRank(usr))
      {
        case -1:
        case 9:
          return false;
        default:
          int clanSide = this.GetClanSide(usr);
          if (clanSide == -1 || this.GetSideCount(clanSide) >= this.maxusers / 2)
            return false;
          if (this.users.Count <= 0)
          {
            usr.room = this;
            usr.roomslot = 0;
            this.users.TryAdd(0, usr);
            this.master = 0;
            return true;
          }
          if (this.users.Count < this.maxusers)
          {
            usr.roomslot = this.FreeRoomSlotBySide(clanSide);
            if (usr.roomslot != -1)
            {
              usr.room = this;
              this.users.TryAdd(usr.roomslot, usr);
              usr.send((Packet) new SP_JoinRoom(usr, this));
              return true;
            }
          }
          return false;
      }
    }

    public bool JoinUser(User usr, int side = 2)
    {
      if (UserManager.ServerUsers.Values.Where<User>((Func<User, bool>) (r =>
      {
          usr.Emblem = 2;
          usr.emblemid = 2;
          if (r.room.id == this.id)
          return r.roomslot == this.master;
        return false;
      })) == null)
        this.remove();
      this.ResetUserStats(usr);
      if (this.voteKick.lockuser.IsLockedUser(usr))
      {
        usr.send((Packet) new SP_Chat("GM", SP_Chat.ChatType.Room_ToAll, "GM >> You have been kicked from the room, you must wait 5 minutes!", 999U, "GM"));
        return true;
      }
      if (this.type == 1 && this.JoinClanWar(usr))
        return true;
      if (this.users.Count <= 0)
      {
        usr.room = this;
        usr.roomslot = 0;
        this.users.TryAdd(0, usr);
        this.master = usr.roomslot;
        
        return true;
      }
      if (this.users.Count < this.maxusers)
      {
        usr.Health = -1;
        if (usr.channel != 3)
        {
          if (side == 0 || side == 1)
          {
            int side1 = side == 0 ? 1 : 0;
            if (this.GetSideCount(side) <= this.GetSideCount(side1))
            {
              usr.roomslot = this.FreeRoomSlotBySide(side);
           //   usr.emblemid = usr.medalid;
              if (usr.roomslot != -1)
              {
                if (this.gameactive)
                  usr.playing = true;
                usr.room = this;
                this.users.TryAdd(usr.roomslot, usr);
                usr.send((Packet) new SP_JoinRoom(usr, this));
                return true;
              }
            }
          }
          else
          {
            int key = this.FreeRoomSlotBySide(this.GetSideCount(1) >= this.GetSideCount(0) ? 0 : 1);
            if (key != -1)
            {
              if (this.gameactive)
                usr.playing = true;
              usr.roomslot = key;
              usr.room = this;
              this.users.TryAdd(key, usr);
              usr.send((Packet) new SP_JoinRoom(usr, this));
              return true;
            }
          }
        }
        else
        {
          for (int key = 0; key < 4; ++key)
          {
            if (!this.users.ContainsKey(key))
            {
              if (this.gameactive)
                usr.playing = true;
              usr.roomslot = key;
              usr.room = this;
              this.users.TryAdd(key, usr);
              usr.send((Packet) new SP_JoinRoom(usr, this));
              break;
            }
          }
          return true;
        }
      }
      return false;
    }

    public User GetUser(int SlotID)
    {
      if (this.users.ContainsKey(SlotID))
        return this.users[SlotID];
      return (User) null;
    }

    public int AddPlacement(User usr, string itemcode)
    {
      int num = this.Placements.Count + 1;
      Placement placement = new Placement(num, usr, itemcode);
      this.Placements.TryAdd(num, placement);
      return num;
    }

    public void RemovePlacement(int placementId)
    {
      if (!this.Placements.ContainsKey(placementId))
        return;
      Placement placement;
      this.Placements.TryRemove(placementId, out placement);
    }

    public Placement getPlacement(int placementId)
    {
      if (this.Placements.ContainsKey(placementId))
        return this.Placements[placementId];
      return (Placement) null;
    }

    public User getPlacementOwner(int placementId)
    {
      if (this.Placements.ContainsKey(placementId))
        return this.Placements[placementId].Planter;
      return (User) null;
    }

    public void updateTime()
    {
      try
      {
        if (!this.firstingame)
          return;
        int type = 0;
        if (this.mode == 4 || this.mode == 5 || this.mode == 8)
        {
          this.WinningTeam = -1;
          if (this.mode == 4)
          {
            int num1 = ((IEnumerable<int>) this.flags).Where<int>((Func<int, bool>) (f => f == 0)).Count<int>();
            int num2 = ((IEnumerable<int>) this.flags).Where<int>((Func<int, bool>) (f => f == 1)).Count<int>();
            if (num1 > num2)
              this.WinningTeam = 0;
            else if (num2 > num1)
              this.WinningTeam = 1;
          }
          else if (this.mode == 8)
            this.WinningTeam = this.flags[8];
          if (this.SiegeWarTime >= 0)
          {
            --this.SiegeWarTime;
            if (this.SiegeWarTime <= 0)
            {
              this.SiegeWar2Explosion();
              this.SiegeWarTime = -1;
            }
          }
          if (this.WinningTeam != -1)
          {
            type = 1;
            if (!this.runningCountdown)
            {
              this.runningCountdown = true;
              this.ConquestCountdown = this.mode == 4 ? 30 : 20;
            }
          }
          if (this.ConquestCountdown > 0)
          {
            --this.ConquestCountdown;
            if (this.ConquestCountdown <= 0)
            {
              this.runningCountdown = false;
              if (this.mode == 4)
              {
                switch (this.WinningTeam)
                {
                  case 0:
                    this.KillsNIULeft -= 10;
                    break;
                  case 1:
                    this.KillsDerbaranLeft -= 10;
                    break;
                }
              }
              else if (this.mode == 8)
              {
                switch (this.WinningTeam)
                {
                  case 0:
                    this.TotalWarDerb += 5;
                    break;
                  case 1:
                    this.TotalWarNIU += 5;
                    break;
                }
              }
              type = 2;
            }
          }
        }
        if (this.channel == 3)
        {
          if (this.zombieRunning && !this.sleep)
          {
            this.timespent += 1000;
            if (this.timeleft > 0 && this.firstingame)
              this.timeleft -= 1000;
          }
        }
        else
        {
          if (this.timeleft > 0 && !this.sleep && this.firstingame)
            this.timeleft -= 1000;
          this.timespent += 1000;
        }
        if (this.sleep)
          return;
        this.send((Packet) new SP_RoomThread(this, type));
      }
      catch (Exception ex)
      {
        Log.WriteError(ex.Message + " " + ex.StackTrace);
      }
    }

    public void DestroyVehicle(Vehicle Vehicle)
    {
      if (Vehicle == null)
        return;
      this.send((Packet) new SP_RoomVehicleExplode(this.id, Vehicle.ID, -1));
      Vehicle.Health = 0;
      Vehicle.ChangedCode = string.Empty;
      Vehicle.TimeWithoutOwner = 0;
    }

    public int SideCountDerb
    {
      get
      {
        return this.users.Values.Where<User>((Func<User, bool>) (u =>
        {
          if (u != null)
            return this.GetSide(u) == 0;
          return false;
        })).Count<User>();
      }
    }

    public int SideCountNIU
    {
      get
      {
        return this.users.Values.Where<User>((Func<User, bool>) (u =>
        {
          if (u != null)
            return this.GetSide(u) == 1;
          return false;
        })).Count<User>();
      }
    }

    public int AliveDerb
    {
      get
      {
        return this.users.Values.Where<User>((Func<User, bool>) (u =>
        {
          if (u.IsAlive() && this.GetSide(u) == 0)
            return u != null;
          return false;
        })).Count<User>();
      }
    }

    public int AliveNIU
    {
      get
      {
        return this.users.Values.Where<User>((Func<User, bool>) (u =>
        {
          if (u.IsAlive() && this.GetSide(u) == 1)
            return u != null;
          return false;
        })).Count<User>();
      }
    }

    public void update()
    {
      while (!this.disposed)
      {
        try
        {
          if (!this.EndGamefreeze)
          {
            if (this.gameactive)
            {
              if (this.Lasttick != DateTime.Now.Second)
              {
                this.Lasttick = DateTime.Now.Second;
                this.updateTime();
                this.users.Values.Where<User>((Func<User, bool>) (usr => usr.spawnprotection > 0)).ToList<User>().ForEach((Action<User>) (u => --u.spawnprotection));
                foreach (Vehicle Vehicle in (IEnumerable<Vehicle>) this.Vehicles.Values)
                {
                  if (Vehicle.SpawnProtection > 0)
                    --Vehicle.SpawnProtection;
                  if (Vehicle.Players.Count == 0 && Vehicle.ChangedCode != string.Empty)
                    ++Vehicle.TimeWithoutOwner;
                  if (Vehicle.ChangedCode != string.Empty && Vehicle.TimeWithoutOwner >= 120)
                    this.DestroyVehicle(Vehicle);
                  if (Vehicle.Health <= 0 && Vehicle.RespawnTime != -1)
                  {
                    ++Vehicle.RespawnTick;
                    int num = Vehicle.RespawnTime;
                    if (this.mapid == 67 || this.mapid == 68)
                      num = 20;
                    if (Vehicle.RespawnTick >= num)
                      this.RespawnVehicle(Vehicle.ID);
                  }
                }
                if (this.mode != 1 && this.channel != 3 && !Game_Server.Configs.Server.Debug && (this.SideCountDerb == 0 || this.SideCountNIU == 0))
                {
                  this.EndGame();
                  return;
                }
                if (this.voteKick.running)
                {
                  bool kicked = this.voteKick.GetPositiveVotes().Count >= this.GetSideCount(this.voteKick.voteSide) / 2 + 1;
                  if (Game_Server.Generic.timestamp >= this.voteKick.timestamp)
                    this.voteKick.StopVote(kicked);
                  else if (kicked)
                    this.voteKick.StopVote(true);
                }
                foreach (User user in (IEnumerable<User>) this.users.Values)
                {
                  if (user.Health < 300 && user.Health > 0 && user.ExplosiveAlive && user.isSpawned)
                  {
                    ++user.HPLossTick;
                    if (user.HPLossTick >= 10)
                    {
                      user.send((Packet) new SP_RoomInitializeUsers(this));
                      if (!user.HasItem("CH01"))
                      {
                        user.Health -= this.channel == 2 ? 25 : 5;
                        if (user.Health <= 0)
                        {
                          user.OnDie();
                          this.send((Packet) new SP_EntitySuicide(user.roomslot, SP_EntitySuicide.SuicideType.KilledByNotHavinHealTreatment, false));
                        }
                      }
                      user.HPLossTick = 0;
                    }
                  }
                }
                RoomMode mode = (RoomMode) this.mode;
                lock (this)
                {
                  switch (mode)
                  {
                    case RoomMode.Explosive:
                    case RoomMode.Annihilation:
                      this.explosive.Update();
                      break;
                    case RoomMode.FFA:
                      this.ffa.Update();
                      break;
                    case RoomMode.FourVersusFour:
                    case RoomMode.TDM:
                      //this.deathmatch.Update();
                    case RoomMode.Conquest:
                    case RoomMode.BGExplosive:
                    case RoomMode.SpecialMode:
                      this.deathmatch.Update();
                      break;
                    case RoomMode.HeroMode:
                      this.heromode.Update();
                      break;
                    case RoomMode.TotalWar:
                      this.totalwar.Update();
                      break;
                    case RoomMode.CaptureMode:
                      this.capturemode.Update();
                      break;
                    case RoomMode.Survival:
                    case RoomMode.Defence:
                      this.zombie.Update();
                      break;
                    case RoomMode.TimeAttack:
                      this.timeattack.Update();
                      break;
                    case RoomMode.Escape: // WIP
                    this.escapemode.Update();
                    break;
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          Log.WriteError("Error at update:" + ex.Message + " " + ex.StackTrace);
        }
        Thread.Sleep(1000);
      }
      Thread.Sleep(1000);
      this.remove();
    }

    public bool isPremMap(int Map)
    {
      return false;
    }

    public void remove()
    {
      if (this.users.Count > 0)
      {
        if (this.gameactive)
          this.EndGame();
        foreach (User user in (IEnumerable<User>) this.users.Values)
          user.send((Packet) new SP_RoomKick(user.roomslot));
      }
      byte[] bytes = new SP_RoomListUpdate(this, 2).GetBytes();
      foreach (User user in UserManager.GetUsersInChannel(this.channel, false))
      {
        if ((Decimal) user.lobbypage == Math.Floor((Decimal) (this.id / 15)))
          user.sendBuffer(bytes);
      }
      this.ch.RemoveRoom(this.id);
    }

    public Clan GetClan(int Side)
    {
      return this.users.Values.Where<User>((Func<User, bool>) (r =>
      {
        if (this.GetSide(r) == Side)
          return r.clan != null;
        return false;
      })).FirstOrDefault<User>()?.clan;
    }

    public bool isMyClan(User usr)
    {
      return this.users.Values.Where<User>((Func<User, bool>) (u => u.clanId == usr.clanId)).Count<User>() > 0;
    }

    public bool isZombieWeapon(string Weapon)
    {
      return Weapon == "DA50" || Weapon == "DA51" || (Weapon == "DA52" || Weapon == "DA53") || (Weapon == "DA54" || Weapon == "DA55" || (Weapon == "DA56" || Weapon == "DA57")) || (Weapon == "DA58" || Weapon == "DA59" || (Weapon == "DA60" || Weapon == "DA61") || (Weapon == "DA62" || Weapon == "DA63" || (Weapon == "DA64" || Weapon == "DA65"))) || (Weapon == "DA66" || Weapon == "DA67" || (Weapon == "DN51" || Weapon == "DN52") || (Weapon == "DN53" || Weapon == "DN54" || (Weapon == "DN55" || Weapon == "DN56")));
    }

        /*    public bool isGameEnding
            {
              get
              {
                return this.gameactive && (this.mode == 2 || this.mode == 3 || (this.mode == 4 || this.mode == 8) || this.mode == 5) && (this.KillsNIULeft <= 30 || this.KillsDerbaranLeft <= 30) || this.gameactive && this.mode == 1 && this.ffakillpoints <= 10;
              }
            }
        */
        /* public bool isGameEnding
         {
             get
             {
                 if (channel == 1)
                 {
                     if (gameactive && (mode == (int)RoomMode.Explosive || mode == 1 || mode == (int)RoomMode.HeroMode) &&
                        (DerbRounds >= explosiveRounds - 2 || NIURounds >= explosiveRounds - 2))
                         return true;
                 }
                 else
                 {
                     if (gameactive && (mode == 2 || mode == 3 || mode == 4 || mode == 8 || mode == 5) &&
                         (KillsNIULeft <= 30 || KillsNIULeft <= 30))
                         return true;
                 }


                 //if (gameactive && 
                 //    (mode == 2 || mode == 3 || mode == 4 || mode == 8 || mode == 5 || mode == (int)RoomMode.Explosive || mode == (int)RoomMode.HeroMode) && 
                 //    (KillsNIULeft <= 30 || KillsDerbaranLeft <= 30) || (channel == 1 && 
                 //    (mode == (int)RoomMode.Explosive || mode == (int)RoomMode.HeroMode) && 
                 //    (DerbRounds >= explosiveRounds - 2 || NIURounds >= explosiveRounds - 2)))
                 //{
                 //    // Less then 30 Kills Left
                 //    return true;
                 //}

                 if (gameactive && mode == 1 && ffakillpoints <= 10)
                 {
                     // Less then 10 Kills Left - FFA
                     return true;
                 }

                 return false;
             }
         }
        */
        public bool isGameEnding
        {
            get
            {
                return this.gameactive && (this.mode == 2 || this.mode == 3 || (this.mode == 4 || this.mode == 8) || this.mode == 5) && (this.KillsNIULeft <= 30 || this.KillsDerbaranLeft <= 30) || this.gameactive && this.mode == 1 && this.ffakillpoints <= 10;
            }
        }

        public bool isJoinable
        {
            get
            {
                return this == null || (this.channel != 3 || !this.gameactive) && (this.type != 1 || !this.gameactive) && (this.users.Count < this.maxusers && (!this.userlimit || this.gameactive)) && (!this.EndGamefreeze && !this.isGameEnding);
            }
        }
        public bool AddSpectator(User usr)
    {
      if (this.spectators.Count >= Game_Server.Configs.Server.MaxSpectators || this.spectators.ContainsKey(usr.userId))
        return false;
      int count = this.spectators.Count;
      usr.spectating = true;
      usr.room = this;
      usr.roomslot = 32 + count;
      usr.spectatorId = count;
      this.spectators.TryAdd(usr.userId, usr);
      return true;
    }

    public void RemoveSpectator(User usr)
    {
      if (!this.spectators.ContainsKey(usr.userId))
        return;
      this.send((Packet) new SP_PlayerInfoSpectate(usr, this));
      usr.send((Packet) new SP_Spectate());
      usr.lobbypage = 0;
      usr.send((Packet) new SP_RoomList(usr, 0, false, 0, 1));
      usr.room = (Room) null;
      usr.roomslot = 0;
      usr.spectating = false;
      User user = (User) null;
      this.spectators.TryRemove(usr.userId, out user);
    }

    protected virtual void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
      this.disposed = true;
    }

    public void InitializeTCP(User usr)
    {
      try
      {
        List<User> users = new List<User>();
        users.AddRange((IEnumerable<User>) this.users.Values);
        users.AddRange((IEnumerable<User>) this.spectators.Values);
        if (users.Contains(usr))
          users.Remove(usr);
        byte[] data = new SP_PlayerInfo(new List<User>()
        {
          usr
        }).GetBytes();
        users.ForEach((Action<User>) (u => u.sendBuffer(data)));
        usr.send((Packet) new SP_PlayerInfo(users));
      }
      catch (Exception ex)
      {
        Log.WriteDebug("Error at Initialize UDP:\r\n" + ex.Message + "\r\n" + ex.StackTrace);
      }
    }

    public void InitializeSpectatorUDP(User usr)
    {
      List<User> users = new List<User>(this.users.Values.Where<User>((Func<User, bool>) (r =>
      {
        if (r.IsConnectionAlive)
          return r != null;
        return false;
      })));
      usr.send((Packet) new SP_PlayerInfo(users));
      this.send((Packet) new SP_PlayerInfoSpectate(usr));
    }

    internal User GetPlayer(int TargetSlot)
    {
      if (this.users.ContainsKey(TargetSlot))
        return this.users[TargetSlot];
      return (User) null;
    }

    internal enum Side
    {
      Neutral = -1, // 0xFFFFFFFF
      Derbaran = 0,
      NIU = 1,
      Random = 2,
    }

    internal class LockUser
    {
      public List<Room.LockUser.LockedUser> LockedUsers = new List<Room.LockUser.LockedUser>();

      public void Lock(User usr)
      {
        if (this.LockedUsers.Where<Room.LockUser.LockedUser>((Func<Room.LockUser.LockedUser, bool>) (r => r.usr.userId == usr.userId)).FirstOrDefault<Room.LockUser.LockedUser>() != null)
          return;
        this.LockedUsers.Add(new Room.LockUser.LockedUser()
        {
          timestamp = Game_Server.Generic.timestamp + 300,
          usr = usr
        });
      }

      public bool IsLockedUser(User usr)
      {
        Room.LockUser.LockedUser lockedUser = this.LockedUsers.Where<Room.LockUser.LockedUser>((Func<Room.LockUser.LockedUser, bool>) (r => r.usr.userId == usr.userId)).FirstOrDefault<Room.LockUser.LockedUser>();
        if (lockedUser == null)
          return false;
        if (lockedUser.timestamp > Game_Server.Generic.timestamp)
          return true;
        this.LockedUsers.Remove(lockedUser);
        return false;
      }

      internal class LockedUser
      {
        public User usr;
        public int timestamp;
      }
    }

    internal class VoteKick
    {
      public List<Room.VoteKick.VoteKickVote> votes = new List<Room.VoteKick.VoteKickVote>();
      public int voteSide = -1;
      public int castedUser = -1;
      public bool running;
      public int timestamp;
      public int lastKickTimestamp;
      public Room r;
      public Room.LockUser lockuser;

      public VoteKick(Room r)
      {
        this.r = r;
        this.lockuser = new Room.LockUser();
      }

      public void StartVote(int tarGetUser, int side)
      {
        this.running = true;
        this.castedUser = tarGetUser;
        this.voteSide = side;
        this.timestamp = Game_Server.Generic.timestamp + 30;
      }

      public void StopVote(bool kicked)
      {
        this.KickUser(kicked);
        this.running = false;
        this.timestamp = 0;
        this.castedUser = -1;
        this.voteSide = -1;
        this.lastKickTimestamp = Game_Server.Generic.timestamp + 60;
        this.votes.Clear();
      }

      public void AddUserVotekick(User usr, bool kick)
      {
        this.votes.Add(new Room.VoteKick.VoteKickVote()
        {
          usr = usr,
          kick = kick
        });
      }

      public List<Room.VoteKick.VoteKickVote> GetPositiveVotes()
      {
        return this.votes.Where<Room.VoteKick.VoteKickVote>((Func<Room.VoteKick.VoteKickVote, bool>) (r => r.kick)).ToList<Room.VoteKick.VoteKickVote>();
      }

      public List<Room.VoteKick.VoteKickVote> GetNegativeVotes()
      {
        return this.votes.Where<Room.VoteKick.VoteKickVote>((Func<Room.VoteKick.VoteKickVote, bool>) (r => !r.kick)).ToList<Room.VoteKick.VoteKickVote>();
      }

      internal void KickUser(bool kicked)
      {
        User player = this.r.GetPlayer(this.castedUser);
        byte[] bytes = new SP_RoomData_VoteKick(this.castedUser, kicked, this.r.id).GetBytes();
        foreach (User usr in (IEnumerable<User>) this.r.users.Values)
        {
          if (this.r.GetSide(usr) == this.voteSide)
            usr.sendBuffer(bytes);
        }
        if (player == null || !kicked)
          return;
        this.r.voteKick.lockuser.Lock(player);
        this.r.RemoveUser(this.castedUser);
      }

      internal struct VoteKickVote
      {
        public User usr;
        public bool kick;
      }
    }
  }
}
