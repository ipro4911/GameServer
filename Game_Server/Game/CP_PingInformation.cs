// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_PingInformation
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_PingInformation : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      int.Parse(this.getBlock(0));
      int num = (int) uint.Parse(this.getBlock(1));
      if (usr.sessionStart + 5 >= Generic.timestamp || usr.tcpClient != null || Game_Server.Configs.Server.Debug)
        return;
      Log.WriteDebug("[DEBUG] " + usr.nickname + " No TCP Client - kick out");
      usr.disconnect();
    }
  }
}
