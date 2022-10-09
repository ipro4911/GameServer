// Decompiled with JetBrains decompiler
// Type: Game_Server.Placement
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server
{
  internal class Placement
  {
    public int ID;
    public User Planter;
    public string Code;
    public bool Used;

    ~Placement()
    {
      GC.Collect();
    }

    public Placement(int id, User planter, string itemcode)
    {
      this.ID = id;
      this.Planter = planter;
      this.Code = itemcode;
      this.Used = false;
    }
  }
}
