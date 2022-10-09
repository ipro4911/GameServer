// Decompiled with JetBrains decompiler
// Type: Game_Server.Configs.Web
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Configs
{
  internal class Web
  {
    public static bool allow = false;
    public static int port = 7352;
    public static bool remote = false;

    public static void Load()
    {
      try
      {
        Web.allow = bool.Parse(IO.ReadValue("WebServer", "Enabled"));
        Web.port = int.Parse(IO.ReadValue("WebServer", "Port"));
        Web.remote = bool.Parse(IO.ReadValue("WebServer", "AllowRemoteRequest"));
      }
      catch (Exception ex)
      {
        Log.WriteError("Couldn't Load server info " + ex.Message);
      }
    }
  }
}
