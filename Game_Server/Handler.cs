// Decompiled with JetBrains decompiler
// Type: Game_Server.Handler
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server
{
  internal class Handler : IDisposable
  {
    private uint timeStamp;
    public int packetId;
    public string[] blocks;

    public void FillData(uint timeStamp, int packetId, string[] blocks)
    {
      this.timeStamp = timeStamp;
      this.packetId = packetId;
      this.blocks = blocks;
    }

    public string[] getAllBlocks
    {
      get
      {
        return this.blocks;
      }
    }

    public string getBlock(int i)
    {
      if (this.blocks[i] != null)
        return this.blocks[i];
      return (string) null;
    }

    public virtual void Handle(User usr)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
    }

    public void WritePacket()
    {
      Log.WriteDebug(string.Join(" ", this.getAllBlocks));
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
