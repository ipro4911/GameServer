    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    using Game_Server.Managers;

    namespace Game_Server.Game
    {
        class CP_CashItemBuy : Handler
        {
            private string itemcode;

            internal enum SubCodes
            {
                OnItemBuy = 1110,
                OnItemUse = 1111,
                OnItemShopOpen = 1113,
                Storage = 1400
            }

            public override void Handle(User usr)
            {



                if (usr.room != null) return;
                SubCodes opcode = (SubCodes)int.Parse(getBlock(0));
                switch (opcode)
                {
                    case SubCodes.OnItemShopOpen:
                        {
                            usr.LoadOutboxItems();
                            usr.RefreshDinars();
                           // usr.RefreshCash(); 
                            break;
                        }
                    case SubCodes.OnItemBuy:
                        {
                            string itemcode = getBlock(6).ToUpper();
                            int period = int.Parse(getBlock(3));

                            ushort days = (ushort)Inventory.GetDaysFromPeriod(period);

                            Item item = ItemManager.GetItem(itemcode);
                            if (item != null)
                            {
                                if (days > 0)
                                {
                                    if (item.Buyable || Configs.Server.ItemShop.hiddenItems.Contains(itemcode))
                                    {
                                        int inventorySlot = Inventory.GetFreeItemSlotCount(usr);
                                        if (inventorySlot > 0)
                                        {
                                            uint price = (uint)item.GetCashPrice(period);

                                            //usr.RefreshCash(); // Let's put it here to avoid any useless query until we need to calculate the price

                                            if (price > 0)
                                            {
                                                bool px_item = (itemcode.ToLower().StartsWith("cz") || itemcode.ToLower().StartsWith("cb"));
                                                int result = (int)(usr.cash - price);
                                                if (item.Premium && usr.premium < 1)
                                                {
                                                    usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.PremiumUsersOnly));
                                                }
                                                else if (usr.level < item.Level && usr.rank < 2)
                                                {
                                                    usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.LevelLow));
                                                }
                                                else if (item.accruable && Inventory.GetEAItem(usr, item.Code) >= item.maxAccrueCount)
                                                {
                                                    usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth));
                                                }
                                                else if (result < 0)
                                                {
                                                    usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NotEnoughDinar));
                                                }
                                                else
                                                {
                                                    if (px_item)
                                                        days = 3600;

                                                    ushort count = 1;

                                                    if (item.accruable)
                                                    {
                                                        ushort d = (ushort)item.GetEACount(period);
                                                        if (d >= 1)
                                                        {
                                                            count = d;
                                                        }
                                                    }


                                                    switch (itemcode)
                                                    {
                                                        case "CB53":
                                                            {
                                                                if (usr.clan == null || usr.clan.maxUsers >= 100)
                                                                {
                                                                    usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth));
                                                                    return;
                                                                }
                                                                usr.clan.maxUsers += 20;
                                                                usr.send(new SP_Chat(Configs.Server.SystemName, SP_Chat.ChatType.Clan, Configs.Server.SystemName + " >> Clan expanded, please re-open clan tab to see changes.", 999, Configs.Server.SystemName));
                                                                usr.send(new SP_DinarItemBuy(usr, itemcode));
                                                                usr.send(new SP_Clan.MyClanInformation(usr));
                                                                DB.RunQuery("UPDATE clans SET maxusers='" + usr.clan.maxUsers + "' WHERE id='" + usr.clan.id + "'");
                                                                return;
                                                            }
                                                        default:
                                                            {
                                                                if ((itemcode.StartsWith("CZ") || itemcode.StartsWith("CC") || itemcode.StartsWith("CR") || itemcode.StartsWith("CB")) && !itemcode.StartsWith("CC0") && itemcode != "CC38")
                                                                {
                                                                    days = 3600;
                                                                }
                                                                Inventory.AddOutBoxItem(usr, itemcode, (ushort)days, count);
                                                                break;
                                                            }
                                                    }

                                                    DB.RunQuery("INSERT INTO purchases_logs (userid, log, timestamp) VALUES ('" + usr.userId + "', '" + usr.nickname + " bought " + item.Name + " for " + days + " days [" + price + " Cash - Game]', '" + Generic.timestamp + "')");

                                                    usr.cash = result;

                                                    DB.RunQuery("UPDATE users SET cash=" + result + " WHERE id='" + usr.userId + "'");
                                                    usr.send(new SP_CashItemBuy(usr));
                                                    usr.send(new SP_OutboxSend(usr));
                                                }
                                            }
                                            else
                                            {
                                                usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth));
                                            }
                                        }
                                        else
                                        {
                                            usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.InventoryFull));
                                        }
                                    }
                                    else
                                    {
                                        usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NoLongerValid));
                                    }
                                }
                                else
                                {
                                    usr.send(new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NoLongerValid));
                                }
                            }
                            break;
                        }
                    case SubCodes.Storage:
                        {
                            int action = int.Parse(getBlock(1));

                            switch (action)
                            {
                                case 2: // Move to storage box
                                    {
                                        string itemCode = getBlock(3);
                                        int invIndex = int.Parse(getBlock(2));
                                        string[] data = usr.inventory[invIndex].Split('-');
                                        if (data[0] == itemCode)
                                        {
                                            int emptyIndex = Array.IndexOf(usr.storageInventory, "^");
                                            if (emptyIndex >= 0 && emptyIndex < usr.storageInventoryMax)
                                            {
                                                usr.storageInventory[emptyIndex] = usr.inventory[invIndex];
                                                usr.inventory[invIndex] = "^";

                                                string invCode = Inventory.calculateInventory(invIndex);

                                                for (int i = 0; i < 5; i++)
                                                {
                                                    for (int j = 0; j < 8; j++)
                                                    {
                                                        if (usr.equipment[i, j] == "I" + invCode || usr.equipment[i, j] == itemCode)
                                                        {
                                                            usr.equipment[i, j] = "^";
                                                        }
                                                        else if (usr.equipment[i, j].Contains("-"))
                                                        {
                                                            string[] multipleSlot = usr.equipment[i, j].Split('-');
                                                            if (multipleSlot[0] == "I" + invCode)
                                                            {
                                                                usr.equipment[i, j] = multipleSlot[1];
                                                            }
                                                            else if (multipleSlot[1] == "I" + invCode)
                                                            {
                                                                usr.equipment[i, j] = multipleSlot[0];
                                                            }
                                                        }
                                                    }
                                                }

                                                usr.SaveEquipment();

                                                usr.send(new SP_StorageInventoryUpdate(usr, action, invIndex, itemCode));

                                                usr.send(new SP_UpdateInventory(usr, null));

                                                DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(usr) + "', storage = '" + Inventory.Storage(usr) + "' WHERE ownerid = '" + usr.userId + "'");
                                            }
                                            else
                                            {
                                                usr.send(new SP_StorageInventoryUpdate(SP_StorageInventoryUpdate.ErrorCode.NoStorageFreeSpace));
                                            }
                                        }
                                        break;
                                    }
                                case 3: // Move to inventory
                                    {
                                        string itemCode = getBlock(3);
                                        int invIndex = int.Parse(getBlock(2));
                                        int wrTime = Generic.WarRockDateTime;
                                        string[] data = usr.storageInventory[invIndex].Split('-');
                                        if (data[0] == itemCode)
                                        {
                                            int time = int.Parse(data[3]);
                                            if (time > wrTime)
                                            {
                                                if (usr.HasItem(itemCode))
                                                {
                                                    usr.storageInventory[invIndex] = "^";

                                                    int inventoryIndex = usr.GetItemIndex(itemCode);

                                                    string[] inventoryString = usr.inventory[inventoryIndex].Split('-');

                                                    DateTime dtStored = DateTime.ParseExact(inventoryString[3], "yyMMddHH", null);

                                                    TimeSpan ts = dtStored - DateTime.Now;

                                                    Inventory.AddItem(usr, itemCode, (int)ts.TotalDays);

                                                    usr.send(new SP_StorageInventoryUpdate(usr, action, invIndex, itemCode));
                                                }
                                                else
                                                {
                                                    int emptyIndex = Array.IndexOf(usr.inventory, "^");
                                                    if (emptyIndex >= 0)
                                                    {
                                                        usr.inventory[emptyIndex] = usr.storageInventory[invIndex];
                                                        usr.storageInventory[invIndex] = "^";

                                                        usr.send(new SP_StorageInventoryUpdate(usr, action, invIndex, itemCode));
                                                    }
                                                    else
                                                    {
                                                        usr.send(new SP_StorageInventoryUpdate(SP_StorageInventoryUpdate.ErrorCode.NoInventoryFreeSpace));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                usr.storageInventory[invIndex] = "^";

                                                usr.send(new SP_StorageInventoryList(usr));
                                            }
                                            DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(usr) + "', storage = '" + Inventory.Storage(usr) + "' WHERE ownerid = '" + usr.userId + "'");
                                        }
                                        break;
                                    }
                                case 4: // Remove all expired items
                                    {
                                        int wrTime = Generic.WarRockDateTime;
                                        for (int i = 0; i < usr.storageInventory.Length; i++)
                                        {
                                            try
                                            {
                                                string data = usr.storageInventory[i];
                                                if (data != "^")
                                                {
                                                    string[] split = data.Split('-');
                                                    int time = int.Parse(split[3]);
                                                    if (time < wrTime)
                                                    {
                                                        usr.storageInventory[i] = "^";
                                                    }
                                                }
                                            }
                                            catch (Exception e) { Console.WriteLine(e); }
                                        }


                                        usr.send(new SP_StorageInventoryList(usr));
                                        DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(usr) + "', storage = '" + Inventory.Storage(usr) + "' WHERE ownerid = '" + usr.userId + "'");
                                        break;
                                    }
                            }

                            break;
                        }
                    case SubCodes.OnItemUse:
                        string block1 = this.getBlock(4);
                        if (!usr.HasItem(block1))
                            break;
                        if (PackageManager.AddItem(usr, block1))
                        {
                            usr.send((Packet)new SP_UseItem(usr, block1));
                            break;
                        }
                        int num3;
                        switch (block1)
                        {
                            case "CB01":
                                string block2 = this.getBlock(5);
                                if (!Game_Server.Generic.IsAlphaNumeric(block2) || DB.RunReader("SELECT * FROM users WHERE nickname='" + block2 + "'").Rows.Count != 0)
                                    return;
                                DB.RunQuery("INSERT INTO changenick_logs (userId, oldnick, newnick, date, timestamp) VALUES ('" + (object)usr.userId + "', '" + usr.nickname + "', '" + block2 + "', '" + Game_Server.Generic.currentDate + "', '" + (object)Game_Server.Generic.timestamp + "')");
                                Log.WriteLine("---" + usr.nickname + " is now known as " + block2 + "---");
                                usr.nickname = block2;
                                DB.RunQuery("UPDATE users SET nickname='" + usr.nickname + "' WHERE id='" + (object)usr.userId + "'");
                                usr.deleteItem(block1);
                                usr.send((Packet)new SP_CashItemUse(usr, block1));
                                num3 = usr.clan.clanRank(usr);
                                if (num3 != 9)
                                {
                                    ClanUsers user = usr.clan.GetUser(usr.userId);
                                    if (user == null)
                                        return;
                                    user.EXP = usr.exp.ToString();
                                    user.nickname = usr.nickname;
                                    return;
                                }
                                ClanPendingUsers pendingUser = usr.clan.getPendingUser(usr.userId);
                                if (pendingUser == null)
                                    return;
                                pendingUser.EXP = usr.exp.ToString();
                                pendingUser.nickname = usr.nickname;
                                return;

                            case "CB03":
                                Game_Server.User user1 = usr;
                                usr.deaths = num3 = 0;
                                int num4 = num3;
                                user1.kills = num4;
                                DB.RunQuery("UPDATE users SET kills = '" + (object)usr.kills + "', deaths = '" + (object)usr.deaths + "' WHERE id='" + (object)usr.userId + "'");
                                usr.deleteItem(block1);
                                usr.send((Packet)new SP_CashItemUse(usr, block1));
                                return;
                            case "CZ73":
                                string block3 = this.getBlock(5);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                string[] strArray1 = block3.Split(' ');
                                for (int index = 0; index < strArray1.Length; ++index)
                                    strArray1[index] = WordFilterManager.Replace(strArray1[index]);
                                string Message = string.Join(" ", strArray1);
                                usr.send((Packet)new SP_CashItemUse(usr, "CB03"));
                                usr.AddAdminCPLog(usr.nickname + " sent message " + Message.Replace('\x001D', ' ') + " [HAM_RADIO]");
                                UserManager.sendToServer((Packet)new SP_Chat(usr.nickname, SP_Chat.ChatType.Notice1, Message, 0U, usr.nickname));
                                return;
                            /*  case "CB99": // need right exe 2017 11 15
                                  DB.RunQuery("UPDATE users SET plusinvslots" + 10 + " WHERE id = '" + usr.userId + "'");
                                  //usr.send(new SP_CashItemBuy(usr));
                                  usr.send(new SP_CustomMessageBox("Extended 10 inv Slots"));
                                  usr.MaxSlots += +10;
                                  usr.send((Packet)new SP_CashItemUse(usr, "CB99"));
                                  Inventory.DecreaseEAItem(usr, "CB99");
                                  return;*/
                            case "CM12": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 100000 Dinars Congratz"));
                                usr.dinar += 100000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                return;
                            /*
                             case "CM07":
                                {


                                    usr.dinar += 10000;

                                    usr.send(new SP_DinarItemBuy(usr, itemcode));
                                    DB.RunQuery("UPDATE users SET dinar=" + 10000 + " WHERE id='" + usr.userId + "'");
                                    return;
                                }*/
                            case "CM06": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 5000 Dinars Congratz"));
                                usr.dinar += 5000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM05": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 3000 Dinars Congratz"));
                                usr.dinar += 3000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM04": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 2000 Dinars Congratz"));
                                usr.dinar += 2000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM03": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 1000 Dinars Congratz"));
                                usr.dinar += 1000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM02": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 500 Dinars Congratz"));
                                usr.dinar += 500;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM01": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 300 Dinars Congratz"));
                                usr.dinar += 300;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM07": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 10000 Dinars Congratz"));
                                usr.dinar += 10000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM11": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 80000 Dinars Congratz"));
                                usr.dinar += 80000;
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM10": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 50000 Dinars Congratz"));
                                usr.dinar += 50000;
                                //usr.send(new SP_OutboxUse(itemcode));
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM13": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 250000 Dinars Congratz"));
                                usr.dinar += 250000;
                                //usr.send(new SP_OutboxUse(itemcode));
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM09": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 30000 Dinars Congratz"));
                                usr.dinar += 30000;
                                //usr.send(new SP_OutboxUse(itemcode));
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                            case "CM08": // Need Custom Msg box address
                                DB.RunQuery("UPDATE users SET dinar = " + usr.dinar + " WHERE id = '" + usr.userId + "'");
                                usr.send(new SP_CashItemBuy(usr));
                                usr.send(new SP_CustomMessageBox("You Got 20000 Dinars Congratz"));
                                usr.dinar += 20000;
                                //usr.send(new SP_OutboxUse(itemcode));
                                usr.send(new SP_OutboxUse(usr, itemcode));
                                usr.RefreshDinars();
                                return;
                       
                            case "CY02":
                                int num66 = Game_Server.Generic.random(0, 5);
                                int days = 1;
                                string itemcode15 = (string)null;
                                switch (num66)
                                {

                                    case 0:
                                        itemcode15 = "DC15";
                                        days = -1;
                                        break;
                                    case 1:
                                        itemcode15 = "DC15";
                                        days = 15;
                                        break;
                                    case 2:
                                        itemcode15 = "DC15";
                                        days = 30;
                                        break;
                                    case 3:
                                        itemcode15 = "CF01";
                                        days = 15;
                                        break;
                                    case 4:
                                        itemcode15 = "CIO1";
                                        days = 15;
                                        break;
                                    case 5:
                                        itemcode15 = "CA01";
                                        days = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode15, days);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode15, days));
                                return;
                            case "CY82":
                                int num6711 = Game_Server.Generic.random(0, 50);
                                int days112345 = 1;
                                string itemcode112345 = (string)null;
                                switch (num6711)
                                {

                                    case 0:
                                        itemcode112345 = "DF36";
                                        days112345 = 15;
                                        break;
                                    case 1:
                                        itemcode112345 = "DF36";
                                        days112345 = 30;
                                        break;
                                    case 2:
                                        itemcode112345 = "DF36";
                                        days112345 = -1;
                                        break;
                                    case 3:
                                        itemcode112345 = "DF15";
                                        days112345 = 15;
                                        break;
                                    case 4:
                                        itemcode112345 = "DF15";
                                        days112345 = 30;
                                        break;
                                    case 5:
                                        itemcode112345 = "DF15";
                                        days112345 = -1;
                                        break;
                                    case 6:
                                        itemcode112345 = "DF17";
                                        days112345 = 15;
                                        break;
                                    case 7:
                                        itemcode112345 = "DF17";
                                        days112345 = 30;
                                        break;
                                    case 8:
                                        itemcode112345 = "DF17";
                                        days112345 = -1;
                                        break;
                                    case 9:
                                        itemcode112345 = "DF66";
                                        days112345 = 15;
                                        break;
                                    case 10:
                                        itemcode112345 = "DF66";
                                        days112345 = 30;
                                        break;
                                    case 11:
                                        itemcode112345 = "DF66";
                                        days112345 = -1;
                                        break;
                                    case 12:
                                        itemcode112345 = "DF96";
                                        days112345 = 15;
                                        break;
                                    case 13:
                                        itemcode112345 = "DF96";
                                        days112345 = 30;
                                        break;
                                    case 14:
                                        itemcode112345 = "DF96";
                                        days112345 = -1;
                                        break;
                                    case 15:
                                        itemcode112345 = "DF26";
                                        days112345 = 15;
                                        break;
                                    case 16:
                                        itemcode112345 = "DF26";
                                        days112345 = 30;
                                        break;
                                    case 17:
                                        itemcode112345 = "DF26";
                                        days112345 = -1;
                                        break;
                                    case 18:
                                        itemcode112345 = "DF38";
                                        days112345 = 15;
                                        break;
                                    case 19:
                                        itemcode112345 = "DF38";
                                        days112345 = 30;
                                        break;
                                    case 20:
                                        itemcode112345 = "DF38";
                                        days112345 = -1;
                                        break;
                                    case 21:
                                        itemcode112345 = "DF39";
                                        days112345 = 15;
                                        break;
                                    case 22:
                                        itemcode112345 = "DF39";
                                        days112345 = 30;
                                        break;
                                    case 23:
                                        itemcode112345 = "DF39";
                                        days112345 = -1;
                                        break;
                                    case 24:
                                        itemcode112345 = "DF42";
                                        days112345 = 15;
                                        break;
                                    case 25:
                                        itemcode112345 = "DF42";
                                        days112345 = 30;
                                        break;
                                    case 26:
                                        itemcode112345 = "DF42";
                                        days112345 = -1;
                                        break;
                                    case 27:
                                        itemcode112345 = "DF81";
                                        days112345 = 15;
                                        break;
                                    case 28:
                                        itemcode112345 = "DF81";
                                        days112345 = 30;
                                        break;
                                    case 29:
                                        itemcode112345 = "DF81";
                                        days112345 = -1;
                                        break;
                                    case 30:
                                        itemcode112345 = "DF82";
                                        days112345 = 15;
                                        break;
                                    case 31:
                                        itemcode112345 = "DF82";
                                        days112345 = 30;
                                        break;
                                    case 32:
                                        itemcode112345 = "DF82";
                                        days112345 = -1;
                                        break;
                                    case 33:
                                        itemcode112345 = "DF92";
                                        days112345 = 15;
                                        break;
                                    case 34:
                                        itemcode112345 = "DF92";
                                        days112345 = 30;
                                        break;
                                    case 35:
                                        itemcode112345 = "DF92";
                                        days112345 = -1;
                                        break;
                                    case 36:
                                        itemcode112345 = "GF10";
                                        days112345 = 15;
                                        break;
                                    case 37:
                                        itemcode112345 = "GF10";
                                        days112345 = 30;
                                        break;
                                    case 38:
                                        itemcode112345 = "GF10";
                                        days112345 = -1;
                                        break;
                                    case 39:
                                        itemcode112345 = "GF27";
                                        days112345 = 15;
                                        break;
                                    case 40:
                                        itemcode112345 = "GF27";
                                        days112345 = 30;
                                        break;
                                    case 41:
                                        itemcode112345 = "GF27";
                                        days112345 = -1;
                                        break;
                                    case 42:
                                        itemcode112345 = "GF40";
                                        days112345 = 15;
                                        break;
                                    case 43:
                                        itemcode112345 = "GF40";
                                        days112345 = 30;
                                        break;
                                    case 44:
                                        itemcode112345 = "GF40";
                                        days112345 = -1;
                                        break;
                                    case 45:
                                        itemcode112345 = "GF58";
                                        days112345 = 15;
                                        break;
                                    case 46:
                                        itemcode112345 = "GF58";
                                        days112345 = 30;
                                        break;
                                    case 47:
                                        itemcode112345 = "GF58";
                                        days112345 = -1;
                                        break;
                                    case 48:
                                        itemcode112345 = "GF59";
                                        days112345 = 15;
                                        break;
                                    case 49:
                                        itemcode112345 = "GF59";
                                        days112345 = 30;
                                        break;
                                    case 50:
                                        itemcode112345 = "GF59";
                                        days112345 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode112345, days112345);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode112345, days112345));
                                return;
                            case "CY80":
                                int num6714 = Game_Server.Generic.random(0, 53);
                                int days1123488 = 1;
                                string itemcode112348 = (string)null;
                                switch (num6714)
                                {
                                    case 0:
                                        itemcode112348 = "DF18";
                                        days1123488 = 15;
                                        break;
                                    case 1:
                                        itemcode112348 = "DF18";
                                        days1123488 = 30;
                                        break;
                                    case 2:
                                        itemcode112348 = "DF18";
                                        days1123488 = -1;
                                        break;
                                    case 3:
                                        itemcode112348 = "DF48";
                                        days1123488 = 15;
                                        break;
                                    case 4:
                                        itemcode112348 = "DF48";
                                        days1123488 = 30;
                                        break;
                                    case 5:
                                        itemcode112348 = "DF48";
                                        days1123488 = -1;
                                        break;
                                    case 6:
                                        itemcode112348 = "DF55";
                                        days1123488 = 15;
                                        break;
                                    case 7:
                                        itemcode112348 = "DF55";
                                        days1123488 = 30;
                                        break;
                                    case 8:
                                        itemcode112348 = "DF55";
                                        days1123488 = -1;
                                        break;
                                    case 9:
                                        itemcode112348 = "DF57";
                                        days1123488 = 15;
                                        break;
                                    case 10:
                                        itemcode112348 = "DF57";
                                        days1123488 = 30;
                                        break;
                                    case 11:
                                        itemcode112348 = "DF57";
                                        days1123488 = -1;
                                        break;
                                    case 12:
                                        itemcode112348 = "DF76";
                                        days1123488 = 15;
                                        break;
                                    case 13:
                                        itemcode112348 = "DF76";
                                        days1123488 = 30;
                                        break;
                                    case 14:
                                        itemcode112348 = "DF76";
                                        days1123488 = -1;
                                        break;
                                    case 15:
                                        itemcode112348 = "DF78";
                                        days1123488 = 15;
                                        break;
                                    case 16:
                                        itemcode112348 = "DF78";
                                        days1123488 = 30;
                                        break;
                                    case 17:
                                        itemcode112348 = "DF78";
                                        days1123488 = -1;
                                        break;
                                    case 18:
                                        itemcode112348 = "DF83";
                                        days1123488 = 15;
                                        break;
                                    case 19:
                                        itemcode112348 = "DF83";
                                        days1123488 = 30;
                                        break;
                                    case 20:
                                        itemcode112348 = "DF83";
                                        days1123488 = -1;
                                        break;
                                    case 21:
                                        itemcode112348 = "DF90";
                                        days1123488 = 15;
                                        break;
                                    case 22:
                                        itemcode112348 = "DF90";
                                        days1123488 = 30;
                                        break;
                                    case 23:
                                        itemcode112348 = "DF90";
                                        days1123488 = -1;
                                        break;
                                    case 24:
                                        itemcode112348 = "DF93";
                                        days1123488 = 15;
                                        break;
                                    case 25:
                                        itemcode112348 = "DF93";
                                        days1123488 = 30;
                                        break;
                                    case 26:
                                        itemcode112348 = "DF93";
                                        days1123488 = -1;
                                        break;
                                    case 27:
                                        itemcode112348 = "GF04";
                                        days1123488 = 15;
                                        break;
                                    case 28:
                                        itemcode112348 = "GF04";
                                        days1123488 = 30;
                                        break;
                                    case 29:
                                        itemcode112348 = "GF04";
                                        days1123488 = -1;
                                        break;
                                    case 30:
                                        itemcode112348 = "GF05";
                                        days1123488 = 15;
                                        break;
                                    case 31:
                                        itemcode112348 = "GF05";
                                        days1123488 = 30;
                                        break;
                                    case 32:
                                        itemcode112348 = "GF05";
                                        days1123488 = -1;
                                        break;
                                    case 33:
                                        itemcode112348 = "GF07";
                                        days1123488 = 15;
                                        break;
                                    case 34:
                                        itemcode112348 = "GF07";
                                        days1123488 = 30;
                                        break;
                                    case 35:
                                        itemcode112348 = "GF07";
                                        days1123488 = -1;
                                        break;
                                    case 36:
                                        itemcode112348 = "GF16";
                                        days1123488 = 15;
                                        break;
                                    case 37:
                                        itemcode112348 = "GF16";
                                        days1123488 = 30;
                                        break;
                                    case 38:
                                        itemcode112348 = "GF16";
                                        days1123488 = -1;
                                        break;
                                    case 39:
                                        itemcode112348 = "GF21";
                                        days1123488 = 15;
                                        break;
                                    case 40:
                                        itemcode112348 = "GF21";
                                        days1123488 = 30;
                                        break;
                                    case 41:
                                        itemcode112348 = "GF21";
                                        days1123488 = -1;
                                        break;
                                    case 42:
                                        itemcode112348 = "GF24";
                                        days1123488 = 15;
                                        break;
                                    case 43:
                                        itemcode112348 = "GF24";
                                        days1123488 = 30;
                                        break;
                                    case 44:
                                        itemcode112348 = "GF24";
                                        days1123488 = -1;
                                        break;
                                    case 45:
                                        itemcode112348 = "GF28";
                                        days1123488 = 15;
                                        break;
                                    case 46:
                                        itemcode112348 = "GF28";
                                        days1123488 = 30;
                                        break;
                                    case 47:
                                        itemcode112348 = "GF28";
                                        days1123488 = -1;
                                        break;
                                    case 48:
                                        itemcode112348 = "GF37";
                                        days1123488 = 15;
                                        break;
                                    case 49:
                                        itemcode112348 = "GF37";
                                        days1123488 = 30;
                                        break;
                                    case 50:
                                        itemcode112348 = "GF37";
                                        days1123488 = -1;
                                        break;
                                    case 51:
                                        itemcode112348 = "GF55";
                                        days1123488 = 15;
                                        break;
                                    case 52:
                                        itemcode112348 = "GF55";
                                        days1123488 = 30;
                                        break;
                                    case 53:
                                        itemcode112348 = "GF55";
                                        days1123488 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode112348, days1123488);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode112348, days1123488));
                                return;
                            case "CY83":
                                int num6713 = Game_Server.Generic.random(0, 31);
                                int days112347 = 1;
                                string itemcode112347 = (string)null;
                                switch (num6713)
                                {
                                    case 0:
                                        itemcode112347 = "DJ17";
                                        days112347 = 7;
                                        break;
                                    case 1:
                                        itemcode112347 = "DJ17";
                                        days112347 = 15;
                                        break;
                                    case 2:
                                        itemcode112347 = "DJ17";
                                        days112347 = 30;
                                        break;
                                    case 3:
                                        itemcode112347 = "DJ17";
                                        days112347 = -1;
                                        break;
                                    case 4:
                                        itemcode112347 = "DJ43";
                                        days112347 = 7;
                                        break;
                                    case 5:
                                        itemcode112347 = "DJ43";
                                        days112347 = 15;
                                        break;
                                    case 6:
                                        itemcode112347 = "DJ43";
                                        days112347 = 30;
                                        break;
                                    case 7:
                                        itemcode112347 = "DJ43";
                                        days112347 = -1;
                                        break;
                                    case 8:
                                        itemcode112347 = "DJ16";
                                        days112347 = 7;
                                        break;
                                    case 9:
                                        itemcode112347 = "DJ16";
                                        days112347 = 15;
                                        break;
                                    case 10:
                                        itemcode112347 = "DJ16";
                                        days112347 = 30;
                                        break;
                                    case 11:
                                        itemcode112347 = "DJ16";
                                        days112347 = -1;
                                        break;
                                    case 12:
                                        itemcode112347 = "DJ78";
                                        days112347 = 7;
                                        break;
                                    case 13:
                                        itemcode112347 = "DJ78";
                                        days112347 = 15;
                                        break;
                                    case 14:
                                        itemcode112347 = "DJ78";
                                        days112347 = 30;
                                        break;
                                    case 15:
                                        itemcode112347 = "DJ78";
                                        days112347 = -1;
                                        break;
                                    case 16:
                                        itemcode112347 = "DJ47";
                                        days112347 = 7;
                                        break;
                                    case 17:
                                        itemcode112347 = "DJ47";
                                        days112347 = 15;
                                        break;
                                    case 18:
                                        itemcode112347 = "DJ47";
                                        days112347 = 30;
                                        break;
                                    case 19:
                                        itemcode112347 = "DJ47";
                                        days112347 = -1;
                                        break;
                                    case 20:
                                        itemcode112347 = "DJ64";
                                        days112347 = 7;
                                        break;
                                    case 21:
                                        itemcode112347 = "DJ64";
                                        days112347 = 15;
                                        break;
                                    case 22:
                                        itemcode112347 = "DJ64";
                                        days112347 = 30;
                                        break;
                                    case 23:
                                        itemcode112347 = "DJ64";
                                        days112347 = -1;
                                        break;
                                    case 24:
                                        itemcode112347 = "DJ71";
                                        days112347 = 7;
                                        break;
                                    case 25:
                                        itemcode112347 = "DJ71";
                                        days112347 = 15;
                                        break;
                                    case 26:
                                        itemcode112347 = "DJ71";
                                        days112347 = 30;
                                        break;
                                    case 27:
                                        itemcode112347 = "DJ71";
                                        days112347 = -1;
                                        break;
                                    case 28:
                                        itemcode112347 = "DJ58";
                                        days112347 = 7;
                                        break;
                                    case 29:
                                        itemcode112347 = "DJ58";
                                        days112347 = 15;
                                        break;
                                    case 30:
                                        itemcode112347 = "DJ58";
                                        days112347 = 30;
                                        break;
                                    case 31:
                                        itemcode112347 = "DJ58";
                                        days112347 = -1;
                                        break;



                                }
                                Inventory.AddItem(usr, itemcode112347, days112347);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode112347, days112347));
                                return;
                            case "CY81":
                                int num6712 = Game_Server.Generic.random(0, 68);
                                int days112346 = 1;
                                string itemcode112346 = (string)null;
                                switch (num6712)
                                {
                                    case 0:
                                        itemcode112346 = "DC64";
                                        days112346 = 15;
                                        break;
                                    case 1:
                                        itemcode112346 = "DC64";
                                        days112346 = 30;
                                        break;
                                    case 2:
                                        itemcode112346 = "DC64";
                                        days112346 = -1;
                                        break;
                                    case 3:
                                        itemcode112346 = "DC16";
                                        days112346 = 15;
                                        break;
                                    case 4:
                                        itemcode112346 = "DC16";
                                        days112346 = 30;
                                        break;
                                    case 5:
                                        itemcode112346 = "DC16";
                                        days112346 = -1;
                                        break;
                                    case 6:
                                        itemcode112346 = "DC34";
                                        days112346 = 15;
                                        break;
                                    case 7:
                                        itemcode112346 = "DC34";
                                        days112346 = 30;
                                        break;
                                    case 8:
                                        itemcode112346 = "DC34";
                                        days112346 = -1;
                                        break;
                                    case 9:
                                        itemcode112346 = "DC19";
                                        days112346 = 15;
                                        break;
                                    case 10:
                                        itemcode112346 = "DC19";
                                        days112346 = 30;
                                        break;
                                    case 11:
                                        itemcode112346 = "DC19";
                                        days112346 = -1;
                                        break;
                                    case 12:
                                        itemcode112346 = "DC67";
                                        days112346 = 15;
                                        break;
                                    case 13:
                                        itemcode112346 = "DC67";
                                        days112346 = 30;
                                        break;
                                    case 14:
                                        itemcode112346 = "DC67";
                                        days112346 = -1;
                                        break;
                                    case 15:
                                        itemcode112346 = "DC94";
                                        days112346 = 15;
                                        break;
                                    case 16:
                                        itemcode112346 = "DC94";
                                        days112346 = 30;
                                        break;
                                    case 17:
                                        itemcode112346 = "DC94";
                                        days112346 = -1;
                                        break;
                                    case 18:
                                        itemcode112346 = "DC95";
                                        days112346 = 15;
                                        break;
                                    case 19:
                                        itemcode112346 = "DC95";
                                        days112346 = 30;
                                        break;
                                    case 20:
                                        itemcode112346 = "DC95";
                                        days112346 = -1;
                                        break;
                                    case 21:
                                        itemcode112346 = "DE35";
                                        days112346 = 15;
                                        break;
                                    case 22:
                                        itemcode112346 = "DE35";
                                        days112346 = 30;
                                        break;
                                    case 23:
                                        itemcode112346 = "DE35";
                                        days112346 = -1;
                                        break;

                                    case 24:
                                        itemcode112346 = "DE38";
                                        days112346 = 15;
                                        break;
                                    case 25:
                                        itemcode112346 = "DE38";
                                        days112346 = 30;
                                        break;
                                    case 26:
                                        itemcode112346 = "DE38";
                                        days112346 = -1;
                                        break;
                                    case 27:
                                        itemcode112346 = "DE37";
                                        days112346 = 15;
                                        break;
                                    case 28:
                                        itemcode112346 = "DE37";
                                        days112346 = 30;
                                        break;
                                    case 29:
                                        itemcode112346 = "DE37";
                                        days112346 = -1;
                                        break;
                                    case 30:
                                        itemcode112346 = "DE41";
                                        days112346 = 15;
                                        break;
                                    case 31:
                                        itemcode112346 = "DE41";
                                        days112346 = 30;
                                        break;
                                    case 32:
                                        itemcode112346 = "DE41";
                                        days112346 = -1;
                                        break;
                                    case 33:
                                        itemcode112346 = "DE43";
                                        days112346 = 15;
                                        break;
                                    case 34:
                                        itemcode112346 = "DE43";
                                        days112346 = 30;
                                        break;
                                    case 35:
                                        itemcode112346 = "DE43";
                                        days112346 = -1;
                                        break;
                                    case 36:
                                        itemcode112346 = "DE42";
                                        days112346 = 15;
                                        break;
                                    case 37:
                                        itemcode112346 = "DE42";
                                        days112346 = 30;
                                        break;
                                    case 38:
                                        itemcode112346 = "DE42";
                                        days112346 = -1;
                                        break;
                                    case 39:
                                        itemcode112346 = "DE44";
                                        days112346 = 15;
                                        break;
                                    case 40:
                                        itemcode112346 = "DE44";
                                        days112346 = 30;
                                        break;
                                    case 41:
                                        itemcode112346 = "DE44";
                                        days112346 = -1;
                                        break;
                                    case 42:
                                        itemcode112346 = "DE76";
                                        days112346 = 15;
                                        break;
                                    case 43:
                                        itemcode112346 = "DE76";
                                        days112346 = 30;
                                        break;
                                    case 44:
                                        itemcode112346 = "DE76";
                                        days112346 = -1;
                                        break;
                                    case 45:
                                        itemcode112346 = "DE91";
                                        days112346 = 15;
                                        break;
                                    case 46:
                                        itemcode112346 = "DE91";
                                        days112346 = 30;
                                        break;
                                    case 47:
                                        itemcode112346 = "DE91";
                                        days112346 = -1;
                                        break;
                                    case 48:
                                        itemcode112346 = "GC03";
                                        days112346 = 15;
                                        break;
                                    case 49:
                                        itemcode112346 = "GC03";
                                        days112346 = 30;
                                        break;
                                    case 50:
                                        itemcode112346 = "GC03";
                                        days112346 = -1;
                                        break;
                                    case 51:
                                        itemcode112346 = "GC09";
                                        days112346 = 15;
                                        break;
                                    case 52:
                                        itemcode112346 = "GC09";
                                        days112346 = 30;
                                        break;
                                    case 53:
                                        itemcode112346 = "GC09";
                                        days112346 = -1;
                                        break;
                                    case 54:
                                        itemcode112346 = "GC18";
                                        days112346 = 15;
                                        break;
                                    case 55:
                                        itemcode112346 = "GC18";
                                        days112346 = 30;
                                        break;
                                    case 56:
                                        itemcode112346 = "GC18";
                                        days112346 = -1;
                                        break;
                                    case 57:
                                        itemcode112346 = "GC23";
                                        days112346 = 15;
                                        break;
                                    case 58:
                                        itemcode112346 = "GC23";
                                        days112346 = 30;
                                        break;
                                    case 59:
                                        itemcode112346 = "GC23";
                                        days112346 = -1;
                                        break;
                                    case 60:
                                        itemcode112346 = "DC17";
                                        days112346 = 15;
                                        break;
                                    case 61:
                                        itemcode112346 = "DC17";
                                        days112346 = 30;
                                        break;
                                    case 62:
                                        itemcode112346 = "DC17";
                                        days112346 = -1;
                                        break;
                                    case 63:
                                        itemcode112346 = "GC25";
                                        days112346 = 15;
                                        break;
                                    case 64:
                                        itemcode112346 = "GC25";
                                        days112346 = 30;
                                        break;
                                    case 65:
                                        itemcode112346 = "GC25";
                                        days112346 = -1;
                                        break;
                                    case 66:
                                        itemcode112346 = "DC04";
                                        days112346 = 15;
                                        break;
                                    case 67:
                                        itemcode112346 = "GC25";
                                        days112346 = 30;
                                        break;
                                    case 68:
                                        itemcode112346 = "DC04";
                                        days112346 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode112346, days112346);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode112346, days112346));
                                return;
                            case "CY91":
                                int num12345 = Game_Server.Generic.random(0, 27);
                                int days11234 = 1;
                                string itemcode11234 = (string)null;
                                switch (num12345)
                                {
                                    case 0:
                                        itemcode11234 = "DH01";
                                        days11234 = 15;
                                        break;
                                    case 1:
                                        itemcode11234 = "DH01";
                                        days11234 = 30;
                                        break;
                                    case 2:
                                        itemcode11234 = "DH01";
                                        days11234 = 45;
                                        break;
                                    case 3:
                                        itemcode11234 = "DH01";
                                        days11234 = -1;
                                        break;
                                    case 4:
                                        itemcode11234 = "D803";
                                        days11234 = 15;
                                        break;
                                    case 5:
                                        itemcode11234 = "D803";
                                        days11234 = 30;
                                        break;
                                    case 6:
                                        itemcode11234 = "D803";
                                        days11234 = 45;
                                        break;
                                    case 7:
                                        itemcode11234 = "D803";
                                        days11234 = -1;
                                        break;
                                    case 8:
                                        itemcode11234 = "D831";
                                        days11234 = 15;
                                        break;
                                    case 9:
                                        itemcode11234 = "D831";
                                        days11234 = 30;
                                        break;
                                    case 10:
                                        itemcode11234 = "D831";
                                        days11234 = 45;
                                        break;
                                    case 11:
                                        itemcode11234 = "D831";
                                        days11234 = -1;
                                        break;
                                    case 12:
                                        itemcode11234 = "DH13";
                                        days11234 = 15;
                                        break;
                                    case 13:
                                        itemcode11234 = "DH13";
                                        days11234 = 30;
                                        break;
                                    case 14:
                                        itemcode11234 = "DH13";
                                        days11234 = 45;
                                        break;
                                    case 15:
                                        itemcode11234 = "DH13";
                                        days11234 = -1;
                                        break;
                                    case 16:
                                        itemcode11234 = "DH14";
                                        days11234 = 15;
                                        break;
                                    case 17:
                                        itemcode11234 = "DH14";
                                        days11234 = 30;
                                        break;
                                    case 18:
                                        itemcode11234 = "DH14";
                                        days11234 = 45;
                                        break;
                                    case 19:
                                        itemcode11234 = "DH14";
                                        days11234 = -1;
                                        break;
                                    case 20:
                                        itemcode11234 = "DH17";
                                        days11234 = 15;
                                        break;
                                    case 21:
                                        itemcode11234 = "DH17";
                                        days11234 = 30;
                                        break;
                                    case 22:
                                        itemcode11234 = "DH17";
                                        days11234 = 45;
                                        break;
                                    case 23:
                                        itemcode11234 = "DH17";
                                        days11234 = -1;
                                        break;
                                    case 24:
                                        itemcode11234 = "DH23";
                                        days11234 = 15;
                                        break;
                                    case 25:
                                        itemcode11234 = "DH23";
                                        days11234 = 30;
                                        break;
                                    case 26:
                                        itemcode11234 = "DH23";
                                        days11234 = 45;
                                        break;
                                    case 27:
                                        itemcode11234 = "DH23";
                                        days11234 = -1;
                                        break;
                               

                                }
                                Inventory.AddItem(usr, itemcode11234, days11234);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode11234, days11234));
                                return;
                            case "CY28":
                                int num5123 = Game_Server.Generic.random(0, 27);
                                int days700 = 1;
                                string itemcode680 = (string)null;
                                switch (num5123)
                                {
                                    case 0:
                                        itemcode680 = "GF10";
                                        days700 = -1;
                                        break;
                                    case 1:
                                        itemcode680 = "GF10";
                                        days700 = 180;
                                        break;
                                    case 2:
                                        itemcode680 = "GF10";
                                        days700 = 90;
                                        break;
                                    case 3:
                                        itemcode680 = "GF10";
                                        days700 = 60;
                                        break;
                                    case 4:
                                        itemcode680 = "GF10";
                                        days700 = 45;
                                        break;
                                    case 5:
                                        itemcode680 = "GF10";
                                        days700 = 30;
                                        break;
                                    case 6:
                                        itemcode680 = "DG95";
                                        days700 = -1;
                                        break;
                                    case 7:
                                        itemcode680 = "DG95";
                                        days700 = 180;
                                        break;
                                    case 8:
                                        itemcode680 = "DG95";
                                        days700 = 90;
                                        break;
                                    case 9:
                                        itemcode680 = "DG95";
                                        days700 = 60;
                                        break;
                                    case 10:
                                        itemcode680 = "DG95";
                                        days700 = 45;
                                        break;
                                    case 11:
                                        itemcode680 = "DG95";
                                        days700 = 30;
                                        break;
                                    case 12:
                                        itemcode680 = "DE77";
                                        days700 = -1;
                                        break;
                                    case 13:
                                        itemcode680 = "DE77";
                                        days700 = 180;
                                        break;
                                    case 14:
                                        itemcode680 = "DE77";
                                        days700 = 90;
                                        break;
                                    case 15:
                                        itemcode680 = "DE77";
                                        days700 = 60;
                                        break;
                                    case 16:
                                        itemcode680 = "DE77";
                                        days700 = 45;
                                        break;
                                    case 17:
                                        itemcode680 = "DE77";
                                        days700 = 30;
                                        break;
                                    case 18:
                                        itemcode680 = "DJ47";
                                        days700 = -1;
                                        break;
                                    case 19:
                                        itemcode680 = "DJ47";
                                        days700 = 180;
                                        break;
                                    case 20:
                                        itemcode680 = "DJ47";
                                        days700 = 90;
                                        break;
                                    case 21:
                                        itemcode680 = "DJ47";
                                        days700 = 60;
                                        break;
                                    case 22:
                                        itemcode680 = "DJ47";
                                        days700 = 45;
                                        break;
                                    case 23:
                                        itemcode680 = "DJ47";
                                        days700 = 30;
                                        break;
                                    case 24:
                                        itemcode680 = "CZ73";
                                        days700 = 1;
                                        break;
                                    case 25:
                                        itemcode680 = "DS01";
                                        days700 = 7;
                                        break;
                                    case 26:
                                        itemcode680 = "CI01";
                                        days700 = 15;
                                        break;
                                    case 27:
                                        itemcode680 = "CC05";
                                        days700 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode680, days700);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode680, days700));
                                return;
                            case "CY92":
                                int num1234 = Game_Server.Generic.random(0, 29);
                                int days7000 = 1;
                                string itemcode6801 = (string)null;
                                switch (num1234)
                                {
                                    case 0:
                                        itemcode6801 = "DF22";
                                        days7000 = 60;
                                        break;
                                    case 1:
                                        itemcode6801 = "DF22";
                                        days7000 = -1;
                                        break;
                                    case 2:
                                        itemcode6801 = "DF07";
                                        days7000 = 60;
                                        break;
                                    case 3:
                                        itemcode6801 = "DF07";
                                        days7000 = -1;
                                        break;
                                    case 4:
                                        itemcode6801 = "DC65";
                                        days7000 = 60;
                                        break;
                                    case 5:
                                        itemcode6801 = "DC65";
                                        days7000 = -1;
                                        break;
                                    case 6:
                                        itemcode6801 = "DB18";
                                        days7000 = 60;
                                        break;
                                    case 7:
                                        itemcode6801 = "DB18";
                                        days7000 = -1;
                                        break;
                                    case 8:
                                        itemcode6801 = "DF49";
                                        days7000 = 60;
                                        break;
                                    case 9:
                                        itemcode6801 = "DF49";
                                        days7000 = -1;
                                        break;
                                    case 10:
                                        itemcode6801 = "D821";
                                        days7000 = 60;
                                        break;
                                    case 11:
                                        itemcode6801 = "D821";
                                        days7000 = -1;
                                        break;
                                    case 12:
                                        itemcode6801 = "D605";
                                        days7000 = 60;
                                        break;
                                    case 13:
                                        itemcode6801 = "D605";
                                        days7000 = -1;
                                        break;
                                    case 14:
                                        itemcode6801 = "DB79";
                                        days7000 = 60;
                                        break;
                                    case 15:
                                        itemcode6801 = "DB79";
                                        days7000 = -1;
                                        break;
                                    case 16:
                                        itemcode6801 = "GC34";
                                        days7000 = 60;
                                        break;
                                    case 17:
                                        itemcode6801 = "GC34";
                                        days7000 = -1;
                                        break;
                                    case 18:
                                        itemcode6801 = "DB71";
                                        days7000 = 60;
                                        break;
                                    case 19:
                                        itemcode6801 = "DB71";
                                        days7000 = -1;
                                        break;
                                    case 20:
                                        itemcode6801 = "DF27";
                                        days7000 = 60;
                                        break;
                                    case 21:
                                        itemcode6801 = "DF27";
                                        days7000 = -1;
                                        break;
                                    case 22:
                                        itemcode6801 = "DF68";
                                        days7000 = 60;
                                        break;
                                    case 23:
                                        itemcode6801 = "DF68";
                                        days7000 = -1;
                                        break;
                                    case 24:
                                        itemcode6801 = "GF54";
                                        days7000 = 60;
                                        break;
                                    case 25:
                                        itemcode6801 = "GF54";
                                        days7000 = -1;
                                        break;
                                    case 26:
                                        itemcode6801 = "GF32";
                                        days7000 = 60;
                                        break;
                                    case 27:
                                        itemcode6801 = "GF32";
                                        days7000 = -1;
                                        break;
                                    case 28:
                                        itemcode6801 = "GF45";
                                        days7000 = 60;
                                        break;
                                    case 29:
                                        itemcode6801 = "GF45";
                                        days7000 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode6801, days7000);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode6801, days7000));
                                return;

                            case "CR80":
                                int num5122 = Game_Server.Generic.random(0, 8);
                                int days699 = 1;
                                string itemcode684 = (string)null;
                                switch (num5122)
                                {
                                    case 0:
                                        itemcode684 = "DD26";
                                        days699 = 30;
                                        break;
                                    case 1:
                                        itemcode684 = "BK15";
                                        days699 = 30;
                                        break;
                                    case 2:
                                        itemcode684 = "CM07";
                                        days699 = 5000;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode684, days699);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode684, days699));
                                return;
                            case "CY89":
                                int num5124 = Game_Server.Generic.random(0, 10);
                                int days7700 = 1;
                                string itemcode7700 = (string)null;
                                switch (num5124)
                                {
                                    case 0:
                                        itemcode7700 = "D901";
                                        days7700 = -1;
                                        break;
                                    case 1:
                                        itemcode7700 = "D904";
                                        days7700 = -1;
                                        break;
                                    case 2:
                                        itemcode7700 = "D834";
                                        days7700 = -1;
                                        break;
                                    case 3:
                                        itemcode7700 = "DH15";
                                        days7700 = -1;
                                        break;
                                    case 4:
                                        itemcode7700 = "DH16";
                                        days7700 = -1;
                                        break;
                                    case 5:
                                        itemcode7700 = "DH19";
                                        days7700 = -1;
                                        break;
                                    case 6:
                                        itemcode7700 = "DH20";
                                        days7700 = -1;
                                        break;
                                    case 7:
                                        itemcode7700 = "DH21";
                                        days7700 = -1;
                                        break;
                                    case 8:
                                        itemcode7700 = "D915";
                                        days7700 = -1;
                                        break;
                                    case 9:
                                        itemcode7700 = "D834";
                                        days7700 = -1;
                                        break;
                                    case 10:
                                        itemcode7700 = "DH22";
                                        days7700 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode7700, days7700);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode7700, days7700));
                                return;
                            case "CY88":
                                int num5125 = Game_Server.Generic.random(0, 39);
                                int days7701 = 1;
                                string itemcode7701 = (string)null;
                                switch (num5125)
                                {
                                    case 0:
                                        itemcode7701 = "D918";
                                        days7701 = 30;
                                        break;
                                    case 1:
                                        itemcode7701 = "D918";
                                        days7701 = -1;
                                        break;
                                    case 2:
                                        itemcode7701 = "D842";
                                        days7701 = 30;
                                        break;
                                    case 3:
                                        itemcode7701 = "D842";
                                        days7701 = -1;
                                        break;
                                    case 4:
                                        itemcode7701 = "D714";
                                        days7701 = 30;
                                        break;
                                    case 5:
                                        itemcode7701 = "D714";
                                        days7701 = -1;
                                        break;
                                    case 6:
                                        itemcode7701 = "D613";
                                        days7701 = 30;
                                        break;
                                    case 7:
                                        itemcode7701 = "D613";
                                        days7701 = -1;
                                        break;
                                    case 8:
                                        itemcode7701 = "D917";
                                        days7701 = 30;
                                        break;
                                    case 9:
                                        itemcode7701 = "D917";
                                        days7701 = -1;
                                        break;
                                    case 10:
                                        itemcode7701 = "D841";
                                        days7701 = 30;
                                        break;
                                    case 11:
                                        itemcode7701 = "D841";
                                        days7701 = -1;
                                        break;
                                    case 12:
                                        itemcode7700 = "D713";
                                        days7701 = 30;
                                        break;
                                    case 13:
                                        itemcode7701 = "D713";
                                        days7701 = -1;
                                        break;
                                    case 14:
                                        itemcode7701 = "D612";
                                        days7701 = 30;
                                        break;
                                    case 15:
                                        itemcode7701 = "D612";
                                        days7701 = -1;
                                        break;
                                    case 16:
                                        itemcode7701 = "D916";
                                        days7701 = 30;
                                        break;
                                    case 17:
                                        itemcode7701 = "D916";
                                        days7701 = -1;
                                        break;
                                    case 18:
                                        itemcode7701 = "D840";
                                        days7701 = 30;
                                        break;
                                    case 19:
                                        itemcode7701 = "D840";
                                        days7701 = -1;
                                        break;
                                    case 20:
                                        itemcode7701 = "D712";
                                        days7701 = 30;
                                        break;
                                    case 21:
                                        itemcode7701 = "D712";
                                        days7701 = -1;
                                        break;
                                    case 22:
                                        itemcode7701 = "D611";
                                        days7701 = 30;
                                        break;
                                    case 23:
                                        itemcode7701 = "D611";
                                        days7701 = -1;
                                        break;
                                    case 24:
                                        itemcode7701 = "D915";
                                        days7701 = 30;
                                        break;
                                    case 25:
                                        itemcode7701 = "D915";
                                        days7701 = -1;
                                        break;
                                    case 26:
                                        itemcode7701 = "D839";
                                        days7701 = 30;
                                        break;
                                    case 27:
                                        itemcode7701 = "D839";
                                        days7701 = -1;
                                        break;
                                    case 28:
                                        itemcode7701 = "D711";
                                        days7701 = 30;
                                        break;
                                    case 29:
                                        itemcode7701 = "D711";
                                        days7701 = -1;
                                        break;
                                    case 30:
                                        itemcode7701 = "DI19";
                                        days7701 = 30;
                                        break;
                                    case 31:
                                        itemcode7701 = "DI19";
                                        days7701 = -1;
                                        break;
                                    case 32:
                                        itemcode7701 = "D838";
                                        days7701 = 30;
                                        break;
                                    case 33:
                                        itemcode7701 = "D838";
                                        days7701 = -1;
                                        break;
                                    case 34:
                                        itemcode7701 = "D914";
                                        days7701 = 30;
                                        break;
                                    case 35:
                                        itemcode7701 = "D914";
                                        days7701 = -1;
                                        break;
                                    case 36:
                                        itemcode7701 = "D710";
                                        days7701 = 30;
                                        break;
                                    case 37:
                                        itemcode7701 = "D710";
                                        days7701 = -1;
                                        break;
                                    case 38:
                                        itemcode7701 = "D610";
                                        days7701 = 30;
                                        break;
                                    case 39:
                                        itemcode7701 = "D610";
                                        days7701 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode7701, days7701);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode7701, days7701));
                                return;

                            case "CY73":
                                int num5121 = Game_Server.Generic.random(0, 8);
                                int days698 = 1;
                                string itemcode683 = (string)null;
                                switch (num5121)
                                {
                                    case 0:
                                        itemcode683 = "BK14";
                                        days698 = 30;
                                        break;
                                    case 1:
                                        itemcode683 = "BK15";
                                        days698 = 30;
                                        break;
                                    case 2:
                                        itemcode683 = "CM07";
                                        days698 = 5000;
                                        break;
                                    case 3:
                                        itemcode683 = "GA02";
                                        days698 = 30;
                                        break;
                                    case 4:
                                        itemcode683 = "GA02";
                                        days698 = -1;
                                        break;
                                    case 5:
                                        itemcode683 = "BD16";
                                        days698 = 30;
                                        break;
                                    case 6:
                                        itemcode683 = "BD16";
                                        days698 = -1;
                                        break;
                                    case 7:
                                        itemcode683 = "CE02";
                                        days698 = 30;
                                        break;
                                    case 8:
                                        itemcode683 = "CE02";
                                        days698 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode683, days698);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode683, days698));
                                return;
                            case "CR04":
                                int num6002 = Game_Server.Generic.random(0, 19);
                                int days70002 = 1;
                                string itemcode7002 = (string)null;
                                switch (num6002)
                                {
                                    case 0:
                                        itemcode7002 = "CD02";
                                        days70002 = 7;
                                        break;
                                    case 1:
                                        itemcode7002 = "CD01";
                                        days70002 = 7;
                                        break;
                                    case 2:
                                        itemcode7002 = "CM07";
                                        days70002 = 30;
                                        break;
                                    case 3:
                                        itemcode7002 = "BV07";
                                        days70002 = 30;
                                        break;
                                    case 4:
                                        itemcode7002 = "BV10";
                                        days70002 = 30;
                                        break;
                                    case 5:
                                        itemcode7002 = "BV14";
                                        days70002 = 30;
                                        break;
                                    case 6:
                                        itemcode7002 = "BD07";
                                        days70002 = -1;
                                        break;
                                    case 7:
                                        itemcode7002 = "CE01";
                                        days70002 = 30;
                                        break;
                                    case 8:
                                        itemcode7002 = "CE02";
                                        days70002 = 15;
                                        break;
                                    case 9:
                                        itemcode7002 = "CFO2";
                                        days70002 = 15;
                                        break;
                                    case 10:
                                        itemcode7002 = "CI01";
                                        days70002 = 15;
                                        break;
                                    case 11:
                                        itemcode7002 = "CB09";
                                        days70002 = 1;
                                        break;
                                    case 12:
                                        itemcode7002 = "BS20";
                                        days70002 = 15;
                                        break;
                                    case 13:
                                        itemcode7002 = "BS21";
                                        days70002 = 15;
                                        break;
                                    case 14:
                                        itemcode7002 = "BS22";
                                        days70002 = 15;
                                        break;
                                    case 15:
                                        itemcode7002 = "BS23";
                                        days70002 = 15;
                                        break;
                                    case 16:
                                        itemcode7002 = "BS24";
                                        days70002 = 15;
                                        break;
                                    case 17:
                                        itemcode7002 = "BS25";
                                        days70002 = 15;
                                        break;
                                    case 18:
                                        itemcode7002 = "BS26";
                                        days70002 = 15;
                                        break;
                                    case 19:
                                        itemcode7002 = "BS27";
                                        days70002 = 15;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode7002, days70002);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode7002, days70002));
                                return;
                            case "CY55":
                                int num6000 = Game_Server.Generic.random(0, 77);
                                int days70001 = 1;
                                string itemcode7001 = (string)null;
                                switch (num6000)
                                {
                                    case 0:
                                        itemcode7001 = "DF42";
                                        days70001 = 30;
                                        break;
                                    case 1:
                                        itemcode7001 = "DF42";
                                        days70001 = -1;
                                        break;
                                    case 2:
                                        itemcode7001 = "DC03";
                                        days70001 = 30;
                                        break;
                                    case 3:
                                        itemcode7001 = "DC01";
                                        days70001 = 30;
                                        break;
                                    case 4:
                                        itemcode7001 = "DC04";
                                        days70001 = 30;
                                        break;
                                    case 5:
                                        itemcode7001 = "CC05";
                                        days70001 = 30;
                                        break;
                                    case 6:
                                        itemcode7001 = "CC07";
                                        days70001 = 7;
                                        break;
                                    case 7:
                                        itemcode7001 = "CC06";
                                        days70001 = 7;
                                        break;
                                    case 8:
                                        itemcode7001 = "CC08";
                                        days70001 = 7;
                                        break;
                                    case 9:
                                        itemcode7001 = "DS03";
                                        days70001 = 7;
                                        break;
                                    case 10:
                                        itemcode7001 = "DS10";
                                        days70001 = 7;
                                        break;
                                    case 11:
                                        itemcode7001 = "CB09";
                                        days70001 = 7;
                                        break;
                                    case 12:
                                        itemcode7001 = "DS05";
                                        days70001 = 7;
                                        break;
                                    case 13:
                                        itemcode7001 = "DF09";
                                        days70001 = 7;
                                        break;
                                    case 14:
                                        itemcode7001 = "CA01";
                                        days70001 = 15;
                                        break;

                                    case 15:
                                        itemcode7001 = "CD02";
                                        days70001 = 15;
                                        break;
                                    case 16:
                                        itemcode7001 = "CD01";
                                        days70001 = 15;
                                        break;
                                    case 17:
                                        itemcode7001 = "CI01";
                                        days70001 = 15;
                                        break;
                                    case 18:
                                        itemcode7001 = "CE02";
                                        days70001 = 15;
                                        break;
                                    case 19:
                                        itemcode7001 = "CE01";
                                        days70001 = 15;
                                        break;
                                    case 20:
                                        itemcode7001 = "CC72";
                                        days70001 = 15;
                                        break;
                                    case 21:
                                        itemcode7001 = "CC76";
                                        days70001 = 15;
                                        break;
                                    case 22:
                                        itemcode7001 = "CD02";
                                        days70001 = 15;
                                        break;
                                    case 23:
                                        itemcode7001 = "CD01";
                                        days70001 = 15;
                                        break;
                                    case 24:
                                        itemcode7001 = "CD03";
                                        days70001 = 15;
                                        break;
                                    case 25:
                                        itemcode7001 = "CD04";
                                        days70001 = 15;
                                        break;
                                    case 26:
                                        itemcode7001 = "DC03";
                                        days70001 = -1;
                                        break;
                                    case 27:
                                        itemcode7001 = "DF05";
                                        days70001 = -1;
                                        break;
                                    case 28:
                                        itemcode7001 = "DG03";
                                        days70001 = -1;
                                        break;
                                    case 29:
                                        itemcode7001 = "DG03";
                                        days70001 = -1;
                                        break;
                                    case 30:
                                        itemcode7001 = "DE01";
                                        days70001 = 30;
                                        break;
                                    case 31:
                                        itemcode7001 = "DE01";
                                        days70001 = -1;
                                        break;
                                    case 32:
                                        itemcode7001 = "DD01";
                                        days70001 = 30;
                                        break;
                                    case 33:
                                        itemcode7001 = "DD01";
                                        days70001 = -1;
                                        break;
                                    case 34:
                                        itemcode7001 = "DD03";
                                        days70001 = 30;
                                        break;
                                    case 35:
                                        itemcode7001 = "DD03";
                                        days70001 = -1;
                                        break;
                                    case 36:
                                        itemcode7001 = "DD02";
                                        days70001 = -1;
                                        break;
                                    case 37:
                                        itemcode7001 = "DD02";
                                        days70001 = 30;
                                        break;
                                    case 38:
                                        itemcode7001 = "DU02";
                                        days70001 = 7;
                                        break;
                                    case 39:
                                        itemcode7001 = "DV01";
                                        days70001 = 7;
                                        break;
                                    case 40:
                                        itemcode7001 = "DG01";
                                        days70001 = -1;
                                        break;
                                    case 41:
                                        itemcode7001 = "DG01";
                                        days70001 = 30;
                                        break;
                                    case 42:
                                        itemcode7001 = "DF13";
                                        days70001 = -1;
                                        break;
                                    case 43:
                                        itemcode7001 = "DF13";
                                        days70001 = 30;
                                        break;
                                    case 44:
                                        itemcode7001 = "DG06";
                                        days70001 = -1;
                                        break;
                                    case 45:
                                        itemcode7001 = "DG06";
                                        days70001 = 30;
                                        break;
                                    case 46:
                                        itemcode7001 = "DG16";
                                        days70001 = -1;
                                        break;
                                    case 47:
                                        itemcode7001 = "DG16";
                                        days70001 = 30;
                                        break;
                                    case 48:
                                        itemcode7001 = "DG01";
                                        days70001 = -1;
                                        break;
                                    case 49:
                                        itemcode7001 = "DG01";
                                        days70001 = 30;
                                        break;
                                    case 50:
                                        itemcode7001 = "DC03";
                                        days70001 = -1;
                                        break;
                                    case 51:
                                        itemcode7001 = "DC03";
                                        days70001 = 30;
                                        break;
                                    case 73:
                                        itemcode7001 = "DC33";
                                        days70001 = -1;
                                        break;
                                    case 52:
                                        itemcode7001 = "DC33";
                                        days70001 = 30;
                                        break;
                                    case 53:
                                        itemcode7001 = "DC01";
                                        days70001 = -1;
                                        break;
                                    case 72:
                                        itemcode7001 = "DC13";
                                        days70001 = -1;
                                        break;
                                    case 71:
                                        itemcode7001 = "DC13";
                                        days70001 = 30;
                                        break;
                                    case 54:
                                        itemcode7001 = "DC10";
                                        days70001 = -1;
                                        break;
                                    case 55:
                                        itemcode7001 = "DC10";
                                        days70001 = 30;
                                        break;
                                    case 56:
                                        itemcode7001 = "DC12";
                                        days70001 = -1;
                                        break;
                                    case 57:
                                        itemcode7001 = "DC12";
                                        days70001 = 30;
                                        break;
                                    case 58:
                                        itemcode7001 = "DC11";
                                        days70001 = -1;
                                        break;
                                    case 59:
                                        itemcode7001 = "DC11";
                                        days70001 = 30;
                                        break;
                                    case 60:
                                        itemcode7001 = "DC09";
                                        days70001 = -1;
                                        break;
                                    case 61:
                                        itemcode7001 = "DC06";
                                        days70001 = 30;
                                        break;
                                    case 62:
                                        itemcode7001 = "DC06";
                                        days70001 = -1;
                                        break;
                                    case 63:
                                        itemcode7001 = "DC05";
                                        days70001 = 30;
                                        break;
                                    case 64:
                                        itemcode7001 = "DC05";
                                        days70001 = -1;
                                        break;
                                    case 65:
                                        itemcode7001 = "DC04";
                                        days70001 = 30;
                                        break;
                                    case 66:
                                        itemcode7001 = "DC04";
                                        days70001 = -1;
                                        break;
                                    case 67:
                                        itemcode7001 = "DC03";
                                        days70001 = 30;
                                        break;
                                    case 68:
                                        itemcode7001 = "DC05";
                                        days70001 = -1;
                                        break;
                                    case 69:
                                        itemcode7001 = "DC01";
                                        days70001 = -1;
                                        break;
                                    case 70:
                                        itemcode7001 = "CC02";
                                        days70001 = 15;
                                        break;
                                    case 74:
                                        itemcode7001 = "CC01";
                                        days70001 = 7;
                                        break;
                                    case 75:
                                        itemcode7001 = "CC08";
                                        days70001 = 7;
                                        break;
                                    case 76:
                                        itemcode7001 = "DO02";
                                        days70001 = 7;
                                        break;
                                    case 77:
                                        itemcode7001 = "CA04";
                                        days70001 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode7001, days70001);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode7001, days70001));
                                return;
                            case "CY44":
                                int num6001 = Game_Server.Generic.random(0, 77);
                                int days70000 = 1;
                                string itemcode7000 = (string)null;
                                switch (num6001)
                                {
                                    case 0:
                                        itemcode7000 = "DF42";
                                        days70000 = 30;
                                        break;
                                    case 1:
                                        itemcode7000 = "DF42";
                                        days70000 = -1;
                                        break;
                                    case 2:
                                        itemcode7000 = "DC03";
                                        days70000 = 30;
                                        break;
                                    case 3:
                                        itemcode7000 = "DC01";
                                        days70000 = 30;
                                        break;
                                    case 4:
                                        itemcode7000 = "DC04";
                                        days70000 = 30;
                                        break;
                                    case 5:
                                        itemcode7000 = "CC05";
                                        days70000 = 30;
                                        break;
                                    case 6:
                                        itemcode7000 = "CC07";
                                        days70000 = 7;
                                        break;
                                    case 7:
                                        itemcode7000 = "CC06";
                                        days70000 = 7;
                                        break;
                                    case 8:
                                        itemcode7000 = "CC08";
                                        days70000 = 7;
                                        break;
                                    case 9:
                                        itemcode7000 = "DS03";
                                        days70000 = 7;
                                        break;
                                    case 10:
                                        itemcode7000 = "DS10";
                                        days70000 = 7;
                                        break;
                                    case 11:
                                        itemcode7000 = "CB09";
                                        days70000 = 7;
                                        break;
                                    case 12:
                                        itemcode7000 = "DS05";
                                        days70000 = 7;
                                        break;
                                    case 13:
                                        itemcode7000 = "DF09";
                                        days70000 = 7;
                                        break;
                                    case 14:
                                        itemcode7000 = "CA01";
                                        days70000 = 15;
                                        break;
                               
                                    case 15:
                                        itemcode7000 = "CD02";
                                        days70000 = 15;
                                        break;
                                    case 16:
                                        itemcode7000 = "CD01";
                                        days70000 = 15;
                                        break;
                                    case 17:
                                        itemcode7000 = "CI01";
                                        days70000 = 15;
                                        break;
                                    case 18:
                                        itemcode7000 = "CE02";
                                        days70000 = 15;
                                        break;
                                    case 19:
                                        itemcode7000 = "CE01";
                                        days70000 = 15;
                                        break;
                                    case 20:
                                        itemcode7000 = "CC72";
                                        days70000 = 15;
                                        break;
                                    case 21:
                                        itemcode7000 = "CC76";
                                        days70000 = 15;
                                        break;
                                    case 22:
                                        itemcode7000 = "CD02";
                                        days70000 = 15;
                                        break;
                                    case 23:
                                        itemcode7000 = "CD01";
                                        days70000 = 15;
                                        break;
                                    case 24:
                                        itemcode7000 = "CD03";
                                        days70000 = 15;
                                        break;
                                    case 25:
                                        itemcode7000 = "CD04";
                                        days70000 = 15;
                                        break;
                                    case 26:
                                        itemcode7000 = "DC03";
                                        days70000 = -1;
                                        break;
                                    case 27:
                                        itemcode7000 = "DF05";
                                        days70000 = -1;
                                        break;
                                    case 28:
                                        itemcode7000 = "DG03";
                                        days70000 = -1;
                                        break;
                                    case 29:
                                        itemcode7000 = "DG03";
                                        days70000 = -1;
                                        break;
                                    case 30:
                                        itemcode7000 = "DE01";
                                        days70000 = 30;
                                        break;
                                    case 31:
                                        itemcode7000 = "DE01";
                                        days70000 = -1;
                                        break;
                                    case 32:
                                        itemcode7000 = "DD01";
                                        days70000 = 30;
                                        break;
                                    case 33:
                                        itemcode7000 = "DD01";
                                        days70000 = -1;
                                        break;
                                    case 34:
                                        itemcode7000 = "DD03";
                                        days70000 = 30;
                                        break;
                                    case 35:
                                        itemcode7000 = "DD03";
                                        days70000 = -1;
                                        break;
                                    case 36:
                                        itemcode7000 = "DD02";
                                        days70000 = -1;
                                        break;
                                    case 37:
                                        itemcode7000 = "DD02";
                                        days70000 = 30;
                                        break;
                                    case 38:
                                        itemcode7000 = "DU02";
                                        days70000 = 7;
                                        break;
                                    case 39:
                                        itemcode7000 = "DV01";
                                        days70000 = 7;
                                        break;
                                    case 40:
                                        itemcode7000 = "DG01";
                                        days70000 = -1;
                                        break;
                                    case 41:
                                        itemcode7000 = "DG01";
                                        days70000 = 30;
                                        break;
                                    case 42:
                                        itemcode7000 = "DF13";
                                        days70000 = -1;
                                        break;
                                    case 43:
                                        itemcode7000 = "DF13";
                                        days70000 = 30;
                                        break;
                                    case 44:
                                        itemcode7000 = "DG06";
                                        days70000 = -1;
                                        break;
                                    case 45:
                                        itemcode7000 = "DG06";
                                        days70000 = 30;
                                        break;
                                    case 46:
                                        itemcode7000 = "DG16";
                                        days70000 = -1;
                                        break;
                                    case 47:
                                        itemcode7000 = "DG16";
                                        days70000 = 30;
                                        break;
                                    case 48:
                                        itemcode7000 = "DG01";
                                        days70000 = -1;
                                        break;
                                    case 49:
                                        itemcode7000 = "DG01";
                                        days70000 = 30;
                                        break;
                                    case 50:
                                        itemcode7000 = "DC03";
                                        days70000 = -1;
                                        break;
                                    case 51:
                                        itemcode7000 = "DC03";
                                        days70000 = 30;
                                        break;
                                    case 73:
                                        itemcode7000 = "DC33";
                                        days70000 = -1;
                                        break;
                                    case 52:
                                        itemcode7000 = "DC33";
                                        days70000 = 30;
                                        break;
                                    case 53:
                                        itemcode7000 = "DC01";
                                        days70000 = -1;
                                        break;
                                    case 72:
                                        itemcode7000 = "DC13";
                                        days70000 = -1;
                                        break;
                                    case 71:
                                        itemcode7000 = "DC13";
                                        days70000 = 30;
                                        break;
                                    case 54:
                                        itemcode7000 = "DC10";
                                        days70000 = -1;
                                        break;
                                    case 55:
                                        itemcode7000 = "DC10";
                                        days70000 = 30;
                                        break;
                                    case 56:
                                        itemcode7000 = "DC12";
                                        days70000 = -1;
                                        break;
                                    case 57:
                                        itemcode7000 = "DC12";
                                        days70000 = 30;
                                        break;
                                    case 58:
                                        itemcode7000 = "DC11";
                                        days70000 = -1;
                                        break;
                                    case 59:
                                        itemcode7000 = "DC11";
                                        days70000 = 30;
                                        break;
                                    case 60:
                                        itemcode7000 = "DC09";
                                        days70000 = -1;
                                        break;
                                    case 61:
                                        itemcode7000 = "DC06";
                                        days70000 = 30;
                                        break;
                                    case 62:
                                        itemcode7000 = "DC06";
                                        days70000 = -1;
                                        break;
                                    case 63:
                                        itemcode7000 = "DC05";
                                        days70000 = 30;
                                        break;
                                    case 64:
                                        itemcode7000 = "DC05";
                                        days70000 = -1;
                                        break;
                                    case 65:
                                        itemcode7000 = "DC04";
                                        days70000 = 30;
                                        break;
                                    case 66:
                                        itemcode7000 = "DC04";
                                        days70000 = -1;
                                        break;
                                    case 67:
                                        itemcode7000 = "DC03";
                                        days70000 = 30;
                                        break;
                                    case 68:
                                        itemcode7000 = "DC05";
                                        days70000 = -1;
                                        break;
                                    case 69:
                                        itemcode7000 = "DC01";
                                        days70000 = -1;
                                        break;
                                    case 70:
                                        itemcode7000 = "CC02";
                                        days70000 = 15;
                                        break;
                                    case 74:
                                        itemcode7000 = "CC01";
                                        days70000 = 7;
                                        break;
                                    case 75:
                                        itemcode7000 = "CC08";
                                        days70000 = 7;
                                        break;
                                    case 76:
                                        itemcode7000 = "DO02";
                                        days70000 = 7;
                                        break;
                                    case 77:
                                        itemcode7000 = "CA04";
                                        days70000 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode7000, days70000);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode7000, days70000));
                                return;

                            case "CY74":
                                int num5119 = Game_Server.Generic.random(0, 39);
                                int days60 = 1;
                                string itemcode68 = (string)null;
                                switch (num5119)
                                {
                                    case 0:
                                        itemcode68 = "DC87";
                                        days60 = 30;
                                        break;

                                    case 1:
                                        itemcode68 = "DC88";
                                        days60 = 30;
                                        break;

                                    case 2:
                                        itemcode68 = "DC89";
                                        days60 = 30;
                                        break;

                                    case 3:
                                        itemcode68 = "DC90";
                                        days60 = 30;
                                        break;

                                    case 4:
                                        itemcode68 = "DC96";
                                        days60 = 30;
                                        break;
                                    case 5:
                                        itemcode68 = "DE17";
                                        days60 = 30;
                                        break;
                                    case 6:
                                        itemcode68 = "DE18";
                                        days60 = 30;
                                        break;
                                    case 7:
                                        itemcode68 = "DE19";
                                        days60 = 30;
                                        break;
                                    case 8:
                                        itemcode68 = "DE20";
                                        days60 = 30;
                                        break;
                                    case 9:
                                        itemcode68 = "DE21";
                                        days60 = 30;
                                        break;

                                    case 10:
                                        itemcode68 = "DE51";
                                        days60 = 30;
                                        break;

                                    case 11:
                                        itemcode68 = "DC97";
                                        days60 = 30;
                                        break;
                                    case 12:
                                        itemcode68 = "GC21";
                                        days60 = -1;
                                        break;
                                    case 13:
                                        itemcode68 = "GC15";
                                        days60 = 30;
                                        break;
                                    case 14:
                                        itemcode68 = "GC11";
                                        days60 = 30;
                                        break;
                                    case 15:
                                        itemcode68 = "DE80";
                                        days60 = 30;
                                        break;
                                    case 16:
                                        itemcode68 = "GC04";
                                        days60 = 30;
                                        break;
                                    case 17:
                                        itemcode68 = "DE74";
                                        days60 = 30;
                                        break;
                                    case 18:
                                        itemcode68 = "DE73";
                                        days60 = 30;
                                        break;
                                    case 19:
                                        itemcode68 = "DE52";
                                        days60 = 30;
                                        break;
                                    case 20:
                                        itemcode68 = "DC87";
                                        days60 = -1;
                                        break;

                                    case 21:
                                        itemcode68 = "DC88";
                                        days60 = -1;
                                        break;

                                    case 22:
                                        itemcode68 = "DC89";
                                        days60 = -1;
                                        break;

                                    case 23:
                                        itemcode68 = "DC90";
                                        days60 = -1;
                                        break;

                                    case 24:
                                        itemcode68 = "DC96";
                                        days60 = -1;
                                        break;
                                    case 25:
                                        itemcode68 = "DE17";
                                        days60 = -1;
                                        break;
                                    case 26:
                                        itemcode68 = "DE18";
                                        days60 = -1;
                                        break;
                                    case 27:
                                        itemcode68 = "DE19";
                                        days60 = -1;
                                        break;
                                    case 28:
                                        itemcode68 = "DE20";
                                        days60 = -1;
                                        break;
                                    case 29:
                                        itemcode68 = "DE21";
                                        days60 = -1;
                                        break;

                                    case 30:
                                        itemcode68 = "DE51";
                                        days60 = -1;
                                        break;

                                    case 31:
                                        itemcode68 = "DC97";
                                        days60 = -1;
                                        break;
                                    case 32:
                                        itemcode68 = "GC21";
                                        days60 = 30;
                                        break;
                                    case 33:
                                        itemcode68 = "GC15";
                                        days60 = -1;
                                        break;
                                    case 34:
                                        itemcode68 = "GC11";
                                        days60 = -1;
                                        break;
                                    case 35:
                                        itemcode68 = "DE80";
                                        days60 = -1;
                                        break;
                                    case 36:
                                        itemcode68 = "GC04";
                                        days60 = -1;
                                        break;
                                    case 37:
                                        itemcode68 = "DE74";
                                        days60 = -1;
                                        break;
                                    case 38:
                                        itemcode68 = "DE73";
                                        days60 = -1;
                                        break;
                                    case 39:
                                        itemcode68 = "DE52";
                                        days60 = -1;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode68, days60);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode68, days60));
                                return;
                            case "CY75":
                                int num5120 = Game_Server.Generic.random(0, 47);
                                int days5 = 1;
                                string itemcode150 = (string)null;
                                switch (num5120)
                                {
                                    case 0:
                                        itemcode150 = "GF50";
                                        days5 = -1;
                                        break;

                                    case 1:
                                        itemcode150 = "GF42";
                                        days5 = -1;
                                        break;

                                    case 2:
                                        itemcode150 = "GF41";
                                        days5 = -1;
                                        break;

                                    case 3:
                                        itemcode150 = "GF36";
                                        days5 = -1;
                                        break;

                                    case 4:
                                        itemcode150 = "GF35";
                                        days5 = -1;
                                        break;
                                    case 5:
                                        itemcode150 = "GF19";
                                        days5 = -1;
                                        break;
                                    case 6:
                                        itemcode150 = "DF99";
                                        days5 = -1;
                                        break;
                                    case 7:
                                        itemcode150 = "DF98";
                                        days5 = -1;
                                        break;
                                    case 8:
                                        itemcode150 = "DF97";
                                        days5 = -1;
                                        break;
                                    case 9:
                                        itemcode150 = "DF94";
                                        days5 = -1;
                                        break;

                                    case 10:
                                        itemcode150 = "GF52";
                                        days5 = -1;
                                        break;

                                    case 11:
                                        itemcode150 = "DF89";
                                        days5 = -1;
                                        break;
                                    case 12:
                                        itemcode150 = "DF85";
                                        days5 = -1;
                                        break;
                                    case 13:
                                        itemcode150 = "DF79";
                                        days5 = -1;
                                        break;
                                    case 14:
                                        itemcode150 = "DF75";
                                        days5 = -1;
                                        break;
                                    case 15:
                                        itemcode150 = "DF74";
                                        days5 = -1;
                                        break;
                                    case 16:
                                        itemcode150 = "DF69";
                                        days5 = -1;
                                        break;
                                    case 17:
                                        itemcode150 = "DF58";
                                        days5 = -1;
                                        break;
                                    case 18:
                                        itemcode150 = "DF52";
                                        days5 = -1;
                                        break;
                                    case 19:
                                        itemcode150 = "DF51";
                                        days5 = -1;
                                        break;
                                    case 20:
                                        itemcode150 = "DF50";
                                        days5 = -1;
                                        break;

                                    case 21:
                                        itemcode150 = "DF47";
                                        days5 = -1;
                                        break;

                                    case 22:
                                        itemcode150 = "DF25";
                                        days5 = -1;
                                        break;

                                    case 23:
                                        itemcode150 = "DF35";
                                        days5 = -1;
                                        break;

                                    case 24:
                                        itemcode150 = "GF50";
                                        days5 = 30;
                                        break;

                                    case 25:
                                        itemcode150 = "GF42";
                                        days5 = 30;
                                        break;

                                    case 26:
                                        itemcode150 = "GF41";
                                        days5 = 30;
                                        break;

                                    case 27:
                                        itemcode150 = "GF36";
                                        days5 = 30;
                                        break;

                                    case 28:
                                        itemcode150 = "GF35";
                                        days5 = 30;
                                        break;
                                    case 29:
                                        itemcode150 = "GF19";
                                        days5 = 30;
                                        break;
                                    case 30:
                                        itemcode150 = "DF99";
                                        days5 = 30;
                                        break;
                                    case 31:
                                        itemcode150 = "DF98";
                                        days5 = 30;
                                        break;
                                    case 32:
                                        itemcode150 = "DF97";
                                        days5 = 30;
                                        break;
                                    case 33:
                                        itemcode150 = "DF94";
                                        days5 = 30;
                                        break;

                                    case 34:
                                        itemcode150 = "GF52";
                                        days5 = 30;
                                        break;

                                    case 35:
                                        itemcode150 = "DF89";
                                        days5 = 30;
                                        break;
                                    case 36:
                                        itemcode150 = "DF85";
                                        days5 = 30;
                                        break;
                                    case 37:
                                        itemcode150 = "DF79";
                                        days5 = 30;
                                        break;
                                    case 38:
                                        itemcode150 = "DF75";
                                        days5 = 30;
                                        break;
                                    case 39:
                                        itemcode150 = "DF74";
                                        days5 = 30;
                                        break;
                                    case 40:
                                        itemcode150 = "DF69";
                                        days5 = 30;
                                        break;
                                    case 41:
                                        itemcode150 = "DF58";
                                        days5 = 30;
                                        break;
                                    case 42:
                                        itemcode150 = "DF52";
                                        days5 = 30;
                                        break;
                                    case 43:
                                        itemcode150 = "DF51";
                                        days5 = 30;
                                        break;
                                    case 44:
                                        itemcode150 = "DF50";
                                        days5 = 30;
                                        break;

                                    case 45:
                                        itemcode150 = "DF47";
                                        days5 = 30;
                                        break;

                                    case 46:
                                        itemcode150 = "DF25";
                                        days5 = 30;
                                        break;

                                    case 47:
                                        itemcode150 = "DF35";
                                        days5 = 30;
                                        break;



                                }
                                Inventory.AddItem(usr, itemcode150, days5);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode150, days5));
                                return;
                        
                            case "CY77":
                                int num510 = Game_Server.Generic.random(0, 21);
                                int days515 = 1;
                                string itemcode159 = (string)null;
                                switch (num510)
                                {
                                    case 0:
                                        itemcode159 = "DJ79";
                                        days515 = -1;
                                        break;

                                    case 1:
                                        itemcode159 = "DJ45";
                                        days515 = -1;
                                        break;

                                    case 2:
                                        itemcode159 = "DJ44";
                                        days515 = -1;
                                        break;

                                    case 3:
                                        itemcode159 = "DJ37";
                                        days515 = -1;
                                        break;

                                    case 4:
                                        itemcode159 = "DJ23";
                                        days515 = -1;
                                        break;
                                    case 5:
                                        itemcode159 = "DJ22";
                                        days515 = -1;
                                        break;
                                    case 6:
                                        itemcode159 = "DJ21";
                                        days515 = -1;
                                        break;
                                    case 7:
                                        itemcode159 = "DJ93";
                                        days515 = -1;
                                        break;
                                    case 8:
                                        itemcode159 = "DJ07";
                                        days515 = -1;
                                        break;
                                    case 9:
                                        itemcode159 = "DJ63";
                                        days515 = -1;
                                        break;

                                    case 10:
                                        itemcode159 = "DJ33";
                                        days515 = -1;
                                        break;
                                    case 11:
                                        itemcode159 = "DJ79";
                                        days515 = -1;
                                        break;

                                    case 12:
                                        itemcode159 = "DJ45";
                                        days515 = -1;
                                        break;

                                    case 13:
                                        itemcode159 = "DJ44";
                                        days515 = -1;
                                        break;

                                    case 14:
                                        itemcode159 = "DJ37";
                                        days515 = -1;
                                        break;

                                    case 15:
                                        itemcode159 = "DJ23";
                                        days515 = -1;
                                        break;
                                    case 16:
                                        itemcode159 = "DJ22";
                                        days515 = -1;
                                        break;
                                    case 17:
                                        itemcode159 = "DJ21";
                                        days515 = -1;
                                        break;
                                    case 18:
                                        itemcode159 = "DJ93";
                                        days515 = -1;
                                        break;
                                    case 19:
                                        itemcode159 = "DJ07";
                                        days515 = -1;
                                        break;
                                    case 20:
                                        itemcode159 = "DJ63";
                                        days515 = -1;
                                        break;

                                    case 21:
                                        itemcode159 = "DJ33";
                                        days515 = -1;
                                        break;





                                }
                                Inventory.AddItem(usr, itemcode159, days515);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode159, days515));
                                return;
                            case "CY76":
                                int num511 = Game_Server.Generic.random(0, 21);
                                int days517 = 1;
                                string itemcode162 = (string)null;
                                switch (num511)
                                {
                                    case 0:
                                        itemcode162 = "GG44";
                                        days517 = -1;
                                        break;

                                    case 1:
                                        itemcode162 = "GG33";
                                        days517 = -1;
                                        break;

                                    case 2:
                                        itemcode162 = "GG29";
                                        days517 = -1;
                                        break;

                                    case 3:
                                        itemcode162 = "GG19";
                                        days517 = -1;
                                        break;

                                    case 4:
                                        itemcode162 = "GG18";
                                        days517 = -1;
                                        break;
                                    case 5:
                                        itemcode162 = "GG09";
                                        days517 = -1;
                                        break;
                                    case 6:
                                        itemcode162 = "DG36";
                                        days517 = -1;
                                        break;
                                    case 7:
                                        itemcode162 = "DG61";
                                        days517 = -1;
                                        break;
                                    case 8:
                                        itemcode162 = "DG23";
                                        days517 = -1;
                                        break;
                                    case 9:
                                        itemcode162 = "DG31";
                                        days517 = -1;
                                        break;
                                    case 10:
                                        itemcode162 = "GG44";
                                        days517 = 30;
                                        break;

                                    case 11:
                                        itemcode162 = "GG33";
                                        days517 = 30;
                                        break;

                                    case 12:
                                        itemcode162 = "GG29";
                                        days517 = 30;
                                        break;

                                    case 13:
                                        itemcode162 = "GG19";
                                        days517 = 30;
                                        break;

                                    case 14:
                                        itemcode162 = "GG18";
                                        days517 = 30;
                                        break;
                                    case 15:
                                        itemcode162 = "GG09";
                                        days517 = 30;
                                        break;
                                    case 16:
                                        itemcode162 = "DG36";
                                        days517 = 30;
                                        break;
                                    case 17:
                                        itemcode162 = "DG61";
                                        days517 = 30;
                                        break;
                                    case 18:
                                        itemcode162 = "DG23";
                                        days517 = 30;
                                        break;
                                    case 19:
                                        itemcode162 = "DG31";
                                        days517 = 30;
                                        break;







                                }
                                Inventory.AddItem(usr, itemcode162, days517);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode162, days517));
                                return;
                            case "CY04":
                                int num65 = Game_Server.Generic.random(0, 5);
                                int days99 = 1;
                                string itemcode20 = (string)null;
                                switch (num65)
                                {

                                    case 0:
                                        itemcode20 = "DC17";
                                        days99 = -1;
                                        break;
                                    case 1:
                                        itemcode20 = "DC17";
                                        days99 = 15;
                                        break;
                                    case 2:
                                        itemcode20 = "DC17";
                                        days99 = 30;
                                        break;
                                    case 3:
                                        itemcode20 = "CF01";
                                        days99 = 15;
                                        break;
                                    case 4:
                                        itemcode20 = "CIO1";
                                        days99 = 15;
                                        break;
                                    case 5:
                                        itemcode20 = "CA01";
                                        days99 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode20, days99);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode20, days99));
                                return;
                            case "CZ55":
                                int num6 = Game_Server.Generic.random(0, 5);
                                int days2 = 1;
                                string itemcode17 = (string)null;
                                switch (num6)
                                {

                                    case 0:
                                        itemcode17 = "D602";
                                        days2 = -1;
                                        break;
                                    case 1:
                                        itemcode17 = "D602";
                                        days2 = 15;
                                        break;
                                    case 2:
                                        itemcode17 = "D602";
                                        days2 = 30;
                                        break;
                                    case 3:
                                        itemcode17 = "DS03";
                                        days2 = 15;
                                        break;
                                    case 4:
                                        itemcode17 = "CA01";
                                        days2 = 15;
                                        break;
                                    case 5:
                                        itemcode17 = "DS10";
                                        days2 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode17, days2);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode17, days2));
                                return;
                            case "CZ68":
                                int num8 = Game_Server.Generic.random(0, 5);
                                int days1 = 1;
                                string itemcode16 = (string)null;
                                switch (num8)
                                {

                                    case 0:
                                        itemcode16 = "DC16";
                                        days1 = -1;
                                        break;
                                    case 1:
                                        itemcode16 = "DC04";
                                        days1 = 15;
                                        break;
                                    case 2:
                                        itemcode16 = "DC04";
                                        days1 = 30;
                                        break;
                                    case 3:
                                        itemcode16 = "CF01";
                                        days1 = 15;
                                        break;
                                    case 4:
                                        itemcode16 = "CIO1";
                                        days1 = 15;
                                        break;
                                    case 5:
                                        itemcode16 = "CA01";
                                        days1 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode16, days1);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode16, days1));
                                return;
                            case "CZ65":
                                int num16 = Game_Server.Generic.random(0, 5);
                                int days111 = 1;
                                string itemcode36 = (string)null;
                                switch (num16)
                                {

                                    case 0:
                                        itemcode36 = "DC65";
                                        days111 = -1;
                                        break;
                                    case 1:
                                        itemcode36 = "DC65";
                                        days111 = 15;
                                        break;
                                    case 2:
                                        itemcode36 = "DC65";
                                        days111 = 30;
                                        break;
                                    case 3:
                                        itemcode36 = "CF01";
                                        days111 = 15;
                                        break;
                                    case 4:
                                        itemcode36 = "CIO1";
                                        days111 = 15;
                                        break;
                                    case 5:
                                        itemcode36 = "CA01";
                                        days111 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode36, days111);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode36, days111));
                                return;
                            case "CY09":
                                int num = Game_Server.Generic.random(0, 5);
                                int days22 = 1;
                                string itemcode26 = (string)null;
                                switch (num)
                                {

                                    case 0:
                                        itemcode26 = "DG33";
                                        days22 = -1;
                                        break;
                                    case 1:
                                        itemcode26 = "DG33";
                                        days22 = 15;
                                        break;

                                    case 3:
                                        itemcode26 = "CF01";
                                        days22 = 15;
                                        break;
                                    case 4:
                                        itemcode26 = "CIO1";
                                        days22 = 15;
                                        break;
                                    case 5:
                                        itemcode26 = "CA01";
                                        days22 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode26, days22);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode26, days22));
                                return;
                            case "CZ94":
                                int num67 = Game_Server.Generic.random(0, 6);
                                int days3 = 1;
                                string itemcode19 = (string)null;
                                switch (num67)
                                {
                                    case 6:
                                        itemcode19 = "DB22";
                                        days3 = -1;
                                        break;
                                    case 0:
                                        itemcode19 = "DB22";
                                        days3 = 5;
                                        break;
                                    case 1:
                                        itemcode19 = "DB24";
                                        days3 = 10;
                                        break;
                                    case 2:
                                        itemcode19 = "DB24";
                                        days3 = 30;
                                        break;
                                    case 3:
                                        itemcode19 = "CF01";
                                        days3 = 15;
                                        break;
                                    case 4:
                                        itemcode19 = "CIO1";
                                        days3 = 15;
                                        break;
                                    case 5:
                                        itemcode19 = "CA01";
                                        days3 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode19, days3);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode19, days3));
                                return;
                            case "CZ60":
                                int num68 = Game_Server.Generic.random(0, 6);
                                int days34 = 1;
                                string itemcode29 = (string)null;
                                switch (num68)
                                {
                                    case 6:
                                        itemcode29 = "DI09";
                                        days34 = -1;
                                        break;
                                    case 0:
                                        itemcode29 = "DI04";
                                        days34 = 5;
                                        break;
                                    case 1:
                                        itemcode29 = "DI04";
                                        days34 = 10;
                                        break;
                                    case 2:
                                        itemcode29 = "DB24";
                                        days34 = 30;
                                        break;
                                    case 3:
                                        itemcode29 = "CF01";
                                        days34 = 15;
                                        break;
                                    case 4:
                                        itemcode29 = "CIO1";
                                        days34 = 15;
                                        break;
                                    case 5:
                                        itemcode29 = "CA01";
                                        days34 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode29, days34);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode29, days34));
                                return;
                            case "CZ66":
                                int num64 = Game_Server.Generic.random(0, 5);
                                int days0 = 1;
                                string itemcode5 = (string)null;
                                switch (num64)
                                {
                                    case 0:
                                        itemcode5 = "DC22";
                                        days0 = -1;
                                        break;
                                    case 1:
                                        itemcode5 = "DC22";
                                        days0 = 15;
                                        break;
                                    case 2:
                                        itemcode5 = "DC22";
                                        days0 = 30;
                                        break;
                                    case 3:
                                        itemcode5 = "CF01";
                                        days0 = 15;
                                        break;
                                    case 4:
                                        itemcode5 = "CIO1";
                                        days0 = 15;
                                        break;
                                    case 5:
                                        itemcode5 = "CA01";
                                        days0 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode5, days0);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode5, days0));
                                return;
                            case "CZ63":
                                int num60 = Game_Server.Generic.random(0, 5);
                                int days14 = 1;
                                string itemcode65 = (string)null;
                                switch (num60)
                                {
                                    case 0:
                                        itemcode65 = "DC24";
                                        days14 = -1;
                                        break;
                                    case 1:
                                        itemcode65 = "DC24";
                                        days14 = 15;
                                        break;
                                    case 2:
                                        itemcode65 = "DC24";
                                        days14 = 30;
                                        break;
                                    case 3:
                                        itemcode65 = "CF01";
                                        days14 = 15;
                                        break;
                                    case 4:
                                        itemcode65 = "CIO1";
                                        days14 = 15;
                                        break;
                                    case 5:
                                        itemcode65 = "CA01";
                                        days14 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode65, days14);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode65, days14));
                                return;
                            case "CZ64":
                                int num61 = Game_Server.Generic.random(0, 5);
                                int days15 = 1;
                                string itemcode67 = (string)null;
                                switch (num61)
                                {
                                    case 0:
                                        itemcode67 = "DC62";
                                        days15 = -1;
                                        break;
                                    case 1:
                                        itemcode67 = "DC62";
                                        days15 = 15;
                                        break;
                                    case 2:
                                        itemcode67 = "DC62";
                                        days15 = 30;
                                        break;
                                    case 3:
                                        itemcode67 = "CF01";
                                        days15 = 15;
                                        break;
                                    case 4:
                                        itemcode67 = "CIO1";
                                        days15 = 15;
                                        break;
                                    case 5:
                                        itemcode67 = "CA01";
                                        days15 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode67, days15);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode67, days15));
                                return;

                            case "CY12":
                                int num90 = Game_Server.Generic.random(0, 5);
                                int days9 = 1;
                                string itemcode14 = (string)null;
                                switch (num90)
                                {
                                    case 0:
                                        itemcode14 = "D806";
                                        days9 = -1;
                                        break;
                                    case 1:
                                        itemcode14 = "D806";
                                        days9 = 15;
                                        break;
                                    case 2:
                                        itemcode14 = "D806";
                                        days9 = 30;
                                        break;
                                    case 3:
                                        itemcode14 = "CF01";
                                        days9 = 15;
                                        break;
                                    case 4:
                                        itemcode14 = "CIO1";
                                        days9 = 15;
                                        break;
                                    case 5:
                                        itemcode14 = "DS03";
                                        days9 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode14, days9);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode14, days9));
                                return;
                            case "CY23":
                                int num91 = Game_Server.Generic.random(0, 5);
                                int days18 = 1;
                                string itemcode74 = (string)null;
                                switch (num91)
                                {
                                    case 0:
                                        itemcode74 = "D705";
                                        days18 = -1;
                                        break;
                                    case 1:
                                        itemcode74 = "D705";
                                        days18 = 15;
                                        break;
                                    case 2:
                                        itemcode74 = "D705";
                                        days18 = 30;
                                        break;
                                    case 3:
                                        itemcode74 = "CF01";
                                        days18 = 15;
                                        break;
                                    case 4:
                                        itemcode74 = "CIO1";
                                        days18 = 15;
                                        break;
                                    case 5:
                                        itemcode74 = "DS03";
                                        days18 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode74, days18);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode74, days18));
                                return;
                            case "CZ52":
                                int num291 = Game_Server.Generic.random(0, 5);
                                int days350 = 1;
                                string itemcode125 = (string)null;
                                switch (num291)
                                {
                                    case 0:
                                        itemcode125 = "D701";
                                        days350 = -1;
                                        break;
                                    case 1:
                                        itemcode125 = "D701";
                                        days350 = 15;
                                        break;
                                    case 2:
                                        itemcode125 = "D701";
                                        days350 = 30;
                                        break;
                                    case 3:
                                        itemcode125 = "CF01";
                                        days350 = 15;
                                        break;
                                    case 4:
                                        itemcode125 = "CIO1";
                                        days18 = 15;
                                        break;
                                    case 5:
                                        itemcode125 = "DS03";
                                        days350 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode125, days350);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode125, days350));
                                return;
                            case "CY24":
                                int num451 = Game_Server.Generic.random(0, 5);
                                int days148 = 1;
                                string itemcode80 = (string)null;
                                switch (num451)
                                {
                                    case 0:
                                        itemcode74 = "D505";
                                        days18 = -1;
                                        break;
                                    case 1:
                                        itemcode74 = "D505";
                                        days18 = 15;
                                        break;
                                    case 2:
                                        itemcode74 = "D705";
                                        days18 = 30;
                                        break;
                                    case 3:
                                        itemcode74 = "CF01";
                                        days18 = 15;
                                        break;
                                    case 4:
                                        itemcode74 = "CIO1";
                                        days18 = 15;
                                        break;
                                    case 5:
                                        itemcode74 = "DS03";
                                        days18 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode80, days148);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode80, days148));
                                return;
                            case "CY10":
                                int num89 = Game_Server.Generic.random(0, 5);
                                int days11 = 1;
                                string itemcode30 = (string)null;
                                switch (num89)
                                {
                                    case 0:
                                        itemcode30 = "D501";
                                        days11 = -1;
                                        break;
                                    case 1:
                                        itemcode30 = "D501";
                                        days11 = 15;
                                        break;
                                    case 2:
                                        itemcode30 = "D501";
                                        days11 = 30;
                                        break;
                                    case 3:
                                        itemcode30 = "CF01";
                                        days11 = 15;
                                        break;
                                    case 4:
                                        itemcode30 = "CIO1";
                                        days11 = 15;
                                        break;
                                    case 5:
                                        itemcode30 = "DS03";
                                        days11 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode30, days11);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode30, days11));
                                return;
                            case "CR19":
                                int num126 = Game_Server.Generic.random(0, 8);
                                string itemcode12 = (string)null;
                                switch (num126)
                                {
                                    case 0:
                                        itemcode12 = "D807";
                                        break;
                                    case 1:
                                        itemcode12 = "D808";
                                        break;
                                    case 2:
                                        itemcode12 = "D809";
                                        break;
                                    case 3:
                                        itemcode12 = "D810";
                                        break;
                                    case 4:
                                        itemcode12 = "D811";
                                        break;
                                    case 5:
                                        itemcode12 = "D812";
                                        break;
                                    case 6:
                                        itemcode12 = "D822";
                                        break;
                                    case 7:
                                        itemcode12 = "D825";
                                        break;
                                    case 8:
                                        itemcode12 = "D826";
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode12, -1);
                                Inventory.DecreaseEAItem(usr, "CR19", 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode12, -1));
                                return;
                            case "CR18":
                                int num17 = Game_Server.Generic.random(0, 30);
                                string itemcode21 = (string)null;
                                switch (num17)
                                {
                                    case 0:
                                        itemcode21 = "DC21";
                                        break;
                                    case 1:
                                        itemcode21 = "DC22";
                                        break;
                                    case 2:
                                        itemcode21 = "DC23";
                                        break;
                                    case 3:
                                        itemcode21 = "DC24";
                                        break;
                                    case 4:
                                        itemcode21 = "DC25";
                                        break;
                                    case 5:
                                        itemcode21 = "DC26";
                                        break;
                                    case 6:
                                        itemcode21 = "DC27";
                                        break;
                                    case 7:
                                        itemcode21 = "DC28";
                                        break;
                                    case 8:
                                        itemcode21 = "DC29";
                                        break;
                                    case 9:
                                        itemcode21 = "DC30";
                                        break;
                                    case 10:
                                        itemcode21 = "DC41";
                                        break;
                                    case 11:
                                        itemcode21 = "DC42";
                                        break;
                                    case 12:
                                        itemcode21 = "DC43";
                                        break;
                                    case 13:
                                        itemcode21 = "DC44";
                                        break;
                                    case 14:
                                        itemcode21 = "DC45";
                                        break;
                                    case 15:
                                        itemcode21 = "DC46";
                                        break;
                                    case 16:
                                        itemcode21 = "DC47";
                                        break;
                                    case 17:
                                        itemcode21 = "DC48";
                                        break;
                                    case 18:
                                        itemcode21 = "DC49";
                                        break;
                                    case 19:
                                        itemcode21 = "DC50";
                                        break;
                                    case 20:
                                        itemcode21 = "DC51";
                                        break;
                                    case 21:
                                        itemcode21 = "DC52";
                                        break;
                                    case 22:
                                        itemcode21 = "DC53";
                                        break;
                                    case 23:
                                        itemcode21 = "DC54";
                                        break;
                                    case 24:
                                        itemcode21 = "DC55";
                                        break;
                                    case 25:
                                        itemcode21 = "DC56";
                                        break;
                                    case 26:
                                        itemcode21 = "DC57";
                                        break;
                                    case 27:
                                        itemcode21 = "DC58";
                                        break;
                                    case 28:
                                        itemcode21 = "DC59";
                                        break;
                                    case 29:
                                        itemcode21 = "DC60";
                                        break;
                                    case 30:
                                        itemcode21 = "DC62";
                                        break;
                                }

                                Inventory.AddItem(usr, itemcode21, -1);
                                Inventory.DecreaseEAItem(usr, "CR18", 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode21, -1));
                                return;
                            case "CB09":
                                if (!usr.HasItem("CB08"))
                                {
                                    usr.send((Packet)new SP_CashItemUse(SP_CashItemUse.ErrCode.NeedSupplyBox, usr, "CB08"));
                                    return;
                                }
                                int num5 = Game_Server.Generic.random(0, 5);
                                string itemcode1 = (string)null;
                                switch (num5)
                                {
                                    case 0:
                                        itemcode1 = "CZ85";
                                        break;
                                    case 1:
                                        itemcode1 = "DU51";
                                        break;
                                    case 2:
                                        itemcode1 = "DC85";
                                        break;
                                    case 3:
                                        itemcode1 = "DF12";
                                        break;
                                    case 4:
                                        itemcode1 = "DE18";
                                        break;
                                    case 5:
                                        itemcode1 = "DE28";
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode1, 7);
                                Inventory.DecreaseEAItem(usr, "CB09", 1);
                                Inventory.DecreaseEAItem(usr, "CB08", 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode1, 7));
                                return;
                            case "CZ81":
                                if (!usr.HasItem("CB08"))
                                {
                                    usr.send((Packet)new SP_CashItemUse(SP_CashItemUse.ErrCode.NeedSupplyBox, usr, "CB08"));
                                    return;
                                }
                                int num58 = Game_Server.Generic.random(0, 5);
                                string itemcode2 = (string)null;
                                switch (num58)
                                {
                                    case 0:
                                        itemcode2 = "DB40";
                                        break;
                                    case 1:
                                        itemcode2 = "DA74";
                                        break;
                                    case 2:
                                        itemcode2 = "DJ93";
                                        break;
                                    case 3:
                                        itemcode2 = "DG39";
                                        break;
                                    case 4:
                                        itemcode2 = "DG63";
                                        break;
                                    case 5:
                                        itemcode2 = "DC48";
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode2, 7);
                                Inventory.DecreaseEAItem(usr, "CZ81", 1);
                                Inventory.DecreaseEAItem(usr, "CB08", 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode2, 7));
                                return;
                            case "CR03":
                                int num79 = Game_Server.Generic.random(0, 5);
                                int days21 = 1;
                                string itemcode75 = (string)null;
                                switch (num79)
                                {
                                    case 0:
                                        itemcode75 = "D817";
                                        days21 = -1;
                                        break;
                                    case 1:
                                        itemcode75 = "D817";
                                        days21 = 15;
                                        break;
                                    case 2:
                                        itemcode75 = "D817";
                                        days21 = 30;
                                        break;
                                    case 3:
                                        itemcode75 = "CF01";
                                        days21 = 15;
                                        break;
                                    case 4:
                                        itemcode75 = "CI01";
                                        days21 = 15;
                                        break;
                                    case 5:
                                        itemcode75 = "DS03";
                                        days21 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode75, days21);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode75, days21));
                                return;
                            case "CR01":
                                int num80 = Game_Server.Generic.random(0, 5);
                                int days28 = 1;
                                string itemcode190 = (string)null;
                                switch (num80)
                                {
                                    case 0:
                                        itemcode190 = "D706";
                                        days28 = -1;
                                        break;
                                    case 1:
                                        itemcode190 = "D706";
                                        days28 = 15;
                                        break;
                                    case 2:
                                        itemcode190 = "D706";
                                        days28 = 30;
                                        break;
                                    case 3:
                                        itemcode190 = "CF01";
                                        days28 = 15;
                                        break;
                                    case 4:
                                        itemcode190 = "CI01";
                                        days28 = 15;
                                        break;
                                    case 5:
                                        itemcode190 = "DS03";
                                        days28 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode190, days28);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode190, days28));
                                return;
                            case "CR56":
                                int num101 = Game_Server.Generic.random(0, 15);
                                int days102 = 1;
                                string itemcode71 = (string)null;
                                switch (num101)
                                {
                                    case 0:
                                        itemcode71 = "DA23";
                                        days102 = 7;
                                        break;
                                    case 1:
                                        itemcode71 = "DA24";
                                        days102 = 7;
                                        break;
                                    case 2:
                                        itemcode71 = "DA25";
                                        days102 = 7;
                                        break;
                                    case 3:
                                        itemcode71 = "DA26";
                                        days102 = 7;
                                        break;
                                    case 4:
                                        itemcode71 = "DA27";
                                        days102 = 7;
                                        break;
                                    case 5:
                                        itemcode71 = "DA28";
                                        days102 = 7;
                                        break;
                                    case 6:
                                        itemcode71 = "DA29";
                                        days102 = 7;
                                        break;
                                    case 7:
                                        itemcode71 = "DA30";
                                        days102 = 7;
                                        break;
                                    case 8:
                                        itemcode71 = "DA31";
                                        days102 = 7;
                                        break;
                                    case 9:
                                        itemcode71 = "DA32";
                                        days102 = 7;
                                        break;
                                    case 10:
                                        itemcode71 = "DA33";
                                        days102 = 7;
                                        break;
                                    case 11:
                                        itemcode71 = "DA34";
                                        days102 = 7;
                                        break;
                                    case 12:
                                        itemcode71 = "DA35";
                                        days102 = 7;
                                        break;
                                    case 13:
                                        itemcode71 = "DA36";
                                        days102 = 7;
                                        break;
                                    case 14:
                                        itemcode71 = "DA37";
                                        days102 = 7;
                                        break;
                                    case 15:
                                        itemcode71 = "DA38";
                                        days102 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode71, days102);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode71, days102));
                                return;
                            case "CZ69":
                                int num62 = Game_Server.Generic.random(0, 5);
                                int days16 = 1;
                                string itemcode70 = (string)null;
                                switch (num62)
                                {
                                    case 0:
                                        itemcode70 = "DC67";
                                        days16 = -1;
                                        break;
                                    case 1:
                                        itemcode70 = "DC04";
                                        days16 = 15;
                                        break;
                                    case 2:
                                        itemcode70 = "DC04";
                                        days16 = 30;
                                        break;
                                    case 3:
                                        itemcode70 = "CF01";
                                        days16 = 15;
                                        break;
                                    case 4:
                                        itemcode70 = "CI01";
                                        days16 = 15;
                                        break;
                                    case 5:
                                        itemcode70 = "CA01";
                                        days16 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode70, days16);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode70, days16));
                                return;
                            case "CZ67":
                                int num7 = Game_Server.Generic.random(0, 5);
                                int days4 = 1;
                                string itemcode3 = (string)null;
                                switch (num7)
                                {
                                    case 0:
                                        itemcode3 = "DC04";
                                        days4 = -1;
                                        break;
                                    case 1:
                                        itemcode3 = "DC04";
                                        days1 = 30;
                                        break;
                                    case 2:
                                        itemcode3 = "DC04";
                                        days4 = 15;
                                        break;
                                    case 3:
                                        itemcode3 = "CI01";
                                        days4 = 15;
                                        break;
                                    case 4:
                                        itemcode3 = "CF01";
                                        days4 = 15;
                                        break;
                                    case 5:
                                        itemcode3 = "CA01";
                                        days4 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode3, days4);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode3, days4));
                                return;
                            case "CR43":
                                int num45 = Game_Server.Generic.random(0, 6);
                                int days10 = 1;
                                string itemcode4 = (string)null;
                                switch (num45)
                                {
                                    case 0:
                                        itemcode4 = "DG68";
                                        days10 = -1;
                                        break;
                                    case 1:
                                        itemcode4 = "DG68";
                                        days10 = 15;
                                        break;
                                    case 2:
                                        itemcode4 = "DG53";
                                        days10 = 7;
                                        break;
                                    case 3:
                                        itemcode4 = "DC84";
                                        days10 = 7;
                                        break;
                                    case 4:
                                        itemcode4 = "DF84";
                                        days10 = 7;
                                        break;
                                    case 5:
                                        itemcode4 = "DA75";
                                        days10 = 7;
                                        break;
                                    case 6:
                                        itemcode4 = "DG68";
                                        days10 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode4, days10);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode4, days10));
                                return;
                            case "CR02":
                                int num9 = Game_Server.Generic.random(0, 5);
                                int days356 = 1;
                                string itemcode45 = (string)null;
                                switch (num9)
                                {
                                    case 0:
                                        itemcode45 = "DT34";
                                        days3 = 90;
                                        break;
                                    case 1:
                                        itemcode45 = "DT34";
                                        days3 = 30;
                                        break;
                                    case 2:
                                        itemcode45 = "DT34";
                                        days3 = 15;
                                        break;
                                    case 3:
                                        itemcode45 = "CF01";
                                        days3 = 15;
                                        break;
                                    case 4:
                                        itemcode45 = "CI01";
                                        days3 = 15;
                                        break;
                                    case 5:
                                        itemcode45 = "DS03";
                                        days3 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode45, days356);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode45, days356));
                                return;
                            case "CR64":
                                int num95 = Game_Server.Generic.random(0, 29);
                                int days338 = 1;
                                string itemcode300 = (string)null;
                                switch (num95)
                                {
                                    case 0:
                                        itemcode300 = "DB22";
                                        days338 = -1;
                                        break;
                                    case 1:
                                        itemcode300 = "DB22";
                                        days338 = 30;
                                        break;
                                    case 2:
                                        itemcode300 = "DB22";
                                        days338 = 90;
                                        break;
                                    case 3:
                                        itemcode300 = "DF24";
                                        days338 = 30;
                                        break;
                                    case 4:
                                        itemcode300 = "DF24";
                                        days338 = -1;
                                        break;
                                    case 5:
                                        itemcode300 = "DF24";
                                        days338 = 90;
                                        break;
                                    case 6:
                                        itemcode300 = "DC80";
                                        days338 = 90;
                                        break;
                                    case 7:
                                        itemcode300 = "DC80";
                                        days338 = 30;
                                        break;
                                    case 8:
                                        itemcode300 = "DC80";
                                        days338 = -1;
                                        break;
                                    case 9:
                                        itemcode300 = "DF34";
                                        days338 = -1;
                                        break;
                                    case 10:
                                        itemcode300 = "DF34";
                                        days338 = 30;
                                        break;
                                    case 11:
                                        itemcode300 = "DF34";
                                        days338 = 90;
                                        break;
                                    case 12:
                                        itemcode300 = "DF42";
                                        days338 = -1;
                                        break;
                                    case 13:
                                        itemcode300 = "DF42";
                                        days338 = 30;
                                        break;
                                    case 14:
                                        itemcode300 = "DF42";
                                        days338 = 90;
                                        break;
                                    case 15:
                                        itemcode300 = "DC82";
                                        days338 = 30;
                                        break;
                                    case 16:
                                        itemcode300 = "DC82";
                                        days338 = -1;
                                        break;
                                    case 17:
                                        itemcode300 = "DC82";
                                        days338 = 90;
                                        break;
                                    case 19:
                                        itemcode300 = "DG46";
                                        days338 = 30;
                                        break;
                                    case 18:
                                        itemcode300 = "DG46";
                                        days338 = -1;
                                        break;
                                    case 20:
                                        itemcode300 = "DG46";
                                        days338 = 90;
                                        break;
                                    case 21:
                                        itemcode300 = "DF76";
                                        days338 = 30;
                                        break;
                                    case 22:
                                        itemcode300 = "DF76";
                                        days338 = 90;
                                        break;
                                    case 23:
                                        itemcode300 = "DF76";
                                        days338 = -1;
                                        break;
                                    case 24:
                                        itemcode300 = "DG60";
                                        days338 = -1;
                                        break;
                                    case 25:
                                        itemcode300 = "DG60";
                                        days338 = 30;
                                        break;
                                    case 26:
                                        itemcode300 = "DG60";
                                        days338 = 90;
                                        break;
                                    case 28:
                                        itemcode300 = "DJ28";
                                        days338 = 90;
                                        break;
                                    case 27:
                                        itemcode300 = "DJ28";
                                        days338 = 30;
                                        break;
                                    case 29:
                                        itemcode300 = "DJ28";
                                        days338 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode300, days338);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode300, days338));
                                return;
                            case "CR63":
                                int num94 = Game_Server.Generic.random(0, 58);
                                int days308 = 1;
                                string itemcode100 = (string)null;
                                switch (num94)
                                {
                                    case 0:
                                        itemcode100 = "DB10";
                                        days308 = -1;
                                        break;
                                    case 1:
                                        itemcode100 = "DB10";
                                        days308 = 30;
                                        break;
                                    case 2:
                                        itemcode100 = "DB10";
                                        days308 = -1;
                                        break;
                                    case 3:
                                        itemcode100 = "DC33";
                                        days308 = 30;
                                        break;
                                    case 4:
                                        itemcode100 = "DC33";
                                        days308 = -1;
                                        break;
                                    case 5:
                                        itemcode100 = "DF35";
                                        days308 = 30;
                                        break;
                                    case 6:
                                        itemcode100 = "DF35";
                                        days308 = -1;
                                        break;
                                    case 7:
                                        itemcode100 = "DG13";
                                        days308 = 30;
                                        break;
                                    case 8:
                                        itemcode100 = "DG13";
                                        days308 = -1;
                                        break;
                                    case 9:
                                        itemcode100 = "DJ33";
                                        days308 = -1;
                                        break;
                                    case 10:
                                        itemcode100 = "DJ33";
                                        days308 = 30;
                                        break;
                                    case 11:
                                        itemcode100 = "DC64";
                                        days308 = 30;
                                        break;
                                    case 12:
                                        itemcode100 = "DC64";
                                        days308 = -1;
                                        break;
                                    case 13:
                                        itemcode100 = "DB16";
                                        days308 = 30;
                                        break;
                                    case 14:
                                        itemcode100 = "DB16";
                                        days308 = -1;
                                        break;
                                    case 15:
                                        itemcode100 = "DF37";
                                        days308 = 30;
                                        break;
                                    case 16:
                                        itemcode100 = "DF37";
                                        days308 = -1;
                                        break;
                                    case 17:
                                        itemcode100 = "DC39";
                                        days308 = -1;
                                        break;
                                    case 19:
                                        itemcode100 = "DC39";
                                        days308 = 30;
                                        break;
                                    case 18:
                                        itemcode100 = "DF95";
                                        days308 = 30;
                                        break;
                                    case 20:
                                        itemcode100 = "DF95";
                                        days308 = -1;
                                        break;
                                    case 21:
                                        itemcode100 = "DE07";
                                        days308 = 30;
                                        break;
                                    case 22:
                                        itemcode100 = "DE07";
                                        days308 = 30;
                                        break;
                                    case 23:
                                        itemcode100 = "DA45";
                                        days308 = -1;
                                        break;
                                    case 24:
                                        itemcode100 = "DF77";
                                        days308 = -1;
                                        break;
                                    case 25:
                                        itemcode100 = "DF77";
                                        days308 = 30;
                                        break;
                                    case 26:
                                        itemcode100 = "DB63";
                                        days308 = 30;
                                        break;
                                    case 28:
                                        itemcode100 = "DB63";
                                        days308 = -1;
                                        break;
                                    case 27:
                                        itemcode100 = "DH09";
                                        days308 = 30;
                                        break;
                                    case 29:
                                        itemcode100 = "DH09";
                                        days308 = -1;
                                        break;
                                    case 30:
                                        itemcode100 = "DF60";
                                        days308 = 30;
                                        break;
                                    case 31:
                                        itemcode100 = "DF60";
                                        days308 = -1;
                                        break;
                                    case 32:
                                        itemcode100 = "DE30";
                                        days308 = 30;
                                        break;
                                    case 33:
                                        itemcode100 = "DE30";
                                        days308 = -1;
                                        break;
                                    case 34:
                                        itemcode100 = "DE35";
                                        days308 = 30;
                                        break;
                                    case 35:
                                        itemcode100 = "DE35";
                                        days308 = -1;
                                        break;
                                    case 36:
                                        itemcode100 = "DE37";
                                        days308 = 30;
                                        break;
                                    case 37:
                                        itemcode100 = "DE37";
                                        days308 = -1;
                                        break;
                                    case 38:
                                        itemcode100 = "DE38";
                                        days308 = 30;
                                        break;
                                    case 39:
                                        itemcode100 = "DE38";
                                        days308 = -1;
                                        break;
                                    case 40:
                                        itemcode100 = "DE40";
                                        days308 = 30;
                                        break;
                                    case 41:
                                        itemcode100 = "DE40";
                                        days308 = -1;
                                        break;
                                    case 42:
                                        itemcode100 = "DE41";
                                        days308 = 30;
                                        break;
                                    case 43:
                                        itemcode100 = "DE41";
                                        days308 = -1;
                                        break;
                                    case 44:
                                        itemcode100 = "DE43";
                                        days308 = 30;
                                        break;
                                    case 45:
                                        itemcode100 = "DE43";
                                        days308 = -1;
                                        break;
                                    case 46:
                                        itemcode100 = "DE52";
                                        days308 = 30;
                                        break;
                                    case 47:
                                        itemcode100 = "DE52";
                                        days308 = -1;
                                        break;
                                    case 48:
                                        itemcode100 = "DA45";
                                        days308 = 30;
                                        break;
                                    case 49:
                                        itemcode100 = "DF77";
                                        days308 = -1;
                                        break;
                                    case 50:
                                        itemcode100 = "DF77";
                                        days308 = 30;
                                        break;
                                    case 51:
                                        itemcode100 = "DA72";
                                        days308 = -1;
                                        break;
                                    case 52:
                                        itemcode100 = "DA72";
                                        days308 = 30;
                                        break;
                                    case 53:
                                        itemcode100 = "DE53";
                                        days308 = -1;
                                        break;
                                    case 54:
                                        itemcode100 = "DE53";
                                        days308 = 30;
                                        break;
                                    case 55:
                                        itemcode100 = "DN14";
                                        days308 = -1;
                                        break;
                                    case 56:
                                        itemcode100 = "DN14";
                                        days308 = 30;
                                        break;
                                    case 57:
                                        itemcode100 = "DB49";
                                        days308 = -1;
                                        break;
                                    case 58:
                                        itemcode100 = "DB49";
                                        days308 = 30;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode100, days308);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode100, days308));
                                return;
                            case "CY98":
                                int num955 = Game_Server.Generic.random(0, 58);
                                int days310 = 1;
                                string itemcode130 = (string)null;
                                switch (num955)
                                {
                                    case 0:
                                        itemcode130 = "DB10";
                                        days308 = -1;
                                        break;
                                    case 1:
                                        itemcode130 = "DB10";
                                        days308 = 30;
                                        break;
                                    case 2:
                                        itemcode130 = "DB10";
                                        days308 = -1;
                                        break;
                                    case 3:
                                        itemcode130 = "DC33";
                                        days308 = 30;
                                        break;
                                    case 4:
                                        itemcode130 = "DC33";
                                        days308 = -1;
                                        break;
                                    case 5:
                                        itemcode130 = "DF35";
                                        days308 = 30;
                                        break;
                                    case 6:
                                        itemcode130 = "DF35";
                                        days308 = -1;
                                        break;
                                    case 7:
                                        itemcode130 = "DG13";
                                        days308 = 30;
                                        break;
                                    case 8:
                                        itemcode130 = "DG13";
                                        days308 = -1;
                                        break;
                                    case 9:
                                        itemcode130 = "DJ33";
                                        days308 = -1;
                                        break;
                                    case 10:
                                        itemcode130 = "DJ33";
                                        days308 = 30;
                                        break;
                                    case 11:
                                        itemcode130 = "DC64";
                                        days308 = 30;
                                        break;
                                    case 12:
                                        itemcode130 = "DC64";
                                        days308 = -1;
                                        break;
                                    case 13:
                                        itemcode130 = "DB16";
                                        days308 = 30;
                                        break;
                                    case 14:
                                        itemcode130 = "DB16";
                                        days308 = -1;
                                        break;
                                    case 15:
                                        itemcode130 = "DF37";
                                        days308 = 30;
                                        break;
                                    case 16:
                                        itemcode130 = "DF37";
                                        days308 = -1;
                                        break;
                                    case 17:
                                        itemcode130 = "DC39";
                                        days308 = -1;
                                        break;
                                    case 19:
                                        itemcode130 = "DC39";
                                        days308 = 30;
                                        break;
                                    case 18:
                                        itemcode130 = "DF95";
                                        days308 = 30;
                                        break;
                                    case 20:
                                        itemcode130 = "DF95";
                                        days308 = -1;
                                        break;
                                    case 21:
                                        itemcode130 = "DE07";
                                        days308 = 30;
                                        break;
                                    case 22:
                                        itemcode130 = "DE07";
                                        days308 = 30;
                                        break;
                                    case 23:
                                        itemcode130 = "DA45";
                                        days308 = -1;
                                        break;
                                    case 24:
                                        itemcode130 = "DF77";
                                        days308 = -1;
                                        break;
                                    case 25:
                                        itemcode130 = "DF77";
                                        days308 = 30;
                                        break;
                                    case 26:
                                        itemcode130 = "DB63";
                                        days308 = 30;
                                        break;
                                    case 28:
                                        itemcode130 = "DB63";
                                        days308 = -1;
                                        break;
                                    case 27:
                                        itemcode130 = "DH09";
                                        days308 = 30;
                                        break;
                                    case 29:
                                        itemcode130 = "DH09";
                                        days308 = -1;
                                        break;
                                    case 30:
                                        itemcode130 = "DF60";
                                        days308 = 30;
                                        break;
                                    case 31:
                                        itemcode130 = "DF60";
                                        days308 = -1;
                                        break;
                                    case 32:
                                        itemcode130 = "DE30";
                                        days308 = 30;
                                        break;
                                    case 33:
                                        itemcode130 = "DE30";
                                        days308 = -1;
                                        break;
                                    case 34:
                                        itemcode130 = "DE35";
                                        days308 = 30;
                                        break;
                                    case 35:
                                        itemcode130 = "DE35";
                                        days308 = -1;
                                        break;
                                    case 36:
                                        itemcode130 = "DE37";
                                        days308 = 30;
                                        break;
                                    case 37:
                                        itemcode130 = "DE37";
                                        days308 = -1;
                                        break;
                                    case 38:
                                        itemcode130 = "DE38";
                                        days308 = 30;
                                        break;
                                    case 39:
                                        itemcode130 = "DE38";
                                        days308 = -1;
                                        break;
                                    case 40:
                                        itemcode130 = "DE40";
                                        days308 = 30;
                                        break;
                                    case 41:
                                        itemcode130 = "DE40";
                                        days308 = -1;
                                        break;
                                    case 42:
                                        itemcode130 = "DE41";
                                        days308 = 30;
                                        break;
                                    case 43:
                                        itemcode130 = "DE41";
                                        days308 = -1;
                                        break;
                                    case 44:
                                        itemcode130 = "DE43";
                                        days308 = 30;
                                        break;
                                    case 45:
                                        itemcode130 = "DE43";
                                        days308 = -1;
                                        break;
                                    case 46:
                                        itemcode130 = "DE52";
                                        days308 = 30;
                                        break;
                                    case 47:
                                        itemcode130 = "DE52";
                                        days308 = -1;
                                        break;
                                    case 48:
                                        itemcode130 = "DA45";
                                        days308 = 30;
                                        break;
                                    case 49:
                                        itemcode130 = "DF77";
                                        days308 = -1;
                                        break;
                                    case 50:
                                        itemcode130 = "DF77";
                                        days308 = 30;
                                        break;
                                    case 51:
                                        itemcode130 = "DA72";
                                        days308 = -1;
                                        break;
                                    case 52:
                                        itemcode130 = "DA72";
                                        days308 = 30;
                                        break;
                                    case 53:
                                        itemcode130 = "DE53";
                                        days308 = -1;
                                        break;
                                    case 54:
                                        itemcode130 = "DE53";
                                        days308 = 30;
                                        break;
                                    case 55:
                                        itemcode130 = "DN14";
                                        days308 = -1;
                                        break;
                                    case 56:
                                        itemcode130 = "DN14";
                                        days308 = 30;
                                        break;
                                    case 57:
                                        itemcode130 = "DB49";
                                        days308 = -1;
                                        break;
                                    case 58:
                                        itemcode130 = "DB49";
                                        days308 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode130, days310);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode130, days310));
                                return;
                            case "CR46":
                                int num222 = Game_Server.Generic.random(0, 6);
                                int days190 = 1;
                                string itemcode181 = (string)null;
                                switch (num222)
                                {
                                    case 0:
                                        itemcode181 = "DF96";
                                        days190 = 30;
                                        break;
                                    case 1:
                                        itemcode181 = "CZ81";
                                        days190 = 1;
                                        break;
                                    case 2:
                                        itemcode181 = "DC19";
                                        days190 = 30;
                                        break;
                                    case 3:
                                        itemcode181 = "DC98";
                                        days190 = 30;
                                        break;
                                    case 4:
                                        itemcode181 = "DE30";
                                        days190 = 30;
                                        break;
                                    case 5:
                                        itemcode181 = "DE46";
                                        days190 = 30;
                                        break;
                                    case 6:
                                        itemcode181 = "CB09";
                                        days190 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode181, days190);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode181, days190));
                                return;
                            case "CR54":
                                int num99 = Game_Server.Generic.random(0, 13);
                                int days311 = 1;
                                string itemcode116 = (string)null;
                                switch (num99)
                                {
                                    case 0:
                                        itemcode116 = "DF80";
                                        days311 = 90;
                                        break;
                                    case 1:
                                        itemcode116 = "DF80";
                                        days311 = -1;
                                        break;
                                    case 2:
                                        itemcode116 = "DF81";
                                        days311 = -1;
                                        break;
                                    case 3:
                                        itemcode116 = "DF81";
                                        days311 = 90;
                                        break;
                                    case 4:
                                        itemcode116 = "DF58";
                                        days311 = -1;
                                        break;
                                    case 5:
                                        itemcode116 = "DF58";
                                        days311 = 90;
                                        break;
                                    case 6:
                                        itemcode116 = "DF69";
                                        days311 = -1;
                                        break;
                                    case 7:
                                        itemcode116 = "DF69";
                                        days311 = 90;
                                        break;
                                    case 8:
                                        itemcode116 = "DF75";
                                        days311 = -1;
                                        break;
                                    case 9:
                                        itemcode116 = "DF75";
                                        days311 = 90;
                                        break;
                                    case 10:
                                        itemcode116 = "DF79";
                                        days311 = -1;
                                        break;
                                    case 11:
                                        itemcode116 = "DF79";
                                        days311 = 90;
                                        break;
                                    case 12:
                                        itemcode116 = "DF89";
                                        days311 = -1;
                                        break;
                                    case 13:
                                        itemcode116 = "DF89";
                                        days311 = 90;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode116, days311);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode116, days311));
                                return;


                            case "CY52":
                                int num968 = Game_Server.Generic.random(0, 70);
                                int days322 = 1;
                                string itemcode127 = (string)null;
                                switch (num968)
                                {
                                    case 0:
                                        itemcode127 = "BV01";
                                        days322 = -1;
                                        break;
                                    case 1:
                                        itemcode127 = "BV02";
                                        days322 = -1;
                                        break;
                                    case 2:
                                        itemcode127 = "BV03";
                                        days322 = -1;
                                        break;
                                    case 3:
                                        itemcode127 = "BV11";
                                        days322 = -1;
                                        break;
                                    case 4:
                                        itemcode127 = "BV15";
                                        days322 = -1;
                                        break;
                                    case 5:
                                        itemcode127 = "DV16";
                                        days322 = -1;
                                        break;
                                    case 6:
                                        itemcode127 = "DV18";
                                        days322 = -1;
                                        break;
                                    case 7:
                                        itemcode127 = "DV19";
                                        days322 = -1;
                                        break;
                                    case 8:
                                        itemcode127 = "DV20";
                                        days322 = -1;
                                        break;
                                    case 9:
                                        itemcode127 = "BV01";
                                        days322 = 30;
                                        break;
                                    case 10:
                                        itemcode127 = "BV01";
                                        days322 = 15;
                                        break;
                                    case 11:
                                        itemcode127 = "BV02";
                                        days322 = 15;
                                        break;
                                    case 12:
                                        itemcode127 = "BV02";
                                        days322 = 30;
                                        break;
                                    case 13:
                                        itemcode127 = "BV03";
                                        days322 = 15;
                                        break;
                                    case 14:
                                        itemcode127 = "BV03";
                                        days322 = 30;
                                        break;
                                    case 15:
                                        itemcode127 = "BV11";
                                        days322 = 30;
                                        break;
                                    case 16:
                                        itemcode127 = "BV11";
                                        days322 = 15;
                                        break;
                                    case 17:
                                        itemcode127 = "BV11";
                                        days322 = 30;
                                        break;
                                    case 18:
                                        itemcode127 = "BV15";
                                        days322 = 15;
                                        break;
                                    case 19:
                                        itemcode127 = "BV15";
                                        days322 = 30;
                                        break;
                                    case 20:
                                        itemcode127 = "BV16";
                                        days322 = 15;
                                        break;
                                    case 21:
                                        itemcode127 = "BV16";
                                        days322 = 30;
                                        break;
                                    case 22:
                                        itemcode127 = "BV18";
                                        days322 = 30;
                                        break;
                                    case 23:
                                        itemcode127 = "BV18";
                                        days322 = 15;
                                        break;
                                    case 24:
                                        itemcode127 = "BV19";
                                        days322 = 30;
                                        break;
                                    case 25:
                                        itemcode127 = "BV19";
                                        days322 = 15;
                                        break;
                                    case 27:
                                        itemcode127 = "BV20";
                                        days322 = 30;
                                        break;
                                    case 26:
                                        itemcode127 = "BV20";
                                        days322 = 15;
                                        break;
                                    case 30:
                                        itemcode127 = "BV20";
                                        days322 = -1;
                                        break;
                                    case 28:
                                        itemcode127 = "BV17";
                                        days322 = 30;
                                        break;
                                    case 29:
                                        itemcode127 = "BV17";
                                        days322 = 15;
                                        break;
                                    case 31:
                                        itemcode127 = "BV08";
                                        days322 = -1;
                                        break;
                                    case 32:
                                        itemcode127 = "BV08";
                                        days322 = 30;
                                        break;
                                    case 33:
                                        itemcode127 = "BV08";
                                        days322 = 15;
                                        break;
                                    case 34:
                                        itemcode127 = "BV10";
                                        days322 = -1;
                                        break;
                                    case 35:
                                        itemcode127 = "BV10";
                                        days322 = 30;
                                        break;
                                    case 36:
                                        itemcode127 = "BV10";
                                        days322 = 15;
                                        break;
                                    case 37:
                                        itemcode127 = "BV04";
                                        days322 = -1;
                                        break;
                                    case 38:
                                        itemcode127 = "BV04";
                                        days322 = 30;
                                        break;
                                    case 39:
                                        itemcode127 = "BV04";
                                        days322 = 15;
                                        break;
                                    case 40:
                                        itemcode127 = "BV05";
                                        days322 = -1;
                                        break;
                                    case 41:
                                        itemcode127 = "BV05";
                                        days322 = 30;
                                        break;
                                    case 42:
                                        itemcode127 = "BV05";
                                        days322 = 15;
                                        break;
                                    case 43:
                                        itemcode127 = "BV06";
                                        days322 = -1;
                                        break;
                                    case 44:
                                        itemcode127 = "BV06";
                                        days322 = 30;
                                        break;
                                    case 45:
                                        itemcode127 = "BV06";
                                        days322 = 15;
                                        break;
                                    case 46:
                                        itemcode127 = "BV06";
                                        days322 = -1;
                                        break;
                                    case 47:
                                        itemcode127 = "BV07";
                                        days322 = 30;
                                        break;
                                    case 48:
                                        itemcode127 = "BV07";
                                        days322 = 15;
                                        break;
                                    case 49:
                                        itemcode127 = "BV07";
                                        days322 = -1;
                                        break;
                                    case 50:
                                        itemcode127 = "BV08";
                                        days322 = 30;
                                        break;
                                    case 51:
                                        itemcode127 = "BV08";
                                        days322 = 15;
                                        break;
                                    case 52:
                                        itemcode127 = "BV08";
                                        days322 = -1;
                                        break;
                                    case 53:
                                        itemcode127 = "BV10";
                                        days322 = 30;
                                        break;
                                    case 54:
                                        itemcode127 = "BV10";
                                        days322 = 15;
                                        break;
                                    case 55:
                                        itemcode127 = "BV10";
                                        days322 = -1;
                                        break;
                                    case 56:
                                        itemcode127 = "BV14";
                                        days322 = 30;
                                        break;
                                    case 57:
                                        itemcode127 = "BV14";
                                        days322 = 15;
                                        break;
                                    case 58:
                                        itemcode127 = "BV14";
                                        days322 = -1;
                                        break;
                                    case 59:
                                        itemcode127 = "BV21";
                                        days322 = 30;
                                        break;
                                    case 60:
                                        itemcode127 = "BV21";
                                        days322 = 15;
                                        break;
                                    case 61:
                                        itemcode127 = "BV21";
                                        days322 = -1;
                                        break;
                                    case 62:
                                        itemcode127 = "BV24";
                                        days322 = 30;
                                        break;
                                    case 63:
                                        itemcode127 = "BV24";
                                        days322 = 15;
                                        break;
                                    case 64:
                                        itemcode127 = "BV24";
                                        days322 = -1;
                                        break;
                                    case 65:
                                        itemcode127 = "BV23";
                                        days322 = 30;
                                        break;
                                    case 66:
                                        itemcode127 = "BV23";
                                        days322 = 15;
                                        break;
                                    case 67:
                                        itemcode127 = "BV23";
                                        days322 = -1;
                                        break;
                                    case 68:
                                        itemcode127 = "BV22";
                                        days322 = 30;
                                        break;
                                    case 69:
                                        itemcode127 = "BV22";
                                        days322 = 15;
                                        break;
                                    case 70:
                                        itemcode127 = "BV22";
                                        days322 = -1;
                                        break;
                                }
                                // Fix #1 to reload chars after opening this mystery box like u would still see same chars and parts just with new item like in papaya it doesnt bug out
                                Inventory.PerformAddItem(usr, itemcode127, days322);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode127, days322));

                                return;
                        
                            case "CZ06":
                                int num192 = Game_Server.Generic.random(0, 4);
                                int days360 = 1;
                                string itemcode146 = (string)null;
                                switch (num192)
                                {
                                    case 0:
                                        itemcode146 = "DE67";
                                        days360 = 30;
                                        break;
                                    case 1:
                                        itemcode146 = "D604";
                                        days360 = -1;
                                        break;
                                    case 2:
                                        itemcode146 = "DF65";
                                        days360 = 30;
                                        break;
                                    case 3:
                                        itemcode146 = "DF49";
                                        days360 = -1;
                                        break;
                                    case 4:
                                        itemcode146 = "DF67";
                                        days360 = 30;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode146, days360);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode146, days360));
                                return;
                            case "CZ04":
                                int num190 = Game_Server.Generic.random(0, 4);
                                int days359 = 1;
                                string itemcode135 = (string)null;
                                switch (num190)
                                {
                                    case 0:
                                        itemcode135 = "DC67";
                                        days359 = 30;
                                        break;
                                    case 1:
                                        itemcode135 = "DF42";
                                        days359 = 30;
                                        break;
                                    case 2:
                                        itemcode135 = "DC78";
                                        days359 = 30;
                                        break;
                                    case 3:
                                        itemcode135 = "DE49";
                                        days359 = 30;
                                        break;
                                    case 4:
                                        itemcode135 = "DF79";
                                        days359 = 30;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode135, days359);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode135, days359));
                                return;
                            case "CR76":
                                int num912 = Game_Server.Generic.random(0, 7);
                                int days206 = 1;
                                string itemcode49 = (string)null;
                                switch (num912)
                                {
                                    case 0:
                                        itemcode49 = "DF99";
                                        days206 = -1;
                                        break;
                                    case 1:
                                        itemcode49 = "DG88";
                                        days206 = -1;
                                        break;
                                    case 2:
                                        itemcode49 = "DE69";
                                        days206 = -1;
                                        break;
                                    case 3:
                                        itemcode49 = "DJ44";
                                        days206 = -1;
                                        break;
                                    case 4:
                                        itemcode49 = "DF99";
                                        days206 = 30;
                                        break;
                                    case 5:
                                        itemcode49 = "DG88";
                                        days206 = 30;
                                        break;
                                    case 6:
                                        itemcode49 = "DE69";
                                        days206 = 30;
                                        break;
                                    case 7:
                                        itemcode49 = "DJ44";
                                        days206 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode49, days206);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode49, days206));
                                return;
                            case "CR58":
                                int num911 = Game_Server.Generic.random(0, 11);
                                int days302 = 1;
                                string itemcode56 = (string)null;
                                switch (num911)
                                {
                                    case 0:
                                        itemcode56 = "DF36";
                                        days302 = -1;
                                        break;
                                    case 1:
                                        itemcode56 = "DF34";
                                        days302 = -1;
                                        break;
                                    case 2:
                                        itemcode56 = "DF65";
                                        days302 = -1;
                                        break;
                                    case 3:
                                        itemcode56 = "DF93";
                                        days302 = 30;
                                        break;
                                    case 4:
                                        itemcode56 = "DF36";
                                        days302 = 30;
                                        break;
                                    case 5:
                                        itemcode56 = "DF34";
                                        days302 = 30;
                                        break;
                                    case 6:
                                        itemcode56 = "DF65";
                                        days302 = 30;
                                        break;
                                    case 7:
                                        itemcode56 = "DF93";
                                        days302 = 30;
                                        break;
                                    case 8:
                                        itemcode56 = "DC34";
                                        days302 = 30;
                                        break;
                                    case 9:
                                        itemcode56 = "DC34";
                                        days302 = -1;
                                        break;
                                    case 10:
                                        itemcode56 = "DC93";
                                        days302 = 30;
                                        break;
                                    case 11:
                                        itemcode56 = "DC93";
                                        days302 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode56, days302);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode56, days302));
                                return;
                            case "CR14":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] items1 = Game_Server.Configs.Server.RandomBoxEvent.items;
                                int index2 = Game_Server.Generic.random(0, items1.Length - 1);
                                string str1 = items1[index2];
                                if (ItemManager.GetItem(str1) != null)
                                {
                                    int days107 = new Random().Next(Game_Server.Configs.Server.RandomBoxEvent.MinDays, Game_Server.Configs.Server.RandomBoxEvent.MaxDays);
                                    if (str1.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str1, days107);
                                    else
                                        Inventory.AddItem(usr, str1, days107);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str1, days107));
                                    return;
                                }
                                Log.WriteError(str1 + " is not a valid item @ random box event!");
                                return;
                            case "CZ99":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] items = Game_Server.Configs.Server.RandomBoxEvent.items;
                                int index3 = Game_Server.Generic.random(0, items.Length - 1);
                                string str = items[index3];
                                if (ItemManager.GetItem(str) != null)
                                {
                                    int days107 = new Random().Next(Game_Server.Configs.Server.RandomBoxEvent.MinDays, Game_Server.Configs.Server.RandomBoxEvent.MaxDays);
                                    if (str.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str, days107);
                                    else
                                        Inventory.AddItem(usr, str, days107);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str, days107));
                                    return;
                                }
                                Log.WriteError(str + " is not a valid item @ random box event!");
                                return;
                            case "CR55":
                                int num102 = Game_Server.Generic.random(0, 93);
                                int days115 = 1;
                                string itemcode103 = (string)null;
                                switch (num102)
                                {
                                    case 0:
                                        itemcode103 = "DC87";
                                        days115 = 90;
                                        break;
                                    case 1:
                                        itemcode103 = "DC87";
                                        days115 = 90;
                                        break;
                                    case 2:
                                        itemcode103 = "DC88";
                                        days115 = 90;
                                        break;
                                    case 3:
                                        itemcode103 = "DC96";
                                        days115 = 90;
                                        break;
                                    case 4:
                                        itemcode103 = "DC89";
                                        days115 = 90;
                                        break;
                                    case 5:
                                        itemcode103 = "DC90";
                                        days115 = 90;
                                        break;
                                    case 6:
                                        itemcode103 = "DC97";
                                        days115 = 90;
                                        break;
                                    case 7:
                                        itemcode103 = "DE17";
                                        days115 = 90;
                                        break;
                                    case 8:
                                        itemcode103 = "DE18";
                                        days115 = 90;
                                        break;
                                    case 9:
                                        itemcode103 = "DE19";
                                        days115 = 90;
                                        break;
                                    case 10:
                                        itemcode103 = "DE20";
                                        days115 = 90;
                                        break;
                                    case 11:
                                        itemcode103 = "DE21";
                                        days115 = 90;
                                        break;
                                    case 12:
                                        itemcode103 = "DE51";
                                        days115 = 90;
                                        break;
                                    case 14:
                                        itemcode103 = "DC87";
                                        days115 = -1;
                                        break;
                                    case 15:
                                        itemcode103 = "DC88";
                                        days115 = -1;
                                        break;
                                    case 16:
                                        itemcode103 = "DC96";
                                        days115 = -1;
                                        break;
                                    case 17:
                                        itemcode103 = "DC89";
                                        days115 = -1;
                                        break;
                                    case 18:
                                        itemcode103 = "DC90";
                                        days115 = -1;
                                        break;
                                    case 19:
                                        itemcode103 = "DC97";
                                        days115 = -1;
                                        break;
                                    case 20:
                                        itemcode103 = "DE17";
                                        days115 = -1;
                                        break;
                                    case 21:
                                        itemcode103 = "DE18";
                                        days115 = -1;
                                        break;
                                    case 22:
                                        itemcode103 = "DE19";
                                        days115 = -1;
                                        break;
                                    case 23:
                                        itemcode103 = "DE20";
                                        days115 = -1;
                                        break;
                                    case 24:
                                        itemcode103 = "DE21";
                                        days115 = -1;
                                        break;
                                    case 25:
                                        itemcode103 = "DE51";
                                        days115 = -1;
                                        break;
                                    case 26:
                                        itemcode103 = "DC21";
                                        days115 = -1;
                                        break;
                                    case 27:
                                        itemcode103 = "DC21";
                                        days115 = 90;
                                        break;
                                    case 29:
                                        itemcode103 = "DC22";
                                        days115 = 90;
                                        break;
                                    case 28:
                                        itemcode103 = "DC22";
                                        days115 = -1;
                                        break;
                                    case 30:
                                        itemcode103 = "DC23";
                                        days115 = 90;
                                        break;
                                    case 31:
                                        itemcode103 = "DC23";
                                        days115 = -1;
                                        break;
                                    case 32:
                                        itemcode103 = "DC24";
                                        days115 = 90;
                                        break;
                                    case 33:
                                        itemcode103 = "DC24";
                                        days115 = -1;
                                        break;
                                    case 34:
                                        itemcode103 = "DC25";
                                        days115 = -1;
                                        break;
                                    case 35:
                                        itemcode103 = "DC25";
                                        days115 = 90;
                                        break;
                                    case 36:
                                        itemcode103 = "DC26";
                                        days115 = 90;
                                        break;
                                    case 37:
                                        itemcode103 = "DC26";
                                        days115 = -1;
                                        break;
                                    case 38:
                                        itemcode103 = "DC27";
                                        days115 = 90;
                                        break;
                                    case 39:
                                        itemcode103 = "DC27";
                                        days115 = -1;
                                        break;
                                    case 41:
                                        itemcode103 = "DC28";
                                        days115 = 90;
                                        break;
                                    case 40:
                                        itemcode103 = "DC28";
                                        days115 = -1;
                                        break;
                                    case 42:
                                        itemcode103 = "DC29";
                                        days115 = 90;
                                        break;
                                    case 43:
                                        itemcode103 = "DC29";
                                        days115 = -1;
                                        break;
                                    case 44:
                                        itemcode103 = "DC30";
                                        days115 = 90;
                                        break;
                                    case 45:
                                        itemcode103 = "DC30";
                                        days115 = -1;
                                        break;
                                    case 46:
                                        itemcode103 = "DC41";
                                        days115 = 90;
                                        break;
                                    case 47:
                                        itemcode103 = "DC41";
                                        days115 = -1;
                                        break;
                                    case 48:
                                        itemcode103 = "DC42";
                                        days115 = 90;
                                        break;
                                    case 49:
                                        itemcode103 = "DC42";
                                        days115 = -1;
                                        break;
                                    case 50:
                                        itemcode103 = "DC43";
                                        days115 = 90;
                                        break;
                                    case 51:
                                        itemcode103 = "DC44";
                                        days115 = -1;
                                        break;
                                    case 52:
                                        itemcode103 = "DC44";
                                        days115 = 90;
                                        break;
                                    case 53:
                                        itemcode103 = "DC45";
                                        days115 = -1;
                                        break;
                                    case 54:
                                        itemcode103 = "DC45";
                                        days115 = 90;
                                        break;
                                    case 55:
                                        itemcode103 = "DC44";
                                        days115 = -1;
                                        break;
                                    case 56:
                                        itemcode103 = "DC45";
                                        days115 = 90;
                                        break;
                                    case 57:
                                        itemcode103 = "DC45";
                                        days115 = -1;
                                        break;
                                    case 58:
                                        itemcode103 = "DC46";
                                        days115 = 90;
                                        break;
                                    case 59:
                                        itemcode103 = "DC46";
                                        days115 = -1;
                                        break;
                                    case 60:
                                        itemcode103 = "DC47";
                                        days115 = 90;
                                        break;
                                    case 61:
                                        itemcode103 = "DC47";
                                        days115 = -1;
                                        break;
                                    case 62:
                                        itemcode103 = "DC48";
                                        days115 = 90;
                                        break;
                                    case 63:
                                        itemcode103 = "DC48";
                                        days115 = -1;
                                        break;
                                    case 64:
                                        itemcode103 = "DC49";
                                        days115 = 90;
                                        break;
                                    case 65:
                                        itemcode103 = "DC49";
                                        days115 = -1;
                                        break;
                                    case 66:
                                        itemcode103 = "DC50";
                                        days115 = 90;
                                        break;
                                    case 67:
                                        itemcode103 = "DC50";
                                        days115 = -1;
                                        break;
                                    case 68:
                                        itemcode103 = "DC51";
                                        days115 = 90;
                                        break;
                                    case 70:
                                        itemcode103 = "DC51";
                                        days115 = -1;
                                        break;
                                    case 69:
                                        itemcode103 = "DC52";
                                        days115 = 90;
                                        break;
                                    case 71:
                                        itemcode103 = "DC52";
                                        days115 = -1;
                                        break;
                                    case 72:
                                        itemcode103 = "DC53";
                                        days115 = 90;
                                        break;
                                    case 73:
                                        itemcode103 = "DC53";
                                        days115 = -1;
                                        break;
                                    case 75:
                                        itemcode103 = "DC54";
                                        days115 = 90;
                                        break;
                                    case 74:
                                        itemcode103 = "DC54";
                                        days115 = -1;
                                        break;
                                    case 76:
                                        itemcode103 = "DC55";
                                        days115 = 90;
                                        break;
                                    case 78:
                                        itemcode103 = "DC55";
                                        days115 = -1;
                                        break;
                                    case 77:
                                        itemcode103 = "DC56";
                                        days115 = 90;
                                        break;
                                    case 79:
                                        itemcode103 = "DC56";
                                        days115 = -1;
                                        break;
                                    case 80:
                                        itemcode103 = "DC57";
                                        days115 = 90;
                                        break;
                                    case 81:
                                        itemcode103 = "DC57";
                                        days115 = -1;
                                        break;
                                    case 82:
                                        itemcode103 = "DC58";
                                        days115 = 90;
                                        break;
                                    case 83:
                                        itemcode103 = "DC58";
                                        days115 = -1;
                                        break;
                                    case 84:
                                        itemcode103 = "DC59";
                                        days115 = 90;
                                        break;
                                    case 85:
                                        itemcode103 = "DC59";
                                        days115 = -1;
                                        break;
                                    case 86:
                                        itemcode103 = "DC60";
                                        days115 = 90;
                                        break;
                                    case 87:
                                        itemcode103 = "DC60";
                                        days115 = -1;
                                        break;
                                    case 88:
                                        itemcode103 = "DC66";
                                        days115 = 90;
                                        break;
                                    case 89:
                                        itemcode103 = "DC66";
                                        days115 = -1;
                                        break;
                                    case 90:
                                        itemcode103 = "DC62";
                                        days115 = 90;
                                        break;
                                    case 91:
                                        itemcode103 = "DC62";
                                        days115 = -1;
                                        break;
                                    case 92:
                                        itemcode103 = "DC63";
                                        days115 = 90;
                                        break;
                                    case 93:
                                        itemcode103 = "DC63";
                                        days115 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode103, days115);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode103, days115));
                                return;
                            case "CR39":
                                int num107 = Game_Server.Generic.random(0, 9);
                                int days117 = 1;
                                string itemcode109 = (string)null;
                                switch (num107)
                                {
                                    case 0:
                                        itemcode109 = "DF52";
                                        days117 = 30;
                                        break;
                                    case 1:
                                        itemcode109 = "DF18";
                                        days117 = 15;
                                        break;
                                    case 2:
                                        itemcode109 = "CZ81";
                                        days117 = 1;
                                        break;
                                    case 3:
                                        itemcode109 = "DF15";
                                        days117 = 7;
                                        break;
                                    case 4:
                                        itemcode109 = "DE35";
                                        days117 = 7;
                                        break;
                                    case 5:
                                        itemcode109 = "DB33";
                                        days117 = 7;
                                        break;
                                    case 6:
                                        itemcode109 = "CB09";
                                        days117 = 1;
                                        break;
                                    case 8:
                                        itemcode109 = "CZ85";
                                        days117 = 1;
                                        break;
                                    case 7:
                                        itemcode109 = "CZ83";
                                        days117 = 1;
                                        break;
                                    case 9:
                                        itemcode109 = "CZ81";
                                        days117 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode109, days117);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode109, days117));
                                return;

                            case "CR96":
                                int num110 = Game_Server.Generic.random(0, 6);
                                int days127 = 1;
                                string itemcode199 = (string)null;
                                switch (num110)
                                {
                                    case 0:
                                        itemcode199 = "GF02";
                                        days127 = 30;
                                        break;
                                    case 1:
                                        itemcode199 = "GF02";
                                        days127 = -1;
                                        break;
                                    case 2:
                                        itemcode199 = "DF50";
                                        days127 = 15;
                                        break;

                                    case 3:
                                        itemcode199 = "CF01";
                                        days127 = 15;
                                        break;

                                    case 4:
                                        itemcode199 = "CI01";
                                        days127 = 15;
                                        break;

                                    case 5:
                                        itemcode199 = "CB09";
                                        days127 = 1;
                                        break;
                                    case 6:
                                        itemcode199 = "CZ84";
                                        days127 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode199, days127);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode199, days127));
                                return;
                            case "CR30":
                                int num1088 = Game_Server.Generic.random(0, 6);
                                int days116 = 1;
                                string itemcode105 = (string)null;
                                switch (num1088)
                                {
                                    case 0:
                                        itemcode105 = "DF39";
                                        days116 = 30;
                                        break;
                                    case 1:
                                        itemcode105 = "DF39";
                                        days116 = 7;
                                        break;
                                    case 2:
                                        itemcode105 = "DF95";
                                        days116 = 7;
                                        break;
                                    case 3:
                                        itemcode105 = "DF15";
                                        days116 = 7;
                                        break;
                                    case 4:
                                        itemcode105 = "DE35";
                                        days116 = 7;
                                        break;
                                    case 5:
                                        itemcode105 = "DF71";
                                        days116 = 7;
                                        break;
                                    case 6:
                                        itemcode105 = "CB09";
                                        days116 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode105, days116);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode105, days116));
                                return;
                            case "CR40":
                                int num72 = Game_Server.Generic.random(0, 5);
                                int days176 = 1;
                                string itemcode120 = (string)null;
                                switch (num72)
                                {
                                    case 0:
                                        itemcode120 = "DF65";
                                        days176 = 30;
                                        break;
                                    case 1:
                                        itemcode120 = "CZ84";
                                        days176 = 1;
                                        break;
                                    case 2:
                                        itemcode120 = "CZ81";
                                        days176 = 1;
                                        break;
                                    case 3:
                                        itemcode120 = "CB09";
                                        days176 = 1;
                                        break;
                                    case 4:
                                        itemcode120 = "CF01";
                                        days176 = 7;
                                        break;
                                    case 5:
                                        itemcode120 = "DB10";
                                        days176 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode120, days176);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode120, days176));
                                return;
                            case "CR41":
                                int num74 = Game_Server.Generic.random(0, 5);
                                int days177 = 1;
                                string itemcode122 = (string)null;
                                switch (num74)
                                {
                                    case 0:
                                        itemcode122 = "DC93";
                                        days177 = 30;
                                        break;
                                    case 1:
                                        itemcode122 = "CZ84";
                                        days177 = 1;
                                        break;
                                    case 2:
                                        itemcode122 = "CZ81";
                                        days177 = 1;
                                        break;
                                    case 3:
                                        itemcode122 = "CB09";
                                        days177 = 1;
                                        break;
                                    case 4:
                                        itemcode122 = "CF02";
                                        days177 = 7;
                                        break;
                                    case 5:
                                        itemcode122 = "DB10";
                                        days177 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode122, days177);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode122, days177));
                                return;
                            case "CR42":
                                int num75 = Game_Server.Generic.random(0, 5);
                                int days178 = 1;
                                string itemcode123 = (string)null;
                                switch (num75)
                                {
                                    case 0:
                                        itemcode123 = "DD07";
                                        days178 = 30;
                                        break;
                                    case 1:
                                        itemcode123 = "CZ84";
                                        days178 = 1;
                                        break;
                                    case 2:
                                        itemcode123 = "CZ81";
                                        days178 = 1;
                                        break;
                                    case 3:
                                        itemcode123 = "CB09";
                                        days178 = 1;
                                        break;
                                    case 4:
                                        itemcode123 = "CF02";
                                        days178 = 7;
                                        break;
                                    case 5:
                                        itemcode123 = "DB10";
                                        days178 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode123, days178);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode123, days178));
                                return;
                            case "CR44":
                                int num76 = Game_Server.Generic.random(0, 5);
                                int days180 = 1;
                                string itemcode124 = (string)null;
                                switch (num76)
                                {
                                    case 0:
                                        itemcode124 = "DF36";
                                        days180 = 30;
                                        break;
                                    case 1:
                                        itemcode124 = "CZ84";
                                        days180 = 1;
                                        break;
                                    case 2:
                                        itemcode124 = "CZ81";
                                        days180 = 1;
                                        break;
                                    case 3:
                                        itemcode124 = "CB09";
                                        days180 = 1;
                                        break;
                                    case 4:
                                        itemcode124 = "DS01";
                                        days180 = 7;
                                        break;
                                    case 5:
                                        itemcode124 = "DB10";
                                        days180 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode124, days180);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode124, days180));
                                return;
                            case "CR38":
                                int num73 = Game_Server.Generic.random(0, 12);
                                int days175 = 1;
                                string itemcode119 = (string)null;
                                switch (num73)
                                {
                                    case 0:
                                        itemcode119 = "DF52";
                                        days175 = 30;
                                        break;
                                    case 1:
                                        itemcode119 = "DF39";
                                        days175 = 7;
                                        break;
                                    case 2:
                                        itemcode119 = "DF95";
                                        days175 = 7;
                                        break;
                                    case 3:
                                        itemcode119 = "DF15";
                                        days175 = 7;
                                        break;
                                    case 4:
                                        itemcode119 = "DE35";
                                        days175 = 7;
                                        break;
                                    case 5:
                                        itemcode119 = "DF71";
                                        days175 = 7;
                                        break;
                                    case 6:
                                        itemcode119 = "CB09";
                                        days175 = 1;
                                        break;
                                    case 7:
                                        itemcode119 = "DF06";
                                        days175 = -1;
                                        break;
                                    case 8:
                                        itemcode119 = "DG03";
                                        days175 = -1;
                                        break;
                                    case 9:
                                        itemcode119 = "CZ84";
                                        days175 = 1;
                                        break;
                                    case 10:
                                        itemcode119 = "CZ85";
                                        days175 = 1;
                                        break;
                                    case 11:
                                        itemcode119 = "DC01";
                                        days175 = -1;
                                        break;
                                    case 12:
                                        itemcode119 = "CZ81";
                                        days175 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode119, days175);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode119, days175));
                                return;
                            case "CR48":
                                int num10 = Game_Server.Generic.random(0, 6);
                                int days307 = 1;
                                string itemcode58 = (string)null;
                                switch (num10)
                                {
                                    case 0:
                                        itemcode58 = "DJ16";
                                        days307 = 30;
                                        break;
                                    case 1:
                                        itemcode58 = "CB09";
                                        days307 = 7;
                                        break;
                                    case 2:
                                        itemcode58 = "CZ81";
                                        days307 = 7;
                                        break;
                                    case 3:
                                        itemcode58 = "CZ84";
                                        days307 = 7;
                                        break;
                                    case 4:
                                        itemcode58 = "DF04";
                                        days307 = -1;
                                        break;
                                    case 5:
                                        itemcode58 = "DF71";
                                        days307 = 7;
                                        break;
                                    case 6:
                                        itemcode58 = "DJ16";
                                        days307 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode58, days307);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode58, days307));
                                return;
                            case "CR93":
                                int num197 = Game_Server.Generic.random(0, 4);
                                int days334 = 1;
                                string itemcode506 = (string)null;
                                switch (num197)
                                {
                                    case 0:
                                        itemcode506 = "D910";
                                        days334 = 60;
                                        break;
                                    case 1:
                                        itemcode506 = "D910";
                                        days334 = 90;
                                        break;
                                    case 2:
                                        itemcode506 = "DI06";
                                        days334 = 180;
                                        break;
                                    case 3:
                                        itemcode506 = "DI06";
                                        days334 = 360;
                                        break;
                                    case 4:
                                        itemcode506 = "DG26";
                                        days334 = 5000;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode506, days334);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode506, days334));
                                return;
                            case "CR61":
                                int num196 = Game_Server.Generic.random(0, 23);
                                int days332 = 1;
                                string itemcode51 = (string)null;
                                switch (num196)
                                {
                                    case 0:
                                        itemcode51 = "DD32";
                                        days332 = -1;
                                        break;
                                    case 1:
                                        itemcode51 = "DD32";
                                        days332 = 30;
                                        break;
                                    case 2:
                                        itemcode51 = "DI06";
                                        days332 = -1;
                                        break;
                                    case 3:
                                        itemcode51 = "DI06";
                                        days332 = 30;
                                        break;
                                    case 4:
                                        itemcode51 = "DG26";
                                        days332 = -1;
                                        break;
                                    case 5:
                                        itemcode51 = "DG26";
                                        days332 = 30;
                                        break;
                                    case 6:
                                        itemcode51 = "DK31";
                                        days332 = -1;
                                        break;
                                    case 7:
                                        itemcode51 = "DK31";
                                        days332 = 30;
                                        break;
                                    case 8:
                                        itemcode51 = "GF16";
                                        days332 = 30;
                                        break;
                                    case 9:
                                        itemcode51 = "DF86";
                                        days332 = -1;
                                        break;
                                    case 10:
                                        itemcode51 = "DF86";
                                        days332 = 30;
                                        break;
                                    case 11:
                                        itemcode51 = "DG79";
                                        days332 = -1;
                                        break;
                                    case 12:
                                        itemcode51 = "DG79";
                                        days332 = 30;
                                        break;
                                    case 13:
                                        itemcode51 = "DE59";
                                        days332 = -1;
                                        break;

                                    case 14:
                                        itemcode51 = "DE59";
                                        days332 = 30;
                                        break;
                                    case 15:
                                        itemcode51 = "DJ37";
                                        days332 = 30;
                                        break;
                                    case 16:
                                        itemcode51 = "DF37";
                                        days332 = -1;
                                        break;
                                    case 17:
                                        itemcode51 = "GF16";
                                        days332 = -1;
                                        break;
                                    case 18:
                                        itemcode51 = "GG04";
                                        days332 = 30;
                                        break;
                                    case 19:
                                        itemcode51 = "GG04";
                                        days332 = -1;
                                        break;
                                    case 20:
                                        itemcode51 = "DE87";
                                        days332 = 30;
                                        break;
                                    case 21:
                                        itemcode51 = "DE87";
                                        days332 = -1;
                                        break;
                                    case 22:
                                        itemcode51 = "DJ59";
                                        days332 = 30;
                                        break;
                                    case 23:
                                        itemcode51 = "DJ59";
                                        days332 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode51, days332);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode51, days332));
                                return;
                            case "CR60":
                                int num100 = Game_Server.Generic.random(0, 21);
                                int days317 = 1;
                                string itemcode98 = (string)null;
                                switch (num100)
                                {
                                    case 0:
                                        itemcode98 = "DC40";
                                        days317 = -1;
                                        break;
                                    case 1:
                                        itemcode98 = "DF14";
                                        days317 = -1;
                                        break;
                                    case 2:
                                        itemcode98 = "DJ07";
                                        days317 = -1;
                                        break;
                                    case 3:
                                        itemcode98 = "DG37";
                                        days317 = -1;
                                        break;
                                    case 4:
                                        itemcode98 = "DJ15";
                                        days317 = -1;
                                        break;
                                    case 5:
                                        itemcode98 = "DC95";
                                        days317 = -1;
                                        break;
                                    case 6:
                                        itemcode98 = "DF26";
                                        days317 = -1;
                                        break;
                                    case 7:
                                        itemcode98 = "DG36";
                                        days317 = -1;
                                        break;
                                    case 8:
                                        itemcode98 = "DC72";
                                        days317 = -1;
                                        break;
                                    case 9:
                                        itemcode98 = "DF27";
                                        days317 = -1;
                                        break;
                                    case 10:
                                        itemcode98 = "DC40";
                                        days317 = 30;
                                        break;
                                    case 11:
                                        itemcode98 = "DF14";
                                        days317 = 30;
                                        break;
                                    case 12:
                                        itemcode98 = "DJ07";
                                        days317 = -1;
                                        break;
                                    case 13:
                                        itemcode98 = "DG37";
                                        days317 = 30;
                                        break;
                                    case 14:
                                        itemcode98 = "DJ15";
                                        days317 = 30;
                                        break;
                                    case 15:
                                        itemcode98 = "DC95";
                                        days317 = 30;
                                        break;
                                    case 16:
                                        itemcode98 = "DF26";
                                        days317 = 30;
                                        break;
                                    case 17:
                                        itemcode98 = "DG36";
                                        days317 = 30;
                                        break;
                                    case 18:
                                        itemcode98 = "DC72";
                                        days317 = 30;
                                        break;
                                    case 19:
                                        itemcode98 = "DF27";
                                        days317 = 30;
                                        break;
                                    case 21:
                                        itemcode98 = "CF01";
                                        days317 = 7;
                                        break;
                                    case 20:
                                        itemcode98 = "CZ85";
                                        days317 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode98, days317);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode98, days317));
                                return;
                            case "CZ90":
                                int num211 = Game_Server.Generic.random(0, 5);
                                int days301 = 1;
                                string itemcode55 = (string)null;
                                switch (num211)
                                {
                                    case 0:
                                        itemcode55 = "DE13";
                                        days301 = -1;
                                        break;
                                    case 1:
                                        itemcode55 = "DE11";
                                        days301 = 30;
                                        break;
                                    case 2:
                                        itemcode55 = "DE11";
                                        days301 = 15;
                                        break;
                                    case 3:
                                        itemcode55 = "CF01";
                                        days301 = 15;
                                        break;
                                    case 4:
                                        itemcode55 = "CI01";
                                        days301 = 15;
                                        break;
                                    case 5:
                                        itemcode55 = "CA01";
                                        days301 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode55, days301);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode55, days301));
                                return;
                            case "CZ91":
                                int num999 = Game_Server.Generic.random(0, 5);
                                int days300 = 1;
                                string itemcode57 = (string)null;
                                switch (num999)
                                {
                                    case 0:
                                        itemcode57 = "DD05";
                                        days300 = -1;
                                        break;
                                    case 1:
                                        itemcode57 = "DD05";
                                        days300 = 30;
                                        break;
                                    case 2:
                                        itemcode57 = "DD05";
                                        days300 = 15;
                                        break;
                                    case 3:
                                        itemcode57 = "CF01";
                                        days300 = 15;
                                        break;
                                    case 4:
                                        itemcode57 = "CI01";
                                        days300 = 15;
                                        break;
                                    case 5:
                                        itemcode57 = "CA01";
                                        days300 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode57, days300);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode57, days300));
                                return;
                            case "CZ93":
                                int num11 = Game_Server.Generic.random(0, 6);
                                int days33 = 1;
                                string itemcode7 = (string)null;
                                switch (num11)
                                {
                                    case 0:
                                        itemcode7 = "DG39";
                                        days33 = -1;
                                        break;
                                    case 1:
                                        itemcode7 = "DG39";
                                        days33 = 5;
                                        break;
                                    case 2:
                                        itemcode7 = "DG38";
                                        days33 = 30;
                                        break;
                                    case 3:
                                        itemcode7 = "DG38";
                                        days33 = 10;
                                        break;
                                    case 4:
                                        itemcode7 = "CF01";
                                        days33 = 15;
                                        break;
                                    case 5:
                                        itemcode7 = "CI01";
                                        days33 = 15;
                                        break;
                                    case 6:
                                        itemcode7 = "CA01";
                                        days33 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode7, days33);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode7, days33));
                                return;
                            case "CZ70":
                                int num31 = Game_Server.Generic.random(0, 5);
                                int days90 = 1;
                                string itemcode8 = (string)null;
                                switch (num31)
                                {
                                    case 0:
                                        itemcode8 = "DC64";
                                        days90 = -1;
                                        break;
                                    case 1:
                                        itemcode8 = "DC04";
                                        days90 = 15;
                                        break;
                                    case 2:
                                        itemcode8 = "DC04";
                                        days90 = 30;
                                        break;
                                    case 3:
                                        itemcode8 = "CF01";
                                        days90 = 10;
                                        break;
                                    case 4:
                                        itemcode8 = "CI01";
                                        days90 = 15;
                                        break;
                                    case 5:
                                        itemcode8 = "CA01";
                                        days90 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode8, days90);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode8, days90));
                                return;
                            case "CZ74":
                                int num32 = Game_Server.Generic.random(0, 5);
                                int days304 = 1;
                                string itemcode11 = (string)null;
                                switch (num32)
                                {
                                    case 0:
                                        itemcode11 = "DC68";
                                        days304 = -1;
                                        break;
                                    case 1:
                                        itemcode11 = "DC76";
                                        days304 = 15;
                                        break;
                                    case 2:
                                        itemcode11 = "DC76";
                                        days304 = 30;
                                        break;
                                    case 3:
                                        itemcode11 = "CF01";
                                        days304 = 10;
                                        break;
                                    case 4:
                                        itemcode11 = "CI01";
                                        days304 = 15;
                                        break;
                                    case 5:
                                        itemcode11 = "CA01";
                                        days304 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode11, days304);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode11, days304));
                                return;
                            case "CZ76":
                                int num30 = Game_Server.Generic.random(0, 5);
                                int days305 = 1;
                                string itemcode91 = (string)null;
                                switch (num30)
                                {
                                    case 0:
                                        itemcode91 = "DC39";
                                        days305 = -1;
                                        break;
                                    case 1:
                                        itemcode91 = "DC39";
                                        days305 = 15;
                                        break;
                                    case 2:
                                        itemcode91 = "DC39";
                                        days305 = 30;
                                        break;
                                    case 3:
                                        itemcode91 = "CF01";
                                        days305 = 10;
                                        break;
                                    case 4:
                                        itemcode91 = "CI01";
                                        days305 = 15;
                                        break;
                                    case 5:
                                        itemcode91 = "CA01";
                                        days305 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode91, days305);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode91, days305));
                                return;

                            case "CZ77":
                                int num85 = Game_Server.Generic.random(0, 5);
                                int days30 = 1;
                                string itemcode92 = (string)null;
                                switch (num85)
                                {
                                    case 0:
                                        itemcode92 = "DI12";
                                        days30 = -1;
                                        break;
                                    case 1:
                                        itemcode92 = "DI11";
                                        days30 = 15;
                                        break;
                                    case 2:
                                        itemcode92 = "DI11";
                                        days30 = 30;
                                        break;
                                    case 3:
                                        itemcode92 = "CF01";
                                        days30 = 10;
                                        break;
                                    case 4:
                                        itemcode92 = "CI01";
                                        days30 = 15;
                                        break;
                                    case 5:
                                        itemcode92 = "CA01";
                                        days30 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode92, days30);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode92, days30));
                                return;
                            case "CZ78":
                                int num77 = Game_Server.Generic.random(0, 5);
                                int days306 = 1;
                                string itemcode93 = (string)null;
                                switch (num77)
                                {
                                    case 0:
                                        itemcode93 = "DF24";
                                        days306 = -1;
                                        break;
                                    case 1:
                                        itemcode93 = "DF23";
                                        days306 = 15;
                                        break;
                                    case 2:
                                        itemcode93 = "DF23";
                                        days306 = 30;
                                        break;
                                    case 3:
                                        itemcode92 = "CF01";
                                        days306 = 10;
                                        break;
                                    case 4:
                                        itemcode92 = "CI01";
                                        days306 = 15;
                                        break;
                                    case 5:
                                        itemcode92 = "CA01";
                                        days306 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode93, days306);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode93, days306));
                                return;
                            case "CR34":
                                int num113 = Game_Server.Generic.random(0, 5);
                                int days64 = 1;
                                string itemcode32 = (string)null;
                                switch (num113)
                                {
                                    case 0:
                                        itemcode32 = "DC80";
                                        days64 = 30;
                                        break;
                                    case 1:
                                        itemcode32 = "DC80";
                                        days64 = -1;
                                        break;
                                    case 2:
                                        itemcode32 = "DB10";
                                        days64 = 7;
                                        break;
                                    case 3:
                                        itemcode32 = "CZ81";
                                        days64 = 1;
                                        break;
                                    case 4:
                                        itemcode32 = "CZ85";
                                        days64 = 1;
                                        break;
                                    case 5:
                                        itemcode32 = "CB09";
                                        days64 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode32, days64);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode32, days64));
                                return;
                            case "CR35":
                                int num125 = Game_Server.Generic.random(0, 5);
                                int days215 = 1;
                                string itemcode620 = (string)null;
                                switch (num125)
                                {
                                    case 0:
                                        itemcode620 = "DG46";
                                        days215 = 30;
                                        break;
                                    case 1:
                                        itemcode620 = "DG46";
                                        days215 = -1;
                                        break;
                                    case 2:
                                        itemcode620 = "DF95";
                                        days215 = 7;
                                        break;
                                    case 3:
                                        itemcode620 = "CZ84";
                                        days215 = 1;
                                        break;
                                    case 4:
                                        itemcode620 = "CB09";
                                        days215 = 1;
                                        break;
                                    case 5:
                                        itemcode620 = "CZ81";
                                        days215 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode620, days215);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode620, days215));
                                return;
                            case "CR69":
                                int num166 = Game_Server.Generic.random(0, 5);
                                int days225 = 1;
                                string itemcode645 = (string)null;
                                switch (num166)
                                {
                                    case 0:
                                        itemcode645 = "DE68";
                                        days225 = 30;
                                        break;
                                    case 1:
                                        itemcode645 = "DE68";
                                        days225 = -1;
                                        break;
                                    case 2:
                                        itemcode645 = "DF95";
                                        days225 = 7;
                                        break;
                                    case 3:
                                        itemcode645 = "CZ84";
                                        days225 = 1;
                                        break;
                                    case 4:
                                        itemcode645 = "CB09";
                                        days225 = 1;
                                        break;
                                    case 5:
                                        itemcode645 = "CZ81";
                                        days225 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode645, days225);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode645, days225));
                                return;
                            case "CY99":
                                int num176 = Game_Server.Generic.random(0, 5);
                                int days555 = 1;
                                string itemcode655 = (string)null;
                                switch (num176)
                                {
                                    case 0:
                                        itemcode655 = "DE68";
                                        days555 = 30;
                                        break;
                                    case 1:
                                        itemcode655 = "DE68";
                                        days555 = -1;
                                        break;
                                    case 2:
                                        itemcode655 = "DF95";
                                        days555 = 7;
                                        break;
                                    case 3:
                                        itemcode655 = "CZ84";
                                        days555 = 1;
                                        break;
                                    case 4:
                                        itemcode655 = "CB09";
                                        days555 = 1;
                                        break;
                                    case 5:
                                        itemcode655 = "CZ81";
                                        days225 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode655, days555);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode655, days555));
                                return;
                            case "CR99":
                                int num170 = Game_Server.Generic.random(0, 5);
                                int days559 = 1;
                                string itemcode651 = (string)null;
                                switch (num170)
                                {
                                    case 0:
                                        itemcode651 = "DE68";
                                        days559 = 30;
                                        break;
                                    case 1:
                                        itemcode651 = "DE68";
                                        days559 = -1;
                                        break;
                                    case 2:
                                        itemcode651 = "DF95";
                                        days559 = 7;
                                        break;
                                    case 3:
                                        itemcode651 = "CZ84";
                                        days559 = 1;
                                        break;
                                    case 4:
                                        itemcode651 = "CB09";
                                        days555 = 1;
                                        break;
                                    case 5:
                                        itemcode651 = "CZ81";
                                        days225 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode651, days559);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode651, days559));
                                return;
                            case "CR68":
                                int num127 = Game_Server.Generic.random(0, 46);
                                int days220 = 1;
                                string itemcode640 = (string)null;
                                switch (num127)
                                {
                                    case 0:
                                        itemcode640 = "DG22";
                                        days220 = 30;
                                        break;
                                    case 1:
                                        itemcode640 = "DG22";
                                        days220 = -1;
                                        break;
                                    case 2:
                                        itemcode640 = "DG24";
                                        days220 = 30;
                                        break;
                                    case 3:
                                        itemcode640 = "DG24";
                                        days220 = -1;
                                        break;
                                    case 4:
                                        itemcode640 = "DG28";
                                        days220 = 30;
                                        break;
                                    case 5:
                                        itemcode640 = "DG28";
                                        days220 = -1;
                                        break;
                                    case 6:
                                        itemcode640 = "DG45";
                                        days220 = 30;
                                        break;
                                    case 7:
                                        itemcode640 = "DG45";
                                        days220 = -1;
                                        break;
                                    case 8:
                                        itemcode640 = "DG46";
                                        days220 = 30;
                                        break;
                                    case 9:
                                        itemcode640 = "DG46";
                                        days220 = -1;
                                        break;
                                    case 10:
                                        itemcode640 = "DG50";
                                        days220 = 30;
                                        break;
                                    case 11:
                                        itemcode640 = "DG50";
                                        days220 = -1;
                                        break;
                                    case 12:
                                        itemcode640 = "DG51";
                                        days220 = 30;
                                        break;
                                    case 13:
                                        itemcode640 = "DG51";
                                        days220 = -1;
                                        break;
                                    case 14:
                                        itemcode640 = "DG55";
                                        days220 = -1;
                                        break;
                                    case 15:
                                        itemcode640 = "DG55";
                                        days220 = 30;
                                        break;
                                    case 16:
                                        itemcode640 = "DG58";
                                        days220 = -1;
                                        break;
                                    case 17:
                                        itemcode640 = "DG58";
                                        days220 = 30;
                                        break;
                                    case 18:
                                        itemcode640 = "DG59";
                                        days220 = 30;
                                        break;
                                    case 19:
                                        itemcode640 = "DG59";
                                        days220 = -1;
                                        break;
                                    case 20:
                                        itemcode640 = "DG71";
                                        days220 = 30;
                                        break;
                                    case 21:
                                        itemcode640 = "DG71";
                                        days220 = -1;
                                        break;
                                    case 22:
                                        itemcode640 = "DG82";
                                        days220 = 30;
                                        break;
                                    case 23:
                                        itemcode640 = "DG82";
                                        days220 = -1;
                                        break;
                                    case 24:
                                        itemcode640 = "DG85";
                                        days220 = 30;
                                        break;
                                    case 25:
                                        itemcode640 = "DG85";
                                        days220 = -1;
                                        break;
                                    case 26:
                                        itemcode640 = "DG86";
                                        days220 = 30;
                                        break;
                                    case 27:
                                        itemcode640 = "DG86";
                                        days220 = -1;
                                        break;
                                    case 28:
                                        itemcode640 = "DG88";
                                        days220 = 30;
                                        break;
                                    case 29:
                                        itemcode640 = "DG91";
                                        days220 = -1;
                                        break;
                                    case 30:
                                        itemcode640 = "DG91";
                                        days220 = 30;
                                        break;
                                    case 31:
                                        itemcode640 = "DG95";
                                        days220 = -1;
                                        break;
                                    case 32:
                                        itemcode640 = "DG95";
                                        days220 = 30;
                                        break;
                                    case 33:
                                        itemcode640 = "DG97";
                                        days220 = -1;
                                        break;
                                    case 34:
                                        itemcode640 = "DG97";
                                        days220 = 30;
                                        break;
                                    case 35:
                                        itemcode640 = "GG08";
                                        days220 = -1;
                                        break;
                                    case 36:
                                        itemcode640 = "GG08";
                                        days220 = 30;
                                        break;
                                    case 37:
                                        itemcode640 = "GG10";
                                        days220 = -1;
                                        break;
                                    case 38:
                                        itemcode640 = "GG10";
                                        days220 = 30;
                                        break;
                                    case 39:
                                        itemcode640 = "GG17";
                                        days220 = 30;
                                        break;
                                    case 40:
                                        itemcode640 = "GG17";
                                        days220 = -1;
                                        break;
                                    case 41:
                                        itemcode640 = "GG27";
                                        days220 = 30;
                                        break;
                                    case 42:
                                        itemcode640 = "GG27";
                                        days220 = -1;
                                        break;
                                    case 43:
                                        itemcode640 = "GG28";
                                        days220 = 30;
                                        break;
                                    case 44:
                                        itemcode640 = "GG28";
                                        days220 = -1;
                                        break;
                                    case 45:
                                        itemcode640 = "GG32";
                                        days220 = 30;
                                        break;
                                    case 46:
                                        itemcode640 = "GG32";
                                        days220 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode640, days220);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode640, days220));
                                return;
                            case "CR87":
                                int num86 = Game_Server.Generic.random(0, 4);
                                int days800 = 1;
                                string itemcode102 = (string)null;
                                switch (num86)
                                {
                                    case 0:
                                        itemcode102 = "DG91";
                                        days800 = 30;
                                        break;
                                    case 1:
                                        itemcode102 = "DG91";
                                        days800 = -1;
                                        break;
                                    case 2:
                                        itemcode102 = "DS01";
                                        days800 = 7;
                                        break;
                                    case 3:
                                        itemcode102 = "CI01";
                                        days800 = 7;
                                        break;
                                    case 4:
                                        itemcode102 = "CC05";
                                        days800 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode102, days800);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode102, days800));
                                return;
                            case "CY50":
                                int num861 = Game_Server.Generic.random(0, 23);
                                int days820 = 1;
                                string itemcode152 = (string)null;
                                switch (num861)
                                {
                                    case 0:
                                        itemcode152 = "BH2A";
                                        days820 = 30;
                                        break;
                                    case 1:
                                        itemcode152 = "BH2A";
                                        days820 = -1;
                                        break;
                                    case 2:
                                        itemcode152 = "BH2A";
                                        days820 = 15;
                                        break;
                                    case 3:
                                        itemcode152 = "BH2E";
                                        days820 = 30;
                                        break;
                                    case 4:
                                        itemcode152 = "BH2E";
                                        days820 = 15;
                                        break;
                                    case 5:
                                        itemcode152 = "BH2E";
                                        days820 = -1;
                                        break;
                                    case 6:
                                        itemcode152 = "BH2B";
                                        days820 = 30;
                                        break;
                                    case 7:
                                        itemcode152 = "BH2B";
                                        days820 = 15;
                                        break;
                                    case 8:
                                        itemcode152 = "BH2B";
                                        days820 = -1;
                                        break;
                                    case 9:
                                        itemcode152 = "BH2B";
                                        days820 = 30;
                                        break;
                                    case 10:
                                        itemcode152 = "BH2B";
                                        days820 = 15;
                                        break;
                                    case 11:
                                        itemcode152 = "BH2B";
                                        days820 = -1;
                                        break;
                                    case 12:
                                        itemcode152 = "BH32";
                                        days820 = 30;
                                        break;
                                    case 13:
                                        itemcode152 = "BH32";
                                        days820 = 15;
                                        break;
                                    case 14:
                                        itemcode152 = "BH32";
                                        days820 = -1;
                                        break;
                                    case 15:
                                        itemcode152 = "BH34";
                                        days820 = 30;
                                        break;
                                    case 16:
                                        itemcode152 = "BH34";
                                        days820 = 15;
                                        break;
                                    case 17:
                                        itemcode152 = "BH34";
                                        days820 = -1;
                                        break;
                                    case 18:
                                        itemcode152 = "BH29";
                                        days820 = 30;
                                        break;
                                    case 19:
                                        itemcode152 = "BH29";
                                        days820 = 15;
                                        break;
                                    case 20:
                                        itemcode152 = "BH29";
                                        days820 = -1;
                                        break;
                                    case 21:
                                        itemcode152 = "BH2D";
                                        days820 = 30;
                                        break;
                                    case 22:
                                        itemcode152 = "BH2D";
                                        days820 = 15;
                                        break;
                                    case 23:
                                        itemcode152 = "BH2D";
                                        days820 = -1;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode152, days820);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode152, days820));
                                return;
                            case "CR70":
                                int num862 = Game_Server.Generic.random(0, 18);
                                int days822 = 1;
                                string itemcode154 = (string)null;
                                switch (num862)
                                {
                                    case 0:
                                        itemcode154 = "BH10";
                                        days822 = 30;
                                        break;
                                    case 1:
                                        itemcode154 = "BH10";
                                        days822 = -1;
                                        break;
                                    case 2:
                                        itemcode154 = "BH10";
                                        days822 = 15;
                                        break;
                                    case 3:
                                        itemcode154 = "BH1C";
                                        days822 = 30;
                                        break;
                                    case 4:
                                        itemcode154 = "BH1C";
                                        days822 = 15;
                                        break;
                                    case 5:
                                        itemcode154 = "BH1C";
                                        days822 = -1;
                                        break;
                                    case 6:
                                        itemcode154 = "BH15";
                                        days822 = 30;
                                        break;
                                    case 7:
                                        itemcode154 = "BH15";
                                        days822 = 15;
                                        break;
                                    case 8:
                                        itemcode154 = "BH15";
                                        days822 = -1;
                                        break;
                                    case 9:
                                        itemcode154 = "BH21";
                                        days822 = 30;
                                        break;
                                    case 10:
                                        itemcode154 = "BH21";
                                        days822 = 15;
                                        break;
                                    case 11:
                                        itemcode154 = "BH21";
                                        days822 = -1;
                                        break;
                                    case 12:
                                        itemcode154 = "BH1B";
                                        days822 = 30;
                                        break;
                                    case 13:
                                        itemcode154 = "BH1B";
                                        days822 = 15;
                                        break;
                                    case 14:
                                        itemcode154 = "BH1B";
                                        days822 = -1;
                                        break;
                                    case 15:
                                        itemcode154 = "BH27";
                                        days822 = 30;
                                        break;
                                    case 16:
                                        itemcode154 = "BH27";
                                        days822 = 15;
                                        break;
                                    case 17:
                                        itemcode154 = "BH27";
                                        days822 = -1;
                                        break;
                                    case 18:
                                        itemcode154 = "BH27";
                                        days822 = 30;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode154, days822);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode154, days822));
                                return;
                            case "CR71":
                                int num872 = Game_Server.Generic.random(0, 17);
                                int days825 = 1;
                                string itemcode155 = (string)null;
                                switch (num872)
                                {
                                    case 0:
                                        itemcode155 = "BH11";
                                        days825 = 30;
                                        break;
                                    case 1:
                                        itemcode155 = "BH11";
                                        days825 = -1;
                                        break;
                                    case 2:
                                        itemcode155 = "BH11";
                                        days825 = 15;
                                        break;
                                    case 3:
                                        itemcode155 = "BH28";
                                        days825 = 30;
                                        break;
                                    case 4:
                                        itemcode155 = "BH28";
                                        days825 = 15;
                                        break;
                                    case 5:
                                        itemcode155 = "BH28";
                                        days825 = -1;
                                        break;
                                    case 6:
                                        itemcode155 = "BH17";
                                        days825 = 30;
                                        break;
                                    case 7:
                                        itemcode155 = "BH17";
                                        days825 = 15;
                                        break;
                                    case 8:
                                        itemcode155 = "BH17";
                                        days825 = -1;
                                        break;
                                    case 9:
                                        itemcode155 = "BH23";
                                        days825 = 30;
                                        break;
                                    case 10:
                                        itemcode155 = "BH23";
                                        days825 = 15;
                                        break;
                                    case 11:
                                        itemcode155 = "BH23";
                                        days825 = -1;
                                        break;
                                    case 12:
                                        itemcode155 = "BH18";
                                        days825 = 30;
                                        break;
                                    case 13:
                                        itemcode155 = "BH18";
                                        days825 = 15;
                                        break;
                                    case 14:
                                        itemcode155 = "BH18";
                                        days825 = -1;
                                        break;
                                    case 15:
                                        itemcode155 = "BH24";
                                        days825 = 30;
                                        break;
                                    case 16:
                                        itemcode155 = "BH24";
                                        days825 = 15;
                                        break;
                                    case 17:
                                        itemcode155 = "BH24";
                                        days825 = -1;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode155, days825);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode155, days825));
                                return;
                            case "CR72":
                                int num874 = Game_Server.Generic.random(0, 17);
                                int days830 = 1;
                                string itemcode160 = (string)null;
                                switch (num874)
                                {
                                    case 0:
                                        itemcode160 = "BH12";
                                        days830 = 30;
                                        break;
                                    case 1:
                                        itemcode160 = "BH12";
                                        days830 = -1;
                                        break;
                                    case 2:
                                        itemcode160 = "BH12";
                                        days830 = 15;
                                        break;
                                    case 3:
                                        itemcode160 = "BH1E";
                                        days830 = 30;
                                        break;
                                    case 4:
                                        itemcode160 = "BH1E";
                                        days830 = 15;
                                        break;
                                    case 5:
                                        itemcode160 = "BH1E";
                                        days830 = -1;
                                        break;
                                    case 6:
                                        itemcode160 = "BH16";
                                        days830 = 30;
                                        break;
                                    case 7:
                                        itemcode160 = "BH16";
                                        days830 = 15;
                                        break;
                                    case 8:
                                        itemcode160 = "BH16";
                                        days830 = -1;
                                        break;
                                    case 9:
                                        itemcode160 = "BH22";
                                        days830 = 30;
                                        break;
                                    case 10:
                                        itemcode160 = "BH22";
                                        days830 = 15;
                                        break;
                                    case 11:
                                        itemcode160 = "BH22";
                                        days830 = -1;
                                        break;
                                    case 12:
                                        itemcode160 = "BH1A";
                                        days830 = 30;
                                        break;
                                    case 13:
                                        itemcode160 = "BH1A";
                                        days830 = 15;
                                        break;
                                    case 14:
                                        itemcode160 = "BH1A";
                                        days830 = -1;
                                        break;
                                    case 15:
                                        itemcode160 = "BH26";
                                        days830 = 30;
                                        break;
                                    case 16:
                                        itemcode160 = "BH26";
                                        days830 = 15;
                                        break;
                                    case 17:
                                        itemcode160 = "BH26";
                                        days830 = -1;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode160, days830);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode160, days830));
                                return;
                            case "CR73":
                                int num875 = Game_Server.Generic.random(0, 17);
                                int days835 = 1;
                                string itemcode165 = (string)null;
                                switch (num875)
                                {
                                    case 0:
                                        itemcode165 = "BH13";
                                        days835 = 30;
                                        break;
                                    case 1:
                                        itemcode165 = "BH13";
                                        days835 = -1;
                                        break;
                                    case 2:
                                        itemcode165 = "BH13";
                                        days835 = 15;
                                        break;
                                    case 3:
                                        itemcode165 = "BH1F";
                                        days835 = 30;
                                        break;
                                    case 4:
                                        itemcode165 = "BH1F";
                                        days835 = 15;
                                        break;
                                    case 5:
                                        itemcode165 = "BH1F";
                                        days835 = -1;
                                        break;
                                    case 6:
                                        itemcode165 = "BH14";
                                        days835 = 30;
                                        break;
                                    case 7:
                                        itemcode165 = "BH14";
                                        days835 = 15;
                                        break;
                                    case 8:
                                        itemcode165 = "BH14";
                                        days835 = -1;
                                        break;
                                    case 9:
                                        itemcode165 = "BH20";
                                        days835 = 30;
                                        break;
                                    case 10:
                                        itemcode165 = "BH20";
                                        days835 = 15;
                                        break;
                                    case 11:
                                        itemcode165 = "BH20";
                                        days835 = -1;
                                        break;
                                    case 12:
                                        itemcode165 = "BH19";
                                        days835 = 30;
                                        break;
                                    case 13:
                                        itemcode165 = "BH19";
                                        days835 = 15;
                                        break;
                                    case 14:
                                        itemcode165 = "BH19";
                                        days835 = -1;
                                        break;
                                    case 15:
                                        itemcode165 = "BH25";
                                        days835 = 30;
                                        break;
                                    case 16:
                                        itemcode165 = "BH25";
                                        days835 = 15;
                                        break;
                                    case 17:
                                        itemcode165 = "BH25";
                                        days835 = -1;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode165, days835);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode165, days835));
                                return;
                            case "CR89":
                                int num129 = Game_Server.Generic.random(0, 98);
                                int days223 = 1;
                                string itemcode643 = (string)null;
                                switch (num129)
                                {
                                    case 0:
                                        itemcode643 = "DF35";
                                        days223 = 30;
                                        break;
                                    case 1:
                                        itemcode643 = "DF35";
                                        days223 = -1;
                                        break;
                                    case 2:
                                        itemcode643 = "DF14";
                                        days223 = 30;
                                        break;
                                    case 3:
                                        itemcode643 = "DF14";
                                        days223 = -1;
                                        break;
                                    case 4:
                                        itemcode643 = "DF65";
                                        days223 = 30;
                                        break;
                                    case 5:
                                        itemcode643 = "DF65";
                                        days223 = -1;
                                        break;
                                    case 6:
                                        itemcode643 = "DF95";
                                        days223 = 30;
                                        break;
                                    case 7:
                                        itemcode643 = "DF95";
                                        days223 = -1;
                                        break;
                                    case 8:
                                        itemcode643 = "DF25";
                                        days223 = 30;
                                        break;
                                    case 9:
                                        itemcode643 = "DF25";
                                        days223 = -1;
                                        break;
                                    case 10:
                                        itemcode643 = "DF47";
                                        days223 = 30;
                                        break;
                                    case 11:
                                        itemcode643 = "DF47";
                                        days223 = -1;
                                        break;
                                    case 12:
                                        itemcode643 = "DF51";
                                        days223 = 30;
                                        break;
                                    case 13:
                                        itemcode643 = "DF51";
                                        days223 = -1;
                                        break;
                                    case 14:
                                        itemcode643 = "DF52";
                                        days223 = -1;
                                        break;
                                    case 15:
                                        itemcode643 = "DF52";
                                        days223 = 30;
                                        break;
                                    case 16:
                                        itemcode643 = "DF53";
                                        days223 = -1;
                                        break;
                                    case 17:
                                        itemcode643 = "DF53";
                                        days223 = 30;
                                        break;
                                    case 18:
                                        itemcode643 = "DF58";
                                        days223 = 30;
                                        break;
                                    case 19:
                                        itemcode643 = "DF58";
                                        days223 = -1;
                                        break;
                                    case 20:
                                        itemcode643 = "DF60";
                                        days223 = 30;
                                        break;
                                    case 21:
                                        itemcode643 = "DF60";
                                        days223 = -1;
                                        break;
                                    case 22:
                                        itemcode643 = "DF61";
                                        days223 = 30;
                                        break;
                                    case 23:
                                        itemcode643 = "DF61";
                                        days223 = -1;
                                        break;
                                    case 24:
                                        itemcode643 = "DF62";
                                        days223 = 30;
                                        break;
                                    case 25:
                                        itemcode643 = "DF62";
                                        days223 = -1;
                                        break;
                                    case 26:
                                        itemcode643 = "DF63";
                                        days223 = 30;
                                        break;
                                    case 27:
                                        itemcode643 = "DF63";
                                        days223 = -1;
                                        break;
                                    case 28:
                                        itemcode643 = "DF64";
                                        days223 = 30;
                                        break;
                                    case 29:
                                        itemcode643 = "DF64";
                                        days223 = -1;
                                        break;
                                    case 30:
                                        itemcode643 = "DF69";
                                        days223 = 30;
                                        break;
                                    case 31:
                                        itemcode643 = "DF69";
                                        days223 = -1;
                                        break;
                                    case 32:
                                        itemcode643 = "DF74";
                                        days223 = 30;
                                        break;
                                    case 33:
                                        itemcode643 = "DF74";
                                        days223 = -1;
                                        break;
                                    case 34:
                                        itemcode643 = "DF75";
                                        days223 = 30;
                                        break;
                                    case 35:
                                        itemcode643 = "DF75";
                                        days223 = -1;
                                        break;
                                    case 36:
                                        itemcode643 = "DF79";
                                        days223 = 30;
                                        break;
                                    case 37:
                                        itemcode643 = "DF79";
                                        days223 = -1;
                                        break;
                                    case 38:
                                        itemcode643 = "DF85";
                                        days223 = 30;
                                        break;
                                    case 39:
                                        itemcode643 = "DF85";
                                        days223 = 30;
                                        break;
                                    case 40:
                                        itemcode643 = "DF86";
                                        days223 = -1;
                                        break;
                                    case 41:
                                        itemcode643 = "DF86";
                                        days223 = 30;
                                        break;
                                    case 42:
                                        itemcode643 = "DF89";
                                        days223 = -1;
                                        break;
                                    case 43:
                                        itemcode643 = "DF89";
                                        days223 = 30;
                                        break;
                                    case 44:
                                        itemcode643 = "DF94";
                                        days223 = -1;
                                        break;
                                    case 45:
                                        itemcode643 = "DF94";
                                        days223 = 30;
                                        break;
                                    case 46:
                                        itemcode643 = "DF97";
                                        days223 = -1;
                                        break;
                                    case 47:
                                        itemcode643 = "DF98";
                                        days223 = 30;
                                        break;
                                    case 48:
                                        itemcode643 = "DF98";
                                        days223 = -1;
                                        break;
                                    case 49:
                                        itemcode643 = "DF99";
                                        days223 = 30;
                                        break;
                                    case 50:
                                        itemcode643 = "DF99";
                                        days223 = -1;
                                        break;
                                    case 51:
                                        itemcode643 = "GF19";
                                        days223 = 30;
                                        break;
                                    case 52:
                                        itemcode643 = "GF19";
                                        days223 = -1;
                                        break;
                                    case 53:
                                        itemcode643 = "GF35";
                                        days223 = 30;
                                        break;
                                    case 54:
                                        itemcode643 = "GF35";
                                        days223 = -1;
                                        break;
                                    case 55:
                                        itemcode643 = "GF36";
                                        days223 = 30;
                                        break;
                                    case 56:
                                        itemcode643 = "GF36";
                                        days223 = -1;
                                        break;
                                    case 57:
                                        itemcode643 = "GF41";
                                        days223 = 30;
                                        break;
                                    case 58:
                                        itemcode643 = "GF41";
                                        days223 = -1;
                                        break;
                                    case 59:
                                        itemcode643 = "GF42";
                                        days223 = 30;
                                        break;
                                    case 60:
                                        itemcode643 = "GF42";
                                        days223 = -1;
                                        break;
                                    case 61:
                                        itemcode643 = "DF35";
                                        days223 = 15;
                                        break;
                                    case 62:
                                        itemcode643 = "DF14";
                                        days223 = 15;
                                        break;
                                    case 63:
                                        itemcode643 = "DF65";
                                        days223 = 15;
                                        break;
                                    case 64:
                                        itemcode643 = "DF95";
                                        days223 = 15;
                                        break;
                                    case 65:
                                        itemcode643 = "DF25";
                                        days223 = 15;
                                        break;
                                    case 66:
                                        itemcode643 = "DF47";
                                        days223 = 15;
                                        break;
                                    case 67:
                                        itemcode643 = "DF50";
                                        days223 = 15;
                                        break;
                                    case 68:
                                        itemcode643 = "DF51";
                                        days223 = 15;
                                        break;
                                    case 69:
                                        itemcode643 = "DF52";
                                        days223 = 15;
                                        break;
                                    case 70:
                                        itemcode643 = "DF53";
                                        days223 = 15;
                                        break;
                                    case 71:
                                        itemcode643 = "DF58";
                                        days223 = 15;
                                        break;
                                    case 72:
                                        itemcode643 = "DF60";
                                        days223 = 15;
                                        break;
                                    case 73:
                                        itemcode643 = "DF61";
                                        days223 = 15;
                                        break;
                                    case 74:
                                        itemcode643 = "DF62";
                                        days223 = 15;
                                        break;
                                    case 75:
                                        itemcode643 = "DF63";
                                        days223 = 15;
                                        break;
                                    case 76:
                                        itemcode643 = "DF64";
                                        days223 = 15;
                                        break;
                                    case 77:
                                        itemcode643 = "DF69";
                                        days223 = 15;
                                        break;
                                    case 78:
                                        itemcode643 = "DF74";
                                        days223 = 15;
                                        break;
                                    case 79:
                                        itemcode643 = "DF75";
                                        days223 = 15;
                                        break;
                                    case 80:
                                        itemcode643 = "DF79";
                                        days223 = 15;
                                        break;
                                    case 81:
                                        itemcode643 = "DF85";
                                        days223 = 15;
                                        break;
                                    case 82:
                                        itemcode643 = "DF86";
                                        days223 = 15;
                                        break;
                                    case 83:
                                        itemcode643 = "DF89";
                                        days223 = 15;
                                        break;
                                    case 84:
                                        itemcode643 = "DF94";
                                        days223 = 15;
                                        break;
                                    case 85:
                                        itemcode643 = "DF97";
                                        days223 = 15;
                                        break;
                                    case 86:
                                        itemcode643 = "DF98";
                                        days223 = 15;
                                        break;
                                    case 87:
                                        itemcode643 = "DF99";
                                        days223 = 15;
                                        break;
                                    case 88:
                                        itemcode643 = "GF19";
                                        days223 = 15;
                                        break;
                                    case 89:
                                        itemcode643 = "GF35";
                                        days223 = 15;
                                        break;
                                    case 90:
                                        itemcode643 = "GF36";
                                        days223 = 15;
                                        break;
                                    case 91:
                                        itemcode643 = "GF41";
                                        days223 = 15;
                                        break;
                                    case 92:
                                        itemcode643 = "GF42";
                                        days223 = 15;
                                        break;
                                    case 93:
                                        itemcode643 = "GF50";
                                        days223 = 30;
                                        break;
                                    case 94:
                                        itemcode643 = "GF50";
                                        days223 = 15;
                                        break;
                                    case 95:
                                        itemcode643 = "GF50";
                                        days223 = -1;
                                        break;
                                    case 96:
                                        itemcode643 = "GF52";
                                        days223 = 30;
                                        break;
                                    case 97:
                                        itemcode643 = "GF52";
                                        days223 = 15;
                                        break;
                                    case 98:
                                        itemcode643 = "GF52";
                                        days223 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode643, days223);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode643, days223));
                                return;
                            case "CR75":
                                int num130 = Game_Server.Generic.random(0, 88);
                                int days229 = 1;
                                string itemcode642 = (string)null;
                                switch (num130)

                                {
                                    case 0:
                                        itemcode642 = "DC33";
                                        days229 = 30;
                                        break;
                                    case 1:
                                        itemcode642 = "DC33";
                                        days229 = -1;
                                        break;
                                    case 2:
                                        itemcode642 = "DC93";
                                        days229 = 30;
                                        break;
                                    case 3:
                                        itemcode642 = "DC93";
                                        days229 = -1;
                                        break;
                                    case 4:
                                        itemcode642 = "DE07";
                                        days229 = 30;
                                        break;
                                    case 5:
                                        itemcode642 = "DE07";
                                        days229 = -1;
                                        break;
                                    case 6:
                                        itemcode642 = "DC73";
                                        days229 = 30;
                                        break;
                                    case 7:
                                        itemcode642 = "DC73";
                                        days229 = -1;
                                        break;
                                    case 8:
                                        itemcode642 = "DC78";
                                        days229 = 30;
                                        break;
                                    case 9:
                                        itemcode642 = "DC78";
                                        days229 = -1;
                                        break;
                                    case 10:
                                        itemcode642 = "DC79";
                                        days229 = 30;
                                        break;
                                    case 11:
                                        itemcode642 = "DC79";
                                        days229 = -1;
                                        break;
                                    case 12:
                                        itemcode642 = "DC79";
                                        days229 = 30;
                                        break;
                                    case 13:
                                        itemcode642 = "DC80";
                                        days229 = -1;
                                        break;
                                    case 14:
                                        itemcode642 = "DC80";
                                        days229 = -1;
                                        break;
                                    case 15:
                                        itemcode642 = "DC98";
                                        days229 = 30;
                                        break;
                                    case 16:
                                        itemcode642 = "DC98";
                                        days229 = -1;
                                        break;
                                    case 17:
                                        itemcode642 = "DE28";
                                        days229 = 30;
                                        break;
                                    case 18:
                                        itemcode642 = "DE28";
                                        days229 = 30;
                                        break;
                                    case 19:
                                        itemcode642 = "DE30";
                                        days229 = -1;
                                        break;
                                    case 20:
                                        itemcode642 = "DE30";
                                        days229 = 30;
                                        break;
                                    case 21:
                                        itemcode642 = "DE31";
                                        days229 = -1;
                                        break;
                                    case 22:
                                        itemcode642 = "DE31";
                                        days229 = 30;
                                        break;
                                    case 23:
                                        itemcode642 = "DE33";
                                        days229 = -1;
                                        break;
                                    case 24:
                                        itemcode642 = "DE33";
                                        days229 = 30;
                                        break;
                                    case 25:
                                        itemcode642 = "DE45";
                                        days229 = -1;
                                        break;
                                    case 26:
                                        itemcode642 = "DE45";
                                        days229 = 30;
                                        break;
                                    case 27:
                                        itemcode642 = "DE46";
                                        days229 = -1;
                                        break;
                                    case 28:
                                        itemcode642 = "DE46";
                                        days229 = 30;
                                        break;
                                    case 29:
                                        itemcode642 = "DE49";
                                        days229 = -1;
                                        break;
                                    case 30:
                                        itemcode642 = "DE55";
                                        days229 = 30;
                                        break;
                                    case 31:
                                        itemcode642 = "DE55";
                                        days229 = -1;
                                        break;
                                    case 32:
                                        itemcode642 = "DE60";
                                        days229 = 30;
                                        break;
                                    case 33:
                                        itemcode642 = "DE60";
                                        days229 = -1;
                                        break;
                                    case 34:
                                        itemcode642 = "DE64";
                                        days229 = 30;
                                        break;
                                    case 35:
                                        itemcode642 = "DE64";
                                        days229 = -1;
                                        break;
                                    case 36:
                                        itemcode642 = "DE65";
                                        days229 = 30;
                                        break;
                                    case 37:
                                        itemcode642 = "DE65";
                                        days229 = -1;
                                        break;
                                    case 38:
                                        itemcode642 = "DE66";
                                        days229 = 30;
                                        break;
                                    case 39:
                                        itemcode642 = "DE66";
                                        days229 = 30;
                                        break;
                                    case 40:
                                        itemcode642 = "DE67";
                                        days229 = -1;
                                        break;
                                    case 41:
                                        itemcode642 = "DE67";
                                        days229 = 30;
                                        break;
                                    case 42:
                                        itemcode642 = "DE69";
                                        days229 = -1;
                                        break;
                                    case 43:
                                        itemcode642 = "DE69";
                                        days229 = 30;
                                        break;
                                    case 44:
                                        itemcode642 = "DE81";
                                        days229 = -1;
                                        break;
                                    case 45:
                                        itemcode642 = "DE81";
                                        days229 = 30;
                                        break;
                                    case 46:
                                        itemcode642 = "DE94";
                                        days229 = -1;
                                        break;
                                    case 47:
                                        itemcode642 = "DE94";
                                        days229 = 30;
                                        break;
                                    case 48:
                                        itemcode642 = "GC06";
                                        days229 = -1;
                                        break;
                                    case 49:
                                        itemcode642 = "GC06";
                                        days229 = 30;
                                        break;
                                    case 50:
                                        itemcode642 = "GC08";
                                        days229 = -1;
                                        break;
                                    case 51:
                                        itemcode642 = "GC08";
                                        days229 = 30;
                                        break;
                                    case 52:
                                        itemcode642 = "GC12";
                                        days229 = -1;
                                        break;
                                    case 53:
                                        itemcode642 = "GC12";
                                        days229 = 30;
                                        break;
                                    case 54:
                                        itemcode642 = "GC13";
                                        days229 = -1;
                                        break;
                                    case 55:
                                        itemcode642 = "DC33";
                                        days229 = 15;
                                        break;
                                    case 56:
                                        itemcode642 = "DC93";
                                        days229 = 15;
                                        break;
                                    case 57:
                                        itemcode642 = "DE07";
                                        days229 = 15;
                                        break;
                                    case 58:
                                        itemcode642 = "DC73";
                                        days229 = 15;
                                        break;
                                    case 59:
                                        itemcode642 = "DC78";
                                        days229 = 15;
                                        break;
                                    case 60:
                                        itemcode642 = "DC79";
                                        days229 = -1;
                                        break;
                                    case 61:
                                        itemcode642 = "DC80";
                                        days229 = 15;
                                        break;
                                    case 62:
                                        itemcode642 = "DC86";
                                        days229 = 15;
                                        break;
                                    case 63:
                                        itemcode642 = "DC98";
                                        days229 = 15;
                                        break;
                                    case 64:
                                        itemcode642 = "DE28";
                                        days229 = 15;
                                        break;
                                    case 65:
                                        itemcode642 = "DE30";
                                        days229 = 15;
                                        break;
                                    case 66:
                                        itemcode642 = "DE31";
                                        days229 = 15;
                                        break;
                                    case 67:
                                        itemcode642 = "DE33";
                                        days229 = 15;
                                        break;
                                    case 68:
                                        itemcode642 = "DE45";
                                        days229 = 15;
                                        break;
                                    case 69:
                                        itemcode642 = "DE46";
                                        days229 = 15;
                                        break;
                                    case 70:
                                        itemcode642 = "DE49";
                                        days229 = 15;
                                        break;
                                    case 71:
                                        itemcode642 = "DE49";
                                        days229 = 15;
                                        break;
                                    case 72:
                                        itemcode642 = "DE55";
                                        days229 = 15;
                                        break;
                                    case 73:
                                        itemcode642 = "DE55";
                                        days229 = 15;
                                        break;
                                    case 74:
                                        itemcode642 = "DE60";
                                        days229 = 15;
                                        break;
                                    case 75:
                                        itemcode642 = "DE64";
                                        days229 = 15;
                                        break;
                                    case 76:
                                        itemcode642 = "DE64";
                                        days229 = 15;
                                        break;
                                    case 77:
                                        itemcode642 = "DE66";
                                        days229 = 15;
                                        break;
                                    case 78:
                                        itemcode642 = "DE67";
                                        days229 = 15;
                                        break;
                                    case 79:
                                        itemcode642 = "DE69";
                                        days229 = 15;
                                        break;
                                    case 80:
                                        itemcode642 = "DE81";
                                        days229 = 15;
                                        break;
                                    case 81:
                                        itemcode642 = "DE94";
                                        days229 = 15;
                                        break;
                                    case 82:
                                        itemcode642 = "GC06";
                                        days229 = 15;
                                        break;
                                    case 83:
                                        itemcode642 = "GC08";
                                        days229 = 15;
                                        break;
                                    case 84:
                                        itemcode642 = "GC12";
                                        days229 = 15;
                                        break;
                                    case 85:
                                        itemcode642 = "GC13";
                                        days229 = 15;
                                        break;
                                    case 86:
                                        itemcode642 = "GC20";
                                        days229 = 30;
                                        break;
                                    case 87:
                                        itemcode642 = "GC20";
                                        days229 = 15;
                                        break;
                                    case 88:
                                        itemcode642 = "GC20";
                                        days229 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode642, days229);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode642, days229));
                                return;
                            case "CR74":
                                int num128 = Game_Server.Generic.random(0, 46);
                                int days221 = 1;
                                string itemcode641 = (string)null;
                                switch (num128)
                                {
                                    case 0:
                                        itemcode641 = "DG22";
                                        days221 = 30;
                                        break;
                                    case 1:
                                        itemcode641 = "DG22";
                                        days221 = -1;
                                        break;
                                    case 2:
                                        itemcode641 = "DG24";
                                        days221 = 30;
                                        break;
                                    case 3:
                                        itemcode641 = "DG24";
                                        days221 = -1;
                                        break;
                                    case 4:
                                        itemcode641 = "DG28";
                                        days221 = 30;
                                        break;
                                    case 5:
                                        itemcode641 = "DG28";
                                        days221 = -1;
                                        break;
                                    case 6:
                                        itemcode641 = "DG45";
                                        days221 = 30;
                                        break;
                                    case 7:
                                        itemcode641 = "DG45";
                                        days221 = -1;
                                        break;
                                    case 8:
                                        itemcode641 = "DG46";
                                        days221 = 30;
                                        break;
                                    case 9:
                                        itemcode641 = "DG46";
                                        days221 = -1;
                                        break;
                                    case 10:
                                        itemcode641 = "DG50";
                                        days221 = 30;
                                        break;
                                    case 11:
                                        itemcode641 = "DG50";
                                        days221 = -1;
                                        break;
                                    case 12:
                                        itemcode641 = "DG51";
                                        days221 = 30;
                                        break;
                                    case 13:
                                        itemcode641 = "DG51";
                                        days221 = -1;
                                        break;
                                    case 14:
                                        itemcode641 = "DG55";
                                        days221 = -1;
                                        break;
                                    case 15:
                                        itemcode641 = "DG55";
                                        days221 = 30;
                                        break;
                                    case 16:
                                        itemcode641 = "DG58";
                                        days221 = -1;
                                        break;
                                    case 17:
                                        itemcode641 = "DG58";
                                        days221 = 30;
                                        break;
                                    case 18:
                                        itemcode641 = "DG59";
                                        days221 = 30;
                                        break;
                                    case 19:
                                        itemcode641 = "DG59";
                                        days221 = -1;
                                        break;
                                    case 20:
                                        itemcode641 = "DG71";
                                        days221 = 30;
                                        break;
                                    case 21:
                                        itemcode641 = "DG71";
                                        days221 = -1;
                                        break;
                                    case 22:
                                        itemcode641 = "DG82";
                                        days221 = 30;
                                        break;
                                    case 23:
                                        itemcode641 = "DG82";
                                        days221 = -1;
                                        break;
                                    case 24:
                                        itemcode641 = "DG85";
                                        days221 = 30;
                                        break;
                                    case 25:
                                        itemcode641 = "DG85";
                                        days221 = -1;
                                        break;
                                    case 26:
                                        itemcode641 = "DG86";
                                        days221 = 30;
                                        break;
                                    case 27:
                                        itemcode641 = "DG86";
                                        days221 = -1;
                                        break;
                                    case 28:
                                        itemcode641 = "DG88";
                                        days221 = 30;
                                        break;
                                    case 29:
                                        itemcode641 = "DG91";
                                        days221 = -1;
                                        break;
                                    case 30:
                                        itemcode641 = "DG91";
                                        days221 = 30;
                                        break;
                                    case 31:
                                        itemcode641 = "DG95";
                                        days221 = -1;
                                        break;
                                    case 32:
                                        itemcode641 = "DG95";
                                        days221 = 30;
                                        break;
                                    case 33:
                                        itemcode641 = "DG97";
                                        days221 = -1;
                                        break;
                                    case 34:
                                        itemcode641 = "DG97";
                                        days221 = 30;
                                        break;
                                    case 35:
                                        itemcode641 = "GG08";
                                        days221 = -1;
                                        break;
                                    case 36:
                                        itemcode641 = "GG08";
                                        days221 = 30;
                                        break;
                                    case 37:
                                        itemcode641 = "GG10";
                                        days221 = -1;
                                        break;
                                    case 38:
                                        itemcode641 = "GG10";
                                        days221 = 30;
                                        break;
                                    case 39:
                                        itemcode641 = "GG17";
                                        days221 = 30;
                                        break;
                                    case 40:
                                        itemcode641 = "GG17";
                                        days221 = -1;
                                        break;
                                    case 41:
                                        itemcode641 = "GG27";
                                        days221 = 30;
                                        break;
                                    case 42:
                                        itemcode641 = "GG27";
                                        days221 = -1;
                                        break;
                                    case 43:
                                        itemcode641 = "GG28";
                                        days221 = 30;
                                        break;
                                    case 44:
                                        itemcode641 = "GG28";
                                        days221 = -1;
                                        break;
                                    case 45:
                                        itemcode641 = "GG32";
                                        days221 = 30;
                                        break;
                                    case 46:
                                        itemcode641 = "GG32";
                                        days221 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode641, days221);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode641, days221));
                                return;
                            case "CY63":
                                int num122 = Game_Server.Generic.random(0, 7);
                                int days392 = 1;
                                string itemcode216 = (string)null;
                                switch (num122)
                                {
                                    case 0:
                                        itemcode216 = "CA04";
                                        days392 = 15;
                                        break;
                                    case 1:
                                        itemcode216 = "CA04";
                                        days392 = 30;
                                        break;
                                    case 2:
                                        itemcode216 = "CA04";
                                        days392 = 45;
                                        break;
                                    case 3:
                                        itemcode216 = "CA04";
                                        days392 = 90;
                                        break;
                                    case 4:
                                        itemcode216 = "CA04";
                                        days392 = 180;
                                        break;
                                    case 5:
                                        itemcode216 = "CA04";
                                        days392 = 365;
                                        break;
                                    case 6:
                                        itemcode216 = "CA04";
                                        days392 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode216, days392);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode216, days392));
                                return;

                            case "CY43":
                                int num12 = Game_Server.Generic.random(0, 27);
                                int days402 = 1;
                                string itemcode212 = (string)null;
                                switch (num12)
                                {
                                    case 0:
                                        itemcode212 = "GF17";
                                        days392 = 365;
                                        break;
                                    case 1:
                                        itemcode212 = "GF17";
                                        days392 = 90;
                                        break;
                                    case 2:
                                        itemcode212 = "GF17";
                                        days392 = 60;
                                        break;
                                    case 3:
                                        itemcode212 = "GF17";
                                        days392 = 30;
                                        break;
                                    case 4:
                                        itemcode212 = "GF17";
                                        days392 = 15;
                                        break;
                                    case 5:
                                        itemcode212 = "GG05";
                                        days392 = 365;
                                        break;
                                    case 7:
                                        itemcode212 = "GG05";
                                        days392 = 90;
                                        break;
                                    case 8:
                                        itemcode212 = "GG05";
                                        days392 = 60;
                                        break;
                                    case 9:
                                        itemcode212 = "GG05";
                                        days392 = 30;
                                        break;
                                    case 10:
                                        itemcode212 = "GG05";
                                        days392 = 15;
                                        break;
                                    case 11:
                                        itemcode212 = "DH14";
                                        days392 = 365;
                                        break;
                                    case 12:
                                        itemcode212 = "DH14";
                                        days392 = 90;
                                        break;
                                    case 13:
                                        itemcode212 = "DH14";
                                        days392 = 60;
                                        break;
                                    case 14:
                                        itemcode212 = "DH14";
                                        days392 = 30;
                                        break;
                                    case 15:
                                        itemcode212 = "DH14";
                                        days392 = 15;
                                        break;
                                    case 16:
                                        itemcode212 = "DJ60";
                                        days392 = 365;
                                        break;
                                    case 17:
                                        itemcode212 = "DJ60";
                                        days392 = 90;
                                        break;
                                    case 18:
                                        itemcode212 = "DJ60";
                                        days392 = 60;
                                        break;
                                    case 19:
                                        itemcode212 = "DJ60";
                                        days392 = 30;
                                        break;
                                    case 20:
                                        itemcode212 = "DJ60";
                                        days392 = 15;
                                        break;
                                    case 21:
                                        itemcode212 = "CZ73";
                                        days392 = -1;
                                        break;
                                    case 22:
                                        itemcode212 = "DS01";
                                        days392 = 7;
                                        break;
                                    case 23:
                                        itemcode212 = "CI01";
                                        days392 = 7;
                                        break;
                                    case 24:
                                        itemcode212 = "DS10";
                                        days392 = 15;
                                        break;
                                    case 25:
                                        itemcode212 = "CF01";
                                        days392 = 15;
                                        break;
                                    case 26:
                                        itemcode212 = "CZ85";
                                        days392 = 15;
                                        break;
                                    case 27:
                                        itemcode212 = "CD02";
                                        days392 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode212, days402);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode212, days402));
                                return;
                            case "CR09":
                                int num112 = Game_Server.Generic.random(0, 5);
                                int days32 = 1;
                                string itemcode6 = (string)null;
                                switch (num112)
                                {
                                    case 0:
                                        itemcode6 = "DC80";
                                        days32 = 30;
                                        break;
                                    case 1:
                                        itemcode6 = "DC80";
                                        days32 = 7;
                                        break;
                                    case 2:
                                        itemcode6 = "DF95";
                                        days32 = 7;
                                        break;
                                    case 3:
                                        itemcode6 = "DF15";
                                        days32 = 7;
                                        break;
                                    case 4:
                                        itemcode6 = "DE35";
                                        days32 = 7;
                                        break;
                                    case 5:
                                        itemcode6 = "DF71";
                                        days32 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode6, days32);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode6, days32));
                                return;
                            case "CY39":
                                int num119 = Game_Server.Generic.random(0, 24);
                                int days323 = 1;
                                string itemcode661 = (string)null;
                                switch (num119)
                                {
                                    case 0:
                                        itemcode661 = "BF5A";
                                        days323 = 365;
                                        break;
                                    case 1:
                                        itemcode661 = "BF5A";
                                        days323 = 90;
                                        break;
                                    case 2:
                                        itemcode661 = "BF5A";
                                        days323 = 30;
                                        break;
                                    case 3:
                                        itemcode661 = "BF5B";
                                        days323 = 365;
                                        break;
                                    case 4:
                                        itemcode661 = "BF5B";
                                        days323 = 90;
                                        break;
                                    case 5:
                                        itemcode661 = "BF5B";
                                        days323 = 30;
                                        break;
                                    case 6:
                                        itemcode661 = "BF4A";
                                        days323 = 365;
                                        break;
                                    case 7:
                                        itemcode661 = "BF4A";
                                        days323 = 90;
                                        break;
                                    case 8:
                                        itemcode661 = "BF4A";
                                        days323 = 30;
                                        break;
                                    case 9:
                                        itemcode661 = "BF4B";
                                        days323 = 365;
                                        break;
                                    case 10:
                                        itemcode661 = "BF4B";
                                        days323 = 90;
                                        break;
                                    case 11:
                                        itemcode661 = "BF4B";
                                        days323 = 30;
                                        break;
                                    case 12:
                                        itemcode661 = "BF5E";
                                        days323 = 365;
                                        break;
                                    case 13:
                                        itemcode661 = "BF5E";
                                        days323 = 90;
                                        break;
                                    case 14:
                                        itemcode661 = "BF5E";
                                        days323 = 30;
                                        break;
                                    case 15:
                                        itemcode661 = "BF5F";
                                        days323 = 365;
                                        break;
                                    case 16:
                                        itemcode661 = "BF5F";
                                        days323 = 90;
                                        break;
                                    case 17:
                                        itemcode661 = "BF5F";
                                        days323 = 30;
                                        break;
                                    case 18:
                                        itemcode661 = "CZ81";
                                        days323 = 1;
                                        break;
                                    case 19:
                                        itemcode661 = "CB09";
                                        days323 = 1;
                                        break;
                                    case 20:
                                        itemcode661 = "CZ75";
                                        days323 = 1;
                                        break;
                                    case 21:
                                        itemcode661 = "CD01";
                                        days323 = 7;
                                        break;
                                    case 22:
                                        itemcode661 = "DS03";
                                        days323 = 7;
                                        break;
                                    case 23:
                                        itemcode661 = "DS01";
                                        days323 = 7;
                                        break;
                                    case 24:
                                        itemcode661 = "CF01";
                                        days323 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode661, days323);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode661, days323));
                                return;
                            case "CR77":
                                int num116 = Game_Server.Generic.random(0, 14);
                                int days362 = 1;
                                string itemcode156 = (string)null;
                                switch (num116)
                                {
                                    case 0:
                                        itemcode156 = "D806";
                                        days362 = -1;
                                        break;
                                    case 1:
                                        itemcode156 = "D806";
                                        days362 = 90;
                                        break;
                                    case 2:
                                        itemcode156 = "D806";
                                        days362 = 30;
                                        break;
                                    case 3:
                                        itemcode156 = "D604";
                                        days362 = -1;
                                        break;
                                    case 4:
                                        itemcode156 = "D604";
                                        days362 = 30;
                                        break;
                                    case 5:
                                        itemcode156 = "D604";
                                        days362 = 90;
                                        break;
                                    case 6:
                                        itemcode156 = "D909";
                                        days362 = -1;
                                        break;
                                    case 7:
                                        itemcode156 = "D909";
                                        days362 = 30;
                                        break;
                                    case 8:
                                        itemcode156 = "D909";
                                        days362 = 90;
                                        break;
                                    case 9:
                                        itemcode156 = "D829";
                                        days362 = -1;
                                        break;
                                    case 10:
                                        itemcode156 = "D829";
                                        days362 = 30;
                                        break;
                                    case 11:
                                        itemcode156 = "D829";
                                        days362 = 90;
                                        break;
                                    case 12:
                                        itemcode156 = "D830";
                                        days362 = -1;
                                        break;
                                    case 13:
                                        itemcode156 = "D830";
                                        days362 = 30;
                                        break;
                                    case 14:
                                        itemcode156 = "D830";
                                        days362 = 90;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode156, days362);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode156, days362));
                                return;


                            case "CY71":
                                int num117 = Game_Server.Generic.random(0, 39);
                                int days172 = 1;
                                string itemcode176 = (string)null;
                                switch (num117)
                                {
                                    case 0:
                                        itemcode176 = "CD01";
                                        days172 = 15;
                                        break;
                                    case 1:
                                        itemcode176 = "CD01";
                                        days172 = 30;
                                        break;
                                    case 2:
                                        itemcode176 = "CD01";
                                        days172 = 45;
                                        break;
                                    case 3:
                                        itemcode176 = "CD01";
                                        days172 = 60;
                                        break;
                                    case 4:
                                        itemcode176 = "CD02";
                                        days172 = 15;
                                        break;
                                    case 5:
                                        itemcode176 = "CD02";
                                        days172 = 30;
                                        break;
                                    case 6:
                                        itemcode176 = "CD02";
                                        days172 = 45;
                                        break;
                                    case 7:
                                        itemcode176 = "CD02";
                                        days172 = 60;
                                        break;
                                    case 8:
                                        itemcode176 = "CD03";
                                        days172 = 15;
                                        break;
                                    case 9:
                                        itemcode176 = "CD03";
                                        days172 = 30;
                                        break;
                                    case 10:
                                        itemcode176 = "CD03";
                                        days172 = 45;
                                        break;
                                    case 11:
                                        itemcode176 = "CD03";
                                        days172 = 60;
                                        break;
                                    case 12:
                                        itemcode176 = "CD04";
                                        days172 = 15;
                                        break;
                                    case 13:
                                        itemcode176 = "CD04";
                                        days172 = 30;
                                        break;
                                    case 14:
                                        itemcode176 = "CD04";
                                        days172 = 45;
                                        break;
                                    case 15:
                                        itemcode176 = "CD04";
                                        days172 = 60;
                                        break;
                                    case 16:
                                        itemcode176 = "CD05";
                                        days172 = 15;
                                        break;
                                    case 17:
                                        itemcode176 = "CD05";
                                        days172 = 30;
                                        break;
                                    case 18:
                                        itemcode176 = "CD05";
                                        days172 = 45;
                                        break;
                                    case 19:
                                        itemcode176 = "CD05";
                                        days172 = 60;
                                        break;
                                    case 20:
                                        itemcode176 = "CD06";
                                        days172 = 15;
                                        break;
                                    case 21:
                                        itemcode176 = "CD06";
                                        days172 = 30;
                                        break;
                                    case 22:
                                        itemcode176 = "CD06";
                                        days172 = 45;
                                        break;
                                    case 23:
                                        itemcode176 = "CD06";
                                        days172 = 60;
                                        break;
                                    case 24:
                                        itemcode176 = "CD07";
                                        days172 = 15;
                                        break;
                                    case 25:
                                        itemcode176 = "CD07";
                                        days172 = 30;
                                        break;
                                    case 26:
                                        itemcode176 = "CD07";
                                        days172 = 45;
                                        break;
                                    case 27:
                                        itemcode176 = "CD07";
                                        days172 = 60;
                                        break;
                                    case 28:
                                        itemcode176 = "CC05";
                                        days172 = 15;
                                        break;
                                    case 29:
                                        itemcode176 = "CC05";
                                        days172 = 30;
                                        break;
                                    case 30:
                                        itemcode176 = "CC05";
                                        days172 = 45;
                                        break;
                                    case 31:
                                        itemcode176 = "CC05";
                                        days172 = 60;
                                        break;
                                    case 32:
                                        itemcode176 = "CC72";
                                        days172 = 15;
                                        break;
                                    case 33:
                                        itemcode176 = "CC72";
                                        days172 = 30;
                                        break;
                                    case 34:
                                        itemcode176 = "CC72";
                                        days172 = 45;
                                        break;
                                    case 35:
                                        itemcode176 = "CC72";
                                        days172 = 60;
                                        break;
                                    case 36:
                                        itemcode176 = "CC76";
                                        days172 = 15;
                                        break;
                                    case 37:
                                        itemcode176 = "CC76";
                                        days172 = 30;
                                        break;
                                    case 38:
                                        itemcode176 = "CC76";
                                        days172 = 45;
                                        break;
                                    case 39:
                                        itemcode176 = "CC76";
                                        days172 = 60;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode176, days172);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode176, days172));
                                return;

                            case "CR49":
                                int num09 = Game_Server.Generic.random(0, 5);
                                int days226 = 1;
                                string itemcode07 = (string)null;
                                switch (num09)
                                {
                                    case 0:
                                        itemcode07 = "DF66";
                                        days226 = 30;
                                        break;
                                    case 1:
                                        itemcode07 = "DF66";
                                        days226 = 3;
                                        break;
                                    case 2:
                                        itemcode07 = "DC94";
                                        days226 = 7;
                                        break;
                                    case 3:
                                        itemcode07 = "DG61";
                                        days226 = 7;
                                        break;
                                    case 4:
                                        itemcode07 = "DF67";
                                        days226 = 7;
                                        break;
                                    case 5:
                                        itemcode07 = "DE09";
                                        days226 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode07, days226);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode07, days226));
                                return;
                            case "CR52":
                                int num92 = Game_Server.Generic.random(0, 5);
                                int days6 = 1;
                                string itemcode88 = (string)null;
                                switch (num92)
                                {
                                    case 0:
                                        itemcode88 = "DF53";
                                        days6 = 30;
                                        break;
                                    case 1:
                                        itemcode88 = "DF53";
                                        days6 = 7;
                                        break;
                                    case 2:
                                        itemcode88 = "DF47";
                                        days6 = 7;
                                        break;
                                    case 3:
                                        itemcode88 = "DF25";
                                        days6 = 7;
                                        break;
                                    case 4:
                                        itemcode88 = "DF95";
                                        days6 = 7;
                                        break;
                                    case 5:
                                        itemcode88 = "DF14";
                                        days6 = 7;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode88, days6);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode88, days6));
                                return;
                            case "CR50":
                                int num25 = Game_Server.Generic.random(0, 5);
                                int days160 = 1;
                                string itemcode132 = (string)null;
                                switch (num25)
                                {
                                    case 0:
                                        itemcode132 = "DE25";
                                        days160 = 30;
                                        break;
                                    case 1:
                                        itemcode132 = "CZ84";
                                        days160 = 1;
                                        break;
                                    case 2:
                                        itemcode132 = "CB09";
                                        days160 = 1;
                                        break;
                                    case 3:
                                        itemcode132 = "CZ81";
                                        days160 = 1;
                                        break;
                                    case 4:
                                        itemcode132 = "CZ84";
                                        days160 = 30;
                                        break;
                                    case 5:
                                        itemcode132 = "CF01";
                                        days160 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode132, days160);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode132, days160));
                                return;
                            case "CR51":
                                int num24 = Game_Server.Generic.random(0, 5);
                                int days159 = 1;
                                string itemcode131 = (string)null;
                                switch (num24)
                                {
                                    case 0:
                                        itemcode131 = "DF54";
                                        days159 = 30;
                                        break;
                                    case 1:
                                        itemcode131 = "CZ85";
                                        days159 = 1;
                                        break;
                                    case 2:
                                        itemcode131 = "CB09";
                                        days159 = 1;
                                        break;
                                    case 3:
                                        itemcode131 = "CZ81";
                                        days159 = 1;
                                        break;
                                    case 4:
                                        itemcode131 = "CZ84";
                                        days159 = 30;
                                        break;
                                    case 5:
                                        itemcode131 = "CF01";
                                        days159 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode131, days159);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode131, days159));
                                return;
                            case "CR12":
                                int num23 = Game_Server.Generic.random(0, 5);
                                int days157 = 1;
                                string itemcode129 = (string)null;
                                switch (num23)
                                {
                                    case 0:
                                        itemcode129 = "DF34";
                                        days157 = 30;
                                        break;
                                    case 1:
                                        itemcode129 = "CZ85";
                                        days157 = 1;
                                        break;
                                    case 2:
                                        itemcode129 = "CB09";
                                        days157 = 1;
                                        break;
                                    case 3:
                                        itemcode129 = "CZ81";
                                        days157 = 1;
                                        break;
                                    case 4:
                                        itemcode129 = "CZ84";
                                        days157 = 30;
                                        break;
                                    case 5:
                                        itemcode129 = "CF01";
                                        days157 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode129, days157);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode129, days157));
                                return;
                            case "CR13":
                                int num235 = Game_Server.Generic.random(0, 7);
                                int days161 = 1;
                                string itemcode169 = (string)null;
                                switch (num235)
                                {
                                    case 0:
                                        itemcode169 = "DC68";
                                        days161 = 30;
                                        break;
                                    case 1:
                                        itemcode169 = "DF17";
                                        days161 = 30;
                                        break;
                                    case 2:
                                        itemcode169 = "DG24";
                                        days161 = 30;
                                        break;
                                    case 3:
                                        itemcode169 = "DG23";
                                        days161 = 30;
                                        break;
                                    case 4:
                                        itemcode169 = "DC67";
                                        days161 = 30;
                                        break;
                                    case 5:
                                        itemcode169 = "DF68";
                                        days161 = 30;
                                        break;
                                    case 6:
                                        itemcode169 = "DJ69";
                                        days161 = 30;
                                        break;
                                    case 7:
                                        itemcode169 = "DC36";
                                        days161 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode169, days161);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode169, days161));
                                return;
                            case "CR11":
                                int num13 = Game_Server.Generic.random(0, 5);
                                int days7 = 1;
                                string itemcode9 = (string)null;
                                switch (num13)
                                {
                                    case 0:
                                        itemcode9 = "DG46";
                                        days7 = 30;
                                        break;
                                    case 1:
                                        itemcode9 = "DG46";
                                        days7 = 3;
                                        break;
                                    case 2:
                                        itemcode9 = "DG51";
                                        days7 = 7;
                                        break;
                                    case 3:
                                        itemcode9 = "DG58";
                                        days7 = 7;
                                        break;
                                    case 4:
                                        itemcode9 = "DG22";
                                        days7 = 7;
                                        break;
                                    case 5:
                                        itemcode9 = "DG45";
                                        days7 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode9, days7);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode9, days7));
                                return;
                            case "CR10":
                                int num14 = Game_Server.Generic.random(0, 5);
                                int days8 = 1;
                                string itemcode10 = (string)null;
                                switch (num14)
                                {
                                    case 0:
                                        itemcode10 = "DC77";
                                        days8 = 30;
                                        break;
                                    case 1:
                                        itemcode10 = "DC77";
                                        days8 = -1;
                                        break;
                                    case 2:
                                        itemcode10 = "DC81";
                                        days8 = 7;
                                        break;
                                    case 3:
                                        itemcode10 = "DC82";
                                        days8 = 7;
                                        break;
                                    case 4:
                                        itemcode10 = "DE25";
                                        days8 = 7;
                                        break;
                                    case 5:
                                        itemcode10 = "DE57";
                                        days8 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode10, days8);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode10, days8));
                                return;
                            case "CR62":
                                int num21 = Game_Server.Generic.random(0, 2);
                                int days118 = 1;
                                string itemcode50 = (string)null;
                                switch (num21)
                                {
                                    case 0:
                                        itemcode50 = "DF89";
                                        days118 = 30;
                                        break;
                                    case 1:
                                        itemcode50 = "DF89";
                                        days118 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode50, days118);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode50, days118));
                                return;
                            case "CY53":
                                int num221 = Game_Server.Generic.random(0, 129);
                                int days119 = 1;
                                string itemcode500 = (string)null;
                                switch (num221)
                                {
                                    case 0:
                                        itemcode500 = "D501";
                                        days119 = 30;
                                        break;
                                    case 1:
                                        itemcode500 = "D501";
                                        days119 = -1;
                                        break;
                                    case 2:
                                        itemcode500 = "D502";
                                        days119 = 30;
                                        break;
                                    case 3:
                                        itemcode500 = "D502";
                                        days119 = -1;
                                        break;
                                    case 4:
                                        itemcode500 = "D601";
                                        days119 = 30;
                                        break;
                                    case 5:
                                        itemcode500 = "D601";
                                        days119 = -1;
                                        break;
                                    case 6:
                                        itemcode500 = "D602";
                                        days119 = 30;
                                        break;
                                    case 7:
                                        itemcode500 = "D602";
                                        days119 = -1;
                                        break;
                                    case 8:
                                        itemcode500 = "D701";
                                        days119 = 30;
                                        break;
                                    case 9:
                                        itemcode500 = "D701";
                                        days119 = -1;
                                        break;
                                    case 10:
                                        itemcode500 = "D702";
                                        days119 = 30;
                                        break;
                                    case 11:
                                        itemcode500 = "D702";
                                        days119 = -1;
                                        break;
                                    case 12:
                                        itemcode500 = "D501";
                                        days119 = 30;
                                        break;
                                    case 13:
                                        itemcode500 = "D801";
                                        days119 = -1;
                                        break;
                                    case 14:
                                        itemcode500 = "D801";
                                        days119 = 30;
                                        break;
                                    case 15:
                                        itemcode500 = "D802";
                                        days119 = -1;
                                        break;
                                    case 16:
                                        itemcode500 = "D802";
                                        days119 = 30;
                                        break;
                                    case 17:
                                        itemcode500 = "D901";
                                        days119 = -1;
                                        break;
                                    case 18:
                                        itemcode500 = "D901";
                                        days119 = 30;
                                        break;
                                    case 19:
                                        itemcode500 = "D902";
                                        days119 = -1;
                                        break;
                                    case 20:
                                        itemcode500 = "D902";
                                        days119 = 30;
                                        break;
                                    case 21:
                                        itemcode500 = "D805";
                                        days119 = -1;
                                        break;
                                    case 22:
                                        itemcode500 = "D805";
                                        days119 = 30;
                                        break;
                                    case 23:
                                        itemcode500 = "D806";
                                        days119 = -1;
                                        break;
                                    case 24:
                                        itemcode500 = "D806";
                                        days119 = 30;
                                        break;
                                    case 25:
                                        itemcode500 = "D807";
                                        days119 = -1;
                                        break;
                                    case 26:
                                        itemcode500 = "D807";
                                        days119 = 30;
                                        break;
                                    case 27:
                                        itemcode500 = "D808";
                                        days119 = -1;
                                        break;
                                    case 28:
                                        itemcode500 = "D808";
                                        days119 = 30;
                                        break;
                                    case 29:
                                        itemcode500 = "D809";
                                        days119 = -1;
                                        break;
                                    case 30:
                                        itemcode500 = "D809";
                                        days119 = 30;
                                        break;
                                    case 31:
                                        itemcode500 = "D813";
                                        days119 = -1;
                                        break;
                                    case 32:
                                        itemcode500 = "D813";
                                        days119 = 30;
                                        break;
                                    case 33:
                                        itemcode500 = "D814";
                                        days119 = 30;
                                        break;
                                    case 34:
                                        itemcode500 = "D814";
                                        days119 = -1;
                                        break;
                                    case 35:
                                        itemcode500 = "D815";
                                        days119 = 30;
                                        break;
                                    case 36:
                                        itemcode500 = "D815";
                                        days119 = -1;
                                        break;
                                    case 37:
                                        itemcode500 = "D603";
                                        days119 = 30;
                                        break;
                                    case 38:
                                        itemcode500 = "D603";
                                        days119 = -1;
                                        break;
                                    case 39:
                                        itemcode500 = "D604";
                                        days119 = 30;
                                        break;
                                    case 40:
                                        itemcode500 = "D604";
                                        days119 = -1;
                                        break;
                                    case 41:
                                        itemcode500 = "D504";
                                        days119 = 30;
                                        break;
                                    case 42:
                                        itemcode500 = "D504";
                                        days119 = -1;
                                        break;
                                    case 43:
                                        itemcode500 = "D704";
                                        days119 = 30;
                                        break;
                                    case 44:
                                        itemcode500 = "D704";
                                        days119 = -1;
                                        break;
                                    case 45:
                                        itemcode500 = "D705";
                                        days119 = 30;
                                        break;
                                    case 46:
                                        itemcode500 = "D705";
                                        days119 = -1;
                                        break;
                                    case 47:
                                        itemcode500 = "D505";
                                        days119 = 30;
                                        break;
                                    case 48:
                                        itemcode500 = "D505";
                                        days119 = -1;
                                        break;
                                    case 49:
                                        itemcode500 = "D705";
                                        days119 = 30;
                                        break;
                                    case 50:
                                        itemcode500 = "D705";
                                        days119 = -1;
                                        break;
                                    case 51:
                                        itemcode500 = "D810";
                                        days119 = 30;
                                        break;
                                    case 52:
                                        itemcode500 = "D810";
                                        days119 = -1;
                                        break;
                                    case 53:
                                        itemcode500 = "D811";
                                        days119 = 30;
                                        break;
                                    case 54:
                                        itemcode500 = "D811";
                                        days119 = -1;
                                        break;
                                    case 55:
                                        itemcode500 = "D812";
                                        days119 = 30;
                                        break;
                                    case 56:
                                        itemcode500 = "D812";
                                        days119 = -1;
                                        break;
                                    case 57:
                                        itemcode500 = "D816";
                                        days119 = 30;
                                        break;
                                    case 58:
                                        itemcode500 = "D816";
                                        days119 = -1;
                                        break;
                                    case 59:
                                        itemcode500 = "D817";
                                        days119 = 30;
                                        break;
                                    case 60:
                                        itemcode500 = "D817";
                                        days119 = -1;
                                        break;
                                    case 61:
                                        itemcode500 = "D818";
                                        days119 = 30;
                                        break;
                                    case 62:
                                        itemcode500 = "D818";
                                        days119 = -1;
                                        break;
                                    case 63:
                                        itemcode500 = "D819";
                                        days119 = 30;
                                        break;
                                    case 64:
                                        itemcode500 = "D819";
                                        days119 = -1;
                                        break;
                                    case 65:
                                        itemcode500 = "D820";
                                        days119 = 30;
                                        break;
                                    case 66:
                                        itemcode500 = "D820";
                                        days119 = -1;
                                        break;
                                    case 67:
                                        itemcode500 = "D821";
                                        days119 = 30;
                                        break;
                                    case 68:
                                        itemcode500 = "D821";
                                        days119 = -1;
                                        break;
                                    case 69:
                                        itemcode500 = "D506";
                                        days119 = 30;
                                        break;
                                    case 70:
                                        itemcode500 = "D506";
                                        days119 = -1;
                                        break;
                                    case 71:
                                        itemcode500 = "D507";
                                        days119 = 30;
                                        break;
                                    case 72:
                                        itemcode500 = "D507";
                                        days119 = -1;
                                        break;
                                    case 73:
                                        itemcode500 = "D905";
                                        days119 = 30;
                                        break;
                                    case 74:
                                        itemcode500 = "D905";
                                        days119 = -1;
                                        break;
                                    case 75:
                                        itemcode500 = "D906";
                                        days119 = 30;
                                        break;
                                    case 76:
                                        itemcode500 = "D906";
                                        days119 = -1;
                                        break;
                                    case 77:
                                        itemcode500 = "D605";
                                        days119 = 30;
                                        break;
                                    case 78:
                                        itemcode500 = "D605";
                                        days119 = -1;
                                        break;
                                    case 79:
                                        itemcode500 = "D605";
                                        days119 = 30;
                                        break;
                                    case 80:
                                        itemcode500 = "D606";
                                        days119 = -1;
                                        break;
                                    case 81:
                                        itemcode500 = "D606";
                                        days119 = 30;
                                        break;
                                    case 82:
                                        itemcode500 = "D706";
                                        days119 = -1;
                                        break;
                                    case 83:
                                        itemcode500 = "D706";
                                        days119 = 30;
                                        break;
                                    case 84:
                                        itemcode500 = "D707";
                                        days119 = -1;
                                        break;
                                    case 85:
                                        itemcode500 = "D707";
                                        days119 = 30;
                                        break;
                                    case 86:
                                        itemcode500 = "D822";
                                        days119 = -1;
                                        break;
                                    case 87:
                                        itemcode500 = "D822";
                                        days119 = 30;
                                        break;
                                    case 88:
                                        itemcode500 = "D823";
                                        days119 = -1;
                                        break;
                                    case 89:
                                        itemcode500 = "D823";
                                        days119 = 30;
                                        break;
                                    case 90:
                                        itemcode500 = "D907";
                                        days119 = -1;
                                        break;
                                    case 91:
                                        itemcode500 = "D907";
                                        days119 = 30;
                                        break;
                                    case 92:
                                        itemcode500 = "DC92";
                                        days119 = -1;
                                        break;
                                    case 93:
                                        itemcode500 = "DC92";
                                        days119 = 30;
                                        break;
                                    case 94:
                                        itemcode500 = "D824";
                                        days119 = -1;
                                        break;
                                    case 95:
                                        itemcode500 = "D824";
                                        days119 = 30;
                                        break;
                                    case 96:
                                        itemcode500 = "D825";
                                        days119 = -1;
                                        break;
                                    case 97:
                                        itemcode500 = "D825";
                                        days119 = 30;
                                        break;
                                    case 98:
                                        itemcode500 = "D826";
                                        days119 = -1;
                                        break;
                                    case 99:
                                        itemcode500 = "D826";
                                        days119 = 30;
                                        break;
                                    case 100:
                                        itemcode500 = "D908";
                                        days119 = -1;
                                        break;
                                    case 101:
                                        itemcode500 = "D908";
                                        days119 = 30;
                                        break;
                                    case 102:
                                        itemcode500 = "D607";
                                        days119 = -1;
                                        break;
                                    case 103:
                                        itemcode500 = "D607";
                                        days119 = 30;
                                        break;
                                    case 104:
                                        itemcode500 = "D828";
                                        days119 = -1;
                                        break;
                                    case 105:
                                        itemcode500 = "D828";
                                        days119 = 30;
                                        break;
                                    case 106:
                                        itemcode500 = "D909";
                                        days119 = -1;
                                        break;
                                    case 107:
                                        itemcode500 = "D909";
                                        days119 = 30;
                                        break;
                                    case 108:
                                        itemcode500 = "D829";
                                        days119 = -1;
                                        break;
                                    case 109:
                                        itemcode500 = "D830";
                                        days119 = 30;
                                        break;
                                    case 110:
                                        itemcode500 = "D910";
                                        days119 = -1;
                                        break;
                                    case 111:
                                        itemcode500 = "D910";
                                        days119 = 30;
                                        break;
                                    case 112:
                                        itemcode500 = "D831";
                                        days119 = -1;
                                        break;
                                    case 113:
                                        itemcode500 = "D831";
                                        days119 = 30;
                                        break;
                                    case 114:
                                        itemcode500 = "D608";
                                        days119 = -1;
                                        break;
                                    case 115:
                                        itemcode500 = "D608";
                                        days119 = 30;
                                        break;
                                    case 116:
                                        itemcode500 = "D832";
                                        days119 = -1;
                                        break;
                                    case 117:
                                        itemcode500 = "D832";
                                        days119 = 30;
                                        break;
                                    case 118:
                                        itemcode500 = "D911";
                                        days119 = -1;
                                        break;
                                    case 119:
                                        itemcode500 = "D911";
                                        days119 = 30;
                                        break;
                                    case 120:
                                        itemcode500 = "D708";
                                        days119 = -1;
                                        break;
                                    case 121:
                                        itemcode500 = "D708";
                                        days119 = 30;
                                        break;
                                    case 122:
                                        itemcode500 = "D833";
                                        days119 = -1;
                                        break;
                                    case 123:
                                        itemcode500 = "D833";
                                        days119 = 30;
                                        break;
                                    case 124:
                                        itemcode500 = "D609";
                                        days119 = -1;
                                        break;
                                    case 125:
                                        itemcode500 = "D609";
                                        days119 = 30;
                                        break;
                                    case 126:
                                        itemcode500 = "D834";
                                        days119 = -1;
                                        break;
                                    case 127:
                                        itemcode500 = "D834";
                                        days119 = 30;
                                        break;
                                    case 128:
                                        itemcode500 = "D913";
                                        days119 = -1;
                                        break;
                                    case 129:
                                        itemcode500 = "D913";
                                        days119 = 30;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode500, days119);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode500, days119));
                                return;
                            case "CR65":
                                int num2221 = Game_Server.Generic.random(0, 50);
                                int days342 = 1;
                                string itemcode504 = (string)null;
                                switch (num2221)
                                {
                                    case 0:
                                        itemcode504 = "DC26";
                                        days342 = -1;
                                        break;
                                    case 1:
                                        itemcode504 = "D812";
                                        days342 = -1;
                                        break;
                                    case 2:
                                        itemcode504 = "DD10";
                                        days342 = -1;
                                        break;
                                    case 3:
                                        itemcode504 = "DE28";
                                        days342 = -1;
                                        break;
                                    case 4:
                                        itemcode504 = "DF58";
                                        days342 = -1;
                                        break;
                                    case 5:
                                        itemcode504 = "DF59";
                                        days342 = -1;
                                        break;
                                    case 6:
                                        itemcode504 = "DG54";
                                        days342 = -1;
                                        break;
                                    case 7:
                                        itemcode504 = "DJ25";
                                        days342 = -1;
                                        break;
                                    case 8:
                                        itemcode504 = "DF81";
                                        days342 = -1;
                                        break;
                                    case 9:
                                        itemcode504 = "DF91";
                                        days342 = -1;
                                        break;
                                    case 10:
                                        itemcode504 = "DE62";
                                        days342 = -1;
                                        break;
                                    case 11:
                                        itemcode504 = "DT15";
                                        days342 = -1;
                                        break;
                                    case 12:
                                        itemcode504 = "DB42";
                                        days342 = -1;
                                        break;
                                    case 13:
                                        itemcode504 = "GF17";
                                        days342 = -1;
                                        break;
                                    case 14:
                                        itemcode504 = "GG05";
                                        days342 = -1;
                                        break;
                                    case 15:
                                        itemcode504 = "DH14";
                                        days342 = -1;
                                        break;
                                    case 16:
                                        itemcode504 = "DJ60";
                                        days342 = -1;
                                        break;
                                    case 17:
                                        itemcode504 = "DC26";
                                        days342 = 90;
                                        break;
                                    case 18:
                                        itemcode504 = "D812";
                                        days342 = 90;
                                        break;
                                    case 19:
                                        itemcode504 = "DD10";
                                        days342 = 90;
                                        break;
                                    case 20:
                                        itemcode504 = "DE28";
                                        days342 = 90;
                                        break;
                                    case 21:
                                        itemcode504 = "DF58";
                                        days342 = 90;
                                        break;
                                    case 22:
                                        itemcode504 = "DF59";
                                        days342 = 90;
                                        break;
                                    case 23:
                                        itemcode504 = "DG54";
                                        days342 = 90;
                                        break;
                                    case 24:
                                        itemcode504 = "DJ25";
                                        days342 = 90;
                                        break;
                                    case 25:
                                        itemcode504 = "DF81";
                                        days342 = 90;
                                        break;
                                    case 26:
                                        itemcode504 = "DF91";
                                        days342 = 90;
                                        break;
                                    case 27:
                                        itemcode504 = "DE62";
                                        days342 = 90;
                                        break;
                                    case 28:
                                        itemcode504 = "DT15";
                                        days342 = 90;
                                        break;
                                    case 29:
                                        itemcode504 = "DB42";
                                        days342 = 90;
                                        break;
                                    case 30:
                                        itemcode504 = "GF17";
                                        days342 = 90;
                                        break;
                                    case 31:
                                        itemcode504 = "GG05";
                                        days342 = 90;
                                        break;
                                    case 32:
                                        itemcode504 = "DH14";
                                        days342 = 90;
                                        break;
                                    case 33:
                                        itemcode504 = "DJ60";
                                        days342 = 90;
                                        break;
                                    case 34:
                                        itemcode504 = "DC26";
                                        days342 = 30;
                                        break;
                                    case 35:
                                        itemcode504 = "D812";
                                        days342 = 30;
                                        break;
                                    case 36:
                                        itemcode504 = "DD10";
                                        days342 = 30;
                                        break;
                                    case 37:
                                        itemcode504 = "DE28";
                                        days342 = 30;
                                        break;
                                    case 38:
                                        itemcode504 = "DF58";
                                        days342 = 30;
                                        break;
                                    case 39:
                                        itemcode504 = "DF59";
                                        days342 = 30;
                                        break;
                                    case 40:
                                        itemcode504 = "DG54";
                                        days342 = 30;
                                        break;
                                    case 41:
                                        itemcode504 = "DJ25";
                                        days342 = 30;
                                        break;
                                    case 42:
                                        itemcode504 = "DF81";
                                        days342 = 30;
                                        break;
                                    case 43:
                                        itemcode504 = "DF91";
                                        days342 = 30;
                                        break;
                                    case 44:
                                        itemcode504 = "DE62";
                                        days342 = 30;
                                        break;
                                    case 45:
                                        itemcode504 = "DT15";
                                        days342 = 30;
                                        break;
                                    case 46:
                                        itemcode504 = "DB42";
                                        days342 = 30;
                                        break;
                                    case 47:
                                        itemcode504 = "GF17";
                                        days342 = 30;
                                        break;
                                    case 48:
                                        itemcode504 = "GG05";
                                        days342 = 30;
                                        break;
                                    case 49:
                                        itemcode504 = "DH14";
                                        days342 = 30;
                                        break;
                                    case 50:
                                        itemcode504 = "DJ60";
                                        days342 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode504, days342);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode504, days342));
                                return;

                            case "CY54":
                                int num22 = Game_Server.Generic.random(0, 47);
                                int days129 = 1;
                                string itemcode350 = (string)null;
                                switch (num22)
                                {
                                    case 0:
                                        itemcode350 = "DA10";
                                        days129 = 30;
                                        break;
                                    case 1:
                                        itemcode350 = "DA10";
                                        days129 = 15;
                                        break;
                                    case 2:
                                        itemcode350 = "DA10";
                                        days129 = -1;
                                        break;
                                    case 3:
                                        itemcode350 = "DA04";
                                        days129 = 30;
                                        break;
                                    case 4:
                                        itemcode350 = "DA04";
                                        days129 = -1;
                                        break;
                                    case 5:
                                        itemcode350 = "DA04";
                                        days129 = 15;
                                        break;
                                    case 6:
                                        itemcode350 = "DA40";
                                        days129 = 30;
                                        break;
                                    case 7:
                                        itemcode350 = "DA40";
                                        days129 = -1;
                                        break;
                                    case 8:
                                        itemcode350 = "DA40";
                                        days129 = 15;
                                        break;
                                    case 9:
                                        itemcode350 = "DA41";
                                        days129 = 30;
                                        break;
                                    case 10:
                                        itemcode350 = "DA41";
                                        days129 = -1;
                                        break;
                                    case 11:
                                        itemcode350 = "DA41";
                                        days129 = 15;
                                        break;
                                    case 12:
                                        itemcode350 = "DA45";
                                        days129 = 30;
                                        break;
                                    case 13:
                                        itemcode350 = "DA45";
                                        days129 = -1;
                                        break;
                                    case 14:
                                        itemcode350 = "DA45";
                                        days129 = 15;
                                        break;
                                    case 15:
                                        itemcode350 = "DA71";
                                        days129 = 30;
                                        break;
                                    case 16:
                                        itemcode350 = "DA71";
                                        days129 = -1;
                                        break;
                                    case 17:
                                        itemcode350 = "DA71";
                                        days129 = 15;
                                        break;
                                    case 18:
                                        itemcode350 = "DA72";
                                        days129 = 30;
                                        break;
                                    case 19:
                                        itemcode350 = "DA72";
                                        days129 = -1;
                                        break;
                                    case 20:
                                        itemcode350 = "DA72";
                                        days129 = 15;
                                        break;
                                    case 21:
                                        itemcode350 = "DA46";
                                        days129 = 30;
                                        break;
                                    case 22:
                                        itemcode350 = "DA46";
                                        days129 = -1;
                                        break;
                                    case 23:
                                        itemcode350 = "DA46";
                                        days129 = 15;
                                        break;
                                    case 24:
                                        itemcode350 = "DA75";
                                        days129 = 30;
                                        break;
                                    case 25:
                                        itemcode350 = "DA75";
                                        days129 = -1;
                                        break;
                                    case 26:
                                        itemcode350 = "DA75";
                                        days129 = 15;
                                        break;
                                    case 27:
                                        itemcode350 = "DA48";
                                        days129 = 30;
                                        break;
                                    case 28:
                                        itemcode350 = "DA48";
                                        days129 = -1;
                                        break;
                                    case 29:
                                        itemcode350 = "DA48";
                                        days129 = 30;
                                        break;
                                    case 30:
                                        itemcode350 = "DA84";
                                        days129 = 15;
                                        break;
                                    case 31:
                                        itemcode350 = "DA84";
                                        days129 = -1;
                                        break;
                                    case 32:
                                        itemcode350 = "DA84";
                                        days129 = 30;
                                        break;
                                    case 33:
                                        itemcode350 = "DA87";
                                        days129 = 15;
                                        break;
                                    case 34:
                                        itemcode350 = "DA87";
                                        days129 = -1;
                                        break;
                                    case 35:
                                        itemcode350 = "DA87";
                                        days129 = 30;
                                        break;
                                    case 36:
                                        itemcode350 = "DA90";
                                        days129 = 15;
                                        break;
                                    case 37:
                                        itemcode350 = "DA90";
                                        days129 = -1;
                                        break;
                                    case 38:
                                        itemcode350 = "DA90";
                                        days129 = 15;
                                        break;
                                    case 39:
                                        itemcode350 = "DA91";
                                        days129 = -1;
                                        break;
                                    case 40:
                                        itemcode350 = "DA91";
                                        days129 = 15;
                                        break;
                                    case 41:
                                        itemcode350 = "DA91";
                                        days129 = 30;
                                        break;
                                    case 42:
                                        itemcode350 = "DA92";
                                        days129 = -1;
                                        break;
                                    case 43:
                                        itemcode350 = "DA92";
                                        days129 = 15;
                                        break;
                                    case 44:
                                        itemcode350 = "DA92";
                                        days129 = 30;
                                        break;
                                    case 45:
                                        itemcode350 = "DA98";
                                        days129 = -1;
                                        break;
                                    case 46:
                                        itemcode350 = "DA98";
                                        days129 = 15;
                                        break;
                                    case 47:
                                        itemcode350 = "DA98";
                                        days129 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode350, days129);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode350, days129));
                                return;
                            case "CR26":
                                int num33 = Game_Server.Generic.random(0, 5);
                                int days39 = 1;
                                string itemcode311 = (string)null;
                                switch (num33)
                                {
                                    case 0:
                                        itemcode311 = "DF48";
                                        days39 = 30;
                                        break;
                                    case 1:
                                        itemcode311 = "DF48";
                                        days39 = 15;
                                        break;
                                    case 2:
                                        itemcode311 = "DF48";
                                        days39 = -1;
                                        break;
                                    case 3:
                                        itemcode311 = "CF01";
                                        days39 = 15;
                                        break;
                                    case 4:
                                        itemcode311 = "CZ81";
                                        days39 = 1;
                                        break;
                                    case 5:
                                        itemcode311 = "DS01";
                                        days39 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode311, days39);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode311, days39));
                                return;

                            case "CR15":
                                int num401 = Game_Server.Generic.random(0, 20);
                                int days13 = 1;
                                string itemcode413 = (string)null;
                                switch (num401)
                                {
                                    case 0:
                                        itemcode413 = "GF08";
                                        days13 = -1;
                                        break;
                                    case 1:
                                        itemcode413 = "GF08";
                                        days13 = 180;
                                        break;
                                    case 2:
                                        itemcode413 = "GF08";
                                        days13 = 90;
                                        break;
                                    case 3:
                                        itemcode413 = "GF08";
                                        days13 = 60;
                                        break;
                                    case 4:
                                        itemcode413 = "GF08";
                                        days13 = 45;
                                        break;
                                    case 5:
                                        itemcode413 = "GF08";
                                        days13 = 30;
                                        break;
                                    case 6:
                                        itemcode413 = "DG94";
                                        days13 = -1;
                                        break;
                                    case 7:
                                        itemcode413 = "DG94";
                                        days13 = 180;
                                        break;
                                    case 8:
                                        itemcode413 = "DG94";
                                        days13 = 90;
                                        break;
                                    case 9:
                                        itemcode413 = "DG94";
                                        days13 = 60;
                                        break;
                                    case 10:
                                        itemcode413 = "DG94";
                                        days13 = 30;
                                        break;
                                    case 11:
                                        itemcode413 = "DD21";
                                        days13 = -1;
                                        break;
                                    case 12:
                                        itemcode413 = "DD21";
                                        days13 = 180;
                                        break;
                                    case 13:
                                        itemcode413 = "DD21";
                                        days13 = 90;
                                        break;
                                    case 14:
                                        itemcode413 = "DD21";
                                        days13 = 60;
                                        break;
                                    case 15:
                                        itemcode413 = "DD21";
                                        days13 = 30;
                                        break;
                                    case 16:
                                        itemcode413 = "DT18";
                                        days13 = -1;
                                        break;
                                    case 17:
                                        itemcode413 = "DT18";
                                        days13 = 180;
                                        break;
                                    case 18:
                                        itemcode413 = "DT18";
                                        days13 = 90;
                                        break;
                                    case 19:
                                        itemcode413 = "DT18";
                                        days13 = 60;
                                        break;
                                    case 20:
                                        itemcode413 = "DT18";
                                        days13 = 30;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode413, days13);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode413, days13));
                                return;

                            case "CR28":
                                {
                                    int num40 = Game_Server.Generic.random(0, 21);
                                    int days811 = 1;
                                    string itemcode411 = (string)null;
                                    switch (num40)
                                    {
                                        case 0:
                                            itemcode411 = "DF33";
                                            days811 = 30;
                                            break;
                                        case 1:
                                            itemcode411 = "DF33";
                                            days811 = -1;
                                            break;

                                        case 2:
                                            itemcode411 = "DC98";
                                            days811 = 15;
                                            break;

                                        case 5:
                                            itemcode411 = "DF06";
                                            days811 = 15;
                                            break;
                                        case 6:
                                            itemcode411 = "CZ84";
                                            days811 = 1;
                                            break;
                                        case 7:
                                            itemcode411 = "CZ85";
                                            days811 = 15;
                                            break;
                                        case 8:
                                            itemcode411 = "DU01";
                                            days811 = 7;
                                            break;
                                        case 9:
                                            itemcode411 = "CZ81";
                                            days811 = 1;
                                            break;
                                        case 10:
                                            itemcode411 = "DS01";
                                            days811 = 7;
                                            break;
                                        case 11:
                                            itemcode411 = "DU01";
                                            days811 = 7;
                                            break;
                                        case 12:
                                            itemcode411 = "DS03";
                                            days811 = 7;
                                            break;
                                        case 13:
                                            itemcode411 = "DF35";
                                            days811 = 7;
                                            break;
                                        case 14:
                                            itemcode411 = "DF06";
                                            days811 = -1;
                                            break;
                                        case 15:
                                            itemcode411 = "DC01";
                                            days811 = 30;
                                            break;
                                        case 16:
                                            itemcode411 = "CZ73";
                                            days811 = 1;
                                            break;
                                        case 17:
                                            itemcode411 = "DB10";
                                            days811 = 30;
                                            break;
                                        case 18:
                                            itemcode411 = "DF07";
                                            days811 = 30;
                                            break;
                                        case 19:
                                            itemcode411 = "DC01";
                                            days811 = -1;
                                            break;
                                        case 20:
                                            itemcode411 = "CF01";
                                            days811 = 30;
                                            break;
                                        case 21:
                                            itemcode411 = "DF07";
                                            days811 = -1;
                                            break;
                                    }
                                    Inventory.AddItem(usr, itemcode411, days811);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, itemcode411, days811));
                                    return;
                                }
                            case "CZ59":
                                {
                                    int num250 = Game_Server.Generic.random(0, 5);
                                    int days921 = 1;
                                    string itemcode601 = (string)null;
                                    switch (num250)
                                    {
                                        case 0:
                                            itemcode601 = "DF40";
                                            days921 = -1;
                                            break;
                                        case 1:
                                            itemcode601 = "DF40";
                                            days921 = 15;
                                            break;

                                        case 2:
                                            itemcode601 = "DF40";
                                            days921 = 30;
                                            break;

                                        case 3:
                                            itemcode601 = "CF01";
                                            days921 = 15;
                                            break;
                                        case 4:
                                            itemcode601 = "CI01";
                                            days921 = 15;
                                            break;
                                        case 5:
                                            itemcode601 = "CA01";
                                            days921 = 15;
                                            break;
                                    }
                                    Inventory.AddItem(usr, itemcode601, days921);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, itemcode601, days921));
                                    return;
                                }

                            case "CR24":
                                int num311 = Game_Server.Generic.random(0, 3);
                                int days91 = 1;
                                string itemcode60 = (string)null;
                                switch (num311)
                                {
                                    case 0:
                                        itemcode60 = "DC61";
                                        days91 = 30;
                                        break;
                                    case 1:
                                        itemcode60 = "DC61";
                                        days91 = -1;
                                        break;

                                    case 2:
                                        itemcode60 = "DC34";
                                        days91 = 30;
                                        break;

                                    case 3:
                                        itemcode60 = "DC34";
                                        days91 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode60, days91);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode60, days91));
                                return;
                            case "CR27":
                                int num2 = Game_Server.Generic.random(0, 10);
                                int days81 = 1;
                                string itemcode41 = (string)null;
                                switch (num2)
                                {
                                    case 0:
                                        itemcode41 = "DC98";
                                        days81 = 30;
                                        break;
                                    case 1:
                                        itemcode41 = "DC98";
                                        days81 = -1;
                                        break;

                                    case 2:
                                        itemcode41 = "DC98";
                                        days81 = 15;
                                        break;
                                    case 3:
                                        itemcode41 = "CF01";
                                        days81 = 15;
                                        break;
                                    case 4:
                                        itemcode41 = "DG30";
                                        days81 = 30;
                                        break;
                                    case 5:
                                        itemcode41 = "DS01";
                                        days81 = 15;
                                        break;
                                    case 6:
                                        itemcode41 = "CA01";
                                        days81 = 15;
                                        break;
                                    case 7:
                                        itemcode41 = "DS03";
                                        days81 = 15;
                                        break;
                                    case 8:
                                        itemcode41 = "DC03";
                                        days81 = 30;
                                        break;
                                    case 9:
                                        itemcode41 = "DB10";
                                        days81 = 30;
                                        break;
                                    case 10:
                                        itemcode41 = "CD07";
                                        days81 = 15;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode41, days81);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode41, days81));
                                return;
                            case "CR25":
                                int num223 = Game_Server.Generic.random(0, 6);
                                int days88 = 1;
                                string itemcode61 = (string)null;
                                switch (num223)
                                {
                                    case 0:
                                        itemcode61 = "DC61";
                                        days88 = 30;
                                        break;
                                    case 1:
                                        itemcode61 = "DC61";
                                        days88 = -1;
                                        break;

                                    case 2:
                                        itemcode61 = "DC34";
                                        days88 = 30;
                                        break;

                                    case 3:
                                        itemcode61 = "DC34";
                                        days88 = -1;
                                        break;
                                    case 4:
                                        itemcode61 = "CZ84";
                                        days88 = -1;
                                        break;
                                    case 5:
                                        itemcode61 = "CZ85";
                                        days88 = -1;
                                        break;
                                    case 6:
                                        itemcode61 = "CZ86";
                                        days88 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode61, days88);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode61, days88));
                                return;
                            case "CR20":
                                int num34 = Game_Server.Generic.random(0, 5);
                                int days89 = 1;
                                string itemcode555 = (string)null;
                                switch (num34)
                                {
                                    case 0:
                                        itemcode555 = "DF50";
                                        days89 = -1;
                                        break;
                                    case 1:
                                        itemcode555 = "DF50";
                                        days89 = 30;
                                        break;

                                    case 2:
                                        itemcode555 = "DF50";
                                        days89 = 15;
                                        break;

                                    case 3:
                                        itemcode555 = "CF01";
                                        days89 = 15;
                                        break;

                                    case 4:
                                        itemcode555 = "CI01";
                                        days89 = 15;
                                        break;

                                    case 5:
                                        itemcode555 = "DS03";
                                        days89 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode555, days89);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode555, days89));
                                return;
                            case "CR66":
                                int num37 = Game_Server.Generic.random(0, 5);
                                int days101 = 1;
                                string itemcode545 = (string)null;
                                switch (num37)
                                {
                                    case 0:
                                        itemcode545 = "DF85";
                                        days101 = -1;
                                        break;
                                    case 1:
                                        itemcode545 = "DF85";
                                        days101 = 30;
                                        break;

                                    case 2:
                                        itemcode545 = "DF50";
                                        days101 = 15;
                                        break;

                                    case 3:
                                        itemcode545 = "CF01";
                                        days101 = 15;
                                        break;

                                    case 4:
                                        itemcode545 = "CI01";
                                        days101 = 15;
                                        break;

                                    case 5:
                                        itemcode545 = "CZ85";
                                        days101 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode545, days101);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode545, days101));
                                return;

                            case "CR67":
                                int num39 = Game_Server.Generic.random(0, 6);
                                int days100 = 1;
                                string itemcode502 = (string)null;
                                switch (num39)
                                {
                                    case 0:
                                        itemcode502 = "DF93";
                                        days100 = -1;
                                        break;
                                    case 1:
                                        itemcode502 = "DF93";
                                        days100 = 30;
                                        break;

                                    case 2:
                                        itemcode502 = "DF50";
                                        days100 = 15;
                                        break;

                                    case 3:
                                        itemcode502 = "CF01";
                                        days100 = 15;
                                        break;

                                    case 4:
                                        itemcode502 = "CI01";
                                        days100 = 15;
                                        break;

                                    case 5:
                                        itemcode502 = "CB09";
                                        days100 = 1;
                                        break;
                                    case 6:
                                        itemcode502 = "CZ84";
                                        days100 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode502, days100);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode502, days100));
                                return;
                            case "CY84":
                                int num4021 = Game_Server.Generic.random(0, 23);
                                int days1350 = 1;
                                string itemcode1350 = (string)null;
                                switch (num4021)
                                {
                                    case 0:
                                        itemcode1350 = "DB43";
                                        days1350 = -1;
                                        break;
                                    case 1:
                                        itemcode1350 = "DB43";
                                        days1350 = 30;
                                        break;

                                    case 2:
                                        itemcode1350 = "DF97";
                                        days1350 = 30;
                                        break;

                                    case 3:
                                        itemcode1350 = "DF97";
                                        days1350 = -1;
                                        break;

                                    case 4:
                                        itemcode1350 = "DG86";
                                        days1350 = 30;
                                        break;

                                    case 5:
                                        itemcode1350 = "DG86";
                                        days1350 = -1;
                                        break;
                                    case 6:
                                        itemcode1350 = "DE66";
                                        days1350 = 30;
                                        break;
                                    case 7:
                                        itemcode1350 = "DE66";
                                        days1350 = -1;
                                        break;
                                    case 8:
                                        itemcode1350 = "DJ43";
                                        days1350 = 30;
                                        break;
                                    case 9:
                                        itemcode1350 = "DJ43";
                                        days1350 = -1;
                                        break;
                                    case 10:
                                        itemcode1350 = "DE66";
                                        days1350 = 30;
                                        break;
                                    case 11:
                                        itemcode1350 = "DE66";
                                        days1350 = -1;
                                        break;
                                    case 12:
                                        itemcode1350 = "DD19";
                                        days1350 = 30;
                                        break;
                                    case 13:
                                        itemcode1350 = "DD19";
                                        days1350 = -1;
                                        break;
                                    case 14:
                                        itemcode1350 = "DB46";
                                        days1350 = 30;
                                        break;
                                    case 15:
                                        itemcode1350 = "DB46";
                                        days1350 = -1;
                                        break;
                                    case 16:
                                        itemcode1350 = "GF03";
                                        days1350 = 30;
                                        break;
                                    case 17:
                                        itemcode1350 = "GF03";
                                        days1350 = -1;
                                        break;
                                    case 18:
                                        itemcode1350 = "DE74";
                                        days1350 = 30;
                                        break;
                                    case 19:
                                        itemcode1350 = "DE74";
                                        days1350 = -1;
                                        break;
                                    case 20:
                                        itemcode1350 = "DJ45";
                                        days1350 = 30;
                                        break;
                                    case 21:
                                        itemcode1350 = "DJ45";
                                        days1350 = -1;
                                        break;
                                    case 22:
                                        itemcode1350 = "DG92";
                                        days1350 = 30;
                                        break;
                                    case 23:
                                        itemcode1350 = "DG92";
                                        days1350 = -1;
                                        break;
                                    case 24:
                                        itemcode1350 = "GA10";
                                        days1350 = 30;
                                        break;
                                    case 25:
                                        itemcode1350 = "GA10";
                                        days1350 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode1350, days1350);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode1350, days1350));
                                return;
                            case "CY87":
                                int num93 = Game_Server.Generic.random(0, 41);
                                int days309 = 1;
                                string itemcode115 = (string)null;
                                switch (num93)
                                {
                                    case 0:
                                        itemcode115 = "DB69";
                                        days309 = 7;
                                        break;
                                    case 1:
                                        itemcode115 = "DB69";
                                        days309 = 15;
                                        break;
                                    case 2:
                                        itemcode115 = "DB69";
                                        days309 = 30;
                                        break;
                                    case 3:
                                        itemcode115 = "DB69";
                                        days309 = 45;
                                        break;
                                    case 4:
                                        itemcode115 = "DB69";
                                        days309 = 90;
                                        break;
                                    case 5:
                                        itemcode115 = "DB69";
                                        days309 = -1;
                                        break;
                                    case 6:
                                        itemcode115 = "GF41";
                                        days309 = 7;
                                        break;
                                    case 7:
                                        itemcode115 = "GF41";
                                        days309 = 15;
                                        break;
                                    case 8:
                                        itemcode115 = "GF41";
                                        days309 = 30;
                                        break;
                                    case 9:
                                        itemcode115 = "GF41";
                                        days309 = 45;
                                        break;
                                    case 10:
                                        itemcode115 = "GF41";
                                        days309 = 90;
                                        break;
                                    case 11:
                                        itemcode115 = "GF41";
                                        days309 = -1;
                                        break;
                                    case 12:
                                        itemcode115 = "GC12";
                                        days309 = 7;
                                        break;
                                    case 13:
                                        itemcode115 = "GC12";
                                        days309 = 15;
                                        break;
                                    case 14:
                                        itemcode115 = "GC12";
                                        days309 = 30;
                                        break;
                                    case 15:
                                        itemcode115 = "GC12";
                                        days309 = 45;
                                        break;
                                    case 16:
                                        itemcode115 = "GC12";
                                        days309 = 90;
                                        break;
                                    case 17:
                                        itemcode115 = "GC12";
                                        days309 = -1;
                                        break;
                                    case 18:
                                        itemcode115 = "DC51";
                                        days309 = 7;
                                        break;
                                    case 19:
                                        itemcode115 = "DC51";
                                        days309 = 15;
                                        break;
                                    case 20:
                                        itemcode115 = "DC51";
                                        days309 = 30;
                                        break;
                                    case 21:
                                        itemcode115 = "DC51";
                                        days309 = 45;
                                        break;
                                    case 22:
                                        itemcode115 = "DC51";
                                        days309 = 90;
                                        break;
                                    case 23:
                                        itemcode115 = "DC51";
                                        days309 = -1;
                                        break;
                                    case 24:
                                        itemcode115 = "D811";
                                        days309 = 7;
                                        break;
                                    case 25:
                                        itemcode115 = "D811";
                                        days309 = 15;
                                        break;
                                    case 26:
                                        itemcode115 = "D811";
                                        days309 = 30;
                                        break;
                                    case 27:
                                        itemcode115 = "D811";
                                        days309 = 45;
                                        break;
                                    case 28:
                                        itemcode115 = "D811";
                                        days309 = 90;
                                        break;
                                    case 29:
                                        itemcode115 = "D811";
                                        days309 = -1;
                                        break;
                                    case 30:
                                        itemcode115 = "DE17";
                                        days309 = 7;
                                        break;
                                    case 31:
                                        itemcode115 = "DE17";
                                        days309 = 15;
                                        break;
                                    case 32:
                                        itemcode115 = "DE17";
                                        days309 = 30;
                                        break;
                                    case 33:
                                        itemcode115 = "DE17";
                                        days309 = 45;
                                        break;
                                    case 34:
                                        itemcode115 = "DE17";
                                        days309 = 90;
                                        break;
                                    case 35:
                                        itemcode115 = "DE17";
                                        days309 = -1;
                                        break;
                                    case 36:
                                        itemcode115 = "DG72";
                                        days309 = 7;
                                        break;
                                    case 37:
                                        itemcode115 = "DG72";
                                        days309 = 15;
                                        break;
                                    case 38:
                                        itemcode115 = "DG72";
                                        days309 = 30;
                                        break;
                                    case 39:
                                        itemcode115 = "DG72";
                                        days309 = 45;
                                        break;
                                    case 40:
                                        itemcode115 = "DG72";
                                        days309 = 90;
                                        break;
                                    case 41:
                                        itemcode115 = "DG72";
                                        days309 = -1;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode115, days309);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode115, days309));
                                return;
                            case "CR78":
                                int num162 = Game_Server.Generic.random(0, 8);
                                int days908 = 1;
                                string itemcode509 = (string)null;
                                switch (num162)
                                {
                                    case 0:
                                        itemcode509 = "GF04";
                                        days908 = 30;
                                        break;
                                    case 1:
                                        itemcode509 = "DB45";
                                        days908 = 30;
                                        break;

                                    case 2:
                                        itemcode509 = "DC01";
                                        days908 = -1;
                                        break;

                                    case 3:
                                        itemcode509 = "DB03";
                                        days908 = 30;
                                        break;

                                    case 4:
                                        itemcode509 = "CF02";
                                        days908 = 15;
                                        break;

                                    case 5:
                                        itemcode509 = "CB09";
                                        days908 = 1;
                                        break;
                                    case 6:
                                        itemcode509 = "CZ81";
                                        days908 = 90;
                                        break;
                                    case 7:
                                        itemcode509 = "DF06";
                                        days908 = 30;
                                        break;
                                    case 8:
                                        itemcode509 = "GF04";
                                        days908 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode509, days908);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode509, days908));
                                return;
                            case "CR86":
                                int num1620 = Game_Server.Generic.random(0, 8);
                                int days9080 = 1;
                                string itemcode5090 = (string)null;
                                switch (num1620)
                                {
                                    case 0:
                                        itemcode5090 = "DT17";
                                        days9080 = 30;
                                        break;
                                    case 1:
                                        itemcode5090 = "DB45";
                                        days9080 = 30;
                                        break;

                                    case 2:
                                        itemcode5090 = "DC01";
                                        days9080 = -1;
                                        break;

                                    case 3:
                                        itemcode5090 = "DB03";
                                        days9080 = 30;
                                        break;

                                    case 4:
                                        itemcode5090 = "CF02";
                                        days9080 = 15;
                                        break;

                                    case 5:
                                        itemcode5090 = "CB09";
                                        days9080 = 1;
                                        break;
                                    case 6:
                                        itemcode5090 = "CZ81";
                                        days9080 = 90;
                                        break;
                                    case 7:
                                        itemcode5090 = "DF06";
                                        days9080 = 30;
                                        break;
                                    case 8:
                                        itemcode5090 = "DT17";
                                        days9080 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode5090, days9080);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode5090, days9080));
                                return;
                            case "CY90":
                                int num1621 = Game_Server.Generic.random(0, 23);
                                int days911 = 1;
                                string itemcode511 = (string)null;
                                switch (num1621)
                                {
                                    case 0:
                                        itemcode511 = "DA48";
                                        days911 = 45;
                                        break;
                                    case 1:
                                        itemcode511 = "DA48";
                                        days911 = 90;
                                        break;

                                    case 2:
                                        itemcode511 = "DA48";
                                        days911 = 180;
                                        break;

                                    case 3:
                                        itemcode511 = "DA48";
                                        days911 = -1;
                                        break;
                                    case 4:
                                        itemcode511 = "DB44";
                                        days911 = 45;
                                        break;
                                    case 5:
                                        itemcode511 = "DB44";
                                        days911 = 90;
                                        break;

                                    case 6:
                                        itemcode511 = "DB44";
                                        days911 = 180;
                                        break;

                                    case 7:
                                        itemcode511 = "DB44";
                                        days911 = -1;
                                        break;
                                    case 8:
                                        itemcode511 = "GF01";
                                        days911 = 45;
                                        break;
                                    case 9:
                                        itemcode511 = "GF01";
                                        days911 = 90;
                                        break;

                                    case 10:
                                        itemcode511 = "GF01";
                                        days911 = 180;
                                        break;

                                    case 11:
                                        itemcode511 = "GF01";
                                        days911 = -1;
                                        break;
                                    case 12:
                                        itemcode511 = "DG89";
                                        days911 = 45;
                                        break;
                                    case 13:
                                        itemcode511 = "DG89";
                                        days911 = 90;
                                        break;

                                    case 14:
                                        itemcode511 = "DG89";
                                        days911 = 180;
                                        break;

                                    case 15:
                                        itemcode511 = "DG89";
                                        days911 = -1;
                                        break;
                                    case 16:
                                        itemcode511 = "DE71";
                                        days911 = 45;
                                        break;
                                    case 17:
                                        itemcode511 = "DE71";
                                        days911 = 90;
                                        break;

                                    case 18:
                                        itemcode511 = "DE71";
                                        days911 = 180;
                                        break;

                                    case 19:
                                        itemcode511 = "DE71";
                                        days911 = -1;
                                        break;
                                    case 20:
                                        itemcode511 = "DH11";
                                        days911 = 45;
                                        break;
                                    case 21:
                                        itemcode511 = "DH11";
                                        days911 = 90;
                                        break;

                                    case 22:
                                        itemcode511 = "DH11";
                                        days911 = 180;
                                        break;

                                    case 23:
                                        itemcode511 = "DH11";
                                        days911 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode511, days911);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode511, days911));
                                return;
                            case "CY29":
                                int num159 = Game_Server.Generic.random(0, 43);
                                int days906 = 1;
                                string itemcode501 = (string)null;
                                switch (num159)
                                {

                                    case 0:
                                        itemcode501 = "DE78";
                                        days906 = 45;
                                        break;
                                    case 1:
                                        itemcode501 = "DE78";
                                        days906 = 60;
                                        break;

                                    case 2:
                                        itemcode501 = "DE78";
                                        days906 = 90;
                                        break;

                                    case 3:
                                        itemcode501 = "DS03";
                                        days906 = 30;
                                        break;

                                    case 4:
                                        itemcode501 = "CF02";
                                        days906 = 15;
                                        break;

                                    case 5:
                                        itemcode501 = "CB09";
                                        days906 = 1;
                                        break;
                                    case 6:
                                        itemcode501 = "CZ81";
                                        days906 = 90;
                                        break;
                                    case 7:
                                        itemcode501 = "CF01";
                                        days906 = 30;
                                        break;
                                    case 8:
                                        itemcode501 = "DE78";
                                        days906 = -1;
                                        break;
                                    case 9:
                                        itemcode501 = "DE87";
                                        days906 = -1;
                                        break;
                                    case 10:
                                        itemcode501 = "GC01";
                                        days906 = -1;
                                        break;
                                    case 11:
                                        itemcode501 = "GC07";
                                        days906 = -1;
                                        break;
                                    case 12:
                                        itemcode501 = "GC17";
                                        days906 = -1;
                                        break;
                                    case 13:
                                        itemcode501 = "GE09";
                                        days906 = -1;
                                        break;
                                    case 14:
                                        itemcode501 = "D838";
                                        days906 = -1;
                                        break;
                                    case 15:
                                        itemcode501 = "GE16";
                                        days906 = -1;
                                        break;
                                    case 16:
                                        itemcode501 = "DE87";
                                        days906 = 45;
                                        break;
                                    case 17:
                                        itemcode501 = "DE87";
                                        days906 = 60;
                                        break;

                                    case 18:
                                        itemcode501 = "DE87";
                                        days906 = 90;
                                        break;

                                    case 19:
                                        itemcode501 = "DE87";
                                        days906 = 30;
                                        break;
                                    case 20:
                                        itemcode501 = "GC01";
                                        days906 = 45;
                                        break;
                                    case 21:
                                        itemcode501 = "GC01";
                                        days906 = 60;
                                        break;

                                    case 22:
                                        itemcode501 = "GC01";
                                        days906 = 90;
                                        break;

                                    case 23:
                                        itemcode501 = "GC01";
                                        days906 = 30;
                                        break;
                                    case 24:
                                        itemcode501 = "GC07";
                                        days906 = 45;
                                        break;
                                    case 25:
                                        itemcode501 = "GC07";
                                        days906 = 60;
                                        break;

                                    case 26:
                                        itemcode501 = "GC07";
                                        days906 = 90;
                                        break;

                                    case 27:
                                        itemcode501 = "GC07";
                                        days906 = 30;
                                        break;
                                    case 28:
                                        itemcode501 = "GC17";
                                        days906 = 45;
                                        break;
                                    case 29:
                                        itemcode501 = "GC17";
                                        days906 = 60;
                                        break;

                                    case 30:
                                        itemcode501 = "GC17";
                                        days906 = 90;
                                        break;

                                    case 31:
                                        itemcode501 = "GC17";
                                        days906 = 30;
                                        break;
                                    case 32:
                                        itemcode501 = "GE09";
                                        days906 = 45;
                                        break;
                                    case 33:
                                        itemcode501 = "GE09";
                                        days906 = 60;
                                        break;

                                    case 34:
                                        itemcode501 = "GE09";
                                        days906 = 90;
                                        break;

                                    case 35:
                                        itemcode501 = "GE09";
                                        days906 = 30;
                                        break;
                                    case 36:
                                        itemcode501 = "D838";
                                        days906 = 45;
                                        break;
                                    case 37:
                                        itemcode501 = "D838";
                                        days906 = 60;
                                        break;

                                    case 38:
                                        itemcode501 = "D838";
                                        days906 = 90;
                                        break;

                                    case 39:
                                        itemcode501 = "D838";
                                        days906 = 30;
                                        break;
                                    case 40:
                                        itemcode501 = "GE16";
                                        days906 = 45;
                                        break;
                                    case 41:
                                        itemcode501 = "GE16";
                                        days906 = 60;
                                        break;

                                    case 42:
                                        itemcode501 = "GE16";
                                        days906 = 90;
                                        break;

                                    case 43:
                                        itemcode501 = "GE16";
                                        days906 = 30;
                                        break;
                                    


                                }
                                Inventory.AddItem(usr, itemcode501, days906);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode501, days906));
                                return;
                            case "CR21":
                                int num144 = Game_Server.Generic.random(0, 5);
                                int days42 = 1;
                                string itemcode612 = (string)null;
                                switch (num144)
                                {
                                    case 0:
                                        itemcode612 = "DC85";
                                        days42 = -1;
                                        break;
                                    case 1:
                                        itemcode612 = "DC85";
                                        days42 = 30;
                                        break;

                                    case 2:
                                        itemcode612 = "DC85";
                                        days42 = 15;
                                        break;

                                    case 3:
                                        itemcode612 = "CF01";
                                        days42 = 15;
                                        break;

                                    case 4:
                                        itemcode612 = "CI01";
                                        days42 = 15;
                                        break;

                                    case 5:
                                        itemcode612 = "DS03";
                                        days42 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode612, days42);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode612, days42));
                                return;
                            case "CR22":
                                int num164 = Game_Server.Generic.random(0, 5);
                                int days511 = 1;
                                string itemcode294 = (string)null;
                                switch (num164)
                                {
                                    case 0:
                                        itemcode294 = "DJ22";
                                        days511 = -1;
                                        break;
                                    case 1:
                                        itemcode294 = "DJ22";
                                        days511 = 30;
                                        break;

                                    case 2:
                                        itemcode294 = "DJ22";
                                        days511 = 15;
                                        break;

                                    case 3:
                                        itemcode294 = "CF01";
                                        days511 = 15;
                                        break;

                                    case 4:
                                        itemcode294 = "CI01";
                                        days511 = 15;
                                        break;

                                    case 5:
                                        itemcode294 = "DS03";
                                        days511 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode294, days511);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode294, days511));
                                return;
                            case "CR23":
                                int num134 = Game_Server.Generic.random(0, 5);
                                int days380 = 1;
                                string itemcode82 = (string)null;
                                switch (num134)
                                {
                                    case 0:
                                        itemcode82 = "DG48";
                                        days380 = -1;
                                        break;
                                    case 1:
                                        itemcode82 = "DG48";
                                        days380 = 30;
                                        break;

                                    case 2:
                                        itemcode82 = "DG48";
                                        days380 = 15;
                                        break;

                                    case 3:
                                        itemcode82 = "CF01";
                                        days380 = 15;
                                        break;

                                    case 4:
                                        itemcode82 = "CI01";
                                        days380 = 15;
                                        break;

                                    case 5:
                                        itemcode82 = "DS03";
                                        days380 = 15;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode82, days380);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode82, days380));
                                return;
                            case "CR53":
                                int num96 = Game_Server.Generic.random(0, 13);
                                int days312 = 1;
                                string itemcode117 = (string)null;
                                switch (num96)
                                {
                                    case 0:
                                        itemcode117 = "DJ25";
                                        days312 = 90;
                                        break;
                                    case 1:
                                        itemcode117 = "DJ25";
                                        days312 = -1;
                                        break;
                                    case 2:
                                        itemcode117 = "DJ31";
                                        days312 = -1;
                                        break;
                                    case 3:
                                        itemcode117 = "DJ31";
                                        days312 = 90;
                                        break;
                                    case 4:
                                        itemcode117 = "DJ32";
                                        days312 = -1;
                                        break;
                                    case 5:
                                        itemcode117 = "DJ32";
                                        days312 = 90;
                                        break;
                                    case 6:
                                        itemcode117 = "DJ32";
                                        days312 = -1;
                                        break;
                                    case 7:
                                        itemcode117 = "DG54";
                                        days312 = 90;
                                        break;
                                    case 8:
                                        itemcode117 = "DG54";
                                        days312 = -1;
                                        break;
                                    case 9:
                                        itemcode117 = "DG55";
                                        days312 = 90;
                                        break;
                                    case 10:
                                        itemcode117 = "DG55";
                                        days312 = -1;
                                        break;
                                    case 11:
                                        itemcode117 = "DG59";
                                        days312 = 90;
                                        break;
                                    case 12:
                                        itemcode117 = "DG59";
                                        days312 = -1;
                                        break;
                                    case 13:
                                        itemcode117 = "DG71";
                                        days312 = 90;
                                        break;
                                    case 14:
                                        itemcode117 = "DG71";
                                        days312 = -1;
                                        break;
                                    case 15:
                                        itemcode117 = "DG72";
                                        days312 = 90;
                                        break;
                                    case 16:
                                        itemcode117 = "DG72";
                                        days312 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode117, days312);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode117, days312));
                                return;

                            case "CY66":
                                int num81 = Game_Server.Generic.random(0, 13);
                                int days316 = 1;
                                string itemcode219 = (string)null;
                                switch (num81)
                                {
                                    case 0:
                                        itemcode219 = "DC22";
                                        days316 = 30;
                                        break;
                                    case 1:
                                        itemcode219 = "DC22";
                                        days316 = -1;
                                        break;
                                    case 2:
                                        itemcode219 = "D807";
                                        days316 = -1;
                                        break;
                                    case 3:
                                        itemcode219 = "D807";
                                        days316 = 30;
                                        break;
                                    case 4:
                                        itemcode219 = "DC87";
                                        days316 = -1;
                                        break;
                                    case 5:
                                        itemcode219 = "DC87";
                                        days316 = 30;
                                        break;
                                    case 6:
                                        itemcode219 = "DD09";
                                        days316 = -1;
                                        break;
                                    case 7:
                                        itemcode219 = "DD09";
                                        days316 = 30;
                                        break;
                                    case 8:
                                        itemcode219 = "DF75";
                                        days316 = -1;
                                        break;
                                    case 9:
                                        itemcode219 = "DF75";
                                        days316 = 30;
                                        break;
                                    case 10:
                                        itemcode219 = "DE49";
                                        days316 = -1;
                                        break;
                                    case 11:
                                        itemcode219 = "DE49";
                                        days316 = 30;
                                        break;
                                    case 12:
                                        itemcode219 = "DG46";
                                        days316 = -1;
                                        break;
                                    case 13:
                                        itemcode219 = "DG46";
                                        days316 = 30;
                                        break;
                                    case 14:
                                        itemcode219 = "DT13";
                                        days316 = -1;
                                        break;
                                    case 15:
                                        itemcode219 = "DT13";
                                        days316 = 30;
                                        break;
                                    case 16:
                                        itemcode219 = "D827";
                                        days316 = -1;
                                        break;
                                    case 17:
                                        itemcode219 = "D827";
                                        days316 = 30;
                                        break;
                                    case 18:
                                        itemcode219 = "DF89";
                                        days316 = -1;
                                        break;
                                    case 19:
                                        itemcode219 = "DF89";
                                        days316 = 30;
                                        break;
                                    case 20:
                                        itemcode219 = "GF08";
                                        days316 = -1;
                                        break;
                                    case 21:
                                        itemcode219 = "GF08";
                                        days316 = 30;
                                        break;
                                    case 22:
                                        itemcode219 = "DG94";
                                        days316 = -1;
                                        break;
                                    case 23:
                                        itemcode219 = "DG94";
                                        days316 = 30;
                                        break;
                                    case 24:
                                        itemcode219 = "DD21";
                                        days316 = -1;
                                        break;
                                    case 25:
                                        itemcode219 = "DD21";
                                        days316 = 30;
                                        break;
                                    case 26:
                                        itemcode219 = "DT18";
                                        days316 = -1;
                                        break;
                                    case 27:
                                        itemcode219 = "DT18";
                                        days316 = 30;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode219, days316);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode219, days316));
                                return;
                            case "CY68":
                                int num83 = Game_Server.Generic.random(0, 15);
                                int days318 = 1;
                                string itemcode221 = (string)null;
                                switch (num83)
                                {
                                    case 0:
                                        itemcode221 = "DC24";
                                        days318 = 30;
                                        break;
                                    case 1:
                                        itemcode221 = "DC24";
                                        days318 = -1;
                                        break;
                                    case 2:
                                        itemcode221 = "D808";
                                        days318 = -1;
                                        break;
                                    case 3:
                                        itemcode221 = "D808";
                                        days318 = 30;
                                        break;
                                    case 4:
                                        itemcode221 = "DC89";
                                        days318 = -1;
                                        break;
                                    case 5:
                                        itemcode221 = "DC89";
                                        days318 = 30;
                                        break;
                                    case 6:
                                        itemcode221 = "DE23";
                                        days318 = -1;
                                        break;
                                    case 7:
                                        itemcode221 = "DE23";
                                        days318 = 30;
                                        break;
                                    case 8:
                                        itemcode221 = "DD11";
                                        days318 = -1;
                                        break;
                                    case 9:
                                        itemcode221 = "DD11";
                                        days318 = 30;
                                        break;
                                    case 10:
                                        itemcode221 = "DF80";
                                        days318 = -1;
                                        break;
                                    case 11:
                                        itemcode221 = "DF80";
                                        days318 = 30;
                                        break;
                                    case 12:
                                        itemcode221 = "DG71";
                                        days318 = -1;
                                        break;
                                    case 13:
                                        itemcode221 = "DG71";
                                        days318 = 30;
                                        break;
                                    case 14:
                                        itemcode221 = "DJ31";
                                        days318 = -1;
                                        break;
                                    case 15:
                                        itemcode221 = "DJ31";
                                        days318 = 30;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode221, days318);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode221, days318));
                                return;
                            case "CY62":
                                int num5118 = Game_Server.Generic.random(0, 11);
                                int days59 = 1;
                                string itemcode66 = (string)null;
                                switch (num5118)
                                {
                                    case 0:
                                        itemcode66 = "DC87";
                                        days59 = -1;
                                        break;
                                    case 1:
                                        itemcode66 = "DC88";
                                        days59 = -1;
                                        break;

                                    case 2:
                                        itemcode66 = "DC89";
                                        days59 = -1;
                                        break;

                                    case 3:
                                        itemcode66 = "DC90";
                                        days59 = -1;
                                        break;

                                    case 4:
                                        itemcode66 = "DC96";
                                        days59 = -1;
                                        break;
                                    case 5:
                                        itemcode66 = "DE17";
                                        days59 = -1;
                                        break;
                                    case 6:
                                        itemcode66 = "DE18";
                                        days59 = -1;
                                        break;
                                    case 7:
                                        itemcode66 = "DE19";
                                        days59 = -1;
                                        break;
                                    case 8:
                                        itemcode66 = "DE20";
                                        days59 = -1;
                                        break;
                                    case 9:
                                        itemcode66 = "DE21";
                                        days59 = -1;
                                        break;

                                    case 10:
                                        itemcode66 = "DE51";
                                        days59 = -1;
                                        break;
                                    case 11:
                                        itemcode66 = "DC97";
                                        days59 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode66, days59);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode66, days59));
                                return;
                            case "CR59":
                                int num518 = Game_Server.Generic.random(0, 15);
                                int days58 = 1;
                                string itemcode605 = (string)null;
                                switch (num518)
                                {
                                    case 0:
                                        itemcode605 = "DF53";
                                        days58 = 30;
                                        break;
                                    case 1:
                                        itemcode605 = "DC99";
                                        days58 = 30;
                                        break;

                                    case 2:
                                        itemcode605 = "DG52";
                                        days58 = 30;
                                        break;

                                    case 3:
                                        itemcode605 = "DF55";
                                        days58 = 30;
                                        break;

                                    case 4:
                                        itemcode605 = "DT11";
                                        days58 = 30;
                                        break;

                                    case 5:
                                        itemcode605 = "DA75";
                                        days58 = 30;
                                        break;
                                    case 6:
                                        itemcode605 = "DB40";
                                        days58 = 30;
                                        break;
                                    case 7:
                                        itemcode605 = "DM10";
                                        days58 = 30;
                                        break;
                                    case 8:
                                        itemcode605 = "DF53";
                                        days58 = -1;
                                        break;
                                    case 9:
                                        itemcode605 = "DC99";
                                        days58 = -1;
                                        break;

                                    case 10:
                                        itemcode605 = "DG52";
                                        days58 = -1;
                                        break;

                                    case 11:
                                        itemcode605 = "DF55";
                                        days58 = -1;
                                        break;

                                    case 12:
                                        itemcode605 = "DT11";
                                        days58 = -1;
                                        break;

                                    case 13:
                                        itemcode605 = "DA75";
                                        days58 = -1;
                                        break;
                                    case 14:
                                        itemcode605 = "DB40";
                                        days58 = -1;
                                        break;
                                    case 15:
                                        itemcode605 = "DM10";
                                        days58 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode605, days58);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode605, days58));
                                return;
                            case "CY69":
                                int num41 = Game_Server.Generic.random(0, 18);
                                int days78 = 1;
                                string itemcode616 = (string)null;
                                switch (num41)
                                {
                                    case 0:
                                        itemcode616 = "DC62";
                                        days78 = 30;
                                        break;
                                    case 1:
                                        itemcode616 = "DC62";
                                        days78 = -1;
                                        break;

                                    case 2:
                                        itemcode616 = "DC17";
                                        days78 = 30;
                                        break;

                                    case 3:
                                        itemcode616 = "D809";
                                        days78 = -1;
                                        break;

                                    case 4:
                                        itemcode616 = "D809";
                                        days78 = 30;
                                        break;

                                    case 5:
                                        itemcode616 = "DC83";
                                        days78 = -1;
                                        break;
                                    case 6:
                                        itemcode616 = "DC83";
                                        days78 = 30;
                                        break;

                                    case 7:
                                        itemcode616 = "DC90";
                                        days78 = -1;
                                        break;
                                    case 8:
                                        itemcode616 = "DC90";
                                        days78 = 30;
                                        break;
                                    case 9:
                                        itemcode616 = "DE24";
                                        days78 = -1;
                                        break;
                                    case 10:
                                        itemcode616 = "DE24";
                                        days78 = 30;
                                        break;
                                    case 11:
                                        itemcode616 = "DC83";
                                        days78 = -1;
                                        break;
                                    case 12:
                                        itemcode616 = "DC83";
                                        days78 = 30;
                                        break;
                                    case 13:
                                        itemcode616 = "DE45";
                                        days78 = -1;
                                        break;
                                    case 14:
                                        itemcode616 = "DE45";
                                        days78 = 30;
                                        break;
                                    case 15:
                                        itemcode616 = "DF69";
                                        days78 = -1;
                                        break;
                                    case 16:
                                        itemcode616 = "DF69";
                                        days78 = 30;
                                        break;
                                    case 17:
                                        itemcode616 = "DG55";
                                        days78 = -1;
                                        break;
                                    case 18:
                                        itemcode616 = "DG55";
                                        days78 = 30;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode616, days78);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode616, days78));
                                return;
                            case "CY67":
                                int num42 = Game_Server.Generic.random(0, 41);
                                int days36 = 1;
                                string itemcode627 = (string)null;
                                switch (num42)
                                {
                                    case 0:
                                        itemcode627 = "DC26";
                                        days36 = 30;
                                        break;
                                    case 1:
                                        itemcode627 = "DC26";
                                        days36 = -1;
                                        break;

                                    case 2:
                                        itemcode627 = "D812";
                                        days36 = 30;
                                        break;

                                    case 3:
                                        itemcode627 = "D812";
                                        days36 = -1;
                                        break;

                                    case 4:
                                        itemcode627 = "DC88";
                                        days36 = 30;
                                        break;

                                    case 5:
                                        itemcode627 = "DC88";
                                        days36 = -1;
                                        break;
                                    case 6:
                                        itemcode627 = "DC83";
                                        days36 = 30;
                                        break;

                                    case 7:
                                        itemcode627 = "DC83";
                                        days36 = -1;
                                        break;
                                    case 8:
                                        itemcode627 = "DD10";
                                        days36 = 30;
                                        break;
                                    case 9:
                                        itemcode627 = "DD10";
                                        days36 = -1;
                                        break;
                                    case 10:
                                        itemcode627 = "DE28";
                                        days36 = 30;
                                        break;
                                    case 11:
                                        itemcode627 = "DE28";
                                        days36 = -1;
                                        break;
                                    case 12:
                                        itemcode627 = "DF58";
                                        days36 = 30;
                                        break;
                                    case 13:
                                        itemcode627 = "DF58";
                                        days36 = -1;
                                        break;
                                    case 14:
                                        itemcode627 = "DE29";
                                        days36 = 30;
                                        break;
                                    case 15:
                                        itemcode627 = "DE29";
                                        days36 = -1;
                                        break;
                                    case 16:
                                        itemcode627 = "DF59";
                                        days36 = 30;
                                        break;
                                    case 17:
                                        itemcode627 = "DF59";
                                        days36 = -1;
                                        break;
                                    case 18:
                                        itemcode627 = "DG54";
                                        days36 = 30;
                                        break;
                                    case 19:
                                        itemcode627 = "DG54";
                                        days36 = -1;
                                        break;
                                    case 20:
                                        itemcode627 = "DJ25";
                                        days36 = 30;
                                        break;
                                    case 21:
                                        itemcode627 = "DJ25";
                                        days36 = -1;
                                        break;
                                    case 22:
                                        itemcode627 = "DF81";
                                        days36 = 30;
                                        break;
                                    case 23:
                                        itemcode627 = "DF81";
                                        days36 = -1;
                                        break;
                                    case 24:
                                        itemcode627 = "DF91";
                                        days36 = 30;
                                        break;
                                    case 25:
                                        itemcode627 = "DF91";
                                        days36 = -1;
                                        break;
                                    case 26:
                                        itemcode627 = "DG82";
                                        days36 = 30;
                                        break;
                                    case 27:
                                        itemcode627 = "DG82";
                                        days36 = -1;
                                        break;
                                    case 28:
                                        itemcode627 = "DE62";
                                        days36 = 30;
                                        break;
                                    case 29:
                                        itemcode627 = "DE62";
                                        days36 = -1;
                                        break;
                                    case 30:
                                        itemcode627 = "DT15";
                                        days36 = 30;
                                        break;
                                    case 31:
                                        itemcode627 = "DT15";
                                        days36 = -1;
                                        break;
                                    case 32:
                                        itemcode627 = "DB42";
                                        days36 = 30;
                                        break;
                                    case 33:
                                        itemcode627 = "DB42";
                                        days36 = -1;
                                        break;
                                    case 34:
                                        itemcode627 = "GF17";
                                        days36 = 30;
                                        break;
                                    case 35:
                                        itemcode627 = "GF17";
                                        days36 = -1;
                                        break;
                                    case 36:
                                        itemcode627 = "GG05";
                                        days36 = 30;
                                        break;
                                    case 37:
                                        itemcode627 = "GG05";
                                        days36 = -1;
                                        break;
                                    case 38:
                                        itemcode627 = "DH14";
                                        days36 = 30;
                                        break;
                                    case 39:
                                        itemcode627 = "DH14";
                                        days36 = -1;
                                        break;
                                    case 40:
                                        itemcode627 = "DJ60";
                                        days36 = 30;
                                        break;
                                    case 41:
                                        itemcode627 = "DJ60";
                                        days36 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode627, days36);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode627, days36));
                                return;
                            case "CR31":
                                int num51 = Game_Server.Generic.random(0, 6);
                                int days68 = 1;
                                string itemcode606 = (string)null;
                                switch (num51)
                                {
                                    case 0:
                                        itemcode606 = "DG45";
                                        days68 = 30;
                                        break;
                                    case 1:
                                        itemcode606 = "CZ85";
                                        days68 = 1;
                                        break;

                                    case 2:
                                        itemcode606 = "CB09";
                                        days68 = 1;
                                        break;

                                    case 3:
                                        itemcode606 = "CF02";
                                        days68 = 7;
                                        break;

                                    case 4:
                                        itemcode606 = "DS01";
                                        days68 = 30;
                                        break;

                                    case 5:
                                        itemcode606 = "DB10";
                                        days68 = 1;
                                        break;
                                    case 6:
                                        itemcode606 = "CZ84";
                                        days68 = 1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode606, days68);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode606, days68));
                                return;
                            case "CR32":
                                int num52 = Game_Server.Generic.random(0, 6);
                                int days69 = 1;
                                string itemcode607 = (string)null;
                                switch (num52)
                                {
                                    case 0:
                                        itemcode607 = "DG24";
                                        days69 = 30;
                                        break;
                                    case 1:
                                        itemcode607 = "CZ85";
                                        days69 = 1;
                                        break;

                                    case 2:
                                        itemcode607 = "CB09";
                                        days69 = 1;
                                        break;

                                    case 3:
                                        itemcode607 = "CF02";
                                        days69 = 7;
                                        break;

                                    case 4:
                                        itemcode607 = "DS01";
                                        days69 = 30;
                                        break;

                                    case 5:
                                        itemcode607 = "DB10";
                                        days69 = 1;
                                        break;
                                    case 6:
                                        itemcode607 = "CZ81";
                                        days69 = 1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode607, days69);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode607, days69));
                                return;
                            case "CR98":
                                int num63 = Game_Server.Generic.random(0, 9);
                                int days12 = 1;
                                string itemcode13 = (string)null;
                                switch (num63)
                                {
                                    case 0:
                                        itemcode13 = "D832";
                                        days12 = 60;
                                        break;
                                    case 1:
                                        itemcode13 = "D832";
                                        days12 = 90;
                                        break;

                                    case 2:
                                        itemcode13 = "D832";
                                        days12 = 180;
                                        break;

                                    case 3:
                                        itemcode13 = "D832";
                                        days12 = 360;
                                        break;

                                    case 4:
                                        itemcode13 = "D832";
                                        days12 = 5000;
                                        break;

                                    case 5:
                                        itemcode13 = "D911";
                                        days12 = 60;
                                        break;
                                    case 6:
                                        itemcode13 = "D911";
                                        days12 = 90;
                                        break;
                                    case 7:
                                        itemcode13 = "D911";
                                        days12 = 180;
                                        break;
                                    case 8:
                                        itemcode13 = "D911";
                                        days12 = 360;
                                        break;
                                    case 9:
                                        itemcode13 = "D911";
                                        days12 = 5000;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode13, days12);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode13, days12));
                                return;
                            case "CY59":
                                int num59 = Game_Server.Generic.random(0, 29);
                                int days83 = 1;
                                string itemcode659 = (string)null;
                                switch (num59)
                                {
                                    case 0:
                                        itemcode659 = "DG22";
                                        days83 = 15;
                                        break;
                                    case 1:
                                        itemcode659 = "DG22";
                                        days83 = 30;
                                        break;
                                    case 2:
                                        itemcode659 = "DG22";
                                        days83 = -1;
                                        break;
                                    case 3:
                                        itemcode659 = "D602";
                                        days83 = 15;
                                        break;
                                    case 4:
                                        itemcode659 = "D602";
                                        days83 = 30;
                                        break;
                                    case 5:
                                        itemcode659 = "D602";
                                        days83 = -1;
                                        break;
                                    case 6:
                                        itemcode659 = "DJ67";
                                        days83 = 15;
                                        break;
                                    case 7:
                                        itemcode659 = "DJ67";
                                        days83 = 30;
                                        break;
                                    case 8:
                                        itemcode659 = "DJ67";
                                        days83 = -1;
                                        break;
                                    case 9:
                                        itemcode659 = "DK03";
                                        days83 = 15;
                                        break;
                                    case 10:
                                        itemcode659 = "DK03";
                                        days83 = 30;
                                        break;
                                    case 11:
                                        itemcode659 = "DK03";
                                        days83 = -1;
                                        break;
                                    case 12:
                                        itemcode659 = "DG31";
                                        days83 = 15;
                                        break;
                                    case 13:
                                        itemcode659 = "DG31";
                                        days83 = 30;
                                        break;
                                    case 14:
                                        itemcode659 = "DG31";
                                        days83 = -1;
                                        break;
                                    case 15:
                                        itemcode659 = "DF87";
                                        days83 = 15;
                                        break;
                                    case 16:
                                        itemcode659 = "DF87";
                                        days83 = 30;
                                        break;
                                    case 17:
                                        itemcode659 = "DF87";
                                        days83 = -1;
                                        break;
                                    case 18:
                                        itemcode659 = "DC93";
                                        days83 = 15;
                                        break;
                                    case 19:
                                        itemcode659 = "DC93";
                                        days83 = 30;
                                        break;
                                    case 20:
                                        itemcode659 = "DC93";
                                        days83 = -1;
                                        break;
                                    case 21:
                                        itemcode659 = "DF65";
                                        days83 = 15;
                                        break;
                                    case 22:
                                        itemcode659 = "DF65";
                                        days83 = 30;
                                        break;
                                    case 23:
                                        itemcode659 = "DF65";
                                        days83 = -1;
                                        break;
                                    case 24:
                                        itemcode659 = "DB17";
                                        days83 = 15;
                                        break;
                                    case 25:
                                        itemcode659 = "DB17";
                                        days83 = 30;
                                        break;
                                    case 26:
                                        itemcode659 = "DB17";
                                        days83 = -1;
                                        break;
                                    case 27:
                                        itemcode659 = "D902";
                                        days83 = 15;
                                        break;
                                    case 28:
                                        itemcode659 = "D902";
                                        days83 = 30;
                                        break;
                                    case 29:
                                        itemcode659 = "D902";
                                        days83 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode659, days83);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode659, days83));
                                return;
                            case "CY58":
                                int num43 = Game_Server.Generic.random(0, 29);
                                int days87 = 1;
                                string itemcode679 = (string)null;
                                switch (num43)
                                {
                                    case 0:
                                        itemcode679 = "DG46";
                                        days87 = 15;
                                        break;
                                    case 1:
                                        itemcode679 = "DG46";
                                        days87 = 30;
                                        break;
                                    case 2:
                                        itemcode679 = "DG46";
                                        days87 = -1;
                                        break;
                                    case 3:
                                        itemcode679 = "DF36";
                                        days87 = 15;
                                        break;
                                    case 4:
                                        itemcode679 = "DF36";
                                        days87 = 30;
                                        break;
                                    case 5:
                                        itemcode679 = "DF36";
                                        days87 = -1;
                                        break;
                                    case 6:
                                        itemcode679 = "DF51";
                                        days87 = 15;
                                        break;
                                    case 7:
                                        itemcode679 = "DF51";
                                        days87 = 30;
                                        break;
                                    case 8:
                                        itemcode679 = "DF51";
                                        days87 = -1;
                                        break;
                                    case 9:
                                        itemcode679 = "DJ59";
                                        days87 = 15;
                                        break;
                                    case 10:
                                        itemcode679 = "DJ59";
                                        days87 = 30;
                                        break;
                                    case 11:
                                        itemcode679 = "DJ59";
                                        days87 = -1;
                                        break;
                                    case 12:
                                        itemcode679 = "DH10";
                                        days87 = 15;
                                        break;
                                    case 13:
                                        itemcode679 = "DH10";
                                        days87 = 30;
                                        break;
                                    case 14:
                                        itemcode679 = "DH10";
                                        days87 = -1;
                                        break;
                                    case 15:
                                        itemcode679 = "DE52";
                                        days87 = 15;
                                        break;
                                    case 16:
                                        itemcode679 = "DE52";
                                        days87 = 30;
                                        break;
                                    case 17:
                                        itemcode679 = "DE52";
                                        days87 = -1;
                                        break;
                                    case 18:
                                        itemcode679 = "D832";
                                        days87 = 15;
                                        break;
                                    case 19:
                                        itemcode679 = "D832";
                                        days87 = 30;
                                        break;
                                    case 20:
                                        itemcode679 = "D832";
                                        days87 = -1;
                                        break;
                                    case 21:
                                        itemcode679 = "DI03";
                                        days87 = 15;
                                        break;
                                    case 22:
                                        itemcode679 = "DI03";
                                        days87 = 30;
                                        break;
                                    case 23:
                                        itemcode679 = "DI03";
                                        days87 = -1;
                                        break;
                                    case 24:
                                        itemcode679 = "DC86";
                                        days87 = 15;
                                        break;
                                    case 25:
                                        itemcode679 = "DC86";
                                        days87 = 30;
                                        break;
                                    case 26:
                                        itemcode679 = "DC86";
                                        days87 = -1;
                                        break;
                                    case 27:
                                        itemcode679 = "DF86";
                                        days87 = 15;
                                        break;
                                    case 28:
                                        itemcode679 = "DF86";
                                        days87 = 30;
                                        break;
                                    case 29:
                                        itemcode679 = "DF86";
                                        days87 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode679, days87);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode679, days87));
                                return;
                            case "CY56":
                                int num491 = Game_Server.Generic.random(0, 29);
                                int days98 = 1;
                                string itemcode681 = (string)null;
                                switch (num491)
                                {
                                    case 0:
                                        itemcode681 = "GF21";
                                        days98 = 15;
                                        break;
                                    case 1:
                                        itemcode681 = "GF21";
                                        days98 = 30;
                                        break;
                                    case 2:
                                        itemcode681 = "GF21";
                                        days98 = -1;
                                        break;
                                    case 3:
                                        itemcode681 = "DF71";
                                        days98 = 15;
                                        break;
                                    case 4:
                                        itemcode681 = "DF71";
                                        days98 = 30;
                                        break;
                                    case 5:
                                        itemcode681 = "DF71";
                                        days98 = -1;
                                        break;
                                    case 6:
                                        itemcode681 = "DD27";
                                        days98 = 15;
                                        break;
                                    case 7:
                                        itemcode681 = "DD27";
                                        days98 = 30;
                                        break;
                                    case 8:
                                        itemcode681 = "DD27";
                                        days98 = -1;
                                        break;
                                    case 9:
                                        itemcode681 = "DE85";
                                        days98 = 15;
                                        break;
                                    case 10:
                                        itemcode681 = "DE85";
                                        days98 = 30;
                                        break;
                                    case 11:
                                        itemcode681 = "DE85";
                                        days98 = -1;
                                        break;
                                    case 12:
                                        itemcode681 = "GG14";
                                        days98 = 15;
                                        break;
                                    case 13:
                                        itemcode681 = "GG14";
                                        days98 = 30;
                                        break;
                                    case 14:
                                        itemcode681 = "GG14 ";
                                        days98 = -1;
                                        break;
                                    case 15:
                                        itemcode681 = "D816";
                                        days98 = 15;
                                        break;
                                    case 16:
                                        itemcode681 = "D816";
                                        days98 = 30;
                                        break;
                                    case 17:
                                        itemcode681 = "D816";
                                        days98 = -1;
                                        break;
                                    case 18:
                                        itemcode681 = "DH15";
                                        days98 = 15;
                                        break;
                                    case 19:
                                        itemcode681 = "DH15";
                                        days98 = 30;
                                        break;
                                    case 20:
                                        itemcode681 = "DH15";
                                        days98 = -1;
                                        break;
                                    case 21:
                                        itemcode681 = "DC85";
                                        days98 = 15;
                                        break;
                                    case 22:
                                        itemcode681 = "DC85";
                                        days98 = 30;
                                        break;
                                    case 23:
                                        itemcode681 = "DC85";
                                        days98 = -1;
                                        break;
                                    case 24:
                                        itemcode681 = "DT17";
                                        days98 = 15;
                                        break;
                                    case 25:
                                        itemcode681 = "DT17";
                                        days98 = 30;
                                        break;
                                    case 26:
                                        itemcode681 = "DT17";
                                        days98 = -1;
                                        break;
                                    case 27:
                                        itemcode681 = "DA08";
                                        days98 = 15;
                                        break;
                                    case 28:
                                        itemcode681 = "DA08";
                                        days98 = 30;
                                        break;
                                    case 29:
                                        itemcode681 = "DA08";
                                        days98 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode681, days98);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode681, days98));
                                return;
                            case "CY61":
                                int num69 = Game_Server.Generic.random(0, 29);
                                int days106 = 1;
                                string itemcode682 = (string)null;
                                switch (num69)
                                {
                                    case 0:
                                        itemcode682 = "D801";
                                        days106 = 15;
                                        break;
                                    case 1:
                                        itemcode682 = "D801";
                                        days106 = 30;
                                        break;
                                    case 2:
                                        itemcode682 = "D801";
                                        days106 = -1;
                                        break;
                                    case 3:
                                        itemcode682 = "DE93";
                                        days106 = 15;
                                        break;
                                    case 4:
                                        itemcode682 = "DE93";
                                        days106 = 30;
                                        break;
                                    case 5:
                                        itemcode682 = "DE93";
                                        days106 = -1;
                                        break;
                                    case 6:
                                        itemcode682 = "DF68";
                                        days106 = 15;
                                        break;
                                    case 7:
                                        itemcode682 = "DF68";
                                        days106 = 30;
                                        break;
                                    case 8:
                                        itemcode682 = "DF68";
                                        days106 = -1;
                                        break;
                                    case 9:
                                        itemcode682 = "DF41";
                                        days106 = 15;
                                        break;
                                    case 10:
                                        itemcode682 = "DF41";
                                        days106 = 30;
                                        break;
                                    case 11:
                                        itemcode682 = "DF41";
                                        days106 = -1;
                                        break;
                                    case 12:
                                        itemcode682 = "DD28";
                                        days106 = 15;
                                        break;
                                    case 13:
                                        itemcode682 = "DD28";
                                        days106 = 30;
                                        break;
                                    case 14:
                                        itemcode682 = "DD28";
                                        days106 = -1;
                                        break;
                                    case 15:
                                        itemcode682 = "DG32";
                                        days106 = 15;
                                        break;
                                    case 16:
                                        itemcode682 = "DG32";
                                        days106 = 30;
                                        break;
                                    case 17:
                                        itemcode682 = "DG32";
                                        days106 = -1;
                                        break;
                                    case 18:
                                        itemcode682 = "GG09";
                                        days106 = 15;
                                        break;
                                    case 19:
                                        itemcode682 = "GG09";
                                        days106 = 30;
                                        break;
                                    case 20:
                                        itemcode682 = "GG09";
                                        days106 = -1;
                                        break;
                                    case 21:
                                        itemcode682 = "D901";
                                        days106 = 15;
                                        break;
                                    case 22:
                                        itemcode682 = "D901";
                                        days106 = 30;
                                        break;
                                    case 23:
                                        itemcode682 = "D901";
                                        days106 = -1;
                                        break;
                                    case 24:
                                        itemcode682 = "DJ39";
                                        days106 = 15;
                                        break;
                                    case 25:
                                        itemcode682 = "DJ39";
                                        days106 = 30;
                                        break;
                                    case 26:
                                        itemcode682 = "DJ39";
                                        days106 = -1;
                                        break;
                                    case 27:
                                        itemcode682 = "DB53";
                                        days106 = 15;
                                        break;
                                    case 28:
                                        itemcode682 = "DB53";
                                        days106 = 30;
                                        break;
                                    case 29:
                                        itemcode682 = "DB53";
                                        days106 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode682, days106);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode682, days106));
                                return;
                            case "CY60":
                                int num50 = Game_Server.Generic.random(0, 29);
                                int days105 = 1;
                                string itemcode685 = (string)null;
                                switch (num50)
                                {
                                    case 0:
                                        itemcode685 = "DE68";
                                        days105 = 15;
                                        break;
                                    case 1:
                                        itemcode685 = "DE68";
                                        days105 = 30;
                                        break;
                                    case 2:
                                        itemcode685 = "DE68";
                                        days105 = -1;
                                        break;
                                    case 3:
                                        itemcode685 = "GF03";
                                        days105 = 15;
                                        break;
                                    case 4:
                                        itemcode685 = "GF03";
                                        days105 = 30;
                                        break;
                                    case 5:
                                        itemcode685 = "GF03";
                                        days105 = -1;
                                        break;
                                    case 6:
                                        itemcode685 = "DG49";
                                        days105 = 15;
                                        break;
                                    case 7:
                                        itemcode685 = "DG49";
                                        days105 = 30;
                                        break;
                                    case 8:
                                        itemcode685 = "DG49";
                                        days105 = -1;
                                        break;
                                    case 9:
                                        itemcode685 = "DC94";
                                        days105 = 15;
                                        break;
                                    case 10:
                                        itemcode685 = "DC94";
                                        days105 = 30;
                                        break;
                                    case 11:
                                        itemcode685 = "DC94";
                                        days105 = -1;
                                        break;
                                    case 12:
                                        itemcode685 = "DC72";
                                        days105 = 15;
                                        break;
                                    case 13:
                                        itemcode685 = "DC72";
                                        days105 = 30;
                                        break;
                                    case 14:
                                        itemcode685 = "DC72";
                                        days105 = -1;
                                        break;
                                    case 15:
                                        itemcode685 = "DF25";
                                        days105 = 15;
                                        break;
                                    case 16:
                                        itemcode685 = "DF25";
                                        days105 = 30;
                                        break;
                                    case 17:
                                        itemcode685 = "DF25";
                                        days105 = -1;
                                        break;
                                    case 18:
                                        itemcode685 = "DC73";
                                        days105 = 15;
                                        break;
                                    case 19:
                                        itemcode685 = "DC73";
                                        days105 = 30;
                                        break;
                                    case 20:
                                        itemcode685 = "DC73";
                                        days105 = -1;
                                        break;
                                    case 21:
                                        itemcode685 = "DG24";
                                        days105 = 15;
                                        break;
                                    case 22:
                                        itemcode685 = "DG24";
                                        days105 = 30;
                                        break;
                                    case 23:
                                        itemcode685 = "DG24";
                                        days105 = -1;
                                        break;
                                    case 24:
                                        itemcode685 = "DI06";
                                        days105 = 15;
                                        break;
                                    case 25:
                                        itemcode685 = "DI06";
                                        days105 = 30;
                                        break;
                                    case 26:
                                        itemcode685 = "DI06";
                                        days105 = -1;
                                        break;
                                    case 27:
                                        itemcode685 = "DH04";
                                        days105 = 15;
                                        break;
                                    case 28:
                                        itemcode685 = "DH04";
                                        days105 = 30;
                                        break;
                                    case 29:
                                        itemcode685 = "DH04";
                                        days105 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode685, days105);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode685, days105));
                                return;
                            case "CY57":
                                int num48 = Game_Server.Generic.random(0, 29);
                                int days77 = 1;
                                string itemcode669 = (string)null;
                                switch (num48)
                                {
                                    case 0:
                                        itemcode669 = "DF93";
                                        days77 = 15;
                                        break;
                                    case 1:
                                        itemcode669 = "DF93";
                                        days77 = 30;
                                        break;
                                    case 2:
                                        itemcode669 = "DF93";
                                        days77 = -1;
                                        break;
                                    case 3:
                                        itemcode669 = "GF17";
                                        days77 = 15;
                                        break;
                                    case 4:
                                        itemcode669 = "GF17";
                                        days77 = 30;
                                        break;
                                    case 5:
                                        itemcode669 = "GF17";
                                        days77 = -1;
                                        break;
                                    case 6:
                                        itemcode669 = "DF53";
                                        days77 = 15;
                                        break;
                                    case 7:
                                        itemcode669 = "DF53";
                                        days77 = 30;
                                        break;
                                    case 8:
                                        itemcode669 = "DF53";
                                        days77 = -1;
                                        break;
                                    case 9:
                                        itemcode669 = "DE80";
                                        days77 = 15;
                                        break;
                                    case 10:
                                        itemcode669 = "DE80";
                                        days77 = 30;
                                        break;
                                    case 11:
                                        itemcode669 = "DE80";
                                        days77 = -1;
                                        break;
                                    case 12:
                                        itemcode669 = "DE46";
                                        days77 = 15;
                                        break;
                                    case 13:
                                        itemcode669 = "DE46";
                                        days77 = 30;
                                        break;
                                    case 14:
                                        itemcode669 = "DE46";
                                        days77 = -1;
                                        break;
                                    case 15:
                                        itemcode669 = "DB27";
                                        days77 = 15;
                                        break;
                                    case 16:
                                        itemcode669 = "DB27";
                                        days77 = 30;
                                        break;
                                    case 17:
                                        itemcode669 = "DB27";
                                        days77 = -1;
                                        break;
                                    case 18:
                                        itemcode669 = "DE62";
                                        days77 = 15;
                                        break;
                                    case 19:
                                        itemcode669 = "DE62";
                                        days77 = 30;
                                        break;
                                    case 20:
                                        itemcode669 = "DE62";
                                        days77 = -1;
                                        break;
                                    case 21:
                                        itemcode669 = "DJ10";
                                        days77 = 15;
                                        break;
                                    case 22:
                                        itemcode669 = "DJ10";
                                        days77 = 30;
                                        break;
                                    case 23:
                                        itemcode669 = "DJ10";
                                        days77 = -1;
                                        break;
                                    case 24:
                                        itemcode669 = "DG77";
                                        days77 = 15;
                                        break;
                                    case 25:
                                        itemcode669 = "DG77";
                                        days77 = 30;
                                        break;
                                    case 26:
                                        itemcode669 = "DG77";
                                        days77 = -1;
                                        break;
                                    case 27:
                                        itemcode669 = "DI12";
                                        days77 = 15;
                                        break;
                                    case 28:
                                        itemcode669 = "DI12";
                                        days77 = 30;
                                        break;
                                    case 29:
                                        itemcode669 = "DI12";
                                        days77 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode669, days77);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode669, days77));
                                return;

                            case "CY70":
                                int num53 = Game_Server.Generic.random(0, 11);
                                int days80 = 1;
                                string itemcode624 = (string)null;
                                switch (num53)
                                {
                                    case 0:
                                        itemcode624 = "DJ33";
                                        days80 = -1;
                                        break;
                                    case 1:
                                        itemcode624 = "DJ63";
                                        days80 = -1;
                                        break;

                                    case 2:
                                        itemcode624 = "DJ07";
                                        days80 = -1;
                                        break;

                                    case 3:
                                        itemcode624 = "DJ93";
                                        days80 = -1;
                                        break;

                                    case 4:
                                        itemcode624 = "DJ13";
                                        days80 = -1;
                                        break;
                                    case 5:
                                        itemcode624 = "DJ22";
                                        days80 = -1;
                                        break;
                                    case 6:
                                        itemcode624 = "DJ23";
                                        days80 = -1;
                                        break;
                                    case 7:
                                        itemcode624 = "DJ26";
                                        days80 = -1;
                                        break;
                                    case 8:
                                        itemcode624 = "DJ37";
                                        days80 = -1;
                                        break;
                                    case 9:
                                        itemcode624 = "DJ44";
                                        days80 = -1;
                                        break;
                                    case 10:
                                        itemcode624 = "DJ45";
                                        days80 = -1;
                                        break;
                                    case 11:
                                        itemcode624 = "DJ61";
                                        days80 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode624, days80);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode624, days80));
                                return;
                            case "CY64":
                                int num712 = Game_Server.Generic.random(0, 24);
                                int days82 = 1;
                                string itemcode625 = (string)null;
                                switch (num712)
                                {
                                    case 0:
                                        itemcode625 = "DC30";
                                        days82 = 15;
                                        break;
                                    case 1:
                                        itemcode625 = "DC30";
                                        days82 = 30;
                                        break;

                                    case 2:
                                        itemcode625 = "DC30";
                                        days82 = 45;
                                        break;

                                    case 3:
                                        itemcode625 = "DC30";
                                        days82 = 90;
                                        break;

                                    case 4:
                                        itemcode625 = "DC30";
                                        days82 = -1;
                                        break;
                                    case 5:
                                        itemcode625 = "D810";
                                        days82 = 15;
                                        break;
                                    case 6:
                                        itemcode625 = "D810";
                                        days82 = 30;
                                        break;
                                    case 7:
                                        itemcode625 = "D810";
                                        days82 = 45;
                                        break;
                                    case 8:
                                        itemcode625 = "D810";
                                        days82 = 90;
                                        break;
                                    case 9:
                                        itemcode625 = "D810";
                                        days82 = -1;
                                        break;
                                    case 10:
                                        itemcode625 = "DD13";
                                        days82 = 15;
                                        break;
                                    case 11:
                                        itemcode625 = "DD13";
                                        days82 = 30;
                                        break;
                                    case 12:
                                        itemcode625 = "DD13";
                                        days82 = 45;
                                        break;
                                    case 13:
                                        itemcode625 = "DD13";
                                        days82 = 90;
                                        break;
                                    case 14:
                                        itemcode625 = "DD13";
                                        days82 = -1;
                                        break;
                                    case 15:
                                        itemcode625 = "GG27";
                                        days82 = 15;
                                        break;
                                    case 16:
                                        itemcode625 = "GG27";
                                        days82 = 30;
                                        break;
                                    case 17:
                                        itemcode625 = "GG27";
                                        days82 = 45;
                                        break;
                                    case 18:
                                        itemcode625 = "GG27";
                                        days82 = 90;
                                        break;
                                    case 19:
                                        itemcode625 = "GG27";
                                        days82 = -1;
                                        break;
                                    case 20:
                                        itemcode625 = "GF36";
                                        days82 = 15;
                                        break;
                                    case 21:
                                        itemcode625 = "GF36";
                                        days82 = 30;
                                        break;
                                    case 22:
                                        itemcode625 = "GF36";
                                        days82 = 45;
                                        break;
                                    case 23:
                                        itemcode625 = "GF36";
                                        days82 = 90;
                                        break;
                                    case 24:
                                        itemcode625 = "GF36";
                                        days82 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode625, days82);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode625, days82));
                                return;
                            case "CY65":
                                int num713 = Game_Server.Generic.random(0, 25);
                                int days86 = 1;
                                string itemcode629 = (string)null;
                                switch (num713)
                                {
                                    case 0:
                                        itemcode629 = "DC56";
                                        days86 = 15;
                                        break;
                                    case 1:
                                        itemcode629 = "DC56";
                                        days86 = 30;
                                        break;

                                    case 2:
                                        itemcode629 = "DC56";
                                        days86 = 45;
                                        break;

                                    case 3:
                                        itemcode629 = "DC56";
                                        days86 = 90;
                                        break;

                                    case 4:
                                        itemcode629 = "DC56";
                                        days86 = -1;
                                        break;
                                    case 5:
                                        itemcode629 = "D609";
                                        days86 = 15;
                                        break;
                                    case 6:
                                        itemcode629 = "D609";
                                        days86 = 30;
                                        break;
                                    case 7:
                                        itemcode629 = "D609";
                                        days86 = 45;
                                        break;
                                    case 8:
                                        itemcode629 = "D609";
                                        days86 = 90;
                                        break;
                                    case 9:
                                        itemcode629 = "D609";
                                        days86 = -1;
                                        break;
                                    case 10:
                                        itemcode629 = "DF79";
                                        days86 = 15;
                                        break;
                                    case 11:
                                        itemcode629 = "DF79";
                                        days86 = 30;
                                        break;
                                    case 12:
                                        itemcode629 = "DF79";
                                        days86 = 45;
                                        break;
                                    case 13:
                                        itemcode629 = "DF79";
                                        days86 = 90;
                                        break;
                                    case 14:
                                        itemcode629 = "DF79";
                                        days86 = -1;
                                        break;
                                    case 15:
                                        itemcode629 = "GC08";
                                        days86 = 15;
                                        break;
                                    case 16:
                                        itemcode629 = "GC08";
                                        days86 = 30;
                                        break;
                                    case 17:
                                        itemcode629 = "GC08";
                                        days86 = 45;
                                        break;
                                    case 18:
                                        itemcode629 = "GC08";
                                        days86 = 90;
                                        break;
                                    case 19:
                                        itemcode629 = "GC08";
                                        days86 = -1;
                                        break;
                                    case 20:
                                        itemcode629 = "GG28";
                                        days86 = 15;
                                        break;
                                    case 21:
                                        itemcode629 = "GG28";
                                        days86 = 30;
                                        break;
                                    case 22:
                                        itemcode629 = "GG28";
                                        days86 = 45;
                                        break;
                                    case 23:
                                        itemcode629 = "GG28";
                                        days86 = 90;
                                        break;
                                    case 24:
                                        itemcode629 = "GG28";
                                        days86 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode629, days86);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode629, days86));
                                return;
                            case "CR94":
                                int num500 = Game_Server.Generic.random(0, 4);
                                int days76 = 1;
                                string itemcode623 = (string)null;
                                switch (num500)
                                {
                                    case 0:
                                        itemcode623 = "D831";
                                        days76 = 60;
                                        break;
                                    case 1:
                                        itemcode623 = "D831";
                                        days76 = 90;
                                        break;

                                    case 2:
                                        itemcode623 = "D831";
                                        days76 = 180;
                                        break;

                                    case 3:
                                        itemcode623 = "D831";
                                        days76 = 360;
                                        break;

                                    case 4:
                                        itemcode623 = "D831";
                                        days76 = 5000;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode623, days76);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode623, days76));
                                return;
                            case "CR95":
                                int num54 = Game_Server.Generic.random(0, 9);
                                int days103 = 1;
                                string itemcode621 = (string)null;
                                switch (num54)
                                {
                                    case 0:
                                        itemcode621 = "D608";
                                        days103 = 60;
                                        break;
                                    case 1:
                                        itemcode621 = "D608";
                                        days103 = 90;
                                        break;

                                    case 2:
                                        itemcode621 = "D608";
                                        days103 = 180;
                                        break;

                                    case 3:
                                        itemcode621 = "D608";
                                        days103 = 360;
                                        break;

                                    case 4:
                                        itemcode621 = "D608";
                                        days103 = 5000;
                                        break;

                                    case 5:
                                        itemcode621 = "D911";
                                        days103 = 60;
                                        break;
                                    case 6:
                                        itemcode621 = "D911";
                                        days103 = 90;
                                        break;
                                    case 7:
                                        itemcode621 = "D911";
                                        days103 = 180;
                                        break;
                                    case 8:
                                        itemcode621 = "D911";
                                        days103 = 360;
                                        break;
                                    case 9:
                                        itemcode621 = "D911";
                                        days103 = 5000;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode621, days103);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode621, days103));
                                return;
                            case "CR37":
                                int num56 = Game_Server.Generic.random(0, 6);
                                int days71 = 1;
                                string itemcode610 = (string)null;
                                switch (num56)
                                {
                                    case 0:
                                        itemcode610 = "DF73";
                                        days71 = 30;
                                        break;
                                    case 1:
                                        itemcode610 = "DC31";
                                        days71 = 7;
                                        break;

                                    case 2:
                                        itemcode610 = "CB09";
                                        days71 = 1;
                                        break;

                                    case 3:
                                        itemcode610 = "CF02";
                                        days71 = 7;
                                        break;

                                    case 4:
                                        itemcode610 = "DS01";
                                        days71 = 30;
                                        break;

                                    case 5:
                                        itemcode610 = "DB10";
                                        days71 = 1;
                                        break;
                                    case 6:
                                        itemcode610 = "CZ81";
                                        days71 = 1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode610, days71);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode610, days71));
                                return;
                            case "CR36":
                                int num55 = Game_Server.Generic.random(0, 6);
                                int days70 = 1;
                                string itemcode609 = (string)null;
                                switch (num55)
                                {
                                    case 0:
                                        itemcode609 = "DE16";
                                        days70 = 30;
                                        break;
                                    case 1:
                                        itemcode609 = "CZ85";
                                        days70 = 1;
                                        break;

                                    case 2:
                                        itemcode609 = "CB09";
                                        days70 = 1;
                                        break;

                                    case 3:
                                        itemcode609 = "CF02";
                                        days70 = 7;
                                        break;

                                    case 4:
                                        itemcode609 = "DS01";
                                        days70 = 30;
                                        break;

                                    case 5:
                                        itemcode609 = "DB10";
                                        days70 = 1;
                                        break;
                                    case 6:
                                        itemcode609 = "CZ81";
                                        days70 = 1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode609, days70);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode609, days70));
                                return;
                            case "CR33":
                                int num515 = Game_Server.Generic.random(0, 6);
                                int days79 = 1;
                                string itemcode608 = (string)null;
                                switch (num515)
                                {
                                    case 0:
                                        itemcode608 = "DF17";
                                        days79 = 30;
                                        break;
                                    case 1:
                                        itemcode608 = "CZ85";
                                        days79 = 1;
                                        break;

                                    case 2:
                                        itemcode608 = "CB09";
                                        days79 = 1;
                                        break;

                                    case 3:
                                        itemcode608 = "CF01";
                                        days79 = 7;
                                        break;

                                    case 4:
                                        itemcode608 = "DS01";
                                        days79 = 30;
                                        break;

                                    case 5:
                                        itemcode608 = "DB10";
                                        days79 = 1;
                                        break;
                                    case 6:
                                        itemcode608 = "CZ81";
                                        days79 = 1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode608, days79);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode608, days79));
                                return;
                            case "CR29":
                                int num15 = Game_Server.Generic.random(0, 5);
                                int days17 = 1;
                                string itemcode111 = (string)null;
                                switch (num15)
                                {
                                    case 0:
                                        itemcode111 = "DC79";
                                        days17 = 30;
                                        break;
                                    case 1:
                                        itemcode111 = "DC79";
                                        days17 = 3;
                                        break;
                                    case 2:
                                        itemcode111 = "DC80";
                                        days17 = 7;
                                        break;
                                    case 3:
                                        itemcode111 = "DC98";
                                        days17 = 7;
                                        break;
                                    case 4:
                                        itemcode111 = "DE30";
                                        days17 = 7;
                                        break;
                                    case 5:
                                        itemcode111 = "DE46";
                                        days17 = 7;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode111, days17);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode111, days17));
                                return;
                            case "CR45":
                                int num115 = Game_Server.Generic.random(0, 6);
                                int days150 = 1;
                                string itemcode121 = (string)null;
                                switch (num115)
                                {
                                    case 0:
                                        itemcode121 = "DG51";
                                        days150 = 30;
                                        break;
                                    case 1:
                                        itemcode121 = "CZ81";
                                        days150 = 1;
                                        break;
                                    case 2:
                                        itemcode121 = "DC80";
                                        days150 = 30;
                                        break;
                                    case 3:
                                        itemcode121 = "DC98";
                                        days150 = 30;
                                        break;
                                    case 4:
                                        itemcode121 = "DE30";
                                        days150 = 30;
                                        break;
                                    case 5:
                                        itemcode121 = "DE46";
                                        days150 = 30;
                                        break;
                                    case 6:
                                        itemcode121 = "CB09";
                                        days150 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode121, days150);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode121, days150));
                                return;
                            case "CY97":
                                int num118 = Game_Server.Generic.random(0, 6);
                                int days152 = 1;
                                string itemcode126 = (string)null;
                                switch (num118)
                                {
                                    case 0:
                                        itemcode126 = "DG51";
                                        days152 = 30;
                                        break;
                                    case 1:
                                        itemcode126 = "CZ81";
                                        days152 = 1;
                                        break;
                                    case 2:
                                        itemcode126 = "DC80";
                                        days152 = 30;
                                        break;
                                    case 3:
                                        itemcode126 = "DC98";
                                        days152 = 30;
                                        break;
                                    case 4:
                                        itemcode126 = "DE30";
                                        days152 = 30;
                                        break;
                                    case 5:
                                        itemcode126 = "DE46";
                                        days152 = 30;
                                        break;
                                    case 6:
                                        itemcode126 = "CB09";
                                        days152 = 1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode126, days152);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode126, days152));
                                return;
                            case "CR57":
                                int num121 = Game_Server.Generic.random(0, 21);
                                int days151 = 1;
                                string itemcode128 = (string)null;
                                switch (num121)
                                {
                                    case 0:
                                        itemcode128 = "DF51";
                                        days151 = 30;
                                        break;
                                    case 1:
                                        itemcode128 = "DF51";
                                        days151 = -1;
                                        break;
                                    case 2:
                                        itemcode128 = "DJ23";
                                        days151 = -1;
                                        break;
                                    case 3:
                                        itemcode128 = "DJ23";
                                        days151 = 30;
                                        break;
                                    case 4:
                                        itemcode128 = "DC86";
                                        days151 = 30;
                                        break;
                                    case 5:
                                        itemcode128 = "DC86";
                                        days151 = -1;
                                        break;
                                    case 6:
                                        itemcode128 = "DG50";
                                        days151 = -1;
                                        break;
                                    case 7:
                                        itemcode128 = "DG50";
                                        days151 = 30;
                                        break;
                                    case 8:
                                        itemcode128 = "DB30";
                                        days151 = -1;
                                        break;
                                    case 10:
                                        itemcode128 = "DA71";
                                        days151 = 30;
                                        break;
                                    case 11:
                                        itemcode128 = "DA71";
                                        days151 = -1;
                                        break;
                                    case 12:
                                        itemcode128 = "DB38";
                                        days151 = 30;
                                        break;
                                    case 13:
                                        itemcode128 = "DB38";
                                        days151 = -1;
                                        break;
                                    case 14:
                                        itemcode128 = "DF83";
                                        days151 = -1;
                                        break;
                                    case 15:
                                        itemcode128 = "DF83";
                                        days151 = 30;
                                        break;
                                    case 16:
                                        itemcode128 = "DE56";
                                        days151 = 30;
                                        break;
                                    case 17:
                                        itemcode128 = "DE56";
                                        days151 = -1;
                                        break;
                                    case 18:
                                        itemcode128 = "DG75";
                                        days151 = 30;
                                        break;
                                    case 19:
                                        itemcode128 = "DG75";
                                        days151 = -1;
                                        break;
                                    case 20:
                                        itemcode128 = "DJ34";
                                        days151 = 30;
                                        break;
                                    case 21:
                                        itemcode128 = "DJ34";
                                        days151 = -1;
                                        break;

                                }

                                Inventory.AddItem(usr, itemcode128, days151);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode128, days151));
                                return;
                            case "CR47":
                                int num189 = Game_Server.Generic.random(0, 28);
                                int days144 = 1;
                                string itemcode112 = (string)null;
                                switch (num189)
                                {
                                    case 0:
                                        itemcode112 = "DE07";
                                        days144 = 30;
                                        break;
                                    case 1:
                                        itemcode112 = "DE07";
                                        days144 = -1;
                                        break;
                                    case 2:
                                        itemcode112 = "DC73";
                                        days144 = 30;
                                        break;
                                    case 3:
                                        itemcode112 = "DC73";
                                        days144 = -1;
                                        break;
                                    case 4:
                                        itemcode112 = "DE33";
                                        days144 = 30;
                                        break;
                                    case 5:
                                        itemcode112 = "DE33";
                                        days144 = -1;
                                        break;
                                    case 6:
                                        itemcode112 = "DE30";
                                        days144 = -1;
                                        break;
                                    case 7:
                                        itemcode112 = "DE30";
                                        days144 = -1;
                                        break;
                                    case 8:
                                        itemcode112 = "DE28";
                                        days144 = -1;
                                        break;
                                    case 9:
                                        itemcode112 = "DE28";
                                        days144 = 30;
                                        break;
                                    case 10:
                                        itemcode112 = "DG45";
                                        days144 = 30;
                                        break;
                                    case 11:
                                        itemcode112 = "DG45";
                                        days144 = -1;
                                        break;
                                    case 12:
                                        itemcode112 = "DG22";
                                        days144 = 30;
                                        break;
                                    case 13:
                                        itemcode112 = "DG22";
                                        days144 = -1;
                                        break;
                                    case 14:
                                        itemcode112 = "DG24";
                                        days144 = 7;
                                        break;
                                    case 15:
                                        itemcode112 = "DG24";
                                        days144 = 7;
                                        break;
                                    case 16:
                                        itemcode112 = "DG28";
                                        days144 = -1;
                                        break;
                                    case 17:
                                        itemcode112 = "DG28";
                                        days144 = 30;
                                        break;
                                    case 18:
                                        itemcode112 = "DG50";
                                        days144 = -1;
                                        break;
                                    case 19:
                                        itemcode112 = "DG50";
                                        days144 = 30;
                                        break;
                                    case 20:
                                        itemcode112 = "CD01";
                                        days144 = 7;
                                        break;
                                    case 21:
                                        itemcode112 = "CD02";
                                        days144 = 7;
                                        break;
                                    case 22:
                                        itemcode112 = "CD03";
                                        days144 = 7;
                                        break;
                                    case 23:
                                        itemcode112 = "CD04";
                                        days144 = 7;
                                        break;
                                    case 24:
                                        itemcode112 = "CD05";
                                        days144 = 7;
                                        break;
                                    case 25:
                                        itemcode112 = "CD06";
                                        days144 = 7;
                                        break;
                                    case 26:
                                        itemcode112 = "CD07";
                                        days144 = 7;
                                        break;
                                    case 27:
                                        itemcode112 = "CK02";
                                        days144 = 1;
                                        break;
                                    case 28:
                                        itemcode112 = "CB09";
                                        days144 = 1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode112, days144);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode112, days144));
                                return;
                            case "CY46":
                                int num87 = Game_Server.Generic.random(0, 85);
                                int days72 = 1;
                                string itemcode24 = (string)null;
                                switch (num87)
                                {
                                    case 0:
                                        itemcode24 = "GF53";
                                        days72 = 30;
                                        break;
                                    case 1:
                                        itemcode24 = "GF53";
                                        days72 = -1;
                                        break;
                                    case 2:
                                        itemcode24 = "GA09";
                                        days72 = -1;
                                        break;
                                    case 3:
                                        itemcode24 = "GA09";
                                        days72 = 30;
                                        break;
                                    case 4:
                                        itemcode24 = "GF51";
                                        days72 = 30;
                                        break;
                                    case 5:
                                        itemcode24 = "GF51";
                                        days72 = -1;
                                        break;
                                    case 6:
                                        itemcode24 = "GF37";
                                        days72 = -1;
                                        break;
                                    case 7:
                                        itemcode24 = "GF37";
                                        days72 = 30;
                                        break;
                                    case 8:
                                        itemcode24 = "DF92";
                                        days72 = -1;
                                        break;
                                    case 9:
                                        itemcode24 = "DF92";
                                        days72 = 30;
                                        break;
                                    case 10:
                                        itemcode24 = "DF86";
                                        days72 = 30;
                                        break;
                                    case 11:
                                        itemcode24 = "DF86";
                                        days72 = -1;
                                        break;
                                    case 12:
                                        itemcode24 = "DF99";
                                        days72 = 30;
                                        break;
                                    case 13:
                                        itemcode24 = "DF99";
                                        days72 = -1;
                                        break;
                                    case 14:
                                        itemcode24 = "GF24";
                                        days72 = 30;
                                        break;
                                    case 15:
                                        itemcode24 = "GF24";
                                        days72 = -1;
                                        break;
                                    case 16:
                                        itemcode24 = "GF54";
                                        days72 = 30;
                                        break;
                                    case 17:
                                        itemcode24 = "GF54";
                                        days72 = -1;
                                        break;
                                    case 18:
                                        itemcode24 = "GA10";
                                        days72 = 30;
                                        break;
                                    case 19:
                                        itemcode24 = "GA10";
                                        days72 = -1;
                                        break;
                                    case 20:
                                        itemcode24 = "GF56";
                                        days72 = 30;
                                        break;
                                    case 21:
                                        itemcode24 = "GF56";
                                        days72 = -1;
                                        break;
                                    case 22:
                                        itemcode24 = "GF19";
                                        days72 = 30;
                                        break;
                                    case 23:
                                        itemcode24 = "GF19";
                                        days72 = -1;
                                        break;
                                    case 24:
                                        itemcode24 = "GF28";
                                        days72 = 30;
                                        break;
                                    case 25:
                                        itemcode24 = "GF28";
                                        days72 = -1;
                                        break;
                                    case 26:
                                        itemcode24 = "GF40";
                                        days72 = 30;
                                        break;
                                    case 27:
                                        itemcode24 = "GF40";
                                        days72 = -1;
                                        break;
                                    case 28:
                                        itemcode24 = "GF55";
                                        days72 = 30;
                                        break;
                                    case 29:
                                        itemcode24 = "GF55";
                                        days72 = -1;
                                        break;
                                    case 30:
                                        itemcode24 = "GF56";
                                        days72 = 30;
                                        break;
                                    case 31:
                                        itemcode24 = "GF56";
                                        days72 = -1;
                                        break;
                                    case 32:
                                        itemcode24 = "GF19";
                                        days72 = 30;
                                        break;
                                    case 33:
                                        itemcode24 = "GF19";
                                        days72 = -1;
                                        break;
                                    case 34:
                                        itemcode24 = "GF28";
                                        days72 = 30;
                                        break;
                                    case 35:
                                        itemcode24 = "GF28";
                                        days72 = -1;
                                        break;
                                    case 36:
                                        itemcode24 = "GF40";
                                        days72 = 30;
                                        break;
                                    case 37:
                                        itemcode24 = "GF40";
                                        days72 = -1;
                                        break;
                                    case 38:
                                        itemcode24 = "DB75";
                                        days72 = 30;
                                        break;
                                    case 39:
                                        itemcode24 = "DB75";
                                        days72 = -1;
                                        break;
                                    case 40:
                                        itemcode24 = "DB74";
                                        days72 = 30;
                                        break;
                                    case 41:
                                        itemcode24 = "DB74";
                                        days72 = -1;
                                        break;
                                    case 42:
                                        itemcode24 = "GF48";
                                        days72 = 30;
                                        break;
                                    case 43:
                                        itemcode24 = "GF48";
                                        days72 = -1;
                                        break;
                                    case 44:
                                        itemcode24 = "GF52";
                                        days72 = 30;
                                        break;
                                    case 45:
                                        itemcode24 = "GF52";
                                        days72 = -1;
                                        break;
                                    case 46:
                                        itemcode24 = "DN05";
                                        days72 = 30;
                                        break;
                                    case 47:
                                        itemcode24 = "DN05";
                                        days72 = -1;
                                        break;
                                    case 48:
                                        itemcode24 = "DM09";
                                        days72 = 30;
                                        break;
                                    case 49:
                                        itemcode24 = "DM09";
                                        days72 = -1;
                                        break;
                                    case 50:
                                        itemcode24 = "GF57";
                                        days72 = 30;
                                        break;
                                    case 51:
                                        itemcode24 = "GF57";
                                        days72 = -1;
                                        break;
                                    case 52:
                                        itemcode24 = "DB77";
                                        days72 = -1;
                                        break;
                                    case 53:
                                        itemcode24 = "DB77";
                                        days72 = -1;
                                        break;
                                    case 54:
                                        itemcode24 = "DI22";
                                        days72 = -1;
                                        break;
                                    case 55:
                                        itemcode24 = "DI22";
                                        days72 = 30;
                                        break;
                                    case 56:
                                        itemcode24 = "GF55";
                                        days72 = -1;
                                        break;
                                    case 57:
                                        itemcode24 = "GF55";
                                        days72 = 30;
                                        break;
                                    case 58:
                                        itemcode24 = "DD36";
                                        days72 = -1;
                                        break;
                                    case 59:
                                        itemcode24 = "DD36";
                                        days72 = 30;
                                        break;
                                    case 60:
                                        itemcode24 = "DF42";
                                        days72 = -1;
                                        break;
                                    case 61:
                                        itemcode24 = "DF42";
                                        days72 = 30;
                                        break;
                                    case 62:
                                        itemcode24 = "DB15";
                                        days72 = 30;
                                        break;
                                    case 63:
                                        itemcode24 = "DB15";
                                        days72 = -1;
                                        break;
                                    case 64:
                                        itemcode24 = "GF46";
                                        days72 = 30;
                                        break;
                                    case 65:
                                        itemcode24 = "GF46";
                                        days72 = -1;
                                        break;
                                    case 66:
                                        itemcode24 = "GF74";
                                        days72 = 30;
                                        break;
                                    case 67:
                                        itemcode24 = "GF74";
                                        days72 = -1;
                                        break;
                                    case 68:
                                        itemcode24 = "GF27";
                                        days72 = 30;
                                        break;
                                    case 69:
                                        itemcode24 = "GF27";
                                        days72 = -1;
                                        break;
                                    case 70:
                                        itemcode24 = "GF06";
                                        days72 = 30;
                                        break;
                                    case 71:
                                        itemcode24 = "GF06";
                                        days72 = -1;
                                        break;
                                    case 72:
                                        itemcode24 = "DD20";
                                        days72 = 30;
                                        break;
                                    case 73:
                                        itemcode24 = "DD20";
                                        days72 = -1;
                                        break;
                                    case 74:
                                        itemcode24 = "DF92";
                                        days72 = 30;
                                        break;
                                    case 75:
                                        itemcode24 = "DF92";
                                        days72 = -1;
                                        break;
                                    case 76:
                                        itemcode24 = "GF67";
                                        days72 = 30;
                                        break;
                                    case 77:
                                        itemcode24 = "GF67";
                                        days72 = -1;
                                        break;
                                    case 78:
                                        itemcode24 = "GF85";
                                        days72 = 30;
                                        break;
                                    case 79:
                                        itemcode24 = "GF85";
                                        days72 = -1;
                                        break;
                                    case 80:
                                        itemcode24 = "GF57";
                                        days72 = 30;
                                        break;
                                    case 81:
                                        itemcode24 = "GF57";
                                        days72 = -1;
                                        break;
                                    case 82:
                                        itemcode24 = "GA23";
                                        days72 = 30;
                                        break;
                                    case 83:
                                        itemcode24 = "GA23";
                                        days72 = -1;
                                        break;
                                    case 84:
                                        itemcode24 = "DD39";
                                        days72 = 30;
                                        break;
                                    case 85:
                                        itemcode24 = "DD39";
                                        days72 = -1;
                                        break;



                                }
                                Inventory.AddItem(usr, itemcode24, days72);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode24, days72));
                                return;
                            case "CY93":
                                int num903 = Game_Server.Generic.random(0, 23);
                                int days733 = 1;
                                string itemcode253 = (string)null;
                                switch (num903)
                                {
                                    case 0:
                                        itemcode253 = "BK17";
                                        days733 = 30;
                                        break;
                                    case 1:
                                        itemcode253 = "BK17";
                                        days733 = 60;
                                        break;
                                    case 2:
                                        itemcode253 = "BK17";
                                        days733 = 90;
                                        break;
                                    case 3:
                                        itemcode253 = "BK17";
                                        days733 = 365;
                                        break;
                                    case 4:
                                        itemcode253 = "BK16";
                                        days733 = 30;
                                        break;
                                    case 5:
                                        itemcode253 = "BK16";
                                        days733 = 60;
                                        break;
                                    case 6:
                                        itemcode253 = "BK16";
                                        days733 = 90;
                                        break;
                                    case 7:
                                        itemcode253 = "BK16";
                                        days733 = 365;
                                        break;
                                    case 8:
                                        itemcode253 = "BK13";
                                        days733 = 30;
                                        break;
                                    case 9:
                                        itemcode253 = "BK13";
                                        days733 = 60;
                                        break;
                                    case 10:
                                        itemcode253 = "BK13";
                                        days733 = 90;
                                        break;
                                    case 11:
                                        itemcode253 = "BK13";
                                        days733 = 365;
                                        break;
                                    case 12:
                                        itemcode253 = "BK12";
                                        days733 = 30;
                                        break;
                                    case 13:
                                        itemcode253 = "BK12";
                                        days733 = 60;
                                        break;
                                    case 14:
                                        itemcode253 = "BK12";
                                        days733 = 90;
                                        break;
                                    case 15:
                                        itemcode253 = "BK12";
                                        days733 = 365;
                                        break;
                                    case 16:
                                        itemcode253 = "BK11";
                                        days733 = 30;
                                        break;
                                    case 17:
                                        itemcode253 = "BK11";
                                        days733 = 60;
                                        break;
                                    case 18:
                                        itemcode253 = "BK11";
                                        days733 = 90;
                                        break;
                                    case 19:
                                        itemcode253 = "BK11";
                                        days733 = 365;
                                        break;
                                    case 20:
                                        itemcode253 = "BI16";
                                        days733 = 30;
                                        break;
                                    case 21:
                                        itemcode253 = "BI16";
                                        days733 = 60;
                                        break;
                                    case 22:
                                        itemcode253 = "BI16";
                                        days733 = 90;
                                        break;
                                    case 23:
                                        itemcode253 = "BI16";
                                        days733 = 365;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode253, days733);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode253, days733));
                                return;
                            case "CY94":
                                int num902 = Game_Server.Generic.random(0, 51);
                                int days732 = 1;
                                string itemcode252 = (string)null;
                                switch (num902)
                                {
                                    case 0:
                                        itemcode252 = "BY13";
                                        days732 = 30;
                                        break;
                                    case 1:
                                        itemcode252 = "BY13";
                                        days732 = 60;
                                        break;
                                    case 2:
                                        itemcode252 = "BY13";
                                        days732 = 90;
                                        break;
                                    case 3:
                                        itemcode252 = "BY13";
                                        days732 = 365;
                                        break;
                                    case 4:
                                        itemcode252 = "BY12";
                                        days732 = 30;
                                        break;
                                    case 5:
                                        itemcode252 = "BY12";
                                        days732 = 60;
                                        break;
                                    case 6:
                                        itemcode252 = "BY12";
                                        days732 = 90;
                                        break;
                                    case 7:
                                        itemcode252 = "BY12";
                                        days732 = 365;
                                        break;
                                    case 8:
                                        itemcode252 = "BY11";
                                        days732 = 30;
                                        break;
                                    case 9:
                                        itemcode252 = "BY11";
                                        days732 = 60;
                                        break;
                                    case 10:
                                        itemcode252 = "BY11";
                                        days732 = 90;
                                        break;
                                    case 11:
                                        itemcode252 = "BY11";
                                        days732 = 365;
                                        break;
                                    case 12:
                                        itemcode252 = "BY10";
                                        days732 = 30;
                                        break;
                                    case 13:
                                        itemcode252 = "BY10";
                                        days732 = 60;
                                        break;
                                    case 14:
                                        itemcode252 = "BY10";
                                        days732 = 90;
                                        break;
                                    case 15:
                                        itemcode252 = "BY10";
                                        days732 = 365;
                                        break;
                                    case 16:
                                        itemcode252 = "BY09";
                                        days732 = 30;
                                        break;
                                    case 17:
                                        itemcode252 = "BY09";
                                        days732 = 60;
                                        break;
                                    case 18:
                                        itemcode252 = "BY09";
                                        days732 = 90;
                                        break;
                                    case 19:
                                        itemcode252 = "BY09";
                                        days732 = 365;
                                        break;
                                    case 20:
                                        itemcode252 = "BY08";
                                        days732 = 30;
                                        break;
                                    case 21:
                                        itemcode252 = "BY08";
                                        days732 = 60;
                                        break;
                                    case 22:
                                        itemcode252 = "BY08";
                                        days732 = 90;
                                        break;
                                    case 23:
                                        itemcode252 = "BY08";
                                        days732 = 365;
                                        break;
                                    case 24:
                                        itemcode252 = "BY07";
                                        days732 = 30;
                                        break;
                                    case 25:
                                        itemcode252 = "BY07";
                                        days732 = 60;
                                        break;
                                    case 26:
                                        itemcode252 = "BY07";
                                        days732 = 90;
                                        break;
                                    case 27:
                                        itemcode252 = "BY07";
                                        days732 = 365;
                                        break;

                                    case 28:
                                        itemcode252 = "BY06";
                                        days732 = 30;
                                        break;
                                    case 29:
                                        itemcode252 = "BY06";
                                        days732 = 60;
                                        break;
                                    case 30:
                                        itemcode252 = "BY06";
                                        days732 = 90;
                                        break;
                                    case 31:
                                        itemcode252 = "BY06";
                                        days732 = 365;
                                        break;
                                    case 32:
                                        itemcode252 = "BY05";
                                        days732 = 30;
                                        break;
                                    case 33:
                                        itemcode252 = "BY05";
                                        days732 = 60;
                                        break;
                                    case 34:
                                        itemcode252 = "BY05";
                                        days732 = 90;
                                        break;
                                    case 35:
                                        itemcode252 = "BY05";
                                        days732 = 365;
                                        break;
                                    case 36:
                                        itemcode252 = "BY04";
                                        days732 = 30;
                                        break;
                                    case 37:
                                        itemcode252 = "BY04";
                                        days732 = 60;
                                        break;
                                    case 38:
                                        itemcode252 = "BY04";
                                        days732 = 90;
                                        break;
                                    case 39:
                                        itemcode252 = "BY04";
                                        days732 = 365;
                                        break;
                                    case 40:
                                        itemcode252 = "BY03";
                                        days732 = 30;
                                        break;
                                    case 41:
                                        itemcode252 = "BY03";
                                        days732 = 60;
                                        break;
                                    case 42:
                                        itemcode252 = "BY03";
                                        days732 = 90;
                                        break;
                                    case 43:
                                        itemcode252 = "BY03";
                                        days732 = 365;
                                        break;
                                    case 44:
                                        itemcode252 = "BY02";
                                        days732 = 30;
                                        break;
                                    case 45:
                                        itemcode252 = "BY02";
                                        days732 = 60;
                                        break;
                                    case 46:
                                        itemcode252 = "BY02";
                                        days732 = 90;
                                        break;
                                    case 47:
                                        itemcode252 = "BY02";
                                        days732 = 365;
                                        break;
                                    case 48:
                                        itemcode252 = "BY01";
                                        days732 = 30;
                                        break;
                                    case 49:
                                        itemcode252 = "BY01";
                                        days732 = 60;
                                        break;
                                    case 50:
                                        itemcode252 = "BY01";
                                        days732 = 90;
                                        break;
                                    case 51:
                                        itemcode252 = "BY01";
                                        days732 = 365;
                                        break;
                                }
                                Inventory.AddCostume(usr, itemcode252, days732);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode252, days732));
                                return;
                            case "CY95":
                                int num901 = Game_Server.Generic.random(0, 35);
                                int days731 = 1;
                                string itemcode251 = (string)null;
                                switch (num901)
                                {
                                    case 0:
                                        itemcode251 = "BP22";
                                        days731 = 30;
                                        break;
                                    case 1:
                                        itemcode251 = "BP22";
                                        days731 = 60;
                                        break;
                                    case 2:
                                        itemcode251 = "BP22";
                                        days731 = 90;
                                        break;
                                    case 3:
                                        itemcode251 = "BP22";
                                        days731 = 365;
                                        break;
                                    case 4:
                                        itemcode251 = "BP21";
                                        days731 = 30;
                                        break;
                                    case 5:
                                        itemcode251 = "BP21";
                                        days731 = 60;
                                        break;
                                    case 6:
                                        itemcode251 = "BP21";
                                        days731 = 90;
                                        break;
                                    case 7:
                                        itemcode251 = "BP21";
                                        days731 = 365;
                                        break;
                                    case 8:
                                        itemcode251 = "BP20";
                                        days731 = 30;
                                        break;
                                    case 9:
                                        itemcode251 = "BP20";
                                        days731 = 60;
                                        break;
                                    case 10:
                                        itemcode251 = "BP20";
                                        days731 = 90;
                                        break;
                                    case 11:
                                        itemcode251 = "BP20";
                                        days731 = 365;
                                        break;
                                    case 12:
                                        itemcode251 = "BP19";
                                        days731 = 30;
                                        break;
                                    case 13:
                                        itemcode251 = "BP19";
                                        days731 = 60;
                                        break;
                                    case 14:
                                        itemcode251 = "BP19";
                                        days731 = 90;
                                        break;
                                    case 15:
                                        itemcode251 = "BP19";
                                        days731 = 365;
                                        break;
                                    case 16:
                                        itemcode251 = "BP18";
                                        days731 = 30;
                                        break;
                                    case 17:
                                        itemcode251 = "BP18";
                                        days731 = 60;
                                        break;
                                    case 18:
                                        itemcode251 = "BP18";
                                        days731 = 90;
                                        break;
                                    case 19:
                                        itemcode251 = "BP18";
                                        days731 = 365;
                                        break;
                                    case 20:
                                        itemcode251 = "BP17";
                                        days731 = 30;
                                        break;
                                    case 21:
                                        itemcode251 = "BP17";
                                        days731 = 60;
                                        break;
                                    case 22:
                                        itemcode251 = "BP17";
                                        days731 = 90;
                                        break;
                                    case 23:
                                        itemcode251 = "BP12";
                                        days731 = 365;
                                        break;
                                    case 24:
                                        itemcode251 = "BP12";
                                        days731 = 30;
                                        break;
                                    case 25:
                                        itemcode251 = "BP12";
                                        days731 = 60;
                                        break;
                                    case 26:
                                        itemcode251 = "BP12";
                                        days731 = 90;
                                        break;
                                    case 27:
                                        itemcode251 = "BP12";
                                        days731 = 365;
                                        break;
                                    case 28:
                                        itemcode251 = "BP08";
                                        days731 = 30;
                                        break;
                                    case 29:
                                        itemcode251 = "BP08";
                                        days731 = 60;
                                        break;
                                    case 30:
                                        itemcode251 = "BP08";
                                        days731 = 90;
                                        break;
                                    case 31:
                                        itemcode251 = "BP08";
                                        days731 = 365;
                                        break;
                                    case 32:
                                        itemcode251 = "BP07";
                                        days731 = 30;
                                        break;
                                    case 33:
                                        itemcode251 = "BP07";
                                        days731 = 60;
                                        break;
                                    case 34:
                                        itemcode251 = "BP07";
                                        days731 = 90;
                                        break;
                                    case 35:
                                        itemcode251 = "BP07";
                                        days731 = 365;
                                        break;

                                }
                                Inventory.AddCostume(usr, itemcode251, days731);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode251, days731));
                                return;
                            case "CY49":
                                int num88 = Game_Server.Generic.random(0, 75);
                                int days73 = 1;
                                string itemcode25 = (string)null;
                                switch (num88)
                                {
                                    case 0:
                                        itemcode25 = "DT07";
                                        days73 = 30;
                                        break;
                                    case 1:
                                        itemcode25 = "DT07";
                                        days73 = -1;
                                        break;
                                    case 2:
                                        itemcode25 = "GA09";
                                        days73 = -1;
                                        break;
                                    case 3:
                                        itemcode25 = "GA09";
                                        days73 = 30;
                                        break;
                                    case 4:
                                        itemcode25 = "DJ78";
                                        days73 = 30;
                                        break;
                                    case 5:
                                        itemcode25 = "DJ78";
                                        days73 = -1;
                                        break;
                                    case 6:
                                        itemcode25 = "DH18";
                                        days73 = -1;
                                        break;
                                    case 7:
                                        itemcode25 = "DH18";
                                        days73 = 30;
                                        break;
                                    case 8:
                                        itemcode25 = "DJ41";
                                        days73 = -1;
                                        break;
                                    case 9:
                                        itemcode25 = "DJ41";
                                        days73 = 30;
                                        break;
                                    case 10:
                                        itemcode25 = "DJ37";
                                        days73 = 30;
                                        break;
                                    case 11:
                                        itemcode25 = "DJ37";
                                        days73 = -1;
                                        break;
                                    case 12:
                                        itemcode25 = "DJ30";
                                        days73 = 30;
                                        break;
                                    case 13:
                                        itemcode25 = "DJ30";
                                        days73 = -1;
                                        break;
                                    case 14:
                                        itemcode25 = "DJ44";
                                        days73 = 30;
                                        break;
                                    case 15:
                                        itemcode25 = "DJ44";
                                        days73 = -1;
                                        break;
                                    case 16:
                                        itemcode25 = "DT40";
                                        days73 = 30;
                                        break;
                                    case 17:
                                        itemcode25 = "DT40";
                                        days73 = -1;
                                        break;
                                    case 18:
                                        itemcode25 = "GA10";
                                        days73 = 30;
                                        break;
                                    case 19:
                                        itemcode25 = "GA10";
                                        days73 = -1;
                                        break;
                                    case 20:
                                        itemcode25 = "DH19";
                                        days73 = 30;
                                        break;
                                    case 21:
                                        itemcode25 = "DH19";
                                        days73 = -1;
                                        break;
                                    case 22:
                                        itemcode25 = "DH10";
                                        days73 = 30;
                                        break;
                                    case 23:
                                        itemcode25 = "DH10";
                                        days73 = -1;
                                        break;
                                    case 24:
                                        itemcode25 = "DT38";
                                        days73 = 30;
                                        break;
                                    case 25:
                                        itemcode25 = "DT38";
                                        days73 = -1;
                                        break;
                                    case 26:
                                        itemcode25 = "DJ17";
                                        days73 = 30;
                                        break;
                                    case 27:
                                        itemcode25 = "DJ17";
                                        days73 = -1;
                                        break;
                                    case 28:
                                        itemcode25 = "DT41";
                                        days73 = 30;
                                        break;
                                    case 29:
                                        itemcode25 = "DT41";
                                        days73 = -1;
                                        break;
                                    case 30:
                                        itemcode25 = "DT22";
                                        days73 = 30;
                                        break;
                                    case 31:
                                        itemcode25 = "DT22";
                                        days73 = -1;
                                        break;
                                    case 32:
                                        itemcode25 = "DJ73";
                                        days73 = 30;
                                        break;
                                    case 33:
                                        itemcode25 = "DJ73";
                                        days73 = -1;
                                        break;
                                    case 34:
                                        itemcode25 = "DJ47";
                                        days73 = 30;
                                        break;
                                    case 35:
                                        itemcode25 = "DJ47";
                                        days73 = -1;
                                        break;
                                    case 36:
                                        itemcode25 = "D913";
                                        days73 = 30;
                                        break;
                                    case 37:
                                        itemcode25 = "D913";
                                        days73 = -1;
                                        break;
                                    case 38:
                                        itemcode25 = "DH19";
                                        days73 = 30;
                                        break;
                                    case 39:
                                        itemcode25 = "DH19";
                                        days73 = -1;
                                        break;
                                    case 40:
                                        itemcode25 = "DB74";
                                        days73 = 30;
                                        break;
                                    case 41:
                                        itemcode25 = "DB74";
                                        days73 = -1;
                                        break;
                                    case 42:
                                        itemcode25 = "DM05";
                                        days73 = 30;
                                        break;
                                    case 43:
                                        itemcode25 = "DM05";
                                        days73 = -1;
                                        break;
                                    case 44:
                                        itemcode25 = "DN05";
                                        days73 = 30;
                                        break;
                                    case 45:
                                        itemcode25 = "DN05";
                                        days73 = -1;
                                        break;
                                    case 46:
                                        itemcode25 = "DB77";
                                        days73 = 30;
                                        break;
                                    case 47:
                                        itemcode25 = "DB77";
                                        days73 = -1;
                                        break;
                                    case 48:
                                        itemcode25 = "DK04";
                                        days73 = -1;
                                        break;
                                    case 49:
                                        itemcode25 = "DK04";
                                        days73 = -1;
                                        break;
                                    case 50:
                                        itemcode25 = "GJ05";
                                        days73 = -1;
                                        break;
                                    case 51:
                                        itemcode25 = "GJ05";
                                        days73 = 30;
                                        break;
                                    case 52:
                                        itemcode25 = "DJ94";
                                        days73 = -1;
                                        break;
                                    case 53:
                                        itemcode25 = "DJ94";
                                        days73 = 30;
                                        break;
                                    case 54:
                                        itemcode25 = "DH19";
                                        days73 = -1;
                                        break;
                                    case 55:
                                        itemcode25 = "DH19";
                                        days73 = 30;
                                        break;
                                    case 56:
                                        itemcode25 = "DJ86";
                                        days73 = -1;
                                        break;
                                    case 57:
                                        itemcode25 = "DJ86";
                                        days73 = 30;
                                        break;
                                    case 58:
                                        itemcode25 = "GA10";
                                        days73 = -1;
                                        break;
                                    case 59:
                                        itemcode25 = "GA10";
                                        days73 = 30;
                                        break;
                                    case 60:
                                        itemcode25 = "DT52";
                                        days73 = -1;
                                        break;
                                    case 61:
                                        itemcode25 = "DT52";
                                        days73 = 30;
                                        break;
                                    case 62:
                                        itemcode25 = "DJ89";
                                        days73 = -1;
                                        break;
                                    case 63:
                                        itemcode25 = "DJ89";
                                        days73 = 30;
                                        break;
                                    case 64:
                                        itemcode25 = "DJ70";
                                        days73 = -1;
                                        break;
                                    case 65:
                                        itemcode25 = "DJ70";
                                        days73 = 30;
                                        break;
                                    case 66:
                                        itemcode25 = "DJ46";
                                        days73 = -1;
                                        break;
                                    case 67:
                                        itemcode25 = "DJ46";
                                        days73 = 30;
                                        break;
                                    case 68:
                                        itemcode25 = "DJ84";
                                        days73 = -1;
                                        break;
                                    case 69:
                                        itemcode25 = "DJ84";
                                        days73 = 30;
                                        break;
                                    case 70:
                                        itemcode25 = "DT50";
                                        days73 = -1;
                                        break;
                                    case 71:
                                        itemcode25 = "DT50";
                                        days73 = 30;
                                        break;
                                    case 72:
                                        itemcode25 = "GA23";
                                        days73 = -1;
                                        break;
                                    case 73:
                                        itemcode25 = "GA23";
                                        days73 = 30;
                                        break;
                                    case 74:
                                        itemcode25 = "DJ77";
                                        days73 = -1;
                                        break;
                                    case 75:
                                        itemcode25 = "DJ77";
                                        days73 = 30;
                                        break;




                                }
                                Inventory.AddItem(usr, itemcode25, days73);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode25, days73));
                                return;
                            case "CY79":
                                int num924 = Game_Server.Generic.random(0, 50);
                                int days75 = 1;
                                string itemcode410 = (string)null;
                                switch (num924)
                                {
                                    case 0:
                                        itemcode410 = "DC01";
                                        days75 = -1;
                                        break;
                                    case 1:
                                        itemcode410 = "DC01";
                                        days75 = 30;
                                        break;
                                    case 2:
                                        itemcode410 = "DC01";
                                        days75 = 15;
                                        break;
                                    case 3:
                                        itemcode410 = "DC31";
                                        days75 = -1;
                                        break;
                                    case 4:
                                        itemcode410 = "DC31";
                                        days75 = 30;
                                        break;
                                    case 5:
                                        itemcode410 = "DC31";
                                        days75 = 15;
                                        break;
                                    case 6:
                                        itemcode410 = "DC65";
                                        days75 = -1;
                                        break;
                                    case 7:
                                        itemcode410 = "DC65";
                                        days75 = 30;
                                        break;
                                    case 8:
                                        itemcode410 = "DC65";
                                        days75 = 15;
                                        break;
                                    case 9:
                                        itemcode410 = "DC91";
                                        days75 = -1;
                                        break;
                                    case 10:
                                        itemcode410 = "DC91";
                                        days75 = 30;
                                        break;
                                    case 11:
                                        itemcode410 = "DC91";
                                        days75 = 15;
                                        break;
                                    case 12:
                                        itemcode410 = "DC70";
                                        days75 = -1;
                                        break;
                                    case 13:
                                        itemcode410 = "DC70";
                                        days75 = 30;
                                        break;
                                    case 14:
                                        itemcode410 = "DC70";
                                        days75 = 15;
                                        break;
                                    case 15:
                                        itemcode410 = "DC72";
                                        days75 = -1;
                                        break;
                                    case 16:
                                        itemcode410 = "DC72";
                                        days75 = 30;
                                        break;
                                    case 17:
                                        itemcode410 = "DC72";
                                        days75 = 15;
                                        break;
                                    case 18:
                                        itemcode410 = "DC84";
                                        days75 = -1;
                                        break;
                                    case 19:
                                        itemcode410 = "DC84";
                                        days75 = 30;
                                        break;
                                    case 20:
                                        itemcode410 = "DC84";
                                        days75 = 15;
                                        break;
                                    case 21:
                                        itemcode410 = "DC85";
                                        days75 = -1;
                                        break;
                                    case 22:
                                        itemcode410 = "DC85";
                                        days75 = 30;
                                        break;
                                    case 23:
                                        itemcode410 = "DC85";
                                        days75 = 15;
                                        break;
                                    case 24:
                                        itemcode410 = "DE52";
                                        days75 = -1;
                                        break;
                                    case 25:
                                        itemcode410 = "DE52";
                                        days75 = 30;
                                        break;
                                    case 26:
                                        itemcode410 = "DE52";
                                        days75 = 15;
                                        break;
                                    case 27:
                                        itemcode410 = "DE70";
                                        days75 = -1;
                                        break;
                                    case 28:
                                        itemcode410 = "DE70";
                                        days75 = 30;
                                        break;
                                    case 29:
                                        itemcode410 = "DE70";
                                        days75 = 15;
                                        break;
                                    case 30:
                                        itemcode410 = "DE73";
                                        days75 = -1;
                                        break;
                                    case 31:
                                        itemcode410 = "DE73";
                                        days75 = 30;
                                        break;
                                    case 32:
                                        itemcode410 = "DE73";
                                        days75 = 15;
                                        break;
                                    case 33:
                                        itemcode410 = "DE74";
                                        days75 = -1;
                                        break;
                                    case 34:
                                        itemcode410 = "DE74";
                                        days75 = 30;
                                        break;
                                    case 35:
                                        itemcode410 = "DE74";
                                        days75 = 15;
                                        break;
                                    case 36:
                                        itemcode410 = "DE80";
                                        days75 = -1;
                                        break;
                                    case 37:
                                        itemcode410 = "DE80";
                                        days75 = 30;
                                        break;
                                    case 38:
                                        itemcode410 = "DE80";
                                        days75 = 15;
                                        break;
                                    case 39:
                                        itemcode410 = "GC04";
                                        days75 = -1;
                                        break;
                                    case 40:
                                        itemcode410 = "GC04";
                                        days75 = 30;
                                        break;
                                    case 41:
                                        itemcode410 = "GC04";
                                        days75 = 15;
                                        break;
                                    case 42:
                                        itemcode410 = "GC11";
                                        days75 = -1;
                                        break;
                                    case 43:
                                        itemcode410 = "GC11";
                                        days75 = 30;
                                        break;
                                    case 44:
                                        itemcode410 = "GC11";
                                        days75 = 15;
                                        break;
                                    case 45:
                                        itemcode410 = "GC15";
                                        days75 = -1;
                                        break;
                                    case 46:
                                        itemcode410 = "GC15";
                                        days75 = 30;
                                        break;
                                    case 47:
                                        itemcode410 = "GC15";
                                        days75 = 15;
                                        break;
                                    case 48:
                                        itemcode410 = "GC21";
                                        days75 = -1;
                                        break;
                                    case 49:
                                        itemcode410 = "GC21";
                                        days75 = 30;
                                        break;
                                    case 50:
                                        itemcode410 = "GC21";
                                        days75 = 15;
                                        break;


                                }
                                Inventory.AddItem(usr, itemcode410, days75);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode410, days75));
                                return;
                            case "CY47":
                                int num921 = Game_Server.Generic.random(0, 72);
                                int days74 = 1;
                                string itemcode28 = (string)null;
                                switch (num921)
                                {
                                    case 0:
                                        itemcode28 = "GG45";
                                        days74 = 30;
                                        break;
                                    case 1:
                                        itemcode28 = "GG45";
                                        days74 = -1;
                                        break;
                                    case 2:
                                        itemcode28 = "GA09";
                                        days74 = -1;
                                        break;
                                    case 3:
                                        itemcode28 = "GA09";
                                        days74 = 30;
                                        break;
                                    case 4:
                                        itemcode28 = "GG43";
                                        days74 = 30;
                                        break;
                                    case 5:
                                        itemcode28 = "GG43";
                                        days74 = -1;
                                        break;
                                    case 6:
                                        itemcode28 = "GG29";
                                        days74 = -1;
                                        break;
                                    case 7:
                                        itemcode28 = "GG29";
                                        days74 = 30;
                                        break;
                                    case 8:
                                        itemcode28 = "DG83";
                                        days74 = -1;
                                        break;
                                    case 9:
                                        itemcode28 = "DG83";
                                        days74 = 30;
                                        break;
                                    case 10:
                                        itemcode28 = "DG79";
                                        days74 = 30;
                                        break;
                                    case 11:
                                        itemcode28 = "DG79";
                                        days74 = -1;
                                        break;
                                    case 12:
                                        itemcode28 = "DG70";
                                        days74 = 30;
                                        break;
                                    case 13:
                                        itemcode28 = "DG70";
                                        days74 = -1;
                                        break;
                                    case 14:
                                        itemcode28 = "DG88";
                                        days74 = 30;
                                        break;
                                    case 15:
                                        itemcode28 = "DG88";
                                        days74 = -1;
                                        break;
                                    case 16:
                                        itemcode28 = "GG15";
                                        days74 = 30;
                                        break;
                                    case 17:
                                        itemcode28 = "GG15";
                                        days74 = -1;
                                        break;
                                    case 18:
                                        itemcode28 = "GG46";
                                        days74 = 30;
                                        break;
                                    case 19:
                                        itemcode28 = "GG46";
                                        days74 = -1;
                                        break;
                                    case 20:
                                        itemcode28 = "GA10";
                                        days74 = 30;
                                        break;
                                    case 21:
                                        itemcode28 = "GA10";
                                        days74 = -1;
                                        break;
                                    case 22:
                                        itemcode28 = "GG08";
                                        days74 = 30;
                                        break;
                                    case 23:
                                        itemcode28 = "GG08";
                                        days74 = -1;
                                        break;
                                    case 24:
                                        itemcode28 = "GG49";
                                        days74 = 30;
                                        break;
                                    case 25:
                                        itemcode28 = "GG49";
                                        days74 = -1;
                                        break;
                                    case 26:
                                        itemcode28 = "DB76";
                                        days74 = 30;
                                        break;
                                    case 27:
                                        itemcode28 = "DB76";
                                        days74 = -1;
                                        break;
                                    case 28:
                                        itemcode28 = "GG47";
                                        days74 = 30;
                                        break;
                                    case 29:
                                        itemcode28 = "GG47";
                                        days74 = -1;
                                        break;
                                    case 30:
                                        itemcode28 = "DB75";
                                        days74 = 30;
                                        break;
                                    case 31:
                                        itemcode28 = "DB75";
                                        days74 = -1;
                                        break;
                                    case 32:
                                        itemcode28 = "DB74";
                                        days74 = 30;
                                        break;
                                    case 33:
                                        itemcode28 = "DB74";
                                        days74 = -1;
                                        break;
                                    case 34:
                                        itemcode28 = "GG40";
                                        days74 = 30;
                                        break;
                                    case 35:
                                        itemcode28 = "GG40";
                                        days74 = -1;
                                        break;
                                    case 36:
                                        itemcode28 = "DM05";
                                        days74 = 30;
                                        break;
                                    case 37:
                                        itemcode28 = "DM05";
                                        days74 = -1;
                                        break;
                                    case 38:
                                        itemcode28 = "DN05";
                                        days74 = 30;
                                        break;
                                    case 39:
                                        itemcode28 = "DN05";
                                        days74 = -1;
                                        break;
                                    case 40:
                                        itemcode28 = "DB77";
                                        days74 = 30;
                                        break;
                                    case 41:
                                        itemcode28 = "DB77";
                                        days74 = -1;
                                        break;
                                    case 42:
                                        itemcode28 = "GG50";
                                        days74 = -1;
                                        break;
                                    case 43:
                                        itemcode28 = "GG50";
                                        days74 = 30;
                                        break;
                                    case 44:
                                        itemcode28 = "GG86";
                                        days74 = -1;
                                        break;
                                    case 45:
                                        itemcode28 = "GG86";
                                        days74 = 30;
                                        break;
                                    case 46:
                                        itemcode28 = "GG73";
                                        days74 = -1;
                                        break;
                                    case 47:
                                        itemcode28 = "GG73";
                                        days74 = 30;
                                        break;
                                    case 48:
                                        itemcode28 = "GG47";
                                        days74 = -1;
                                        break;
                                    case 49:
                                        itemcode28 = "GG47";
                                        days74 = 30;
                                        break;
                                    case 50:
                                        itemcode28 = "GG51";
                                        days74 = -1;
                                        break;
                                    case 51:
                                        itemcode28 = "GG51";
                                        days74 = 30;
                                        break;
                                    case 52:
                                        itemcode28 = "GG38";
                                        days74 = -1;
                                        break;
                                    case 53:
                                        itemcode28 = "GG38";
                                        days74 = 30;
                                        break;
                                    case 54:
                                        itemcode28 = "GG88";
                                        days74 = -1;
                                        break;
                                    case 55:
                                        itemcode28 = "GG88";
                                        days74 = 30;
                                        break;
                                    case 56:
                                        itemcode28 = "GG67";
                                        days74 = -1;
                                        break;
                                    case 57:
                                        itemcode28 = "GG67";
                                        days74 = 30;
                                        break;
                                    case 58:
                                        itemcode28 = "GG18";
                                        days74 = -1;
                                        break;
                                    case 59:
                                        itemcode28 = "GG18";
                                        days74 = 30;
                                        break;
                                    case 60:
                                        itemcode28 = "DG93";
                                        days74 = -1;
                                        break;
                                    case 61:
                                        itemcode28 = "DG93";
                                        days74 = 30;
                                        break;
                                    case 62:
                                        itemcode28 = "GG60";
                                        days74 = -1;
                                        break;
                                    case 63:
                                        itemcode28 = "GG60";
                                        days74 = 30;
                                        break;
                                    case 64:
                                        itemcode28 = "GG81";
                                        days74 = -1;
                                        break;
                                    case 65:
                                        itemcode28 = "GG81";
                                        days74 = 30;
                                        break;
                                    case 67:
                                        itemcode28 = "GG50";
                                        days74 = -1;
                                        break;
                                    case 68:
                                        itemcode28 = "GG50";
                                        days74 = 30;
                                        break;
                                    case 69:
                                        itemcode28 = "DG40";
                                        days74 = -1;
                                        break;
                                    case 70:
                                        itemcode28 = "DG40";
                                        days74 = 30;
                                        break;
                                    case 71:
                                        itemcode28 = "GA23";
                                        days74 = -1;
                                        break;
                                    case 72:
                                        itemcode28 = "GA23";
                                        days74 = -1;
                                        break;

                                }
                                Inventory.AddItem(usr, itemcode28, days74);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode28, days74));
                                return;
                            case "CY78":
                                int num400 = Game_Server.Generic.random(0, 39);
                                int days126 = 1;
                                string itemcode139 = (string)null;
                                switch (num400)
                                {

                                    case 0:
                                        itemcode139 = "DF70";
                                        days126 = -1;
                                        break;
                                    case 1:
                                        itemcode139 = "DF70";
                                        days126 = 30;
                                        break;
                                    case 2:
                                        itemcode139 = "DF70";
                                        days126 = 15;
                                        break;
                                    case 3:
                                        itemcode139 = "DF87";
                                        days126 = -1;
                                        break;
                                    case 4:
                                        itemcode139 = "DF87";
                                        days126 = 30;
                                        break;
                                    case 5:
                                        itemcode139 = "DF87";
                                        days126 = 15;
                                        break;
                                    case 6:
                                        itemcode139 = "D607";
                                        days126 = -1;
                                        break;
                                    case 7:
                                        itemcode139 = "D607";
                                        days126 = 30;
                                        break;
                                    case 8:
                                        itemcode139 = "D607";
                                        days126 = 15;
                                        break;
                                    case 9:
                                        itemcode139 = "GF02";
                                        days126 = -1;
                                        break;
                                    case 10:
                                        itemcode139 = "GF02";
                                        days126 = 30;
                                        break;
                                    case 11:
                                        itemcode139 = "GF02";
                                        days126 = 15;
                                        break;
                                    case 12:
                                        itemcode139 = "GF06";
                                        days126 = -1;
                                        break;
                                    case 13:
                                        itemcode139 = "GF06";
                                        days126 = 30;
                                        break;
                                    case 14:
                                        itemcode139 = "GF06";
                                        days126 = 15;
                                        break;
                                    case 15:
                                        itemcode139 = "D609";
                                        days126 = -1;
                                        break;
                                    case 16:
                                        itemcode139 = "D609";
                                        days126 = 30;
                                        break;
                                    case 17:
                                        itemcode139 = "D609";
                                        days126 = 15;
                                        break;
                                    case 18:
                                        itemcode139 = "GF09";
                                        days126 = -1;
                                        break;
                                    case 19:
                                        itemcode139 = "GF09";
                                        days126 = 30;
                                        break;
                                    case 20:
                                        itemcode139 = "GF09";
                                        days126 = 15;
                                        break;
                                    case 21:
                                        itemcode139 = "GF12";
                                        days126 = -1;
                                        break;
                                    case 22:
                                        itemcode139 = "GF12";
                                        days126 = 30;
                                        break;
                                    case 23:
                                        itemcode139 = "GF12";
                                        days126 = 15;
                                        break;
                                    case 24:
                                        itemcode139 = "GF17";
                                        days126 = -1;
                                        break;
                                    case 25:
                                        itemcode139 = "GF17";
                                        days126 = 30;
                                        break;
                                    case 26:
                                        itemcode139 = "GF17";
                                        days126 = 15;
                                        break;
                                    case 27:
                                        itemcode139 = "GF26";
                                        days126 = -1;
                                        break;
                                    case 28:
                                        itemcode139 = "GF26";
                                        days126 = 30;
                                        break;
                                    case 29:
                                        itemcode139 = "GF26";
                                        days126 = 15;
                                        break;
                                    case 30:
                                        itemcode139 = "GF31";
                                        days126 = -1;
                                        break;
                                    case 31:
                                        itemcode139 = "GF44";
                                        days126 = 30;
                                        break;
                                    case 32:
                                        itemcode139 = "GF44";
                                        days126 = 15;
                                        break;
                                    case 33:
                                        itemcode139 = "GF44";
                                        days126 = -1;
                                        break;
                                    case 34:
                                        itemcode139 = "GF51";
                                        days126 = 30;
                                        break;
                                    case 35:
                                        itemcode139 = "GF51";
                                        days126 = 15;
                                        break;
                                    case 36:
                                        itemcode139 = "GF51";
                                        days126 = -1;
                                        break;
                                    case 37:
                                        itemcode139 = "GF56";
                                        days126 = 30;
                                        break;
                                    case 38:
                                        itemcode139 = "GF56";
                                        days126 = 15;
                                        break;
                                    case 39:
                                        itemcode139 = "GF56";
                                        days126 = -1;
                                        break;
                                


                                }
                                Inventory.AddItem(usr, itemcode139, days126);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode139, days126));
                                return;
                            case "CY48":
                                int num97 = Game_Server.Generic.random(0, 76);
                                int days123 = 1;
                                string itemcode118 = (string)null;
                                switch (num97)
                                {
                                    case 0:
                                        itemcode118 = "GE06";
                                        days123 = 30;
                                        break;
                                    case 1:
                                        itemcode118 = "GE06";
                                        days123 = -1;
                                        break;
                                    case 2:
                                        itemcode118 = "GA09";
                                        days123 = -1;
                                        break;
                                    case 3:
                                        itemcode118 = "GA09";
                                        days123 = 30;
                                        break;
                                    case 4:
                                        itemcode118 = "GE05";
                                        days123 = 30;
                                        break;
                                    case 5:
                                        itemcode118 = "GE05";
                                        days123 = -1;
                                        break;
                                    case 6:
                                        itemcode118 = "GC09";
                                        days123 = -1;
                                        break;
                                    case 7:
                                        itemcode118 = "GC09";
                                        days123 = 30;
                                        break;
                                    case 8:
                                        itemcode118 = "DE63";
                                        days123 = -1;
                                        break;
                                    case 9:
                                        itemcode118 = "DE63";
                                        days123 = 30;
                                        break;
                                    case 10:
                                        itemcode118 = "DE59";
                                        days123 = 30;
                                        break;
                                    case 11:
                                        itemcode118 = "DE59";
                                        days123 = -1;
                                        break;
                                    case 12:
                                        itemcode118 = "DE22";
                                        days123 = 30;
                                        break;
                                    case 13:
                                        itemcode118 = "DE22";
                                        days123 = -1;
                                        break;
                                    case 14:
                                        itemcode118 = "DE69";
                                        days123 = 30;
                                        break;
                                    case 15:
                                        itemcode118 = "DE69";
                                        days123 = -1;
                                        break;
                                    case 16:
                                        itemcode118 = "DE96";
                                        days123 = 30;
                                        break;
                                    case 17:
                                        itemcode118 = "DE96";
                                        days123 = -1;
                                        break;
                                    case 18:
                                        itemcode118 = "GA10";
                                        days123 = 30;
                                        break;
                                    case 19:
                                        itemcode118 = "GA10";
                                        days123 = -1;
                                        break;
                                    case 20:
                                        itemcode118 = "GE07";
                                        days123 = 30;
                                        break;
                                    case 21:
                                        itemcode118 = "GE07";
                                        days123 = -1;
                                        break;
                                    case 22:
                                        itemcode118 = "GC23";
                                        days123 = 30;
                                        break;
                                    case 23:
                                        itemcode118 = "GC23";
                                        days123 = -1;
                                        break;
                                    case 24:
                                        itemcode118 = "DE90";
                                        days123 = 30;
                                        break;
                                    case 25:
                                        itemcode118 = "DE90";
                                        days123 = -1;
                                        break;
                                    case 26:
                                        itemcode118 = "GC22";
                                        days123 = 30;
                                        break;
                                    case 27:
                                        itemcode118 = "GC22";
                                        days123 = -1;
                                        break;
                                    case 28:
                                        itemcode118 = "DD34";
                                        days123 = 30;
                                        break;
                                    case 29:
                                        itemcode118 = "DD34";
                                        days123 = -1;
                                        break;
                                    case 30:
                                        itemcode118 = "DB76";
                                        days123 = 30;
                                        break;
                                    case 31:
                                        itemcode118 = "DB76";
                                        days123 = -1;
                                        break;
                                    case 32:
                                        itemcode118 = "GE01";
                                        days123 = 30;
                                        break;
                                    case 33:
                                        itemcode118 = "GE01";
                                        days123 = -1;
                                        break;
                                    case 34:
                                        itemcode118 = "GE08";
                                        days123 = 30;
                                        break;
                                    case 35:
                                        itemcode118 = "GE08";
                                        days123 = -1;
                                        break;
                                    case 36:
                                        itemcode118 = "DB76";
                                        days123 = 30;
                                        break;
                                    case 37:
                                        itemcode118 = "DB75";
                                        days123 = -1;
                                        break;
                                    case 38:
                                        itemcode118 = "GE02";
                                        days123 = 30;
                                        break;
                                    case 39:
                                        itemcode118 = "GE02";
                                        days123 = -1;
                                        break;
                                    case 40:
                                        itemcode118 = "DB74";
                                        days123 = 30;
                                        break;
                                    case 41:
                                        itemcode118 = "DB74";
                                        days123 = -1;
                                        break;
                                    case 42:
                                        itemcode118 = "GC18";
                                        days123 = 30;
                                        break;
                                    case 43:
                                        itemcode118 = "GC18";
                                        days123 = -1;
                                        break;
                                    case 44:
                                        itemcode118 = "DM05";
                                        days123 = 30;
                                        break;
                                    case 45:
                                        itemcode118 = "DM05";
                                        days123 = -1;
                                        break;
                                    case 46:
                                        itemcode118 = "DN05";
                                        days123 = 30;
                                        break;
                                    case 47:
                                        itemcode118 = "DN05";
                                        days123 = -1;
                                        break;
                                    case 48:
                                        itemcode118 = "GC24";
                                        days123 = -1;
                                        break;
                                    case 49:
                                        itemcode118 = "GC24";
                                        days123 = 30;
                                        break;
                                    case 50:
                                        itemcode118 = "DC84";
                                        days123 = -1;
                                        break;
                                    case 51:
                                        itemcode118 = "DC84";
                                        days123 = 30;
                                        break;
                                    case 52:
                                        itemcode118 = "GC48";
                                        days123 = -1;
                                        break;
                                    case 53:
                                        itemcode118 = "GC48";
                                        days123 = 30;
                                        break;
                                    case 54:
                                        itemcode118 = "GE09";
                                        days123 = -1;
                                        break;
                                    case 55:
                                        itemcode118 = "GE09";
                                        days123 = 30;
                                        break;
                                    case 56:
                                        itemcode118 = "DC91";
                                        days123 = -1;
                                        break;
                                    case 57:
                                        itemcode118 = "DC91";
                                        days123 = 30;
                                        break;
                                    case 58:
                                        itemcode118 = "GC32";
                                        days123 = -1;
                                        break;
                                    case 59:
                                        itemcode118 = "DN15";
                                        days123 = 30;
                                        break;
                                    case 60:
                                        itemcode118 = "DN15";
                                        days123 = -1;
                                        break;
                                    case 61:
                                        itemcode118 = "CI01";
                                        days123 = -1;
                                        break;
                                    case 62:
                                        itemcode118 = "GE19";
                                        days123 = 30;
                                        break;
                                    case 63:
                                        itemcode118 = "GE19";
                                        days123 = -1;
                                        break;
                                    case 64:
                                        itemcode118 = "GC35";
                                        days123 = 30;
                                        break;
                                    case 65:
                                        itemcode118 = "GC35";
                                        days123 = -1;
                                        break;
                                    case 66:
                                        itemcode118 = "DE99";
                                        days123 = 30;
                                        break;
                                    case 67:
                                        itemcode118 = "DE99";
                                        days123 = -1;
                                        break;
                                    case 68:
                                        itemcode118 = "DH13";
                                        days123 = 30;
                                        break;
                                    case 69:
                                        itemcode118 = "DH13";
                                        days123 = -1;
                                        break;
                                    case 70:
                                        itemcode118 = "GC30";
                                        days123 = 30;
                                        break;
                                    case 71:
                                        itemcode118 = "GC30";
                                        days123 = -1;
                                        break;
                                    case 72:
                                        itemcode118 = "GC46";
                                        days123 = 30;
                                        break;
                                    case 73:
                                        itemcode118 = "GC46";
                                        days123 = -1;
                                        break;
                                    case 74:
                                        itemcode118 = "GC46";
                                        days123 = 30;
                                        break;
                                    case 75:
                                        itemcode118 = "DH08";
                                        days123 = -1;
                                        break;
                                    case 76:
                                        itemcode118 = "DH08";
                                        days123 = 30;
                                        break;
                                    case 77:
                                        itemcode118 = "GA23";
                                        days123 = 30;
                                        break;
                                    case 78:
                                        itemcode118 = "GA23";
                                        days123 = -1;
                                        break;
                                }
                                Inventory.AddItem(usr, itemcode118, days123);
                                Inventory.DecreaseEAItem(usr, block1, 1);
                                usr.send((Packet)new SP_WinItem(usr, itemcode118, days123));
                                return;
                            case "CC56":
                            case "CC57":
                            case "CC36":
                            case "CC37":
                                Item obj2 = ItemManager.GetItem(block1);
                                if (obj2 == null)
                                    return;
                                List<PackageItem> packageItems = obj2.packageItems;
                                if (packageItems.Count <= 0 || Inventory.GetFreeItemSlotCount(usr) <= 0)
                                    return;
                                int index1 = Game_Server.Generic.random(0, packageItems.Count - 1);
                                if (index1 == 0 && Game_Server.Generic.random(100, 1000) > 200)
                                    index1 = Game_Server.Generic.random(1, packageItems.Count - 1);
                                PackageItem packageItem = packageItems[index1];
                                if (ItemManager.GetItem(packageItem.item) != null)
                                {
                                    int days94 = (int)packageItem.days;
                                    Inventory.AddItem(usr, packageItem.item, days94);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, packageItem.item, days94));
                                    return;
                                }
                                Log.WriteError(packageItem.ToString() + " is not a valid item @ random box!");
                                return;

                            case "CR16":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] attendanceBox = Game_Server.Configs.Server.ItemShop.attendanceBox;
                                int index5 = Game_Server.Generic.random(0, attendanceBox.Length - 1);
                                string str2 = attendanceBox[index5];
                                if (ItemManager.GetItem(str2) != null)
                                {
                                    int days97 = new Random().Next(3, 30);
                                    if (str2.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str2, days97);
                                    else
                                        Inventory.AddItem(usr, str2, days97);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str2, days97));
                                    return;
                                }
                                Log.WriteError(str2 + " is not a valid item @ attendance box box event!");
                                return;
                            case "CR17":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] attendanceBox5 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                                int index9 = Game_Server.Generic.random(0, attendanceBox5.Length - 1);
                                string str8 = attendanceBox5[index9];
                                if (ItemManager.GetItem(str8) != null)
                                {
                                    int days97 = new Random().Next(3, 30);
                                    if (str8.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str8, days97);
                                    else
                                        Inventory.AddItem(usr, str8, days97);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str8, days97));
                                    return;
                                }
                                Log.WriteError(str8 + " is not a valid item @ attendance box box event!");
                                return;

                            case "CR06":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] attendanceBox2 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                                int index6 = Game_Server.Generic.random(0, attendanceBox2.Length - 1);
                                string str5 = attendanceBox2[index6];
                                if (ItemManager.GetItem(str5) != null)
                                {
                                    int days95 = new Random().Next(3, 30);
                                    if (str5.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str5, days95);
                                    else
                                        Inventory.AddItem(usr, str5, days95);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str5, days95));
                                    return;
                                }
                                Log.WriteError(str5 + " is not a valid item @ attendance box box event!");
                                return;
                            case "CR08":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] attendanceBox4 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                                int index8 = Game_Server.Generic.random(0, attendanceBox4.Length - 1);
                                string str7 = attendanceBox4[index8];
                                if (ItemManager.GetItem(str7) != null)
                                {
                                    int days92 = new Random().Next(3, 30);
                                    if (str7.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str7, days92);
                                    else
                                        Inventory.AddItem(usr, str7, days92);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str7, days92));
                                    return;
                                }
                                Log.WriteError(str7 + " is not a valid item @ attendance box box event!");
                                return;
                            case "CR07":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] attendanceBox3 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                                int index7 = Game_Server.Generic.random(0, attendanceBox3.Length - 1);
                                string str4 = attendanceBox3[index7];
                                if (ItemManager.GetItem(str4) != null)
                                {
                                    int days107 = new Random().Next(3, 30);
                                    if (str4.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str4, days107);
                                    else
                                        Inventory.AddItem(usr, str4, days107);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str4, days107));
                                    return;
                                }
                                Log.WriteError(str4 + " is not a valid item @ attendance box box event!");
                                return;

                            case "CR05":
                                if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                    return;
                                string[] items2 = Game_Server.Configs.Server.ChristmasBoxEvent.items;
                                int index4 = Game_Server.Generic.random(0, items2.Length - 1);
                                string str3 = items2[index4];
                                if (ItemManager.GetItem(str3) != null)
                                {
                                    int days93 = new Random().Next(Game_Server.Configs.Server.ChristmasBoxEvent.MinDays, Game_Server.Configs.Server.ChristmasBoxEvent.MaxDays);
                                    if (str3.ToUpper().StartsWith("B"))
                                        Inventory.AddCostume(usr, str3, days93);
                                    else
                                        Inventory.AddItem(usr, str3, days93);
                                    Inventory.DecreaseEAItem(usr, block1, 1);
                                    usr.send((Packet)new SP_WinItem(usr, str3, days93));
                                    return;
                                }
                                Log.WriteError(str3 + " is not a valid item @ random box event!");
                                return;
                            case null:
                                return;
                            default:
                                return;
                        }
                }
            }
        }


        class SP_WinItem : Packet
        {
            public SP_WinItem(User usr, string itemcode, int days)
            {
                newPacket(30720);
                addBlock(1111);
                addBlock(1);
                addBlock("CB09");
                addBlock(Inventory.Itemlist(usr));
                //T,T,F,F CF02 30 47728
                addBlock(usr.AvailableSlots);
                addBlock(itemcode);
                addBlock(days);
                addBlock(usr.dinar);
            }
        }

        class SP_UseItem : Packet
        {
            public SP_UseItem(User usr, string itemcode)
            {
                //30720 1111 1 CB09 DB33-3-0-13080422-0,CB08-2-0-13052022-3,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,DA09-1-0-13070215-0,DF03-1-0-13070214-0,DT01-1-0-13071700-0,^,DH01-1-0-13071921-0,DI01-1-0-13062921-0,CF02-3-0-13072602-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ T,T,F,F CF02 30 47728 
                newPacket(30720);
                addBlock(1111);
                addBlock(1);
                addBlock(itemcode);
                addBlock(Inventory.Itemlist(usr));
                addBlock(usr.AvailableSlots);
                addBlock(itemcode);
                addBlock(0);
                addBlock(usr.dinar);
            }
        }

        class SP_CashItemBuy : Packet
        {
            public SP_CashItemBuy(User usr, string ItemCode, int Days)
            {
                //30720 1111 1 CB09 DB33-3-0-13080422-0,CB08-2-0-13052022-3,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,DA09-1-0-13070215-0,DF03-1-0-13070214-0,DT01-1-0-13071700-0,^,DH01-1-0-13071921-0,DI01-1-0-13062921-0,CF02-3-0-13072602-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ T,T,F,F CF02 30 47728 
                newPacket(30720);
                addBlock(1111);
                addBlock(1);
                addBlock(ItemCode);
                addBlock(Inventory.Itemlist(usr));
                addBlock(usr.AvailableSlots);
                addBlock(ItemCode);
                addBlock(Days);
                addBlock(usr.dinar);
            }

            public SP_CashItemBuy(User usr)
            {
                newPacket(30720);
                addBlock(1113);
                addBlock(1);
                addBlock(1110);
                addBlock(1110);
                addBlock(126);
                addBlock(4);
                addBlock(0);
                addBlock(190);
                addBlock(usr.cash);
            }

            public SP_CashItemBuy(User usr, string Items)
            {
                newPacket(30720);
                addBlock(1110);
                addBlock(1110);
                addBlock(126);
                addBlock(4);
                addBlock(0);
                addBlock(190);
                addBlock(usr.cash);
                addBlock(Items);
                addBlock(usr.AvailableSlots);
                addBlock(0);
                addBlock(usr.dinar);
            }
        }

        class SP_StorageInventoryList : Packet
        {
            public SP_StorageInventoryList(User usr)
            {
                newPacket(30720);
                addBlock(1400);
                addBlock(1);
                addBlock(0);
                addBlock(usr.storageInventoryMax);
                addBlock(Inventory.Storage(usr));
            }
        }

        class SP_StorageInventoryUpdate : Packet
        {
            internal enum ErrorCode : uint
            {
                NoInventoryFreeSpace = 97070,
                NoStorageFreeSpace = 97071
            }

            public SP_StorageInventoryUpdate(ErrorCode code)
            {
                newPacket(30720);
                addBlock(1400);
                addBlock((uint)code);
            }

            public SP_StorageInventoryUpdate(User usr, int action, int index, string itemCode)
            {
                newPacket(30720);
                addBlock(1400);
                addBlock(1);
                addBlock(action);
                addBlock(usr.storageInventoryMax);
                addBlock(index);
                addBlock(itemCode);
                addBlock(Inventory.Storage(usr));
                addBlock(Inventory.Itemlist(usr));
            }
        }

        class SP_CashItemUse : Packet
        {
            internal enum ErrCode
            {
                NeedSupplyBox = -3
            }

            public SP_CashItemUse(SP_CashItemUse.ErrCode ErrCode, User usr, string ItemCode)
            {
                newPacket(30720);
                addBlock(1111);
                addBlock((int)ErrCode);
                addBlock(ItemCode);
                addBlock(Inventory.Itemlist(usr));
            }

            public SP_CashItemUse(User usr, string ItemCode)
            {
                newPacket(30720);
                addBlock(1111);
                addBlock(1);
                addBlock(ItemCode);
                addBlock(Inventory.Itemlist(usr));
                if (ItemCode == "CB03")
                {
                    addBlock(usr.AvailableSlots);
                    addBlock(0);
                    addBlock(0);
                    addBlock(usr.dinar);
                }
                else if (ItemCode == "CB01")
                {
                    addBlock(usr.AvailableSlots);
                    addBlock(usr.nickname);
                }
                else if (ItemCode == "CB99")
                {
                    addBlock(usr.AvailableSlots);
                    addBlock(usr.MaxSlots);
                }
            }
        }
    }
