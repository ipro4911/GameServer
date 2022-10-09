using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Game_Server.Managers;
using Game_Server;

namespace Game_Server.Game
{
    class CP_CharacterInfo : Handler
    {
        public override void Handle(User usr)
        {

            if (blocks.Length < 17 || blocks[18] != "dnjfhr^") usr.disconnect();
            int userId = int.Parse(getBlock(1));
            if (userId > 0)
            {
                /* Blocks changes on the other WarRock Version */
                int ticketId = int.Parse(getBlock(0));
                string username = getBlock(3);
                string nickname = getBlock(4);

                if (username.Length > 0 && username.Length <= 16 && ticketId >= 0 && userId >= 0)
                {
                    DataTable dt = DB.RunReader("SELECT * FROM users WHERE username='" + DB.Stripslash(username) + "' AND id='" + userId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataRow userInformation = dt.Rows[0];
                            int dbTicketId = 0;
                            int.TryParse(userInformation["ticketid"].ToString(), out dbTicketId);
                            //if (dbTicketId == ticketId  dbTicketId = -1);
                            if (dbTicketId == ticketId && dbTicketId == -1 || Configs.Server.Debug)
                            {
                                usr.userId = userId;
                                usr.username = userInformation["username"].ToString();
                                usr.nickname = userInformation["nickname"].ToString();
                                usr.exp = int.Parse(userInformation["exp"].ToString());
                                usr.dinar = Convert.ToInt32(userInformation["dinar"].ToString());
                                usr.kills = int.Parse(userInformation["kills"].ToString());
                                usr.deaths = int.Parse(userInformation["deaths"].ToString());
                                usr.premium = byte.Parse(userInformation["premium"].ToString());
                                uint.TryParse(userInformation["premiumExpire"].ToString(), out usr.premiumExpire);
                                usr.cash = int.Parse(userInformation["cash"].ToString());
                                usr.rank = int.Parse(userInformation["rank"].ToString());
                                usr.coupons = int.Parse(userInformation["coupons"].ToString());
                                usr.todaycoupons = int.Parse(userInformation["todaycoupon"].ToString());
                                usr.clanId = int.Parse(userInformation["clanID"].ToString());
                                usr.headshots = int.Parse(userInformation["headshots"].ToString());
                                uint.TryParse(userInformation["mutedExpire"].ToString(), out usr.mutedexpire);
                                usr.firstlogin = int.Parse(userInformation["firstlogin"].ToString());
                                usr.country = userInformation["country"].ToString();
                                usr.coupontime = int.Parse(userInformation["coupontime"].ToString());
                                // usr.storageInventoryMax = usr.rank > 2 ? 24 : 12;
                                if (usr.premium == (byte)0)
                                    usr.storageInventoryMax = 32;
                                else if (usr.premium == (byte)1)
                                    usr.storageInventoryMax = 40;
                                else if (usr.premium == (byte)2)
                                    usr.storageInventoryMax = 60;
                                else if (usr.premium == (byte)3)
                                    usr.storageInventoryMax = 80;
                                else if (usr.premium == (byte)4)
                                    usr.storageInventoryMax = 80;
                                uint.TryParse(userInformation["donationexpire"].ToString(), out usr.donationexpire);
                                int randombox = 0;
                                int.TryParse(userInformation["randombox"].ToString(), out randombox);
                                usr.RandomBoxToday = (randombox == 1);

                                usr.rewardEvent.doneToday = int.Parse(userInformation["loginEventToday"].ToString()) == 1;

                                int.TryParse(userInformation["loginEventProgress"].ToString(), out usr.rewardEvent.progress);

                                int.TryParse(userInformation["killcount"].ToString(), out usr.eventcount);

                                ushort.TryParse(userInformation["wonMatchs"].ToString(), out usr.wonMatchs);
                                ushort.TryParse(userInformation["lostMatchs"].ToString(), out usr.lostMatchs);

                                int lastjoin = 0;
                                int.TryParse(userInformation["lastjoin"].ToString(), out lastjoin);

                                if (string.Compare(userInformation["retailcode"].ToString(), "null", true) != 0)
                                {
                                    usr.retail = userInformation["retailcode"].ToString();
                                    int.TryParse(userInformation["retailclass"].ToString(), out usr.retailclass);
                                }

                                string chat_color = userInformation["chat_color"].ToString();

                                if (chat_color != "" && chat_color.Length >= 6)
                                {
                                    usr.chatColor = Generic.ConvertHexToRGB(chat_color);
                                }

                                usr.ticketId = ticketId;

                                if (usr.rank > 0)
                                {
                                    string LastDayStats = userInformation["Lastdaystats"].ToString(); ;

                                    string today = DateTime.Now.ToString("dd-MM-yyyy");

                                    if (LastDayStats == today) usr.dailystats = true;

                                    if (usr.dinar < 0) usr.dinar = 0;
                                    if (usr.cash < 0) usr.cash = 0;

                                    if (usr.rank < 5)
                                    {
                                        Country c = Program.ipLookup.getCountry(usr.IP);
                                        usr.country = c.getCode();
                                    }

                                    /* Clan Part */

                                    usr.clan = Managers.ClanManager.GetClan(usr.clanId);

                                    if (usr.clan != null)
                                    {
                                        if (usr.clan.clanRank(usr) != 9)
                                        {
                                            if (usr.clan.ClanUsers.ContainsKey(usr.userId))
                                            {
                                                ClanUsers c = (ClanUsers)usr.clan.ClanUsers[usr.userId];
                                                if (usr.exp.ToString() != c.EXP)
                                                {
                                                    c.EXP = usr.exp.ToString();
                                                }

                                                if (string.Compare(usr.nickname, nickname, true) == 0)
                                                {
                                                    c.nickname = usr.nickname;
                                                }
                                            }

                                            if (!usr.clan.Users.ContainsKey(usr.userId))
                                            {
                                                usr.clan.Users.TryAdd(usr.userId, usr);
                                            }
                                        }
                                        else
                                        {
                                            if (usr.clan.pendingUsers.ContainsKey(usr.userId))
                                            {
                                                ClanPendingUsers c = (ClanPendingUsers)usr.clan.pendingUsers[usr.userId];
                                                if (usr.exp.ToString() != c.EXP)
                                                {
                                                    c.EXP = usr.exp.ToString();
                                                }

                                                if (string.Compare(usr.nickname, nickname, true) == 0)
                                                {
                                                    c.nickname = usr.nickname;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (usr.clanId != -1)
                                        {
                                            DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + userId + "'");
                                        }
                                    }

                                    usr.accesslevel = (usr.rank > 2 ? 3 : 0);

                                    #region Equipment Reset System
                                    for (int Class = 0; Class < 5; Class++)
                                    {
                                        for (int Slot = 0; Slot < 8; Slot++)
                                        {
                                            if (Slot == (int)User.Slots.Hands)
                                                usr.equipment[Class, Slot] = "DA02"; //Knuckle
                                            else if (Slot == (int)User.Slots.HandGun)
                                                usr.equipment[Class, Slot] = "DB01"; //Colt
                                            else if (Slot == (int)User.Slots.Weapon1)
                                            {
                                                switch (Class)
                                                {
                                                    case (int)User.Classes.Engeneer:
                                                    case (int)User.Classes.Medic:
                                                        usr.equipment[Class, Slot] = "DF01"; // MP7
                                                        break;
                                                    case (int)User.Classes.Sniper:
                                                        usr.equipment[Class, Slot] = "DG05";
                                                        break;
                                                    case (int)User.Classes.Assault:
                                                        usr.equipment[Class, Slot] = "DC02"; // K2
                                                        break;
                                                    case (int)User.Classes.Heavy:
                                                        usr.equipment[Class, Slot] = "DJ01";
                                                        break;
                                                }
                                            }
                                            else if (Slot == (int)User.Slots.equipment)
                                            {
                                                switch (Class)
                                                {
                                                    case (int)User.Classes.Engeneer:
                                                        usr.equipment[Class, Slot] = "DR01"; // Spanner
                                                        break;
                                                    case (int)User.Classes.Medic:
                                                        usr.equipment[Class, Slot] = "DQ01"; // Medic Kit 1
                                                        break;
                                                    case (int)User.Classes.Sniper:
                                                    case (int)User.Classes.Assault:
                                                        usr.equipment[Class, Slot] = "DN01"; // Grenade
                                                        break;
                                                    case (int)User.Classes.Heavy:
                                                        usr.equipment[Class, Slot] = "DL01"; // Mine
                                                        break;
                                                }
                                            }
                                            else
                                                usr.equipment[Class, Slot] = "^";
                                        }
                                    }
                                    #endregion

                                    int wrtime = Generic.WarRockDateTime;

                                    DataTable eq = DB.RunReader("SELECT class0, class1, class2, class3, class4, inventory, storage FROM equipment WHERE ownerid='" + usr.userId + "'");
                                    if (eq.Rows.Count > 0)
                                    {
                                        DataRow row = eq.Rows[0];
                                        usr.inventory = row["inventory"].ToString().Split(',');
                                        usr.storageInventory = row["storage"].ToString().Split(',');

                                        for (int i = 0; i < usr.inventory.Length; i++)
                                        {
                                            string str = usr.inventory[i];
                                            string v = str.Split('-')[0];

                                            usr.inventory[i] = "^";

                                            if (str != "^" && v.Length == 4)
                                            {
                                                usr.inventory[i] = str;
                                            }
                                        }

                                        bool tosave = false;

                                        for (int i = 0; i < usr.inventory.Length; i++)
                                        {
                                            if (usr.inventory[i] != "^")
                                            {
                                                try
                                                {
                                                    string[] weaponData = usr.inventory[i].Split('-');
                                                    string code = weaponData[0];
                                                    if (code.Length == 4)
                                                    {
                                                        int time = 0;
                                                        int.TryParse(weaponData[3], out time);
                                                        if (time < wrtime)
                                                        {
                                                            usr.expiredItems.Add(code);
                                                            usr.inventory[i] = "^";
                                                            tosave = true;
                                                        }
                                                    }
                                                }
                                                catch (Exception e) { Console.WriteLine(e); }
                                            }
                                        }

                                        if (tosave)
                                        {
                                            DB.RunQuery("UPDATE equipment SET inventory='" + Inventory.Itemlist(usr) + "' WHERE ownerid='" + usr.userId + "'");
                                        }

                                        string[] equipment = new string[] { row["class0"].ToString(), row["class1"].ToString(), row["class2"].ToString(), row["class3"].ToString(), row["class4"].ToString() };

                                        bool forcesave = false;

                                        string[] SplitSlots = usr.AvailableSlots.Split(new char[] { ',' });
                                        for (int i = 0; i < 5; i++)
                                        {
                                            string[] fetchItem = equipment[i].Split(',');
                                            for (int j = 0; j < 8; j++)
                                            {
                                                string inventoryCode = fetchItem[j];

                                                string dbCode = fetchItem[j];

                                                try
                                                {
                                                    Managers.Item item = null;
                                                    if (!dbCode.Contains("-"))
                                                    {
                                                        dbCode = dbCode.StartsWith("I") ? usr.GetItemByID(dbCode) : dbCode;
                                                        item = Managers.ItemManager.GetItem(dbCode);
                                                    }

                                                    bool sixSlot = false;

                                                    if (inventoryCode != "^")
                                                    {
                                                        if (inventoryCode.StartsWith("I") && inventoryCode.Contains("-"))
                                                        {
                                                            string[] splitItems = inventoryCode.Split('-');
                                                            if (usr.HasItem(splitItems[0]) && usr.HasItem(splitItems[1]))
                                                            {
                                                                sixSlot = true;
                                                            }
                                                        }

                                                        bool reset = true;

                                                        bool HasItem = false;

                                                        if (!inventoryCode.Contains("-"))
                                                        {
                                                            HasItem = usr.HasItem(inventoryCode);
                                                        }

                                                        if (usr.IsWhitelistedWeapon(inventoryCode) || HasItem || sixSlot)
                                                        {
                                                            usr.equipment[i, j] = inventoryCode;
                                                            reset = false;
                                                        }

                                                        if (j >= 4)
                                                        {
                                                            int t = j - 4;
                                                            reset = !(SplitSlots[t] == "T" && inventoryCode != usr.retail);
                                                        }

                                                        if (item != null)
                                                        {
                                                            reset = (!item.UseableSlot(j) || !item.UseableBranch(i) && i != 7);
                                                        }

                                                        if (reset)
                                                        {
                                                            usr.equipment[i, j] = "^";
                                                            forcesave = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        usr.equipment[i, j] = "^";
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log.WriteError("Error loading: " + dbCode);
                                                }
                                            }
                                        }

                                        if (forcesave)
                                        {
                                            usr.SaveEquipment();
                                        }
                                    }

                                    DataTable skin = DB.RunReader("SELECT class_0, class_1, class_2, class_3, class_4, inventory FROM users_costumes WHERE ownerid='" + usr.userId + "'");
                                    if (skin.Rows.Count > 0)
                                    {
                                        DataRow row = skin.Rows[0];
                                        string[] myskin = new string[] { row["class_0"].ToString(), row["class_1"].ToString(), row["class_2"].ToString(), row["class_3"].ToString(), row["class_4"].ToString() };
                                        for (int i = 0; i < 5; i++)
                                        {
                                            usr.costumes_char[i] = myskin[i];
                                        }

                                        usr.costume = row["inventory"].ToString().Split(',');

                                        bool forcesave = false;

                                        for (int i = 0; i < usr.costume.Length; i++)
                                        {
                                            if (usr.costume[i] != "^")
                                            {
                                                try
                                                {
                                                    string[] costumeData = usr.costume[i].Split('-');
                                                    string code = costumeData[0];
                                                    if (code.Length == 4)
                                                    {
                                                        int time = 0;
                                                        int.TryParse(costumeData[3], out time);
                                                        if (time < wrtime)
                                                        {
                                                            usr.expiredItems.Add(code);
                                                            usr.costume[i] = "^";
                                                            forcesave = true;
                                                        }
                                                    }
                                                }
                                                catch (Exception e) { Console.WriteLine(e); }
                                            }
                                        }

                                        if (forcesave)
                                        {
                                            DB.RunQuery("UPDATE users_costumes SET inventory='" + Inventory.Costumelist(usr) + "' WHERE ownerid='" + usr.userId + "'");
                                        }
                                    }
                                    else
                                    {
                                        DB.RunQuery("INSERT INTO users_costumes (ownerid) VALUES ('" + usr.userId + "')");
                                    }

                                    int inventoryLength = usr.inventory.Length;
                                    int costumeInventoryLength = usr.costume.Length;
                                    int storageInventoryLength = usr.storageInventory.Length;

                                    if (inventoryLength < Configs.Server.Player.MaxInventorySlot)
                                    {
                                        Array.Resize(ref usr.inventory, Configs.Server.Player.MaxInventorySlot);
                                        for (int i = inventoryLength; i < Configs.Server.Player.MaxInventorySlot; i++)
                                        {
                                            usr.inventory[i] = "^";
                                        }
                                        DB.RunQuery("UPDATE equipment SET inventory='" + Inventory.Itemlist(usr) + "' WHERE ownerid='" + usr.userId + "'");
                                    }

                                    if (costumeInventoryLength < Configs.Server.Player.MaxCostumeSlot)
                                    {
                                        Array.Resize(ref usr.costume, Configs.Server.Player.MaxCostumeSlot);
                                        for (int i = costumeInventoryLength; i < Configs.Server.Player.MaxCostumeSlot; i++)
                                        {
                                            usr.costume[i] = "^";
                                        }
                                        DB.RunQuery("UPDATE users_costumes SET inventory='" + Inventory.Costumelist(usr) + "' WHERE ownerid='" + usr.userId + "'");
                                    }

                                    if (storageInventoryLength < usr.storageInventoryMax)
                                    {
                                        Array.Resize(ref usr.storageInventory, usr.storageInventoryMax);
                                        for (int i = storageInventoryLength; i < usr.storageInventoryMax; i++)
                                        {
                                            usr.storageInventory[i] = "^";
                                        }
                                        DB.RunQuery("UPDATE equipment SET storage='" + Inventory.Storage(usr) + "' WHERE ownerid='" + usr.userId + "'");
                                    }

                                    usr.CheckForCostume();
                                    usr.LoadInboxItems();
                                    usr.LoadRetails();
                                    if (usr.nickname.StartsWith("MOD") && usr.rank > 3)
                                    {
                                        UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "MOD" + usr.nickname + "Joined Server", 0U, "NULL"));
                                    }
                                    else if (usr.nickname.StartsWith("GM") && usr.rank < 6)
                                    {
                                        UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "GM" + usr.nickname + "Joined Server", 0U, "NULL"));
                                    }
                                    else if (usr.nickname.StartsWith("D") && usr.rank < 2)
                                    {
                                        UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "D" + usr.nickname + "Joined Server", 0U, "NULL"));
                                    }
                                    else if (usr.nickname.StartsWith("DEV") && usr.rank > 6)
                                    {
                                        UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "DEV" + usr.nickname + "Joined Server", 0U, "NULL"));
                                    }
                                    //usr.send(new SP_KillCount(SP_KillCount.ActionType.Hide)); // Clear previously data
                                    if (Managers.UserManager.addUser(usr)) // Do not add after packet send
                                    {
                                        #region Random Weapon Login

                                        int eventId = 1;

                                        if (!usr.CheckForEvent(eventId) && Configs.Server.LoginEvent.enabled)
                                        {
                                            if (Inventory.GetFreeItemSlotCount(usr) > 0)
                                            {
                                                string[] randomItems = Configs.Server.LoginEvent.items;
                                                int index = Generic.random(0, randomItems.Length - 1);
                                                string item = randomItems[index];
                                                Managers.Item i = Managers.ItemManager.GetItem(item);
                                                if (i != null)
                                                {
                                                    int days = new Random().Next(Configs.Server.LoginEvent.MinDays, Configs.Server.LoginEvent.MaxDays);
                                                    Inventory.AddItem(usr, item, days);
                                                    usr.AddEvent(eventId);
                                                    // usr.send(new SP_CustomMessageBox("You've obtained " + i.Name + " for " + days + " day(s)."));
                                                }
                                                else
                                                {
                                                    Log.WriteError(item + " is not a valid item @ log in event!");
                                                }
                                            }
                                        }

                                        #endregion

                                        usr.send(new SP_CharacterInfo(usr));
                                        Managers.UserManager.UpdateUserlist(usr);

                                        if (usr.expiredItems.Count > 0)
                                        {
                                            const int b = 30;
                                            int length = (int)Math.Ceiling((decimal)usr.expiredItems.Count / b);
                                            for (int i = 0; i < length; i++)
                                            {
                                                usr.send(new SP_UpdateInventory(usr, usr.expiredItems.Skip(i * b).Take(b).ToList()));
                                            }
                                            usr.expiredItems.Clear();
                                        }

                                        if (usr.expiredCostumes.Count > 0)
                                        {
                                            const int b = 30;
                                            int length = (int)Math.Ceiling((decimal)usr.expiredCostumes.Count / b);
                                            for (int i = 0; i < length; i++)
                                            {
                                                usr.send(new SP_UpdateInventory(usr, usr.expiredCostumes.Skip(i * b).Take(b).ToList()));
                                            }
                                            usr.expiredCostumes.Clear();
                                        }

                                        usr.PremiumTimeLeft();
                                        usr.PingTime = DateTime.Now;
                                        usr.send(new SP_PingInformation(usr));
                                        usr.LoadOutboxItems();
                                        usr.send(new SP_StorageInventoryList(usr));

                                        usr.send(new SP_MyRank(usr));

                                        usr.LoadFriends();

                                        if (usr.rank == 2) // Donator
                                        {
                                            DateTime now = DateTime.Now;
                                            DateTime dexp = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(usr.donationexpire);
                                            TimeSpan ts = dexp - now;
                                            System.Drawing.Color c = System.Drawing.Color.FromArgb(15, 192, 252);

                                            usr.send(new SP_ColoredChat(Configs.Server.SystemName + " >> Your donator status will expire in " + ((int)Math.Round(ts.TotalDays) - 1) + " days", SP_ColoredChat.ChatType.Normal, c));
                                        }

                                        if (usr.firstlogin == 2)
                                        {
                                            usr.CheckForFirstLogin();
                                            usr.send(new SP_UpdateInventory(usr, null));
                                        }
                                        else if (lastjoin <= (Generic.timestamp - (60 * 86400)))
                                        {
                                            usr.ComeBackReward();
                                            usr.send(new SP_UpdateInventory(usr, null));
                                        }

                                        foreach (TempItem i in usr.InBoxItems)
                                        {
                                            string days = "day(s)";
                                            if (i.days >= 3600)
                                            {
                                                days = "One use / Permanent";
                                            }
                                            usr.send(new SP_Chat("SYSTEM", SP_Chat.ChatType.Room_ToAll, "SYSTEM >> You've got " + i.name + " for " + i.days + " " + days + "!", 998, "NULL"));
                                        }

                                        usr.sessionStart = Generic.timestamp;

                                        //usr.send(new SP_CustomMessageBox("We're sorry to disturb your game guys, but we're actually trying to fix an important issue, we apologize for the issue."));

                                        // usr.send(new SP_AntiCheat(usr));
                                        // usr.AntiCheatTick = (uint)(Generic.timestamp + Configs.Server.AntiCheat.routinetick);
                                    }
                                    else
                                    {
                                        // User already connected (?)
                                        Log.WriteError(usr.nickname + " > logged in but couldn't be added to the stuck");
                                        usr.disconnect();
                                    }
                                }
                                else
                                {
                                    // Logging while banned BOT (?)
                                    Log.WriteError(usr.nickname + " > logged in as banned user");
                                    usr.disconnect();
                                }
                            }
                            else
                            {
                                // Invalid Ticket ID
                                Log.WriteError(username + " tried to login with wrong ticket id!");
                                usr.disconnect();

                            }
                        }
                        catch (Exception ex)
                        {
                            // Error parsing user info
                            Log.WriteError("Error parsing user information for user " + username);
                            Log.WriteError(ex.ToString());
                            usr.disconnect();
                        }
                    }
                    else
                    {
                        // No user data found
                        Log.WriteError("No user data found for user " + username);
                        usr.disconnect();
                    }
                }
                else
                {
                    // Invalid packet data
                    Log.WriteError(username + " -> error with " + (username.Length == 0 ? " username length" : " Ticket ID"));
                    usr.disconnect();
                }

                DB.RunQuery("UPDATE users SET ticketid='-1' WHERE id='" + usr.userId + "'");
            }
            else
            {
                // Invalid request
                Log.WriteError(usr.nickname + " > logged in - invalid request");
                usr.disconnect();
            }
        }
    }

    class SP_CharacterInfo : Packet
    {
        internal enum ErrCodes
        {
            /* Thanks to CodeDragon for the ErrCodes*/
            NormalProcedure = 73030,
            InvalidPacket = 90100,
            UnregisteredUser = 90101,
            ServerInaccessible = 90105,
            TraineeServer = 90106,
            ServerFull = 91020,
            UpdateFailed = 91040,
            SyncFail = 91050,
            IDUsed = 92040,
            PremiumOnly = 98010
        }

        public SP_CharacterInfo(ErrCodes ErrCode)
        {
            newPacket(25088);
            addBlock((int)ErrCode);
        }

        public SP_CharacterInfo(User usr)
        {
            /*25088 
             * 1
             * GS205 
             * 3 
             * 23909385 
             * 5 
             * qweqweee 
             * -1 -1 NULL -1 -1 
             * 0 
             * 1 
             * 0 
             * 0 
             * 0 
             * 60000
             * 0 
             * 0 
             * 0 
             * 0 
             * F,F,F,F DA02,DB01,DF01,DR01,^,^,^,^ DA02,DB01,DF01,DQ01,^,^,^,^ DA02,DB01,DG05,DN01,^,^,^,^ DA02,DB01,DC02,DN01,^,^,^,^ DA02,DB01,DJ01,DL01,^,^,^,^ ^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 
             * 30 
             * BA01,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA03,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA04,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA05,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 
             * ^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 
             * 
             * 0 0 -1 0 0 0 0 0*
             */
            newPacket(25088);
            addBlock(1);
            addBlock("#BrokenWar"); // Game Server ID
            addBlock("#"); // ?
            addBlock(usr.userId);
            addBlock(usr.sessionId);
            addBlock(usr.nickname);
            // Clan
            if (usr.clan != null)
            {
                addBlock(usr.clan.id);
                addBlock(usr.clan.iconid);
                addBlock(usr.clan.name);
                addBlock(-1);
                addBlock(usr.clan.clanRank(usr));
            }
            else
            {
                Fill(-1, 5);
            }
            addBlock(0);
            addBlock(usr.level); // Level for EXP
            addBlock(usr.exp); // Exp 
            addBlock(0); // Unknown
            addBlock(0);
            addBlock(usr.dinar);
            addBlock(usr.kills); // Kills
            addBlock(usr.deaths); // Deaths
            //addBlock(usr.rewardEvent.doneToday ? 1 : 0); // Already done firstlogin today
            //addBlock(usr.rewardEvent.progress); // Progress
            addBlock(usr.wonMatchs); // Won matchs
            addBlock(usr.lostMatchs); // Lost matchs
            addBlock(usr.AvailableSlots); // Example: F,F,F,F -> 5,6,7,8 slots are disabled [F = False, T = True]
            for (int i = 0; i < 5; i++)
            {
                addBlock(usr.GetEquipment(i));
            }
            addBlock(Inventory.Itemlist(usr));

            addBlock(Configs.Server.Player.MaxInventorySlot);

            for (int i = 0; i < 5; i++)
            {
                addBlock(usr.costumes_char[i]);
            }

            addBlock(Inventory.Costumelist(usr));

            addBlock(usr.premium); // Premium (1 -> Bronze, 2 -> Silver, 3 -> Gold, 4 -> Platinum)
            addBlock(0);
            addBlock(usr.HasSmileBadge); // Smile Badge
            Fill(0, 5);
        }
    }

}
