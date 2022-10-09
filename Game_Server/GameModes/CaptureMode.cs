// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.CaptureMode
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.GameModes
{
  internal class CaptureMode
  {
    private Room room;
    public int NIUPoints;
    public int DerbaranPoints;

    ~CaptureMode()
    {
      GC.Collect();
    }

    public void Update()
    {
      Room room = this.room;
    }

    public CaptureMode(Room room)
    {
      this.room = room;
      this.NIUPoints = this.DerbaranPoints = 0;
    }
  }
}
