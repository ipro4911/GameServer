// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_PlayerInfoSpectate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class SP_PlayerInfoSpectate : Packet
  {
    public SP_PlayerInfoSpectate(Game_Server.User usr, Room Room)
    {
      this.newPacket((ushort) 29953);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) usr.spectatorId);
      this.addBlock((object) usr.userId);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) "0");
      this.addBlock((object) usr.accesslevel);
      //UserManager.sendToServer((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "A GM is Spectating your room", 0U, "NULL"));

        }


        public SP_PlayerInfoSpectate(Game_Server.User usr)
    {
      this.newPacket((ushort) 29953);
      this.addBlock((object) 1);
      this.addBlock((object) 1);
      this.addBlock((object) usr.spectatorId);
      this.addBlock((object) usr.userId);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) "0");
        //  addBlock(usr.Class);
         //   addBlock(usr.weapon);
      this.addBlock((object) usr.accesslevel);
      this.addBlock((object) usr.RemoteIP);
      this.addBlock((object) usr.RemotePort);
      this.addBlock((object) usr.LocalIP);
      this.addBlock((object) usr.LocalPort);
      this.addBlock((object) 0);
    }
  }
}
