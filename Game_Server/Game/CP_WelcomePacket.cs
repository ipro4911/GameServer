// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_WelcomePacket
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_WelcomePacket : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      usr.macAddress = this.getBlock(2);
      if (usr.macAddress.Length > 0 && Generic.IsAlphaNumeric(usr.macAddress))
      {
        if (!BanManager.isMacBanned(usr.macAddress))
        {
          usr.send((Packet) new SP_WelcomePacket(usr));
        }
        else
        {
          Log.WriteError(usr.IP + " -> tried to connect with Mac Banned");
          usr.disconnect();
        }
      }
      else
      {
        Log.WriteError("Invalid Mac Address from " + usr.username);
        usr.disconnect();
      }
    }
  }
}
