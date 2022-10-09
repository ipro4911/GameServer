// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_JoinRoom
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class CP_JoinRoom : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null)
      {
        usr.disconnect();
      }
      else
      {
        int roomId = int.Parse(this.getBlock(0));
        string block = this.getBlock(1);
        int side = int.Parse(this.getBlock(2));
        Room room = ChannelManager.channels[usr.channel].GetRoom(roomId);
        try
        {
          if (room != null)
          {
            if (room.isJoinable)
            {
              if (!room.EndGamefreeze)
              {
                bool flag = (int) usr.level >= 10 * (room.levellimit - 1) + 1 || usr.level <= (byte) 10 && room.levellimit == 1 || room.levellimit == 0;
                int num = usr.clan != null ? usr.clan.clanRank(usr) : 9;
                if (!flag)
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.BadLevel));
                else if (room.enablepassword == 1 && block != room.password)
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.InvalidPassword));
                else if (usr.premium < (byte) 1 && room.premiumonly == 1)
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.OnlyPremium));
                else if (room.type == 1 && (num == -1 || num == 9))
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
                else if (room.type == 1 && (room.SideCountDerb > 0 && room.SideCountNIU > 0 && !room.isMyClan(usr) || usr.clan == null))
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
                else if (room.users.Count >= room.maxusers || room.userlimit && usr.rank < 3)
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
                else if (room.JoinUser(usr, side))
                {
                  room.ch.UpdateLobby(room);
                  room.InitializeUDP(usr);
                  UserManager.UpdateUserlist(usr);
                }
                else
                  usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
              }
              else
                usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GameIsEnding));
            }
            else if (room.isGameEnding && room.gameactive)
              usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GameIsEnding));
            else
              usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
          }
          else
            usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
        }
        catch (Exception ex)
        {
          Log.WriteError("Cannot join in the room:\r\n" + ex.Message + "\r\n" + ex.StackTrace);
          usr.send((Packet) new SP_JoinRoom(SP_JoinRoom.ErrorCodes.GenericError));
        }
      }
    }
  }
}
