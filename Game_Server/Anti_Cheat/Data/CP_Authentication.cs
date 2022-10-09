// Decompiled with JetBrains decompiler
// Type: Game_Server.Anti_Cheat.Data.CP_Authentication
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Anti_Cheat.Data
{
  internal class CP_Authentication : Game_Server.Anti_Cheat.Structure.Handler
  {
    public override void Handle(Client usr)
    {
      Log.WriteDebug("Received authentication packet from session " + (object) usr.sessionId);
      usr.send((Game_Server.Anti_Cheat.Structure.Packet) new SP_Connect(usr));
    }
  }
}
