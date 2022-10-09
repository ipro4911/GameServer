// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_Spectate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_Spectate : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.rank > 2 && usr.channel != -1)
      {
        switch (int.Parse(this.getBlock(0)))
        {
          case 0:
            if (usr.room != null)
              usr.room.RemoveSpectator(usr);
            usr.lobbypage = 0;
            usr.send((Packet) new SP_RoomList(usr, usr.lobbypage, false, 0, 1));
            break;
          case 1:
            int roomId = int.Parse(this.getBlock(1));
            Room room = ChannelManager.channels[usr.channel].GetRoom(roomId);
            if (room == null)
              break;
            if (room.AddSpectator(usr))
            {
              usr.send((Packet) new SP_Spectate(usr, room));
              room.InitializeSpectatorUDP(usr);
              break;
            }
            usr.send((Packet) new SP_Chat("SPECTATE", SP_Chat.ChatType.Room_ToAll, "SPECTATE >> There is no empty slot for this room!", 999U, usr.nickname));
            break;
        }
      }
      else
      {
        Log.WriteError(usr.nickname + " tried to spectate (Rank: " + (object) usr.rank + " - AccessLevel: " + (object) usr.accesslevel + ")");
        usr.disconnect();
      }
    }

    internal enum Type
    {
      LeaveRoom,
      JoinRoom,
    }
  }
}
