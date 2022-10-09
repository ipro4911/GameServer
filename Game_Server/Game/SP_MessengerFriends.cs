// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_MessengerFriends
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_MessengerFriends : Packet
  {
    public SP_MessengerFriends(Game_Server.User usr)
    {
      this.newPacket((ushort) 32256);
      this.addBlock((object) 1);
      this.addBlock((object) 5606);
      this.addBlock((object) usr.Friends.Count);
      foreach (Messenger messenger in (IEnumerable<Messenger>) usr.Friends.Values)
      {
        if (messenger.id > 0 && messenger != null)
        {
          this.addBlock((object) 1);
          this.addBlock((object) messenger.nickname);
          this.addBlock((object) (UserManager.GetUser(messenger.id) != null ? 1 : 0));
          this.addBlock((object) (messenger.requesterId != usr.userId || messenger.status != 5 ? messenger.status : 4));
        }
      }
    }
  }
}
