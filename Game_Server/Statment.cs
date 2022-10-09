// Decompiled with JetBrains decompiler
// Type: Game_Server.Statment
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;

namespace Game_Server
{
  public class Statment : IDisposable
  {
    public readonly Dictionary<string, object> parameters = new Dictionary<string, object>();
    public string query;

    public Statment(string query)
    {
      this.query = query;
    }

    public void AddValue(string key, object value)
    {
      if (this.parameters.ContainsKey(key))
        return;
      this.parameters.Add(key, value);
    }

    protected virtual void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
