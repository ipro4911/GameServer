// Decompiled with JetBrains decompiler
// Type: Game_Server.RetailSystem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Game_Server
{
  internal class RetailSystem
  {
    private static ConcurrentDictionary<int, string> retails = new ConcurrentDictionary<int, string>();
    public static bool Enabled = false;

    public static string GetRetailByClass(int Class)
    {
      if (RetailSystem.Enabled && RetailSystem.retails.ContainsKey(Class))
      {
        string retail = RetailSystem.retails[Class];
        if (retail != "^")
          return retail;
      }
      return (string) null;
    }

    public static bool IsRetail(string weapon)
    {
      return retails.Values.Where<string>((Func<string, bool>) (w => w == weapon)).Count<string>() > 0;
    }

    public static void LoadRetails()
    {
      try
      {
        RetailSystem.retails.Clear();
        RetailSystem.Enabled = bool.Parse(IO.ReadValue(nameof (RetailSystem), "Enabled"));
        if (!RetailSystem.Enabled)
          return;
        RetailSystem.retails.TryAdd(0, IO.ReadValue(nameof (RetailSystem), "Engineer"));
        RetailSystem.retails.TryAdd(1, IO.ReadValue(nameof (RetailSystem), "Medic"));
        RetailSystem.retails.TryAdd(2, IO.ReadValue(nameof (RetailSystem), "Sniper"));
        RetailSystem.retails.TryAdd(3, IO.ReadValue(nameof (RetailSystem), "Assault"));
        RetailSystem.retails.TryAdd(4, IO.ReadValue(nameof (RetailSystem), "Heavy"));
      }
      catch
      {
        Log.WriteError("Couldn't Load RetaiLSystem");
      }
    }
  }
}
