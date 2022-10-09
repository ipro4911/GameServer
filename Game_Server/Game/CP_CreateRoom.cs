// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CreateRoom
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class CP_CreateRoom : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Channel channel = ChannelManager.channels[usr.channel];
      int getOpenId = channel.GetOpenID;
      if (getOpenId < 0 || usr.room != null)
        return;
            Room room = new Room();
      room.mapid = int.Parse(this.getBlock(4));
      if (room.isPremMap(room.mapid) && usr.premium < (byte) 1)
      {
        usr.send((Packet) new SP_CreateRoom(SP_CreateRoom.ErrorCode.FailedToCreate));
      }
      else
      {
        room.id = getOpenId;
        room.channel = usr.channel;
                
                room.name = this.getBlock(0);
               
        room.enablepassword = int.Parse(this.getBlock(1));
                
        room.maxusers = int.Parse(this.getBlock(3)) + 1;
                
                room.password = this.getBlock(2);
                room.supermaster = usr.HasItem("CC02"); // sent wrongly in chapter 1,man good old times 
        room.mode = int.Parse(this.getBlock(5));
        room.type = int.Parse(this.getBlock(7));
        room.ping = int.Parse(this.getBlock(4));

        if (room.channel == 3)
        {
          room.type = 0;
          room.zombiedifficulty = (int) byte.Parse(this.getBlock(13));
        }
        room.levellimit = int.Parse(this.getBlock(8));
        room.premiumonly = int.Parse(this.getBlock(9));
        room.votekickOption = int.Parse(this.getBlock(10));
        int i = room.mode == 0 || room.mode == 7 || room.mode == 15 ? 12 : 11;
        room.ping = int.Parse(this.getBlock(11));
        room.rounds = int.Parse(this.getBlock(i));
        room.timelimit = int.Parse(this.getBlock(14));
        room.autostart = int.Parse(this.getBlock(16)) == 1;
                if (room.name.StartsWith("E|") && usr.rank > 5)//  && room.name.Length > 2)
                {
                    room.name = room.name.Substring(2);
                    room.type = 3;
                }
                if (room.type == 1)
        {
          room.type = 0;
          if (usr.clan != null)
          {
            int num = usr.clan.clanRank(usr);
            if (num > 0 && num < 9)
              room.type = 1;
          }
        }
        if (room.type == 1 && room.mode == 1)
          room.mode = 0;
        else if (room.mode == 1 && usr.premium == (byte) 0)
          room.rounds = 2;
        room.new_mode = (int) byte.Parse(this.getBlock(17));
        room.new_mode_sub = int.Parse(this.getBlock(18));
                
                if (room.new_mode > 6)
          room.new_mode = 6;
                
                room.password = this.getBlock(2);
                if ((int) usr.level >= 10 * (room.levellimit - 1) + 1 || usr.level <= (byte) 10 && room.levellimit == 1 || room.levellimit == 0)
        {
          switch (room.channel)
          {
            case 1:
              switch (room.maxusers)
              {
                case 1:
                  room.maxusers = 8;
                  break;
                case 2:
                  room.maxusers = 16;
                  break;
                case 3:
                  room.maxusers = 20;
                  break;
                case 4:
                  room.maxusers = 24;
                  break;
              }
              break;
            case 2:
              switch (room.maxusers)
              {
                case 1:
                  room.maxusers = 8;
                  break;
                case 2:
                  room.maxusers = 16;
                  break;
                case 3:
                  room.maxusers = 20;
                  break;
                case 4:
                  room.maxusers = 24;
                  break;
                case 5:
                  room.maxusers = 32;
                  break;
              }
                            break;
               case 3:
              room.zombiedifficulty = (int) byte.Parse(this.getBlock(13));
              room.maxusers = 4;
              break;
          }
          
                    room.ch = channel;
          if (channel.AddRoom(getOpenId, room))
          {
            if (room.JoinUser(usr, 0))
            {
              if (room.channel == 3 && room.mode != 13)
              {
                for (int index = 0; index < 28; ++index)
                  room.Zombies.Add(index + 4, new Zombie(index + 4, 0, 0, 0));
              }
              usr.send((Packet) new SP_JoinRoom(usr, room));
              byte[] bytes1 = new SP_RoomListUpdate(room, 0).GetBytes();
              byte[] bytes2 = new SP_RoomListUpdate(room, 1).GetBytes();
              foreach (Game_Server.User user in UserManager.GetUsersInChannel(room.channel, false))
              {
                if ((Decimal) user.lobbypage == Math.Floor((Decimal) (room.id / 13)))
                {
                  user.sendBuffer(bytes1);
                  user.sendBuffer(bytes2);
                }
              }
              UserManager.UpdateUserlist(usr);
            }
            else
            {
              Log.WriteError("Error: " + usr.nickname + " while adding room to the stuck");
              usr.send((Packet) new SP_CreateRoom(SP_CreateRoom.ErrorCode.FailedToCreate));
            }
          }
          else
          {
            Log.WriteError("Error: " + usr.nickname + " while creating a room @ join [0]");
            usr.send((Packet) new SP_CreateRoom(SP_CreateRoom.ErrorCode.FailedToCreate));
          }
        }
        else
          usr.send((Packet) new SP_CreateRoom(SP_CreateRoom.ErrorCode.FailedToCreate));
      }
    }
  }
}
