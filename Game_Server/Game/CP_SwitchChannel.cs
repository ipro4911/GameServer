// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_SwitchChannel
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_SwitchChannel : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      int num = int.Parse(this.getBlock(0));
      if (num >= 1 || num <= 3)
      {
        if (num == 1 && Game_Server.Configs.Server.Channels.Infantry || num == 2 && Game_Server.Configs.Server.Channels.Vehicular || num == 3 && Game_Server.Configs.Server.Channels.Zombie)
          usr.channel = num;
        else
          usr.send((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Lobby_ToAll, Game_Server.Configs.Server.SystemName + " >> This channel has been disabled, please choose a other one", 999U, "NULL"));
        usr.send((Packet) new SP_SwitchChannel(usr.channel));
        usr.lobbypage = 0;
        usr.send((Packet) new SP_RoomList(usr, usr.lobbypage, false, 0, 1));
        UserManager.UpdateUserlist(usr);
      }
      else
        usr.disconnect();
    }
  }
}
