using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

using Game_Server.Managers;

namespace Game_Server.Game
{
    class CP_Clan : Handler
    {
        internal enum Subtype
        {
            CheckForDuplicate = 0,
            AddClan = 1,
            ApplyClan = 2,
            LeaveClan = 3,
            MyClan = 4,
            Members = 5,
            SearchClan = 6,
            ClanInfo = 7,
            ChangeAnnDec = 8,
            JoinAction = 9,
            RankAction = 10,
            Promote = 11,
            NickChange = 12,
            MarkChange = 14,
            DisbandClan = 16,
            NewSearchClan = 17
        }

        internal enum SearchType
        {
            Name = 0,
            Master = 1
        }

        public override void Handle(User usr)
        {

            Subtype type = (Subtype)int.Parse(getBlock(0));
            switch (type)
            {
                case Subtype.CheckForDuplicate:
                    {
                        string name = getBlock(1).Replace(" ", "");
                        if (name.Length > 4)
                        {
                            ClanManager.CheckForDuplicate(usr, name);
                        }
                        break;
                    }
                case Subtype.AddClan:
                    {
                        string name = getBlock(1).Replace(" ", "");
                        if (name.Length > 4)
                        {
                            ClanManager.AddClan(usr, name);
                        }
                        break;
                    }
                case Subtype.ApplyClan:
                    {
                        int clanId = int.Parse(getBlock(1));
                        if (clanId > 0)
                        {
                            string time = DateTime.Now.ToString("dd/MM/yyyy");

                            Clan clan = ClanManager.GetClan(clanId);
                            if (clan != null)
                            {
                                if (clan.ClanUsers.Count >= clan.maxUsers) { return; }

                                ClanPendingUsers pending = new ClanPendingUsers(usr.userId, usr.nickname, usr.exp.ToString(), time);
                                clan.pendingUsers.TryAdd(usr.userId, pending);

                                usr.clan = clan;

                                usr.send(new SP_Clan(SP_Clan.ClanCodes.ApplyClan));

                                if (usr.clan != null)
                                {
                                    DB.RunQuery("UPDATE clans_invite SET clanid='" + clanId + "' WHERE userid='" + usr.userId + "'");
                                }
                                else
                                {

                                    DB.RunQuery("INSERT INTO clans_invite (userid, clanid) VALUES ('" + usr.userId + "', '" + clanId + "')");
                                }

                                DB.RunQuery("UPDATE users SET clanrank='9', clanid='" + clanId + "' WHERE id='" + usr.userId + "'");
                            }
                        }
                        break;
                    }
                case Subtype.LeaveClan:
                    {
                        if (usr.clan != null)
                        {
                            Clan clan = usr.clan;
                            if (clan.pendingUsers.ContainsKey(usr.userId))
                            {
                                ClanPendingUsers u;
                                clan.pendingUsers.TryRemove(usr.userId, out u);
                            }

                            int clanrank = clan.clanRank(usr);
                            if (clanrank == 2)
                            {
                                foreach (User u in clan.GetUsers())
                                {
                                    u.clan = null;
                                }

                                ClanManager.RemoveClan(usr);

                                DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE clanid='" + clan.id + "'");
                                DB.RunQuery("DELETE FROM clans WHERE id='" + clan.id + "'");
                            }
                            else
                            {
                                usr.clan = null;

                                if (clanrank == 9)
                                {
                                    ClanPendingUsers u;
                                    clan.pendingUsers.TryRemove(usr.userId, out u);
                                }
                                else
                                {
                                    ClanUsers u;
                                    clan.ClanUsers.TryRemove(usr.userId, out u);
                                }

                                usr.send(new SP_Clan(SP_Clan.ClanCodes.LeaveClan));

                                DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + usr.userId + "'");
                                DB.RunQuery("UPDATE clans SET count='" + usr.clan.ClanUsers.Count + "' WHERE id='" + usr.clan.id + "'");
                            }
                        }
                        break;
                    }
                case Subtype.MyClan:
                    {
                        if (usr.clan == null) return;
                        usr.send(new SP_Clan.MyClanInformation(usr));
                        break;
                    }
                case Subtype.Members:
                    {
                        if (usr.clan == null) return;
                        int page = int.Parse(getBlock(1));
                        int clanrank = usr.clan.clanRank(usr);

                        if (page == 1)
                        {
                            usr.send(new SP_Clan.UserList.NormalUser(usr.clan));
                        }
                        else if (page == 2 && (clanrank == 1 || clanrank == 2))
                        {
                            usr.send(new SP_Clan.UserList.Pending(usr));
                        }

                        break;
                    }
                case Subtype.SearchClan:
                    {
                        int t = int.Parse(getBlock(1));
                        SearchType subtype = (SearchType)t;
                        int page = int.Parse(getBlock(1));
                        string key = getBlock(2);
                        List<Clan> list = new List<Clan>();

                        //Log.WriteDebug(string.Join(" ", getAllBlocks));

                        switch (subtype)
                        {
                            case SearchType.Name:
                                {
                                    list = new List<Clan>(ClanManager.Clans.Values.Where(c => c != null && c.name.ToLower().Contains(key.ToLower())).ToArray());
                                    break;
                                }
                            case SearchType.Master:
                                {
                                    list = new List<Clan>(ClanManager.Clans.Values.Where(c => c != null && c.Master.ToLower().Contains(key.ToLower())).ToArray());
                                    break;
                                }
                            default:
                                {
                                    list = new List<Clan>(ClanManager.Clans.Values.Where(c => c != null && (c.Master.ToLower().Contains(key.ToLower()) || c.name.ToLower().Contains(key.ToLower()))).ToArray());
                                    break;
                                }
                        }

                        if (key.Length >= 3)
                        {
                            usr.send(new SP_Clan.SearchClan(list));
                        }
                        else
                        {
                            usr.send(new SP_Clan.CheckClan(SP_Clan.CheckClan.ErrorCodes.NotFound));
                        }
                        break;
                    }
                case Subtype.ClanInfo:
                    {
                        int clanId = int.Parse(getBlock(1));
                        if (clanId > 0)
                        {
                            Clan clan = ClanManager.GetClan(clanId);
                            if (clan != null)
                            {
                                usr.send(new SP_Clan.UserList.NormalUser(usr, clan));
                            }
                        }
                        break;
                    }
                case Subtype.ChangeAnnDec:
                    {
                        if (usr.clan == null) return;
                        int clanrank = usr.clan.clanRank(usr);
                        if (clanrank < 1 || clanrank == 9 || usr.clan == null) return;

                        string Message = DB.Stripslash(getBlock(2));

                        bool description = getBlock(1) == "0";
                        if (description)
                        {
                            usr.clan.Description = Message;
                        }
                        else
                        {
                            usr.clan.Announcment = Message;
                        }

                        // Send query after to let the server be lagfree

                        DB.RunQuery("UPDATE clans SET description='" + usr.clan.Description + "', announcment='" + usr.clan.Announcment + "' WHERE id='" + usr.clan.id + "'");
                        break;
                    }
                case Subtype.JoinAction:
                    {
                        if (usr.clan == null) return;
                        int subtype = int.Parse(getBlock(1));
                        int userId = int.Parse(getBlock(2));

                        Clan clan = usr.clan;

                        switch (subtype)
                        {
                            case 0: // Accept Join 
                                {
                                    if (clan.ClanUsers.Count >= clan.maxUsers)
                                    {
                                        usr.send(new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> No more slot available for the clan, please expand if is possible", usr.sessionId, usr.nickname));
                                        return;
                                    }

                                    string time = DateTime.Now.ToString("dd/MM/yyyy");

                                    DataTable dt = DB.RunReader("SELECT * FROM users WHERE id='" + userId + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow row = dt.Rows[0];
                                        DB.RunQuery("DELETE FROM clans_invite WHERE userid='" + userId + "'");
                                        DB.RunQuery("UPDATE clans SET count='" + clan.ClanUsers.Count + "' WHERE id='" + clan.id + "'");
                                        DB.RunQuery("UPDATE users SET clanid='" + clan.id + "', clanrank='0', clanjoindate='" + time + "' WHERE id='" + userId + "'");
                                        if (clan.pendingUsers.ContainsKey(userId))
                                        {
                                            ClanPendingUsers u;
                                            clan.pendingUsers.TryRemove(userId, out u);
                                        }
                                        ClanUsers c = new ClanUsers(userId, row["nickname"].ToString(), row["exp"].ToString(), time, 0);
                                        clan.ClanUsers.TryAdd(userId, c);
                                    }

                                    User user = UserManager.GetUser(userId);
                                    if (user != null)
                                    {
                                        user.clan = clan;
                                        clan.Users.TryAdd(userId, user);
                                    }

                                    ClanPendingUsers cc;
                                    if (clan.pendingUsers.ContainsKey(userId))
                                    {
                                        clan.pendingUsers.TryRemove(userId, out cc);
                                    }

                                    break;
                                }
                            case 1: // Refuse Join
                                {
                                    DB.RunQuery("DELETE FROM clans_invite WHERE userid='" + userId + "'");
                                    DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + userId + "'");
                                    User u = UserManager.GetUser(userId);
                                    if (u != null)
                                    {
                                        u.clan = null;
                                    }

                                    ClanPendingUsers c;
                                    if (clan.pendingUsers.ContainsKey(userId))
                                    {
                                        clan.pendingUsers.TryRemove(userId, out c);
                                    }
                                    break;
                                }
                        }

                        DB.RunQuery("DELETE FROM clans_invite WHERE userid='" + userId + "'");

                        usr.send(new SP_Clan.UserList.Pending(subtype, userId));

                        break;
                    }
                case Subtype.RankAction:
                    {
                        if (usr.clan == null) return;
                        int subtype = int.Parse(getBlock(1));
                        int userId = int.Parse(getBlock(2));
                        int clanrank = usr.clan.clanRank(usr);
                        if (clanrank >= 1)
                        {
                            clanrank = 0;
                            switch (subtype)
                            {
                                case 0:
                                    {
                                        clanrank = 1;
                                        DB.RunQuery("UPDATE users SET clanrank='1' WHERE id='" + userId + "'");
                                        break;
                                    }
                                case 1:
                                    {
                                        clanrank = 0;
                                        DB.RunQuery("UPDATE users SET clanrank='0' WHERE id='" + userId + "'");
                                        break;
                                    }
                                case 2:
                                    {
                                        DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + userId + "'");
                                        DB.RunQuery("UPDATE clans SET count='" + usr.clan.ClanUsers.Count + "' WHERE id='" + usr.clan.id + "'");
                                        User u2;
                                        ClanUsers u;
                                        usr.clan.Users.TryRemove(userId, out u2);
                                        usr.clan.ClanUsers.TryRemove(userId, out u);
                                        break;
                                    }
                            }

                            if (subtype != 2)
                            {
                                usr.clan.ClanUsers.Values.Where(s => s.id == userId).First().clanrank = clanrank;
                            }
                            else
                            {
                                User u = UserManager.GetUser(userId);
                                if (u != null)
                                {
                                    u.clan = null;
                                }
                            }

                            usr.send(new SP_Clan.Change(subtype, userId));
                        }
                        break;
                    }
                case Subtype.Promote:
                    {
                        if (usr.clan == null) return;
                        int userId = int.Parse(getBlock(1));
                        Clan clan = usr.clan;

                        if (clan != null)
                        {
                            DataTable dt = DB.RunReader("SELECT * FROM users WHERE id='" + userId + "'");
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                if (userId != usr.userId)
                                {
                                    DB.RunQuery("UPDATE users SET clanrank='0' WHERE id='" + usr.userId + "'");
                                    DB.RunQuery("UPDATE users SET clanrank='2' WHERE id='" + userId + "'");
                                    clan.Master = row["nickname"].ToString();
                                    clan.MasterEXP = row["exp"].ToString();
                                    clan.ClanUsers.Values.Where(r => string.Compare(r.nickname, clan.Master, true) == 0).First().clanrank = 2;
                                    clan.ClanUsers.Values.Where(r => string.Compare(r.nickname, usr.nickname, true) == 0).First().clanrank = 0;

                                    byte[] buffer = (new SP_Chat("ClanSystem", SP_Chat.ChatType.Clan, "ClanSystem >> " + usr.nickname + " passed master to " + clan.Master + " :/", (uint)clan.id, "NULL")).GetBytes();

                                    foreach (User u in clan.Users.Values)
                                    {
                                        u.sendBuffer(buffer);
                                    }

                                    usr.send(new SP_Clan.Change());
                                }
                            }
                        }
                        break;
                    }
                case Subtype.NickChange:
                    {
                        if (usr.clan == null) return;
                        string newNick = getBlock(1);
                        Clan c = ClanManager.GetClanByName(newNick);
                        if (usr.clan != null)
                        {
                            if (c == null)
                            {
                                if (usr.HasItem("CB02"))
                                {
                                    DB.RunQuery("UPDATE clans SET name='" + newNick + "' WHERE id='" + usr.clan.id + "'");
                                    c.name = newNick;
                                    usr.deleteItem("CB02");
                                    usr.send(new SP_Clan.Change(usr, true));
                                }
                            }
                            else
                            {
                                usr.send(new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> A clan has already this name, please choose another one", usr.sessionId, usr.nickname));
                            }
                        }

                        break;
                    }
                case Subtype.MarkChange:
                    {
                        if (usr.clan == null) return;
                        uint iconID = uint.Parse(getBlock(1));
                        if (usr.HasItem("CB54") && usr.clan != null)
                        {
                            DB.RunQuery("UPDATE clans SET iconid='" + iconID + "' WHERE id='" + usr.clan.id + "'");
                            usr.clan.iconid = iconID;

                            usr.deleteItem("CB54");
                            usr.send(new SP_Clan.Change(usr, false));
                        }
                        break;
                    }
                case Subtype.DisbandClan:
                    {
                        if (usr.clan == null) return;
                        ClanManager.RemoveClan(usr);
                        break;
                    }
                case Subtype.NewSearchClan:
                    {
                        int page = int.Parse(getBlock(1));
                        int sortType = int.Parse(getBlock(2));
                        List<Clan> clans = new List<Clan>();

                        switch (sortType)
                        {
                            case 0: // By rank desc
                                {
                                    clans = ClanManager.Clans.Values.OrderByDescending(r => r.exp).Skip(page * 10).Take(10).ToList();
                                    break;
                                }
                            case 1: // By rank asc
                                {
                                    clans = ClanManager.Clans.Values.OrderBy(r => r.exp).Skip(page * 10).Take(10).ToList();
                                    break;
                                }
                        }

                        usr.send(new SP_Clan.SearchClan(page, sortType, clans));
                        break;
                    }
                default:
                    {
                        Log.WriteError("Unknown Clan Subtype " + (int)Subtype.DisbandClan);
                        break;
                    }
            }
        }
    }

    class SP_Clan : Packet
    {
        public enum ClanCodes
        {
            CreateClan = 1,
            ApplyClan = 2,
            LeaveClan = 3,
            Open = 4,
            MemberList = 5,
            SearchClan = 6,
            DisbandClan = 16
        }

        public SP_Clan(ClanCodes type)
        {
            newPacket(26384);
            addBlock((int)type);
            addBlock(1);
        }

        internal class SearchClan : Packet
        {
            public SearchClan(int page, int sortType, List<Clan> list)
            {
                int len = list.Count;
                if (len > 10) len = 10;

                newPacket(26384);
                addBlock(17);
                addBlock(1);
                addBlock(page);
                addBlock(sortType);
                addBlock(len);
                for (int i = (10 * page); i < len; i++)
                {
                    Clan c = list[i];
                    if (c != null)
                    {
                        addBlock(c.id);
                        addBlock(c.Master);
                        addBlock(c.MasterEXP);
                        addBlock(c.name);
                        addBlock(c.GetRank());
                        addBlock(c.ClanUsers.Count);
                        addBlock(c.iconid);
                        addBlock((10 * page) + (i + 1));
                        addBlock(c.Description.Replace((char)0x20, (char)0x1D));
                        addBlock(c.GetCreationDate());
                    }
                }
            }
            public SearchClan(List<Clan> list)
            {
                int len = list.Count;

                newPacket(26384);
                addBlock(6);
                addBlock(1);
                addBlock(len);
                for (int i = 0; i < len; i++)
                {
                    Clan c = list[i];
                    if (c != null)
                    {
                        addBlock(c.id);
                        addBlock(c.Master);
                        addBlock(c.MasterEXP);
                        addBlock(c.name);
                        addBlock(c.GetRank());
                        addBlock(c.ClanUsers.Count);
                        addBlock(c.iconid);
                        addBlock(i + 1);
                        addBlock(c.Description.Replace((char)0x20, (char)0x1D));
                        addBlock(c.GetCreationDate());
                    }
                }
            }
        }

        internal class CheckClan : Packet
        {
            internal enum ErrorCodes
            {
                NotExist = 1,
                Exist = 62003,
                NotFound = 63001
            }

            public CheckClan(ErrorCodes err)
            {
                newPacket(26384);
                addBlock(0);
                addBlock((int)err);
            }
        }

        internal class Change : Packet
        {
            public Change()
            {
                newPacket(26384);
                addBlock(11);
                addBlock(1);
            }

            public Change(int SubType, int UserID)
            {
                //26384 10 1 2 23343041
                newPacket(26384);
                addBlock(10);
                addBlock(1);
                addBlock(SubType);
                addBlock(UserID);
            }

            public Change(User usr, bool isNick)
            {
                //26384 12 1 DB33-3-0-13080422-0,CB08-2-0-13052022-4,CC02-3-0-13080422-0,DS01-3-0-13080903-0,CA01-3-0-13081400-0,CD01-3-0-13080422-0,CD02-3-0-13080422-0,DB04-1-0-13070914-0,DA09-1-0-13070215-0,DF03-1-0-13070214-0,DT01-1-0-13071700-0,^,DH01-1-0-13071921-0,DI01-1-0-13062921-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 
                newPacket(26384);
                addBlock(isNick ? 12 : 14);
                addBlock(1);
                addBlock(Inventory.Itemlist(usr));
            }
        }

        internal class CreateClan : Packet
        {
            public CreateClan(string name, int clanId, uint dinar)
            {
                newPacket(26384);
                addBlock(ClanCodes.CreateClan);
                addBlock(1); // Success (?)
                addBlock(clanId);
                addBlock(2);
                addBlock(name);
                addBlock(dinar);
            }
        }

        internal class MyClanInformation : Packet
        {
            public MyClanInformation(Game_Server.User usr)
            {
                Game_Server.Clan clan = usr.clan;
                int num = clan.maxUsers / 20 - 1;
                int clanRank = ClanManager.GetClanRank(clan.id);
                int count = clan.ClanWars.Count;
                this.newPacket((ushort)26384);
                this.addBlock((object)4);
                this.addBlock((object)1);
                this.addBlock((object)clan.clanRank(usr));
                this.addBlock((object)clan.name);
                this.addBlock((object)clan.Master);
                this.addBlock((object)clan.MasterEXP);
                this.addBlock((object)num);
                this.addBlock((object)clan.ClanUsers.Count);
                this.addBlock((object)(clan.pendingUsers.Count > 0 ? 1 : 0));
                this.addBlock((object)clan.win);
                this.addBlock((object)clan.lose);
                this.addBlock((object)clan.exp);
                this.addBlock((object)clanRank);
                this.addBlock((object)0);
                this.addBlock((object)clan.Description.Replace(' ', '\x001D'));
                this.addBlock((object)clan.Announcment.Replace(' ', '\x001D'));
                this.addBlock((object)clan.iconid);
                this.addBlock((object)count);
                foreach (ClanWar clanWar in clan.ClanWars.Values.Where<ClanWar>((Func<ClanWar, bool>)(u => u != null)).Take<ClanWar>(count > 3 ? 3 : count))
                {
                    this.addBlock((object)clanWar.versusClan);
                    this.addBlock((object)clanWar.score);
                    this.addBlock((object)(clanWar.won ? 1 : 0));
                }
            }
        }

        internal class UserList
        {
            internal class NormalUser : Packet
            {
                public NormalUser(User usr, Clan Clan)
                {
                    //26384 7 19538 
                    //26384 7 1 19538 Tokkesik2ke ToXiiC 1589017 0 1 sdffdfds 1001001 
                    //26384 7 1 19538 MontanaWarRock iSkyKinqz 1589017 0 1 sdffdfds 1025003
                    int Count = (Clan.maxUsers / 20) - 1;
                    newPacket(26384);
                    addBlock((int)CP_Clan.Subtype.ClanInfo);
                    addBlock(1);
                    addBlock(Clan.id);
                    addBlock(Clan.name);
                    addBlock(Clan.Master);
                    addBlock(Clan.MasterEXP);
                    addBlock(Count);
                    addBlock(Clan.ClanUsers.Count);
                    addBlock(Clan.Description.Replace((char)0x20, (char)0x1D));
                    addBlock(Clan.iconid);
                }

                public NormalUser(User User, List<Clan> list)
                {
                    newPacket(26384);
                    addBlock((int)CP_Clan.Subtype.SearchClan); // OPCode
                    addBlock(1);
                    //26384 6 1 1 19538 ToXiiC 1589017 Tokkesik2ke 0 1 1001001
                    addBlock(list.Count);
                    foreach (Clan c in list)
                    {
                        int count = (c.maxUsers / 20) - 1;
                        addBlock(c.id);
                        addBlock(c.Master);
                        addBlock(c.MasterEXP);
                        addBlock(c.name);
                        addBlock(count);
                        addBlock(c.ClanUsers.Count);
                        addBlock(c.iconid);
                    }
                }


                public NormalUser(Clan c) // Fixed #1 by Lucio Furry
                {
                    //26384 5 1 6 15351850 1 9776577 ateyooftw 2013.04.22 2013.04.22 0 23338906 1 29609 Exothebest 2013.01.30 2013.01.30 0 23430589 1 266 abobbetteeee 2013.04.21 2013.04.21 0 23323522 2 1576021 ToXiiC 2013.01.27 2013.01.18 203 23346580 1 5437 Maist0 2013.01.30 2013.01.30 0 23412876 1 6964 NoCeilings 2013.04.05 2013.04.05 0 
                    newPacket(26384);
                    addBlock((int)CP_Clan.Subtype.Members); // OPCode
                    //addBlock(5);
                    addBlock(1);
                    addBlock(0);
                    addBlock(c.ClanUsers.Count);
                    foreach (ClanUsers usr in c.getAllUsers())
                    {
                        addBlock(usr.id);
                        addBlock(usr.clanrank);
                        addBlock(usr.EXP);
                        addBlock(usr.nickname);
                        addBlock(usr.ClanJoinDate);
                        addBlock(usr.ClanJoinDate);
                        addBlock(0);
                        addBlock(6);
                        addBlock(Game_Server.Configs.Server.serverId);
                    }
                }
            }



            internal class Pending : Packet
            {
                public Pending()
                {
                    newPacket(26384);
                    addBlock(3);
                    addBlock(1);
                }

                public Pending(int Subtype, int ClanID)
                {
                    newPacket(26384);
                    addBlock(9);
                    addBlock(1);
                    addBlock(Subtype);
                    addBlock(ClanID);
                }

                public Pending(User usr)
                {
                    List<ClanPendingUsers> p = new List<ClanPendingUsers>(usr.clan.pendingUsers.Values);
                    //26384 5 1 6 15351850 1 9776577 ateyooftw 2013.04.22 2013.04.22 0 23338906 1 29609 Exothebest 2013.01.30 2013.01.30 0 23430589 1 266 abobbetteeee 2013.04.21 2013.04.21 0 23323522 2 1576021 ToXiiC 2013.01.27 2013.01.18 203 23346580 1 5437 Maist0 2013.01.30 2013.01.30 0 23412876 1 6964 NoCeilings 2013.04.05 2013.04.05 0 
                    /*newPacket(26384);
                    addBlock(CP_Clan.Subtype.Members); // OPCode
                    addBlock(5);
                    addBlock(2);
                    //addBlock(1);
                  //  addBlock(1);
             //       addBlock(1);*/
                    newPacket(26384);
                    addBlock((int)CP_Clan.Subtype.Members); // OPCode
                    //addBlock(5);
                    addBlock(1);
                    addBlock(0);
                    addBlock(p.Count);
                    foreach (ClanPendingUsers d in p)
                    {
                        addBlock(d.id);
                        addBlock(9); // Clan rank
                        addBlock(d.EXP);
                        addBlock(d.nickname);
                        addBlock(d.ClanJoinDate);
                        addBlock(d.ClanJoinDate);
                        addBlock(36); // ServerID
                    }
                }
            }
        }
    }
}