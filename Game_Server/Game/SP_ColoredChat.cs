// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ColoredChat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Drawing;

namespace Game_Server.Game
{
  internal class SP_ColoredChat : Packet
  {
    public SP_ColoredChat(string Message, SP_ColoredChat.ChatType type, Color color)
    {
      this.newPacket((ushort) 29697);
      this.addBlock((object) 1);
      this.addBlock((object) Message);
      this.addBlock((object) (int) type);
      this.addBlock((object) (int) color.R);
      this.addBlock((object) (int) color.G);
      this.addBlock((object) (int) color.B);
    }

    internal enum ChatType
    {
      Normal = 0,
      Clan = 2,
    }
  }
}
