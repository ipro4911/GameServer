// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomDataHandler
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Room_Data
{
  internal class RoomDataHandler : IDisposable
  {
    private int subtype;
    public object[] sendBlocks;
    public bool lobbychanges;
    public bool sendPacket;

    public void FillData(int subtype, object[] sendBlocks)
    {
      this.subtype = subtype;
      this.sendBlocks = sendBlocks;
      this.sendPacket = false;
    }

    public string[] getAllBlocks
    {
      get
      {
        return (string[]) this.sendBlocks;
      }
    }

    public string getBlock(int i)
    {
      if (this.sendBlocks[i] != null)
        return this.sendBlocks[i].ToString();
      return (string) null;
    }

    public virtual void Handle(User usr, Room room)
    {
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
