//// Decompiled with JetBrains decompiler
//// Type: Game_Server.Game.CP_LeaveRoom
//// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
//// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

//namespace Game_Server.Game
//{
//  internal class CP_LeaveRoom : Handler
//  {
//    public override void Handle(Game_Server.User usr)
//    {
//      Room room = usr.room;
//      if (room == null)
//        return;
//      if (room.users.ContainsKey(usr.roomslot))
//      {
//        if (room.users[usr.roomslot].userId == usr.userId)
//        {
//          room.RemoveUser(usr.roomslot);
//        }
//        else
//        {
//          Log.WriteError("Something went wrong while leaving room");
//          usr.disconnect();
//        }
//      }
//      usr.lobbypage = 0;
//      usr.send((Packet) new SP_RoomList(usr, usr.lobbypage, false, 0, 1));
//    }
//  }
//}
