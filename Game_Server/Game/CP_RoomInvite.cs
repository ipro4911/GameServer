// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_RoomInvite
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_RoomInvite : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room == null || usr.room.users.Count >= usr.room.maxusers)
        return;
      string block1 = this.getBlock(0);
      string block2 = this.getBlock(1);
      if (block1 == "NULL")
      {
        byte[] bytes = new SP_RoomInvite(usr, block2).GetBytes();
        Game_Server.User randomUser = UserManager.GetRandomUser();
        if (randomUser == null || randomUser.room != null || (randomUser.channel != usr.channel || randomUser.userId == usr.userId))
          return;
        randomUser.sendBuffer(bytes);
        usr.sendBuffer(bytes);
      }
      else
      {
        byte[] bytes = new SP_RoomInvite(usr, block2).GetBytes();
        Game_Server.User user = UserManager.GetUser(block1);
        if (user != null)
        {
          if (user.room == null)
          {
            if (user.channel != usr.channel)
              return;
            user.sendBuffer(bytes);
            usr.sendBuffer(bytes);
          }
          else
            usr.send((Packet) new SP_RoomInvite(SP_RoomInvite.ErrorCodes.IsPlaying));
        }
        else
          usr.send((Packet) new SP_RoomInvite(SP_RoomInvite.ErrorCodes.GenericError));
      }
    }
  }
}
