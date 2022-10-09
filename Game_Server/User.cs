// Decompiled with JetBrains decompiler
// Type: Game_Server.User
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Configs;
using Game_Server.Game;
using Game_Server.Managers;
using Game_Server.Networking;
using Game_Server.Room_Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static Game_Server.Game.RankingList;

namespace Game_Server
{
    internal class User : IDisposable
    {
        public int rank = 1;
        public LoginEventStatus rewardEvent = new LoginEventStatus();
        public double LastChatTick = -1.0;
        public double LastReadyTick = -1.0;
        public double LastStartTick = -1.0;
        public string[,] equipment = new string[5, 8];
        public string[] costumes_char = new string[5];
        public int clanId = -1;
        public int channel = -1;
        public int LastHackBase = -1;
        public Color chatColor = Color.Empty;
        public int Health = -1;
        public string classCode = "-1";
        public string retail = "";
        public int retailclass = -1;
        public List<string> expiredItems = new List<string>();
        public List<string> expiredCostumes = new List<string>();
        public ConcurrentDictionary<int, Messenger> Friends = new ConcurrentDictionary<int, Messenger>();
        public ConcurrentDictionary<int, OutboxItem> OutboxItems = new ConcurrentDictionary<int, OutboxItem>();
        private byte[] buffer = new byte[1024];
        public DateTime pingToServer = DateTime.Now;
        public List<TempItem> InBoxItems = new List<TempItem>();
        public int heartBeatTime = -1;
        public int lastKillUser = -1;
        public int userId;
        public uint sessionId;
        public ushort connectionId;
        public TCP_Client tcpClient;
        public string username;
        public string nickname;
        public string hwid;
        public bool AntiCheatCheck;
        public uint AntiCheatTick;
        public int exp;
        public int dinar;
        public int cash;
        public int kills;
        public int deaths;
        public byte premium;
        public uint premiumExpire;
        public int coupons;
        public int todaycoupons;
        public int coupontime;
        public int storageInventoryMax;
        public int medalid;
        public int ticketId;
        public int accesslevel;
        public string macAddress;
        public string country;
        public int emblemid;
        public int headshots;
        public uint mutedexpire;
        public int mutewarn;
        public int eventcount;
        public int firstlogin;
        public uint donationexpire;
        public bool dailystats;
        public ushort wonMatchs;
        public ushort lostMatchs;
        public bool shoxDetection;
        public bool GMMode;
        public bool AFK;
        public int LastDieTick;
        public int LastClientTick;
        public bool spectating;
        public bool RandomBoxToday;
        public Clan clan;
        public string[] inventory;
        public string[] storageInventory;
        public string[] costume;
        public int lobbypage;
        public int TotalWarSupport;
        public int TotalWarPoint;
        public VehicleSeat currentSeat;
        public Vehicle currentVehicle;
        public int LastHackTick;
        public int LastRepairTick;
        public int LastAmmoRechargeTick;
        public int LastSuicideTick;
        public int HPLossTick;
        public Room room;
        public int roomslot;
        public int spectatorId;
        public bool isReady;
        public bool isSpawned;
        public bool isHacking;
        public bool hasC4;
        public bool ExplosiveAlive;
        public bool RandomSupplyBoxSelected;
        public bool playing;
        public int rKills;
        public int rDeaths;
        public int rHeadShots;
        public int rPoints;
        public int rAssist;
        public int rFlags;
        public int weapon;
        public int Class;
        public int skillPoints;
        public int rKillSinceSpawn;
        public bool mapLoaded;
        public int timeattackBossDamage;
        public int timeattackDamagedDoor;
        public int hackTick;
        public int hackingBase;
        public int spawnprotection;
        public int hackPercentage;
        public int ExpEarned;
        public int DinarEarned;
        public int droppedAmmo;
        public int droppedFlash;
        public int droppedM14;
        public int droppedMedicBox;
        public int Plantings;
        public int PlayedEventMap;
        public int actualUserlistType;
        public string localIp;
        public string remoteIp;
        public Socket socket;
        public uint ping;
        public ushort throwNades;
        public ushort throwRockets;
        public string IP;
        public IPEndPoint remoteEndPoint;
        public IPEndPoint localEndPoint;
        public long RemoteIP;
        public uint RemotePort;
        public long LocalIP;
        public uint LocalPort;
        public bool disconnected;
        public int lastShoxTick;
        internal object lastP2SUpdate;
        public int MaxSlots = 200;
        public int timeAttackSpawns, timeattackBoxChoose = 0;

        ~User()
        {
            GC.Collect();
        }

        public byte level
        {
            get
            {
                return LevelCalculator.getLevelforExp(this.exp);
            }
        }

        public bool clanPending
        {
            get
            {
                if (this.clan == null)
                    return false;
                int num = this.clan.clanRank(this);
                if (num != 9)
                    return num == -1;
                return true;
            }
        }

        public bool hasRetail()
        {
            return this.retail != "null";
        }

        public bool hasRetail(string strCode)
        {
            return strCode.ToUpper().Equals(this.retail.ToUpper());
        }

        public void AddPremium(byte premiumId, ushort days)
        {
            if (premium == premiumId)
            {
                premiumExpire += (uint)(days * 86400);
            }
            else
            {
                premiumExpire = (uint)(Generic.timestamp + (days * 86400));
            }
            this.premium = premiumId;
            this.send((Packet)new SP_CustomPremium((int)this.premium));
            this.send((Packet)new SP_PingInformation(this));
            this.PremiumTimeLeft();
          //  this.PingTime = DateTime.Now;
            DB.RunQuery("UPDATE users SET premium='" + (object)premiumId + "', premiumExpire='" + (object)this.premiumExpire + "' WHERE id='" + (object)this.userId + "'");
            this.PingTime = DateTime.Now;
        }

        public void AddAdminCPLog(string Log)
        {
            Log = DB.Stripslash(Log);
            DB.RunQuery("INSERT INTO admincp_logs (adminid, log, date, timestamp) VALUES ('" + (object)this.userId + "', '" + Log + " [Server]', '" + Game_Server.Generic.currentDate + "', '" + (object)Game_Server.Generic.timestamp + "')");
        }

