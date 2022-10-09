// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_Chat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_Chat : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      CP_Chat.ChatType chatType = (CP_Chat.ChatType) int.Parse(this.getBlock(0));
      int num1 = int.Parse(this.getBlock(1));
      string block1 = this.getBlock(2);
      string block2 = this.getBlock(3);
      string msg = block2.Split(new string[1]
      {
        ">>" + (object) '\x001D'
      }, StringSplitOptions.None)[1].Replace('\x001D', ' ');
      if (usr.isCommand(msg) || usr.channel == -1)
        return;
      if (msg.Length < 170)
      {
        int timestamp = Game_Server.Generic.timestamp;
        if ((long) usr.mutedexpire > (long) timestamp)
        {
          DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) usr.mutedexpire);
          string str = dateTime.ToString("HH:ss");
          if (dateTime.Day != DateTime.Now.Day && dateTime.Month != DateTime.Now.Month && dateTime.Year != DateTime.Now.Year)
            str = " of the " + dateTime.ToString("dd/MM/yyyy");
          usr.send((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Game_Server.Configs.Server.SystemName + " >> You are muted, you will be unmuted at " + str + "!", 999U, Game_Server.Configs.Server.SystemName));
        }
        else
        {
          switch (chatType)
          {
            case CP_Chat.ChatType.Unknown:
              if (block1.Length > 0)
              {
                Game_Server.User user = UserManager.GetUser(block1);
                if (user != null && user.sessionId > 0U && (!user.GMMode || usr.rank >= 4))
                {
                  byte[] bytes = new SP_Chat(usr, SP_Chat.ChatType.Whisper, block2, (long) user.sessionId, user.nickname).GetBytes();
                  usr.sendBuffer(bytes);
                  if (!usr.Equals((object) user))
                  {
                    user.sendBuffer(bytes);
                    break;
                  }
                  break;
                }
                usr.send((Packet) new SP_Chat(SP_Chat.ErrorCodes.ErrorUser, new object[1]
                {
                  (object) (block1 + (object) '\x001D')
                }));
                break;
              }
              break;
            case CP_Chat.ChatType.LobbyToChannel:
              if (usr.room != null)
                return;
              if (usr.chatColor == Color.Empty || usr.rank < 2)
              {
                if (usr.rank > 2)
                  num1 = 999;
                UserManager.sendToChannel(usr.channel, false, (Packet) new SP_Chat(usr, SP_Chat.ChatType.Room_ToAll, block2, (long) num1, block1));
                break;
              }
              UserManager.sendToChannel(usr.channel, false, (Packet) new SP_ColoredChat(block2, SP_ColoredChat.ChatType.Normal, usr.chatColor));
              break;
            case CP_Chat.ChatType.RoomToAll:
              if (usr.room != null)
              {
                if (usr.chatColor != Color.Empty && !usr.room.gameactive && usr.rank >= 2)
                {
                  usr.room.send((Packet) new SP_ColoredChat(block2, SP_ColoredChat.ChatType.Normal, usr.chatColor));
                  break;
                }
                if (usr.HasItem("CC02") && usr.room.status == 1 && (usr.roomslot == usr.room.master && usr.rank < 3))
                  num1 = 998;
                else if (usr.rank > 2)
                  num1 = 999;
                usr.room.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Room_ToAll, block2, (long) num1, block1));
                break;
              }
              usr.disconnect();
              break;
            case CP_Chat.ChatType.RoomToTeam:
              if (usr.room != null)
              {
                if (usr.rank > 2)
                  num1 = 999;
                byte[] bytes = new SP_Chat(usr, SP_Chat.ChatType.Room_ToTeam, block2, (long) num1, block1).GetBytes();
                int mySide = usr.room.GetSide(usr);
                foreach (Game_Server.User user in usr.room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (u => usr.room.GetSide(u) == mySide)))
                  user.sendBuffer(bytes);
                using (IEnumerator<Game_Server.User> enumerator = usr.room.spectators.Values.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                    enumerator.Current.sendBuffer(bytes);
                  break;
                }
              }
              else
                break;
            case CP_Chat.ChatType.Whisper:
              if (block1.Length > 0)
              {
                Game_Server.User user = UserManager.GetUser(block1);
                if (user != null && user.sessionId > 0U && (!user.GMMode || usr.rank >= 4))
                {
                  byte[] bytes = new SP_Chat(usr, SP_Chat.ChatType.Whisper, block2, (long) user.sessionId, user.nickname).GetBytes();
                  usr.sendBuffer(bytes);
                  if (!usr.Equals((object) user))
                  {
                    user.sendBuffer(bytes);
                    break;
                  }
                  break;
                }
                usr.send((Packet) new SP_Chat(SP_Chat.ErrorCodes.ErrorUser, new object[1]
                {
                  (object) (block1 + (object) '\x001D')
                }));
                break;
              }
              break;
            case CP_Chat.ChatType.LobbyToAllChannels:
              if (usr.room != null)
                return;
              if (usr.chatColor == Color.Empty || usr.rank < 2)
              {
                if (usr.rank > 2)
                  num1 = 999;
                UserManager.sendToChannel(1, false, (Packet) new SP_Chat(usr, SP_Chat.ChatType.Lobby_ToAll, block2, (long) num1, block1));
                UserManager.sendToChannel(2, false, (Packet) new SP_Chat(usr, SP_Chat.ChatType.Lobby_ToAll, block2, (long) num1, block1));
                UserManager.sendToChannel(3, false, (Packet) new SP_Chat(usr, SP_Chat.ChatType.Lobby_ToAll, block2, (long) num1, block1));
                break;
              }
              UserManager.sendToChannel(1, false, (Packet) new SP_ColoredChat(block2, SP_ColoredChat.ChatType.Normal, usr.chatColor));
              UserManager.sendToChannel(2, false, (Packet) new SP_ColoredChat(block2, SP_ColoredChat.ChatType.Normal, usr.chatColor));
              UserManager.sendToChannel(3, false, (Packet) new SP_ColoredChat(block2, SP_ColoredChat.ChatType.Normal, usr.chatColor));
              break;
            case CP_Chat.ChatType.RadioChat:
              if (usr.room == null)
                return;
              byte[] bytes1 = new SP_Chat(usr, SP_Chat.ChatType.RadioChat, block2, (long) num1, block1).GetBytes();
              int mySide1 = usr.room.GetSide(usr);
              using (IEnumerator<Game_Server.User> enumerator = usr.room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (u => usr.room.GetSide(u) == mySide1)).GetEnumerator())
              {
                while (enumerator.MoveNext())
                  enumerator.Current.sendBuffer(bytes1);
                break;
              }
            case CP_Chat.ChatType.Clan:
              if (usr.clan != null)
              {
                if (usr.clan.clanRank(usr) != 9)
                {
                  using (IEnumerator<Game_Server.User> enumerator = usr.clan.Users.Values.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      Game_Server.User current = enumerator.Current;
                      if (current.room != null)
                      {
                        int num2 = current.playing ? 1 : 0;
                      }
                      current.send((Packet) new SP_Chat(usr, SP_Chat.ChatType.Clan, usr.nickname + " >> " + msg, (long) usr.clanId, current.nickname));
                    }
                    break;
                  }
                }
                else
                  break;
              }
              else
              {
                usr.send((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Whisper, Game_Server.Configs.Server.SystemName + " >> Chat available after creating or being accepted from a clan", usr.sessionId, usr.nickname));
                break;
              }
            default:
              Log.WriteDebug("New unknow operation for chat: " + (object) chatType);
              Log.WriteDebug("Blocks: " + string.Join(" ", this.getAllBlocks));
              break;
          }
           string[] args = msg.Split((char)0x20);
                    if (args.Length >= 2)
                    {
                        if (args[0] == "Frost" && args[1] == "Soldiers!")
                        {

                            if (usr.CheckForEvent(1) == false && usr.room == null)
                            {

                                string ItemCode = "CR17";

                                int Days = -1;


                                switch (new Random().Next(0, 1))
                                {
                                    case 0: { ItemCode = "CR17"; break; }
                                  //  case 1: { ItemCode = "CR17"; break; }

                                }

                                Inventory.AddItem(usr, ItemCode, Days);
                                usr.send(new SP_ChatEvent(usr, ItemCode));
                                DB.RunQuery("INSERT INTO users_events (userid, eventid) VALUES ('" + usr.userId + "', '1')");

                            }
                        }
                    }
                    bool enabled = Game_Server.Configs.Server.ChatEvent.enabled;
          int eventId = Game_Server.Configs.Server.ChatEvent.eventId;
          string message = Game_Server.Configs.Server.ChatEvent.message;
          if (!enabled || chatType != CP_Chat.ChatType.LobbyToChannel && chatType != CP_Chat.ChatType.LobbyToAllChannels || (usr.CheckForEvent(eventId) || !(msg.Replace('\x001D', ' ') == message)))
            return;
          string[] items = Game_Server.Configs.Server.ChatEvent.items;
          int index = Game_Server.Generic.random(0, items.Length - 1);
          int days = Game_Server.Generic.random(1, 3);
          Item obj = ItemManager.GetItem(items[index]);
          if (obj == null)
            return;
          Game_Server.Configs.Server.ChatEvent.popupMessage.Replace("%item%", obj.Name).Replace("%days%", days.ToString());
          if (obj.Code.ToUpper().StartsWith("B"))
            Inventory.AddCostume(usr, obj.Code, days);
          else
            Inventory.AddItem(usr, obj.Code, days);
          usr.AddEvent(eventId, !Game_Server.Configs.Server.ChatEvent.daily);
          usr.send((Packet) new SP_UpdateInventory(usr, usr.expiredItems));
          usr.send((Packet) new SP_ChatEvent(usr, obj.Code));
        }
      }
      else
        usr.disconnect();
    }

    internal enum ChatType
    {
      Unknown = 2,
      LobbyToChannel = 3,
      RoomToAll = 4,
      RoomToTeam = 5,
      Whisper = 6,
      LobbyToAllChannels = 8,
      RadioChat = 9,
      Clan = 10, // 0x0000000A
    }
  }
}
