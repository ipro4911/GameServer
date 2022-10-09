// Decompiled with JetBrains decompiler
// Type: Game_Server.Anti_Cheat.Structure.Handler
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Anti_Cheat.Structure
{
  internal class Handler
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

    public virtual void Handle(Client usr)
    {
    }
  }
}
