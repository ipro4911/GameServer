// Decompiled with JetBrains decompiler
// Type: Game_Server.Configs.Main
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Configs
{
  internal class Main
  {
    public static void setup()
    {
      try
      {
        Server.Load();
        Web.Load();
        RetailSystem.LoadRetails();
        Log.WriteLine("Configs successfully loaded");
      }
      catch (Exception ex)
      {
        Log.WriteError("Couldn't setup configs (" + ex.Message + ") @ " + ex.StackTrace);
      }
    }
  }
}
