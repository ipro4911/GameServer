// Decompiled with JetBrains decompiler
// Type: Game_Server.Anti_Cheat.Structure.Packet
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Text;

namespace Game_Server.Anti_Cheat.Structure
{
  internal class Packet
  {
    private string[] blocks = new string[0];
    private int packetId;

    public byte[] GetBytes()
    {
      string str = Environment.TickCount.ToString() + " " + (object) this.packetId + " ";
      for (int index = 0; index < this.blocks.Length; ++index)
        str = str + this.blocks[index].Replace(' ', '\x001D') + " ";
      return Cryption.encrypt(Encoding.GetEncoding("Windows-1250").GetBytes(str.ToString() + (object) ' ' + (object) '\n'));
    }

    protected void newPacket(int packetId)
    {
      this.packetId = packetId;
    }

    protected void addBlock(object block)
    {
      int newSize = this.blocks.Length + 1;
      Array.Resize<string>(ref this.blocks, newSize);
      this.blocks[newSize - 1] = block.ToString();
    }

    protected void Fill(object block, int length)
    {
      int newSize = this.blocks.Length + length;
      Array.Resize<string>(ref this.blocks, newSize);
      for (int index = 1; index <= length; ++index)
        this.blocks[newSize - index] = block.ToString();
    }
  }
}
