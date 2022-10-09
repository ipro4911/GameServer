// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_RoomList
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_RoomList : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      int type = int.Parse(this.getBlock(1));
      bool waiting = type != 1;
      int pageIdx = 0;
      switch (type)
      {
        case 1:
          usr.lobbypage = int.Parse(this.getBlock(0));
          break;
        case 2:
          pageIdx = int.Parse(this.getBlock(0));
          usr.lobbypage = pageIdx / 13;
          break;
        case 3:
          pageIdx = int.Parse(this.getBlock(0));
          break;
      }
      usr.send((Packet) new SP_RoomList(usr, usr.lobbypage, waiting, pageIdx, type));
    }
  }
}
