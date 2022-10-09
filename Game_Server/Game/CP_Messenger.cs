// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_Messenger
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System.Data;

namespace Game_Server.Game
{
  internal class CP_Messenger : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      switch (int.Parse(this.getBlock(0)))
      {
        case 5606:
          usr.send((Packet) new SP_MessengerFriends(usr));
          break;
        case 5608:
          string block1 = this.getBlock(1);
          if (block1.Length <= 0 || block1.Length > 32)
            break;
          string Message = this.getBlock(2).Trim();
          Game_Server.User user1 = UserManager.GetUser(block1);
          if (user1 == null)
            break;
          Messenger friend1 = usr.GetFriend(user1.userId);
          if (friend1 == null || friend1.status != 0 && friend1.status != 1 || !friend1.isOnline)
            break;
          byte[] bytes1 = new SP_MessengerMessage(usr.nickname, user1.nickname, Message).GetBytes();
          usr.sendBuffer(bytes1);
          user1.sendBuffer(bytes1);
          break;
        case 5609:
          string block2 = this.getBlock(1);
          if (block2.Length <= 0 || block2.Length > 32)
            break;
          Messenger friend2 = usr.GetFriend(block2);
          if (friend2 == null || friend2.status != 0 && friend2.status != 1 || !friend2.isOnline)
            break;
          usr.send((Packet) new SP_Unknown((ushort) 32256, new object[4]
          {
            (object) 1,
            (object) 5609,
            (object) friend2.nickname,
            (object) 0
          }));
          break;
        case 5610:
          string Query1 = this.getBlock(1).Trim();
          if (Query1.Length <= 0 || Query1.Length > 32)
            break;
          DataTable dataTable1 = DB.RunReader("SELECT * FROM users WHERE nickname='" + DB.Stripslash(Query1) + "'");
          if (dataTable1.Rows.Count <= 0)
            break;
          int id1 = int.Parse(dataTable1.Rows[0]["id"].ToString());
          if (id1 == usr.userId)
            break;
          DB.RunQuery("UPDATE friends SET requesterid='-1', status='1' WHERE id1='" + (object) id1 + "' AND id2='" + (object) usr.userId + "' OR id1='" + (object) usr.userId + "' AND id2='" + (object) id1 + "'");
          Game_Server.User user2 = UserManager.GetUser(id1);
          if (user2 != null)
          {
            Messenger friend3 = user2.GetFriend(usr.userId);
            if (friend3 != null)
            {
              friend3.status = 1;
              friend3.isOnline = true;
            }
            user2.send((Packet) new SP_MessengerFriends(user2));
          }
          Messenger friend4 = usr.GetFriend(id1);
          if (friend4 != null)
          {
            if (user2 != null)
              friend4.isOnline = true;
            friend4.status = 1;
          }
          usr.send((Packet) new SP_MessengerFriends(usr));
          break;
        case 5611:
          string block3 = this.getBlock(1);
          if (block3.Length <= 0 || block3.Length > 32)
            break;
          int num1 = int.Parse(DB.RunReaderOnce("id", "SELECT * FROM users WHERE nickname='" + DB.Stripslash(block3) + "'").ToString());
          if (num1 <= 0)
            break;
          DB.RunQuery("DELETE FROM friends WHERE id1='" + (object) num1 + "' AND id2='" + (object) usr.userId + "' OR id1='" + (object) usr.userId + "' AND id2='" + (object) num1 + "'");
          Game_Server.User user3 = UserManager.GetUser(num1);
          if (user3 != null)
          {
            Messenger messenger;
            user3.Friends.TryRemove(usr.userId, out messenger);
            user3.send((Packet) new SP_MessengerFriends(usr));
          }
          Messenger messenger1;
          usr.Friends.TryRemove(num1, out messenger1);
          usr.send((Packet) new SP_MessengerFriends(usr));
          break;
        case 5612:
          string block4 = this.getBlock(1);
          if (block4.Length <= 0 || block4.Length > 32)
            break;
          int id2 = int.Parse(DB.RunReaderOnce("id", "SELECT * FROM users WHERE nickname='" + DB.Stripslash(block4) + "'").ToString());
          if (id2 <= 0)
            break;
          Messenger friend5 = usr.GetFriend(id2);
          friend5.status = friend5.status == 1 ? 2 : 1;
          DB.RunQuery("UPDATE friends SET status='" + (object) friend5.status + "' WHERE id1='" + (object) id2 + "' AND id2='" + (object) usr.userId + "' OR id1='" + (object) usr.userId + "' AND id2='" + (object) id2 + "'");
          Game_Server.User user4 = UserManager.GetUser(id2);
          if (user4 != null)
          {
            user4.send((Packet) new SP_MessengerFriends(user4));
            Messenger friend3 = usr.GetFriend(id2);
            if (friend3 != null)
              friend3.status = friend5.status;
          }
          usr.send((Packet) new SP_MessengerFriends(usr));
          break;
        case 5615:
          string block5 = this.getBlock(1);
          if (block5.Length <= 0 || block5.Length > 32)
            break;
          DataTable dataTable2 = DB.RunReader("SELECT * FROM users WHERE nickname='" + block5 + "'");
          if (dataTable2.Rows.Count > 0)
          {
            int num2 = int.Parse(dataTable2.Rows[0]["id"].ToString());
            if (num2 == usr.userId || usr.userId == -1 || num2 <= 0)
              break;
            DB.RunQuery("INSERT INTO friends (id1, id2, requesterid, status) VALUES ('" + (object) usr.userId + "', '" + (object) num2 + "', '" + (object) usr.userId + "', '5')");
            Game_Server.User user5 = UserManager.GetUser(num2);
            byte[] bytes2 = new SP_MessengerFriendRequest(usr.nickname, block5).GetBytes();
            if (user5 != null)
            {
              user5.AddFriend(usr.userId, usr.userId);
              user5.sendBuffer(bytes2);
              user5.send((Packet) new SP_MessengerFriends(usr));
            }
            usr.AddFriend(num2, usr.userId);
            usr.sendBuffer(bytes2);
            usr.send((Packet) new SP_MessengerFriends(usr));
            break;
          }
          usr.send((Packet) new SP_Messenger(SP_Messenger.Subtype.InvalidNickname));
          break;
        case 5616:
          string Query2 = this.getBlock(1).Trim();
          if (Query2.Length <= 0 || Query2.Length > 32)
            break;
          int num3 = int.Parse(DB.RunReaderOnce("id", "SELECT * FROM users WHERE nickname='" + DB.Stripslash(Query2) + "'").ToString());
          if (num3 <= 0)
            break;
          DB.RunQuery("DELETE FROM friends WHERE id1='" + (object) num3 + "' AND id2='" + (object) usr.userId + "' OR id1='" + (object) usr.userId + "' AND id2='" + (object) num3 + "'");
          Game_Server.User user6 = UserManager.GetUser(num3);
          if (user6 != null)
          {
            usr.RemoveFriend(usr.userId);
            user6.send((Packet) new SP_MessengerFriends(user6));
          }
          usr.RemoveFriend(num3);
          usr.send((Packet) new SP_MessengerFriends(usr));
          break;
      }
    }

    internal enum Subtype
    {
      FriendList = 5606, // 0x000015E6
      SendMessage = 5608, // 0x000015E8
      AvailableToChat = 5609, // 0x000015E9
      FriendAccept = 5610, // 0x000015EA
      DeleteFriend = 5611, // 0x000015EB
      BlockUnblock = 5612, // 0x000015EC
      FriendRequest = 5615, // 0x000015EF
      FriendDecline = 5616, // 0x000015F0
    }
  }
}
