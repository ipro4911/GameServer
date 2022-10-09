// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Chat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_Chat : Packet
  {
    public SP_Chat(
      string Name,
      SP_Chat.ChatType Type,
      string Message,
      uint TargetID,
      string TargetName)
    {
      this.newPacket((ushort) 29696);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) Name);
      this.addBlock((object) (int) Type);
      this.addBlock((object) TargetID);
      this.addBlock((object) TargetName);
      this.addBlock((object) Message);
    }

    public SP_Chat(
      Game_Server.User usr,
      SP_Chat.ChatType Type,
      string Message,
      long TargetID,
      string TargetName)
    {
      this.newPacket((ushort) 29696);
      this.addBlock((object) 1);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) usr.nickname);
      this.addBlock((object) (int) Type);
      this.addBlock((object) TargetID);
      this.addBlock((object) TargetName);
      this.addBlock((object) Message);
    }

    public SP_Chat(SP_Chat.ErrorCodes ErrCode, params object[] Params)
    {
      this.newPacket((ushort) 29696);
      this.addBlock((object) (int) ErrCode);
      ((IEnumerable<object>) Params).ToList<object>().ForEach((Action<object>) (obj => this.addBlock(obj)));
    }

    internal enum ChatType
    {
      Notice1 = 1,
      Notice2 = 2,
      Lobby_ToChannel = 3,
      Room_ToAll = 4,
      Room_ToTeam = 5,
      Whisper = 6,
      Lobby_ToAll = 8,
      RadioChat = 9,
      Clan = 10, // 0x0000000A
    }

    internal enum ErrorCodes
    {
      ErrorUser = 95040, // 0x00017340
    }
  }
}