        public bool isCommand(string msg)
        {
            string[] args = msg.Split((char)0x20);

            if (args.Length >= 1)
            {
                string str = args[0];
                if (str.Length > 0)
                {
                    switch (args[0].Substring(1).ToLower())
                    {
                        case "ping":
                            {
                                User usr = UserManager.GetUser(args[1]);
                                if (usr != null)
                                {
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + usr.nickname + " has a ping of " + usr.ping + "ms to the server", 999, "NULL"));
                                }
                                return true;
                            }

                        case "setsize": // New command 08.10.2022
                            {
                                if (rank < 6) return false;

                                if (room == null) return true;
                                if (room.gameactive) return true;
                                int.TryParse(args[1], out room.maxusers);


                                room.send(new Game_Server.Room_Data.SP_RoomData(room.id, -1, 34, room.master, room.id, 2, 51, 0, room.maxusers, 0, 0, 0, 0, 0, 0, 0));

                                room.send(new SP_RoomInfoUpdate(room));
                                room.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> >> Room slots extended " + room.maxusers + "/32", 999, "NULL"));
                                return true;
                            }

                        case "notice":
                            {
                                if (rank < 3) return false;

                                msg = msg.Substring(7);

                                if (msg.Length > 0)
                                {
                                    AddAdminCPLog(nickname + " sent this notice to the server: " + msg);

                                    UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, msg, 999, "NULL"));
                                }

                                return true;
                            }
                        case "giveammo": // TODO check how this work and fix player not getting ammo 
                            {
                                if (rank < 6) return false;

                                User usr = UserManager.GetUser(args[1]);
                                if (usr != null)
                                {
                                    if (usr.LastAmmoRechargeTick > Generic.timestamp)
                                    {
                                        return false;
                                    }

                                    usr.LastAmmoRechargeTick = Generic.timestamp + 4;


                                    usr.throwNades = 0;
                                    usr.throwRockets = 0;
                                  //  send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + usr.nickname + " Replenished ammo of " + usr.weapon + "Weapon", 999 , "NULL"));
                                }
                                return true;
                            }
                        case "weed": // theese type of custom commands require certain address to work properly dont have time for building dsetup im in college
                            {
                                if (rank < 3) return false;

                                byte[] packet = (new SP_CustomWeed()).GetBytes();

                                AddAdminCPLog(nickname + " sent weed sound to the server");

                                foreach (User u in UserManager.ServerUsers.Values)
                                {
                                    u.sendBuffer(packet);
                                }

                                return true;
                            }
                        case "mlg":
                            {
                                if (rank < 3) return false;

                                byte[] packet = (new SP_CustomMLG()).GetBytes();

                                AddAdminCPLog(nickname + " sent MLG sound to the server");

                                foreach (User u in UserManager.ServerUsers.Values)
                                {
                                    u.sendBuffer(packet);
                                }

                                return true;
                            }
                        case "fps":
                            {
                                send(new SP_CustomShowFPS());
                                return true;
                            }
                        case "ban":
                            {
                                if (rank < 5) return false;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    if (rank > target.rank)
                                    {
                                        int bantime = -1;
                                        int hours = -1;

                                        int.TryParse(args[2], out hours);
                                        if (hours != 03)
                                        {
                                            string reason = "Unknown";
                                            if (args[3] != null)
                                            {
                                                reason = string.Empty;
                                                for (int I = 3; I < args.Length; I++)
                                                {
                                                    reason += args[I] + " ";
                                                }
                                                reason = reason.Substring(0, reason.Length - 1);
                                            }

                                            args[3] = reason;

                                            if (hours > 0)
                                            {
                                                DateTime current = DateTime.Now.AddHours(hours);
                                                bantime = int.Parse(String.Format("{0:yyMMddHH}", current));
                                            }

                                            AddAdminCPLog(nickname + " banned " + target.nickname + " for " + hours + " hours [Reason: " + reason + "]");
                                            DB.RunQuery("UPDATE users SET bantime = '" + bantime + "', banned = '1', banreason = '" + DB.Stripslash(reason) + "' WHERE id = '" + target.userId + "'");
                                            UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, target.nickname + " has been banned for " + reason + "!", 999, "NULL"));
                                            target.disconnect();
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "The user has a higher rank than your!", 999, "NULL"));
                                        return true;
                                    }
                                }
                                else
                                {
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User is not online or doesn't exist!!", 999, "NULL"));
                                }
                                return true;
                            }
                        case "messagebox":
                            {
                                if (rank < 3) return false;

                                string message = msg.Substring(11);

                                this.send(new SP_CustomMessageBox(message));

                                return true;
                            }
                        case "extendtime":
                            {
                                if (rank < 3) return false;

                                if (room != null && room.gameactive)
                                {
                                    int minutes = -1;
                                    int.TryParse(args[1], out minutes);
                                    if (minutes != -1)
                                    {
                                        room.timeleft += minutes * 60000;
                                        room.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> >> Round Time extended for " + minutes + " minutes!", 999, "NULL"));
                                    }
                                }

                                return true;
                            }
                        case "reload":
                            {
                                if (rank < 6) return false;

                                AddAdminCPLog(nickname + " reloaded classes!");
                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Successfully reloaded classes!", 999, "NULL"));
                                ItemManager.DecryptBinFile("items.bin");
                                ItemManager.LoadItems();
                                MapDataManager.Load();
                                VehicleManager.Load();
                                ZombieManager.Load();
                                CarePackage.Load();
                                WordFilterManager.Load();
                                GunSmithManager.Load();
                                RetailSystem.LoadRetails();
                                BanManager.Load();
                                ZombieManager.Load();
                                Packet_Manager.setup();
                                RoomPacketManager.setup();
                                Main.setup();
                                Configs.Server.LoadSub();

                                return true;
                            }
                        case "endgame":
                            {
                                if (rank < 3) return false;

                                if (room != null && room.gameactive)
                                {
                                    room.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Ending Game!", 999, "NULL"));
                                    room.EndGame();
                                }

                                return true;
                            }
                        case "uptime":
                            {
                                TimeSpan ExpireDate = DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime;
                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> Online since " + ExpireDate.Days + " days, " + ExpireDate.Hours + " hours, " + ExpireDate.Minutes + " minutes :)", sessionId, nickname));
                                return true;
                            }
                        /*case "online":
                            {
                                string[] strArray = str.Split(' ');
                                foreach (User virtualUser in UserManager.getAllUsers())
                                {
                                    if (virtualUser != null && (virtualUser.nickname.ToLower().Equals(strArray[1].ToLower()) || virtualUser.nickname.ToLower().Equals(strArray[1].ToLower())))
                                    {
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> Total Users " + virtualUser + "Online", sessionId, nickname));
                                        break;
                                    }
                                }
                                //User virtualUser in UserManager.getAllUsers())
                                // var u =  Cha();
                                return true;
                            }
                    */
                        case "gmmode":
                            {
                                if (rank < 4) return false;

                                GMMode = !GMMode;

                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Configs.Server.SystemName + " >> GM Mode turned " + (GMMode ? "on" : "off"), sessionId, nickname));
                                return true;
                            }
                        case "hwban":
                            {
                                if (rank < 6) return false;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    if (rank > target.rank)
                                    {
                                        if (SecurityManager.IsAllLettersOrDigits(nickname))
                                        {
                                            AddAdminCPLog(nickname + " banned " + target.nickname + " for Mac address [" + target.macAddress + "] and HWID [" + target.hwid + "]");
                                        }
                                        else
                                        {
                                            System.Console.Write("blocked string: " + nickname);
                                        }

                                        DB.RunQuery("INSERT INTO macs_ban (`mac`) VALUES ('" + target.macAddress + "')");
                                        DB.RunQuery("INSERT INTO hwid_bans (hwid) VALUES ('" + target.hwid + "')");
                                        DB.RunQuery("UPDATE users SET active='0', banned='1', bantime='-1', banreason='Banned from Network' WHERE id='" + target.userId + "'");
                                        BanManager.Load();
                                        UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, target.nickname + " has been banned from Montana Network", 999, "NULL"));
                                        target.disconnect();
                                        return true;
                                    }
                                    else
                                    {
                                        UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "The user has a higher rank than your!", 999, "NULL"));
                                        return true;
                                    }
                                }

                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User is not online or doesn't exist!!", 999, "NULL"));

                                return true;
                            }
                        case "givecoupon":
                            {
                                if (rank < 4) return false;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    int coupon = int.Parse(args[2]);
                                    int.TryParse(args[2], out coupon);
                                    if (coupon != -1)
                                    {
                                        target.coupons += coupon;

                                        AddAdminCPLog(nickname + " gaved " + coupon + " coupons to " + target.nickname);

                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Sucessfully gaved " + coupon.ToString("N0") + " coupons to " + target.nickname + "!", 999, "NULL"));
                                        target.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You received " + coupon.ToString("N0") + " coupons from" + nickname + "!", 999, "NULL"));

                                        DB.RunQuery("UPDATE users SET coupons='" + target.coupons + "' WHERE id='" + target.userId + "'");
                                        return true;
                                    }
                                }
                                else
                                {
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User is not online or doesn't exist!!", 999, "NULL"));
                                }

                                return true;
                            }
                        case "givecash":
                            {
                                if (rank < 4) return false;

                                if (args.Length < 3) return true;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    uint cash = 0;
                                    uint.TryParse(args[2], out cash);
                                    if (cash > 0)
                                    {
                                        target.cash += (int)cash;

                                        AddAdminCPLog(nickname + " gave " + cash + " cash to " + target.nickname);

                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Sucessfully gaved " + cash.ToString("N0") + " cash to " + target.nickname + "!", 999, "NULL"));
                                        target.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You received " + cash.ToString("N0") + " cash from " + nickname + "!", 999, "NULL"));

                                        DB.RunQuery("UPDATE users SET cash='" + target.cash + "' WHERE id='" + target.userId + "'");
                                        return true;
                                    }
                                }
                                else
                                {
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User is not online or doesn't exist!!", 999, "NULL"));
                                }

                                return true;
                            }
                        case "giveitem":
                            {
                                if (rank < 4) return false;

                                if (args.Length < 4) return true;

                                string itemcode = args[2].ToUpper();
                                int days = 0;
                                int.TryParse(args[3], out days);
                                if (days != 0)
                                {
                                    if (days == -1) days = 3600;
                                    Item Item = ItemManager.GetItem(itemcode);
                                    if (Item != null)
                                    {
                                        User target = UserManager.GetUser(args[1]);
                                        if (target != null)
                                        {
                                            string getDays = days.ToString();
                                            bool addedItem = PackageManager.AddItem(target, itemcode);
                                            if (!addedItem)
                                            {
                                                if (Item != null)
                                                {
                                                    if ((Item.accruable || Item.BuyType == 4) && HasItem(Item.Code))
                                                    {
                                                        if (Inventory.GetEAItem(target, itemcode) < Item.maxAccrueCount)
                                                        {
                                                            Inventory.IncreaseEAItem(this, Item.Code, days);
                                                        }
                                                        else
                                                        {
                                                            send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + target.nickname + " has reached max accruable count", 999, "NULL"));
                                                            return true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (itemcode.StartsWith("B"))
                                                        {
                                                            if (Inventory.GetFreeCostumeSlotCount(this) <= 0)
                                                            {
                                                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + target.nickname + " has no empty slot", 999, "NULL"));
                                                                return true;
                                                            }
                                                            else
                                                            {
                                                                Inventory.AddCostume(target, itemcode, days);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Inventory.GetFreeItemSlotCount(this) <= 0)
                                                            {
                                                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + target.nickname + " has no empty slot", 999, "NULL"));
                                                                return true;
                                                            }
                                                            else
                                                            {
                                                                Inventory.AddItem(target, itemcode, days);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (days == 3600 || addedItem) getDays = "One use / permanent";
                                            target.send(new SP_UpdateInventory(target, target.expiredItems));

                                            AddAdminCPLog(nickname + " gaved " + Item.Name + " item to " + target.nickname + " for " + getDays + " days");
                                            send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Sucessfully gave " + Item.Name + " for " + getDays + " days to " + target.nickname + "!", 999, "NULL"));
                                            target.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You received " + Item.Name + " for " + getDays + " days from " + nickname, 999, "NULL"));
                                            return true;
                                        }
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User is not online or doesn't exist!!", 999, "NULL"));
                                        return true;
                                    }
                                    else
                                    {
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> This item is not in our item list!!", 999, "NULL"));
                                        return true;
                                    }
                                }
                                return true;
                            }
                        case "giveroom":
                            {
                                if (rank < 4) return false;

                                if (room == null) return true;

                                if (args.Length < 4) return true;

                                string itemcode = args[2].ToUpper();
                                int days = 0;
                                int.TryParse(args[3], out days);
                                if (days != 0)
                                {
                                    if (days == -1) days = 3600;
                                    Item Item = ItemManager.GetItem(itemcode);
                                    if (Item != null)
                                    {
                                        string getDays = days.ToString();
                                        if (days == 3600) getDays = "One use / permanent";

                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Sucessfully gave " + Item.Name + " for " + getDays + " days to the room!", 999, "NULL"));
                                        byte[] packet = new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You received " + Item.Name + " for " + getDays + " days from " + nickname, 999, "NULL").GetBytes();

                                        foreach (User usr in room.users.Values)
                                        {
                                            if (itemcode.StartsWith("B"))
                                            {
                                                if (Inventory.GetFreeCostumeSlotCount(this) <= 0)
                                                {
                                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + usr.nickname + " has no empty slot", 999, "NULL"));
                                                    return true;
                                                }
                                                else
                                                {
                                                    Inventory.AddCostume(usr, itemcode, days);
                                                }
                                            }
                                            else
                                            {
                                                if (Inventory.GetFreeItemSlotCount(this) <= 0)
                                                {
                                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + usr.nickname + " has no empty slot", 999, "NULL"));
                                                    return true;
                                                }
                                                else
                                                {
                                                    Inventory.AddItem(usr, itemcode, days);
                                                }
                                            }
                                            usr.send(new SP_UpdateInventory(usr, usr.expiredItems));
                                            usr.sendBuffer(packet);
                                            AddAdminCPLog(nickname + " gaved " + Item.Name + " item to " + usr.nickname + " for " + getDays + " days");
                                        }
                                        return true;
                                    }
                                    else
                                    {
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> This item is not in our item list!!", 999, "NULL"));
                                        return true;
                                    }
                                }
                                return true;
                            }
                        case "flushevent":
                            {
                                if (rank < 5) return false;

                                string nickname = args[1];
                                User u = UserManager.GetUser(nickname);
                                if (u != null)
                                {
                                    int id = -1;
                                    int.TryParse(args[2], out id);
                                    if (id != -1)
                                    {
                                        DB.RunQuery("DELETE FROM users_events WHERE eventid='" + id + "' AND userid='" + u.userId + "'");
                                    }
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Flushed event id " + id + " for user " + u.nickname, 999, "NULL"));
                                }
                                return true;
                            }
                        case "maps":
                            {
                                if (rank < 4) return false;

                                foreach (MapData m in MapDataManager.datas.Values)
                                {
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> " + m.name + " - ID: " + m.mapId + " - Flags: " + m.flags, 999, "NULL"));
                                }

                                return true;
                            }
                        case "godmodeon":
                            {
                                if (rank < 5) return false;
                                this.Health = 99999999;

                                return true;
                            }
                        case "godmodeoff":
                            {
                                if (rank < 5) return false;
                                this.Health = 1000;

                                return true;
                            }
                        case "sethp":
                            {
                                if (rank < 5) return false;
                                try
                                {
                                    User target = UserManager.GetUser(args[1]);
                                    int hp = int.Parse(args[2]);
                                    target.Health = hp;

                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        
                            
                        case "suicide":
                            {
                                try
                                {
                                    User u = UserManager.GetUser(this.nickname);
                                    room.send(new SP_EntitySuicide(u.roomslot, SP_EntitySuicide.SuicideType.Suicide, true));
                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "spawn":
                            {
                                if (rank < 3) return false;
                                try
                                {
                                   //room.send(new RoomHandler_Spawn());
                                    User u = UserManager.GetUser(this.nickname);
                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "kill":
                            {
                                if (rank < 3) return false;
                                try
                                {
                                    User u = UserManager.GetUser(args[1]);
                                    room.send(new SP_EntitySuicide(u.roomslot, SP_EntitySuicide.SuicideType.Suicide, true));
                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "test1":
                            {
                                if (rank < 6) return false;
                                try
                                {
                                    User u = UserManager.GetUser(this.nickname);

                                    room.send(new SP_Unknown(30000, 1, u.roomslot, room.id, 2, 159, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, "$"));

                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "newround3":
                            {
                                if (rank < 6) return false;
                                try
                                {
                                    room.send(new SP_InitializeNewRound(room));

                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "newround4":
                            {
                                if (rank < 6) return false;
                                try
                                {
                                    this.Health = -1;
                                    this.isSpawned = false;

                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "newround5":
                            {
                                if (rank < 6) return false;
                                try
                                {
                                    this.Health = 1000;
                                    this.isSpawned = true;

                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "setweapon": 
                            {
                                if (rank < 5) return false;
                                try
                                {
                                    User target = UserManager.GetUser(args[2]);
                                    int wep = int.Parse(args[2]);
                                    target.weapon = wep;
                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "kiliul":
                            {
                                if (rank < 6) return false;
                                try
                                {
                                    User target = UserManager.GetUser(args[1]);
                                    target.Health = 0;

                                    return true;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }

                            }
                        case "map":
                            {
                                if (rank < 4) return false;
                                if (room == null) return true;
                                if (room.gameactive) return true;
                                int.TryParse(args[1], out room.mapid);

                                /* Not used in chapter 3 */

                                room.send(new Game_Server.Room_Data.SP_RoomData(room.id, -1, 51, room.master, room.id, 2, 51, 0, room.mapid, 0, 0, 0, 0, 0, 0, 0));

                                room.send(new SP_RoomInfoUpdate(room));
                                return true;
                            }
                        case "event":
                            {
                                if (rank < 4) return false;

                                try
                                {
                                    int Seconds = 0;
                                    int.TryParse(args[1], out Seconds);
                                    double EXP = 0;
                                    double.TryParse(args[2].Replace(".", ","), out EXP);
                                    double Dinar = 0;
                                    double.TryParse(args[3].Replace(".", ","), out Dinar);
                                    int minute = (Seconds * 60);
                                    if (Seconds == -1)
                                    {
                                        EXPEventManager.StopEvent();
                                    }
                                    else
                                    {
                                        EXPEventManager.EventType = 16; // 4 = EXP Event - 16 = Hot Time Event
                                        EXPEventManager.StartEvent(minute, EXP, Dinar);
                                        UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice2, "A EXP Event" + " has started Troopers", 999, "NULL"));
                                    }


                                    Log.WriteLine(nickname + " " + (Seconds == -1 ? "stopped" : "started") + " EXP/Dinar Event [EXP: x" + EXPEventManager.EXPRate + " / Dinar: x" + EXPEventManager.DinarRate + "] for " + args[1] + " minutes!");

                                    AddAdminCPLog(nickname + " " + (Seconds == -1 ? "stopped" : "started") + " EXP/Dinar event [EXP: x" + EXPEventManager.EXPRate + " / Dinar: x" + EXPEventManager.DinarRate + "] for " + args[1] + " minutes!");
                                }
                                catch (Exception e) { System.Console.WriteLine(e); }
                                return true;
                            }
                        case "rdis":
                            {
                                if (rank < 5) return false;
                                int roomToClose = -1;
                                int.TryParse(args[1], out roomToClose);
                                if (roomToClose != -1)
                                {
                                    Room target = ChannelManager.channels[channel].GetRoom(roomToClose);
                                    if (target != null)
                                    {
                                        foreach (User usr in target.users.Values)
                                        {
                                            usr.send(new SP_LeaveRoom(usr, target, usr.roomslot, target.master));
                                            usr.room = null;
                                        }

                                        AddAdminCPLog(nickname + " closed room ID: " + target.id);
                                        target.remove();
                                        return true;
                                    }
                                }
                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> This room doesn't exist!!", 999, "NULL"));
                                return true;
                            }
                        case "setcolor":
                            {
                                if (rank < 2) return false;

                                string color = args[1];

                                if (color != "")
                                {
                                    if (color.StartsWith("#"))
                                    {
                                        color = color.Substring(1);
                                    }
                                    DB.RunQuery("UPDATE users SET chat_color='" + color + "' WHERE id='" + userId + "'");
                                    this.chatColor = Generic.ConvertHexToRGB(color);
                                }
                                else
                                {
                                    DB.RunQuery("UPDATE users SET chat_color='' WHERE id='" + userId + "'");
                                    this.chatColor = Color.Empty;
                                }

                                return true;
                            }
                        case "setlevel":
                            {
                                if (rank < 5) return false;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    int lvl = -1;
                                    int.TryParse(args[2], out lvl);
                                    if (lvl != -1)
                                    {
                                        uint exp = LevelCalculator.getExpForLevel(lvl);
                                        DB.RunQuery("UPDATE users SET exp='" + exp + "' WHERE id='" + target.userId + "'");
                                        AddAdminCPLog(nickname + " set " + target.nickname + " to level " + (level == 0 ? 1 : int.Parse(args[2])));
                                        target.disconnect();
                                    }
                                    return true;
                                }

                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + args[1] + " is not online or doesn't exist!", 999, "NULL"));
                                return true;
                            }
                        case "userinfo":
                            {
                                try
                                {
                                    if (rank < 4) return false;

                                    User target = UserManager.GetUser(args[1]);

                                    if (target != null)
                                    {
                                        string UIP = target.IP;

                                        if (target.rank > 5 && target.userId == 1)
                                        {
                                            UIP = "202.58.48.240";
                                        }

                                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(target.premiumExpire);

                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Informations about " + target.nickname + "!", 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> ID: " + target.userId + " | Username: " + target.username, 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Weapon ID: " + target.weapon, 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Premium: " + target.premium + " | Premium expires: " + dt.ToString("HH:mm - dd/MM/yyyy"), 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Level: " + LevelCalculator.getLevelforExp(target.exp) + " | Cash: " + target.cash.ToString("N0") + " | Dinar: " + target.dinar.ToString("N0"), 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Room: " + (target.room != null ? target.room.id.ToString() : "N/A"), 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> HWID: " + target.hwid, 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> IP: " + UIP + " | Rank: " + target.rank, 999, "Server"));

                                        if (target.mutedexpire > Generic.timestamp)
                                        {
                                            DateTime dt3 = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(target.mutedexpire);
                                            send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Mute ends: " + dt3.ToString("HH:mm - dd/MM/yyyy"), 999, "Server"));
                                        }

                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Country: " + target.country, 999, "Server"));
                                        return true;
                                    }

                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + args[1] + " is not online or doesn't exist!", 999, "Server"));
                                    return true;
                                }
                                catch
                                {
                                    return true;
                                }
                            }
                        case "roominfo":
                            {
                                if (rank < 4) return false;
                                int roomId = -1;
                                int.TryParse(args[1], out roomId);
                                if (roomId != -1)
                                {
                                    Room TargetRoom = ChannelManager.channels[channel].GetRoom(roomId);
                                    if (TargetRoom != null)
                                    {
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Room ID: " + roomId, 999, "NULL"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Room Name: " + TargetRoom.name, 999, "NULL"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Room Status: " + (TargetRoom.status == 2 ? "Play" : "Wait"), 999, "NULL"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Password: " + TargetRoom.password, 999, "NULL"));
                                        if (TargetRoom.MapData != null)
                                        {
                                            send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Map: " + TargetRoom.MapData.name + " (" + TargetRoom.mapid + ")", 999, "NULL"));
                                        }
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Type: " + TargetRoom.type + " / Mode: " + TargetRoom.mode, 999, "NULL"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Players: " + TargetRoom.users.Count + "/" + TargetRoom.maxusers + ", Spectators " + TargetRoom.spectators.Count + "/" + Configs.Server.MaxSpectators, 999, "NULL"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Master: " + (TargetRoom.users[TargetRoom.master].nickname), 999, "NULL"));
                                    }
                                }
                                return true;
                            }
                        case "kick":
                            {
                                if (rank < 3) return false;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    if (rank >= target.rank)
                                    {
                                        AddAdminCPLog(nickname + " kicked " + target.nickname + " from the server");
                                        target.disconnect();
                                        return true;
                                    }
                                    else
                                    {
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You cant kick " + target.nickname + " becaus he has an higer rank!", 999, "Server"));
                                        return true;
                                    }
                                }
                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + args[1] + " is not online or doesn't exist!", 999, "Server"));
                                return true;
                            }
                        case "kickr":
                            {
                                if (rank < 4) return false;

                                User target = UserManager.GetUser(args[1]);
                                if (target != null)
                                {
                                    AddAdminCPLog(nickname + " kicked " + target.nickname + " from the room");
                                    if (target.room == null) return true;
                                    target.room.RemoveUser(target.roomslot);
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Kicked the user from the room", 999, "SYSTEM"));
                                    return true;
                                }
                                send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + args[1] + " is not online or doesn't exist!", 999, "Server"));
                                return true;
                            }
                        case "mute":
                            {
                                if (rank < 4) return false;

                                int minutes = 0;
                                int.TryParse(args[2], out minutes);

                                if (minutes != 0)
                                {
                                    User target = UserManager.GetUser(args[1]);
                                    if (target != null)
                                    {
                                        int Hours = (int)(Math.Ceiling((decimal)minutes / 60));
                                        if (Hours >= 72 && rank < 5)
                                        {
                                            send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You can't mute for more than 72 hours!!", 999, "Server"));
                                        }
                                        else
                                        {
                                            AddAdminCPLog(nickname + " muted " + target.nickname + " for " + minutes + " minutes!");
                                            send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + target.nickname + " has been muted for " + minutes + " minutes!", 999, "Server"));
                                            target.mutedexpire = (uint)(Generic.timestamp + (minutes * 60));
                                            target.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You have been muted from " + nickname + " for " + minutes + " minutes!", 999, "Server"));
                                            DB.RunQuery("UPDATE users SET muted='1', mutedExpire='" + target.mutedexpire + "' WHERE id='" + target.userId + "'");
                                        }
                                        return true;
                                    }
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + args[1] + " is not online or doesn't exist!", 999, "Server"));
                                    return true;
                                }
                                return true;
                            }
                        case "unmute":
                            {
                                try
                                {
                                    if (rank < 3) return false;

                                    User target = UserManager.GetUser(args[1]);
                                    if (target != null)
                                    {
                                        target.mutedexpire = 0;
                                        target.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> You have been unmuted from " + nickname + "!", 999, "Server"));
                                        send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + target.nickname + " have been unmuted!", 999, "Server"));
                                        DB.RunQuery("UPDATE users SET muted='0', mutedExpire='-1' WHERE id='" + target.userId + "'");
                                        return true;
                                    }
                                    send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> User " + args[1] + " is not online or doesn't exist!", 999, "Server"));
                                    return true;
                                }
                                catch
                                {
                                    return true;
                                }
                            }
                        case "stop":
                            {
                                try
                                {
                                    if (rank < 6) return false;
                                    AddAdminCPLog(nickname + " stopped the server!");

                                    UserManager.sendToServer(new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "Server is going to be restarted!", 999, "NULL"));
                                    UserManager.sendToServer(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Configs.Server.SystemName + " >> Server is going to be restarted, sorry!!!", 999, "Server"));

                                    System.Threading.Thread.Sleep(500);
                                    Program.shutDown();
                                    return true;
                                }
                                catch { return false; }
                            }
                        case "clean":
                            if (this.rank < 6)
                                return true;
                            this.room.send((Packet)new SP_Chat("SYSTEM", SP_Chat.ChatType.Room_ToAll, "SYSTEM >> Room Removed!", 999, "NULL"));
                            this.room.remove();
                            return true;
                        case "lol":
                            {
                                try
                                {

                                    return true;
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                            }
                    }
                }
            }
            return false;
        }

        public long PremiumTimeLeft()
        {
            if (premiumExpire > Generic.timestamp)
            {
                return (uint)(premiumExpire - Generic.timestamp);
            }
            else if (premium > 0)
            {
                DB.RunQuery("UPDATE users SET premium='" + premium + "', premiumExpire='-1' WHERE id='" + userId + "'");
                premium = 0;
                return -1;
            }
            return -1;
        }

        public void SwitchWeapon(string weapon)
        {
            Item obj = ItemManager.GetItem(weapon);
            if (obj == null || obj.ID < 0 || obj.ID == this.weapon)
                return;
            int useableSlot = obj.GetUseableSlot();
            if (useableSlot < 0)
                return;
            this.weapon = obj.ID;
            this.room.send((Packet)new SP_Unknown((ushort)30000, new object[15]
            {
        (object) 1,
        (object) this.roomslot,
        (object) this.room.id,
        (object) 2,
        (object) 155,
        (object) 0,
        (object) 0,
        (object) obj.ID,
        (object) useableSlot,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0,
        (object) 0
            }));
        }

        public void LoadOutboxItems()
        {
            this.OutboxItems.Clear();
            DataTable dataTable = DB.RunReader("SELECT * FROM outbox WHERE ownerid='" + this.userId.ToString() + "' ORDER BY timestamp DESC");
            for (int index = 0; index < dataTable.Rows.Count; ++index)
            {
                DataRow row = dataTable.Rows[index];
                if (row != null)
                {
                    try
                    {
                        int num = int.Parse(row["id"].ToString());
                        string itemcode = row["itemcode"].ToString();
                        ushort days = 3600;
                        if (row["days"].ToString() != "-1")
                            days = ushort.Parse(row["days"].ToString());
                        int timestamp = int.Parse(row["timestamp"].ToString());
                        ushort count = ushort.Parse(row["count"].ToString());
                        OutboxItem outboxItem = new OutboxItem(num, itemcode, days, timestamp, count);
                        this.OutboxItems.TryAdd(num, outboxItem);
                    }
                    catch
                    {
                    }
                }
            }
            this.send((Packet)new SP_Outbox(this));
        }

        public void LoadInboxItems()
        {
            List<int> source = new List<int>();
            DataTable dataTable = DB.RunReader("SELECT * FROM inbox WHERE ownerid='" + (object)this.userId + "'");
            for (int index = 0; index < dataTable.Rows.Count; ++index)
            {
                DataRow row = dataTable.Rows[index];
                int num = int.Parse(row["id"].ToString());
                try
                {
                    string str = row["itemcode"].ToString();
                    ushort days = ushort.Parse(row["days"].ToString());
                    if (ItemManager.GetItem(str) != null)
                    {
                        if (str.StartsWith("B"))
                        {
                            if (Inventory.GetFreeCostumeSlotCount(this) > 0)
                            {
                                if (Inventory.AddCostume(this, str, (int)days))
                                {
                                    this.InBoxItems.Add(new TempItem(str, days));
                                    source.Add(num);
                                }
                            }
                        }
                        else if (Inventory.GetFreeItemSlotCount(this) > 0)
                        {
                            if (!PackageManager.AddItem(this, str))
                                Inventory.AddItem(this, str, (int)days);
                            source.Add(num);
                            this.InBoxItems.Add(new TempItem(str, days));
                        }
                    }
                }
                catch
                {
                }
            }
            if (source.Count <= 0)
                return;
            DB.RunQuery("DELETE FROM inbox WHERE id IN (" + string.Join(",", source.Select<int, string>((Func<int, string>)(x => x.ToString())).ToArray<string>()) + ")");
        }

        public void LoadFriends()
        {
            this.Friends.Clear();
            DataTable dataTable1 = DB.RunReader("SELECT * FROM friends WHERE id1='" + (object)this.userId + "' OR id2='" + (object)this.userId + "'");
            for (int index = 0; index < dataTable1.Rows.Count; ++index)
            {
                try
                {
                    DataRow row = dataTable1.Rows[index];
                    int num = int.Parse((row["id1"].ToString() == this.userId.ToString() ? row["id2"] : row["id1"]).ToString());
                    string str = "UnknownUser";
                    DataTable dataTable2 = DB.RunReader("SELECT * FROM users WHERE id='" + (object)num + "'");
                    if (dataTable2.Rows.Count > 0)
                        str = dataTable2.Rows[0]["nickname"].ToString();
                    this.Friends.TryAdd(num, new Messenger(num, str.ToString(), int.Parse(row["status"].ToString()), int.Parse(row["requesterid"].ToString()))
                    {
                        isOnline = false
                    });
                }
                catch
                {
                }
            }
            UserManager.SetOnlineToFriends(this, true);
        }

        public void CheckForFirstLogin()
        {
            this.dinar = 60000;
            this.send((Packet)new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> " + this.nickname + ", you've received starter bonusses! Have fun!", this.sessionId, this.nickname));
            DB.RunQuery("UPDATE users SET firstlogin='1' WHERE id='" + (object)this.userId + "'");
        }

        public string AvailableSlots
        {
            get
            {
                string[] strArray = "F,F,F,F".Split(',');
                if (this.HasItem("CA01") || this.premium >= (byte)3)
                    strArray[0] = "T";
                if (this.HasItem("DS05") || this.HasItem("DU04") || (this.HasItem("DS10") || this.HasItem("DV01")) || (this.HasItem("DS01") || this.HasItem("DU05") || (this.HasItem("DU01") || this.HasItem("DU02"))) || this.HasItem("DS03"))
                    strArray[1] = "T";
                if (this.HasItem("CA03"))
                    strArray[2] = "T";
                if (this.HasItem("CA04"))
                    strArray[3] = "T";
                return string.Join(",", strArray);
            }
        }

        public void KillEventCheck()
        {
            if (this.room == null || this.room.channel == 3 || this.eventcount >= 100)
                return;
            ++this.eventcount;
            string ItemCode = (string)null;
            bool flag = true;
            switch (this.eventcount)
            {
                case 20:
                    ItemCode = "DA11";
                    break;
                case 40:
                    ItemCode = "DM07";
                    break;
                case 60:
                    ItemCode = "DG28";
                    break;
                case 80:
                    ItemCode = "DD04";
                    break;
                case 100:
                    ItemCode = "DC70";
                    break;
                default:
                    flag = false;
                    break;
            }
            if (flag)
            {
                new Random().Next(0, 4);
                int num = 3;
                if (ItemCode.StartsWith("B"))
                    Inventory.AddCostume(this, ItemCode, num);
                else
                    Inventory.AddItem(this, ItemCode, num);
                this.send((Packet)new SP_Event(this, ItemCode, num));
            }
            else
                this.send((Packet)new SP_EventCount(this));
        }

        public void RandomGunsmithResource()
        {
            string[] strArray = new string[3]
            {
        "CZ85",
        "CZ84",
        "CZ83"
            };
            int type = Game_Server.Generic.random(0, 300);
            int index = 0;
            if (type > 200)
                index = 2;
            else if (type > 100)
                index = 1;
            string Code = strArray[index];
            if (ItemManager.GetItem(Code) == null)
                return;
            Inventory.AddItem(this, Code, 1);
            this.send((Packet)new SP_MaterialEarned(this, type));
        }

        public void OnDie()
        {
            this.Health = -1;
            this.isSpawned = false;
            if (this.room.GetSide(this) == 0)
                --this.room.KillsDerbaranLeft;
            else
                --this.room.KillsNIULeft;
            if (this.room.heromode != null)
            {
                if (this.room.derbHeroUsr == this.roomslot)
                {
                    this.room.derbHeroUsr = -1;
                    --this.room.derbHeroKill;
                }
                else if (this.room.niuHeroUsr == this.roomslot)
                {
                    this.room.niuHeroUsr = -1;
                    --this.room.niuHeroKill;
                }
                this.room.heromode.CheckForNewRound();
            }
            if (this.isHacking)
            {
                this.isHacking = false;
                this.room.send((Packet)new SP_RoomHackMission(this.roomslot, this.hackingBase == 0 ? this.room.HackPercentage.BaseA : this.room.HackPercentage.BaseB, 3, this.hackingBase));
            }
            if (this.hasC4)
            {
                this.room.send((Packet)new SP_Unknown((ushort)29985, new object[8]
                {
          (object) 0,
          (object) 0,
          (object) 1,
          (object) 0,
          (object) 0,
          (object) 0,
          (object) 0,
          (object) 0
                }));
                this.room.PickuppedC4 = false;
                this.hasC4 = false;
                this.room.send((Packet)new SP_Unknown((ushort)29985, new object[8]
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
            }
            ++this.rDeaths;
            ++this.rPoints;
            this.LastDieTick = Game_Server.Generic.timestamp + 1;
            this.classCode = "-1";
        }

        public string GetEquipment(int c)
        {
            string[] strArray = new string[8];
            for (int index = 0; index < strArray.Length; ++index)
                strArray[index] = this.equipment[c, index];
            return string.Join(",", strArray);
        }

        public void SaveEquipment()
        {
            string[] strArray = new string[5];
            for (int c = 0; c < 5; ++c)
                strArray[c] = this.GetEquipment(c);
            DB.RunQuery("UPDATE equipment SET class0 = '" + strArray[0] + "', class1 = '" + strArray[1] + "', class2 = '" + strArray[2] + "', class3 = '" + strArray[3] + "', class4 = '" + strArray[4] + "' WHERE ownerid='" + (object)this.userId + "'");
        }

        public string GetItemByID(string ID)
        {
            int result = -1;
            int.TryParse(ID.Remove(0, 1), out result);
            if (result < 0)
                return "I000";
            return this.inventory[result].Split('-')[0];
        }

        public int HasSmileBadge
        {
            get
            {
                return !HasItem("CK01") ? 1 : 0;
            }
        }

        public bool IsWhitelistedWeapon(string Weapon)
        {
            return !Weapon.Contains("-") && (Weapon == "DF01" || Weapon == "DQ01" || (Weapon == "DR01" || this.hasRetail(Weapon)) || (Weapon == "DF02" || Weapon == "D601" || (Weapon == "DG17" || Weapon == "DH06")) || (Weapon == "DH02" || Weapon == "DN01" || (Weapon == "DC02" || Weapon == "DG05") || (Weapon == "DB01" || Weapon == "DL01" || (Weapon == "DJ01" || Weapon == "DA02"))) || (Weapon == "DA50" || Weapon == "EA03" || (Weapon == "DA51" || Weapon == "DA52") || (Weapon == "DA53" || Weapon == "DA54" || (Weapon == "DN51" || Weapon == "DN52")) || (Weapon == "D001" || this.retail != null && (Weapon == "DQ02" || Weapon == "DO01"))) || RetailSystem.Enabled && RetailSystem.IsRetail(Weapon));
        }

        public string GetInventoryCode(string ID)
        {
            int index = int.Parse(ID.Remove(0, 1));
            if (this.inventory[index].Length <= 0)
                return (string)null;
            return this.inventory[index].Split('-')[0].ToUpper();
        }

        public void LoadRetails()
        {
            if (RetailSystem.Enabled)
            {
                for (int Class = 0; Class < 5; ++Class)
                {
                    string strCode = this.equipment[Class, 7];
                    if (strCode != "^" || !this.HasItem(strCode))
                    {
                        string retailByClass = RetailSystem.GetRetailByClass(Class);
                        if (retailByClass != null)
                            this.equipment[Class, 7] = retailByClass;
                    }
                }
            }
            if (!this.hasRetail() || this.retailclass == -1)
                return;
            for (int index = 0; index < 5; ++index)
            {
                if (index != this.retailclass)
                {
                    string strCode = this.equipment[index, 7];
                    if (strCode != null && (strCode == "^" || !this.HasItem(strCode)))
                        this.equipment[index, 7] = index == 1 ? "DQ02" : "DO01";
                }
            }
            string itemById = this.equipment[this.retailclass, 7];
            if (!(itemById != this.retail))
                return;
            if (itemById.StartsWith("I"))
                itemById = this.GetItemByID(itemById);
            if (!(itemById == "^") && this.HasItem(itemById))
                return;
            this.equipment[this.retailclass, 7] = this.retail;
        }

        public int GetItemIndex(string Code)
        {
            if (Code.StartsWith("I"))
                Code = this.GetInventoryCode(Code);
            for (int index = 0; index < this.inventory.Length; ++index)
            {
                if (this.inventory[index] != "^")
                {
                    if (string.Compare(this.inventory[index].Split('-')[0], Code, true) == 0)
                        return index;
                }
            }
            return -1;
        }

        public int GetCostumeIndex(string Code)
        {
            for (int index = 0; index < this.costume.Length; ++index)
            {
                if (this.costume[index] != "^")
                {
                    if (string.Compare(this.costume[index].Split('-')[0], Code, true) == 0)
                        return index;
                }
            }
            return -1;
        }

        public bool HasCostume(string strCode)
        {
            return strCode == "BA01" || strCode == "BA02" || (strCode == "BA03" || strCode == "BA04") || (strCode == "BA05" || this.GetCostumeIndex(strCode) != -1);
        }

        public bool HasItem(string strCode)
        {
            return this.GetItemIndex(strCode) != -1;
        }

        public void RefreshCash()
        {
            this.cash = int.Parse(DB.runReadRow("SELECT cash FROM users WHERE id='" + this.userId + "'")[0]);
            //this.cash = int.Parse(DB.RunReaderOnce("cash", "SELECT * FROM users WHERE id='" + (object)this.userId + "'").ToString());
            this.send((Packet)new SP_CashItemBuy(this));
        }

        public void RefreshDinars()
        {
            this.dinar = int.Parse(DB.RunReaderOnce("dinar", "SELECT * FROM users WHERE id='" + (object)this.userId + "'").ToString());
        }

        public void SaveStats()
        {
            DB.RunQuery("UPDATE users SET kills='" + (object)this.kills + "', deaths='" + (object)this.deaths + "', headshots='" + (object)this.headshots + "', wonMatchs = '" + (object)this.wonMatchs + "', lostMatchs = '" + (object)this.lostMatchs + "', killcount='" + (object)this.eventcount + "', exp='" + (object)this.exp + "' WHERE id='" + (object)this.userId + "'");
        }

        public bool deleteItem(string item)
        {
            int itemIndex = this.GetItemIndex(item);
            if (itemIndex == -1)
                return false;
            this.inventory[itemIndex] = "^";
            DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(this) + "' WHERE ownerid = '" + (object)this.userId + "'");
            return true;
        }

        public bool deleteCostume(string item)
        {
            int costumeIndex = this.GetCostumeIndex(item);
            if (costumeIndex == -1)
                return false;
            this.costume[costumeIndex] = "^";
            DB.RunQuery("UPDATE users_costumes SET inventory = '" + Inventory.Costumelist(this) + "' WHERE ownerid = '" + (object)this.userId + "'");
            return true;
        }

        public bool CheckForEvent(int id)
        {
            return DB.RunReader("SELECT id FROM users_events WHERE userid='" + (object)this.userId + "' AND eventid='" + (object)id + "'").Rows.Count != 0;
        }

        public void AddEvent(int id, bool permanent = false)
        {
            DB.RunQuery("INSERT INTO users_events (eventid, userid, permanent, timestamp) VALUES ('" + (object)id + "','" + (object)this.userId + "', '" + (object)(permanent ? 1 : 0) + "', '" + (object)Game_Server.Generic.timestamp + "')");
        }

        public void CheckForCostume()
        {
            bool flag = false;
            string strCode1 = this.costumes_char[0].Split(',')[0];
            string strCode2 = this.costumes_char[1].Split(',')[0];
            string strCode3 = this.costumes_char[2].Split(',')[0];
            string strCode4 = this.costumes_char[3].Split(',')[0];
            string strCode5 = this.costumes_char[4].Split(',')[0];
            if (!this.HasCostume(strCode1))
            {
                this.costumes_char[0] = "BA01,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
                flag = true;
            }
            if (!this.HasCostume(strCode2))
            {
                this.costumes_char[1] = "BA02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
                flag = true;
            }
            if (!this.HasCostume(strCode3))
            {
                this.costumes_char[2] = "BA03,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
                flag = true;
            }
            if (!this.HasCostume(strCode4))
            {
                this.costumes_char[3] = "BA04,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
                flag = true;
            }
            if (!this.HasCostume(strCode5))
            {
                this.costumes_char[4] = "BA05,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
                flag = true;
            }
            for (int index1 = 0; index1 < this.costume.Length; ++index1)
            {
                string str = this.costume[index1];
                for (int index2 = 0; index2 < 5; ++index2)
                {
                    if (!this.HasCostume(str) && str != "^")
                    {
                        string[] strArray = this.costumes_char[index2].Split(',');
                        for (int index3 = 0; index3 < strArray.Length; ++index3)
                        {
                            if (string.Compare(strArray[index3], str, true) == 0)
                            {
                                flag = true;
                                this.costumes_char[index2].Split(',')[index3] = "^";
                            }
                        }
                    }
                }
            }
            if (!flag)
                return;
            DB.RunQuery("UPDATE users_costumes SET class_0='" + this.costumes_char[0] + "', class_1='" + this.costumes_char[1] + "', class_2='" + this.costumes_char[2] + "', class_3='" + this.costumes_char[3] + "', class_4='" + this.costumes_char[4] + "' WHERE ownerid='" + (object)this.userId + "'");
        }

        public void AddFriend(int uid, int requester)
        {
            if (this.Friends.ContainsKey(uid) || uid == this.userId)
                return;
            User user = UserManager.GetUser(uid);
            string nickname = user == null ? DB.RunReaderOnce("nickname", "SELECT * FROM users WHERE id='" + (object)uid + "'").ToString() : user.nickname;
            this.Friends.TryAdd(uid, new Messenger(uid, nickname, 5, requester)
            {
                isOnline = false
            });
        }

        public void RemoveFriend(int uid)
        {
            if (!this.Friends.ContainsKey(uid))
                return;
            this.Friends[uid] = (Messenger)null;
            Messenger messenger;
            this.Friends.TryRemove(uid, out messenger);
        }

        public Messenger GetFriend(int id)
        {
            if (this.Friends.ContainsKey(id))
                return this.Friends[id];
            return (Messenger)null;
        }

        public Messenger GetFriend(string nick)
        {
            return this.Friends.Values.Where<Messenger>((Func<Messenger, bool>)(r => string.Compare(r.nickname, nick, true) == 0)).FirstOrDefault<Messenger>() ?? (Messenger)null;
        }

        public void AddDailyStats(
          int kills,
          int deaths,
          int headshots,
          int expearned,
          int dinarearned)
        {
            string str = DateTime.Now.ToString("dd-MM-yyyy");
            if (this.dailystats)
            {
                DB.RunQuery("UPDATE users_stats SET totalexp='" + (object)this.exp + "', nickname='" + this.nickname + "', headshots=headshots+" + (object)headshots + ", country='" + this.country + "', premium='" + (object)this.premium + "', exp=exp+" + (object)expearned + ", dinar=dinar+" + (object)dinarearned + ", kills=kills+" + (object)kills + ", deaths=deaths+" + (object)deaths + ", timestamp='" + (object)Game_Server.Generic.timestamp + "' WHERE userid='" + (object)this.userId + "' AND date='" + str + "'");
            }
            else
            {
                this.dailystats = true;
                DB.RunQuery("UPDATE users SET Lastdaystats='" + str + "' WHERE id='" + (object)this.userId + "'");
                DB.RunQuery("INSERT INTO users_stats (userid, nickname, totalexp, kills, deaths, headshots, exp, dinar, premium, country, date, timestamp) VALUES ('" + (object)this.userId + "', '" + this.nickname + "', '" + (object)this.exp + "', '" + (object)kills + "', '" + (object)deaths + "', '" + (object)headshots + "', '" + (object)this.ExpEarned + "', '" + (object)this.DinarEarned + "', '" + (object)this.premium + "', '" + this.country + "', '" + str + "', '" + (object)Game_Server.Generic.timestamp + "')");
            }
        }

        public void RetrievePing()
        {
            try
            {
                Ping ping = new Ping();
                ping.PingCompleted += new PingCompletedEventHandler(this.RetrievePing_Complete);
                ping.SendAsync(IPAddress.Parse(this.IP), (object)(Game_Server.Configs.Server.MaxPing + 100));
            }
            catch (Exception ex)
            {
                Log.WriteError("Ping error: " + ex.Message + " " + ex.StackTrace);
            }
        }

        private void RetrievePing_Complete(object sender, PingCompletedEventArgs e)
        {
            PingReply reply = e.Reply;
            if (reply.Status != IPStatus.Success)
                return;
            this.ping = (uint)Math.Ceiling((Decimal)reply.RoundtripTime);
            if ((long)this.ping <= (long)Game_Server.Configs.Server.MaxPing)
                return;
            for (int index = 0; index < 3; ++index)
                this.send((Packet)new SP_Chat("SYSTEM", SP_Chat.ChatType.Notice1, "You have a too high ping (" + (object)this.ping + " ms). Max is " + (object)Game_Server.Configs.Server.MaxPing + " ms", 999U, "SYSTEM"));
            this.disconnect();
        }

        public uint ConvertIPAddress(IPEndPoint ipeo)
        {
            return BitConverter.ToUInt32(ipeo.Address.GetAddressBytes(), 0);
        }

        public ushort ReversePort(IPEndPoint ipEndp)
        {
            byte[] bytes = BitConverter.GetBytes((ushort)ipEndp.Port);
            Array.Reverse((Array)bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public void setRemoteEndPoint(IPEndPoint Target)
        {
            try
            {
                this.RemoteIP = (long)this.ConvertIPAddress(Target);
                this.RemotePort = (uint)this.ReversePort(Target);
                this.remoteEndPoint = Target;
            }
            catch (Exception ex)
            {
                Log.WriteError("An error has occurred on setRemoteEndPoint: " + ex.Message);
            }
        }

        public void setLocalEndPoint(IPEndPoint Target)
        {
            try
            {
                this.LocalIP = (long)this.ConvertIPAddress(Target);
                this.LocalPort = (uint)this.ReversePort(Target);
                this.localEndPoint = Target;
            }
            catch (Exception ex)
            {
                Log.WriteError("An error has occurred on setLocalEndPoint: " + ex.Message);
            }
        }

        /* public void send(Packet p)
         {
             try
             {
                 byte[] bytes = p.GetBytes();
                 if (bytes != null)
                 {
                     if (bytes.Length > 0)
                             this.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), (object)null);
                 }
             }
             catch
             {
                 this.disconnect();
             }
             p.Dispose();
         }

         public void sendBuffer(byte[] buffer)
         {
             try
             {
                 if (buffer == null || buffer.Length <= 0)
                     return;
                 this.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), (object)null);
             }
             catch
             {
                 this.disconnect();
             }
         }

         private void sendCallBack(IAsyncResult iAr)
         {
             try
             {
                 this.socket.EndSend(iAr);
             }
             catch
             {
             }
         }*/

        /*   public void send(Packet p)
           {
               try
               {
                   byte[] sendBuffer = p.GetBytes();
                   if (sendBuffer != null && sendBuffer.Length > 0)
                   {
                       socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null);
                   }
               }
               catch
               {
                   disconnect();
               }
               p.Dispose();
           }

           public void sendBuffer(byte[] buffer)
           {
               try
               {
                   if (buffer != null && buffer.Length > 0)
                   {
                       socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null);
                   }
               }
               catch
               {
                   disconnect();
               }
           }*/

        /* private void sendCallBack(IAsyncResult iAr)
         {
             try { socket.EndSend(iAr); }
             catch (Exception e) { } //{ System.Console.WriteLine(e); }
         }*/

        public void send(Packet p)
        {
            byte[] bytes = p.GetBytes();
            try
            {
                this.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), null);
            }
            catch
            {
                this.disconnect();
            }
        }

        public void sendBuffer(byte[] buffer)
        {
            try
            {
                this.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), null);
            }
            catch
            {
                this.disconnect();
            }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try
            {
                this.socket.EndSend(iAr);
            }
            catch
            {
                this.disconnect();
            }
        }
        public User(uint sessionId, Socket socket)
        {
            this.socket = socket;
            this.sessionId = sessionId;
            this.connectionId = TCP.GetFreeConnectionID;
            try
            {
                this.IP = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
            }
            catch (Exception ex)
            {
                Log.WriteError("Unable to get IP address: " + ex.Message + " - " + ex.StackTrace);
            }
            this.inventory = new string[Game_Server.Configs.Server.Player.MaxInventorySlot];
            this.costume = new string[Game_Server.Configs.Server.Player.MaxCostumeSlot];
            this.remoteIp = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
            this.localIp = (socket.LocalEndPoint as IPEndPoint).Address.ToString();
            for (int index = 0; index < 5; ++index)
                this.costumes_char[index] = "BA0" + (object)(index + 1) + ",^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
            new Thread(new ThreadStart(this.ReceiveData)).Start();
        }

        private void ReceiveData()
        {
            try
            {
                while (!this.disconnected && this.socket.Connected)
                {
                    int length = this.socket.Receive(this.buffer);
                    if (length > 0)
                    {
                        byte[] inputByte = new byte[length];
                        Array.Copy((Array)this.buffer, 0, (Array)inputByte, 0, length);
                        this.LastClientTick = Game_Server.Generic.timestamp;
                        string str = Encoding.GetEncoding("Windows-1250").GetString(Cryption.decrypt(inputByte));
                        string[] separator = new string[1] { "\n" };
                        foreach (string packetStr in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (packetStr.Length > 5)
                            {
                                try
                                {
                                    using (Handler packet = Packet_Manager.ParsePacket(packetStr))
                                    {
                                        packet?.Handle(this);
                                        packet.Dispose();
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    else
                    {
                        this.disconnect();
                        break;
                    }
                }
            }
            catch
            {
                this.disconnect();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            int num = disposing ? 1 : 0;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
            GC.Collect();
        }

        public void RefreshFriends()
        {
            if (this.actualUserlistType != 0)
                return;
            List<User> users = new List<User>();
            foreach (Messenger messenger in this.Friends.Values.Where<Messenger>((Func<Messenger, bool>)(r =>
            {
                if (r.isOnline)
                    return r.status != 5;
                return false;
            })).ToList<Messenger>())
            {
                User user = UserManager.GetUser(messenger.id);
                if (user != null)
                    users.Add(user);
            }
            this.send((Packet)new SP_UserList(SP_UserList.Type.Friends, users));
        }

        public void disconnect()
        {
            if (this.socket != null)
            {
                try
                {
                    this.socket.Close();
                }
                catch
                {
                }
                this.socket = (Socket)null;
            }
            foreach (Messenger messenger in (IEnumerable<Messenger>)this.Friends.Values)
                messenger.Dispose();
            this.Friends.Clear();
            this.OutboxItems.Clear();
            if (this.disconnected)
                return;
            this.disconnected = true;
            DB.RunQuery("UPDATE users SET online='0' WHERE id='" + (object)this.userId + "'");
            try
            {
                if (this.room != null)
                {
                    if (this.spectating)
                        this.room.RemoveSpectator(this);
                    else
                        this.room.RemoveUser(this.roomslot);
                    this.room = (Room)null;
                }
            }
            catch
            {
            }
            if (this.clan != null)
            {
                if (this.clan.Users.ContainsKey(this.userId))
                {
                    User user;
                    this.clan.Users.TryRemove(this.userId, out user);
                }
                this.clan = (Clan)null;
            }
            /*if (this.nickname.StartsWith("MOD") && this.rank > 3)
            {
                UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "MOD" + this.nickname + "Left Server", 0U, "NULL"));
            }
            else if (this.nickname.StartsWith("GM") && this.rank < 6)
            {
                UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "GM" + this.nickname + "Left Server", 0U, "NULL"));
            }
           
            else if (this.nickname.StartsWith("DEV") && this.rank > 6)
            {
                UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "DEV" + this.nickname + "Left Server", 0U, "NULL"));
            }*/
            UserManager.RemoveUser(this);
            this.Dispose();
        }
       

public bool IsConnectionAlive
        {
            get
            {
                return this.socket != null && this.socket.Connected;
            }
        }

        public int sessionStart { get; set; }

        public DateTime PingTime { get; set; }

        public void ComeBackReward()
        {
            this.AddPremium((byte)3, (ushort)15);
            this.cash += 10000;
            DB.RunQuery("UPDATE users SET cash='" + (object)this.cash + "' WHERE id = '" + (object)this.userId + "'");
            Inventory.AddItem(this, "DF35", 15);
            Inventory.AddItem(this, "DC33", 15);
            Inventory.AddItem(this, "DG08", 15);
            Inventory.AddItem(this, "DJ33", 15);
            Inventory.AddItem(this, "DN03", 15);
            Inventory.AddCostume(this, "BD02", 7);
            Inventory.AddCostume(this, "BF44", 7);
            Inventory.AddCostume(this, "BA11", 7);
            Inventory.AddCostume(this, "BA12", 7);
            Inventory.AddCostume(this, "BA13", 7);
            Inventory.AddCostume(this, "BA14", 7);
            Inventory.AddCostume(this, "BA15", 7);
            this.send((Packet)new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> Welcome back, " + this.nickname + "! Come back reward has been given. Check your inventory!", this.sessionId, this.nickname));
        }

       /* public bool IsAlive()
        {
            if (this.Health > 0 && this.ExplosiveAlive)
                return this.isSpawned;
            return false;
        }*/

        public bool IsAlive()
        {
            return Health > 0 && ExplosiveAlive && isSpawned;
        }


        internal enum Classes
        {
            Engeneer,
            Medic,
            Sniper,
            Assault,
            Heavy,
        }

        internal enum Slots
        {
            Hands,
            HandGun,
            Weapon1,
            equipment,
            Weapon2,
            Px,
            NuLL,
            Retail,
        }
    }

    public class LoginEventStatus
    {
        internal int progress;
        internal bool doneToday;
    }
}
