// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.TotalWar
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.GameModes
{
  internal class TotalWar
  {
    private Room room;

    ~TotalWar()
    {
      GC.Collect();
    }

    public TotalWar(Room room)
    {
      this.room = room;
    }

    public void Update()
    {
      if (this.room == null)
        return;
      if (this.room.timeleft <= 0)
      {
        this.room.EndGame();
      }
      else
      {
        if (this.room.TotalWarDerb < this.room.kills && this.room.TotalWarNIU < this.room.kills)
          return;
        this.room.EndGame();
      }
    }
  }
}
