// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CarePackage
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_CarePackage : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null || !Game_Server.Configs.Server.Player.CarePackage)
        return;
      usr.send((Packet) new SP_CARE_PACKAGE());
    }
  }
}
