// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ScoreboardInformations
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_ScoreboardInformations : Packet
  {
    public SP_ScoreboardInformations(Room r, long Time)
    {
      this.newPacket((ushort) 30053);
      this.addBlock((object) 3);
      if (r.timeattack == null)
        return;
      this.addBlock((object) Time);
      switch (r.timeattack.Stage)
      {
        case 0:
          using (IEnumerator<Game_Server.User> enumerator = r.users.Values.OrderByDescending<Game_Server.User, int>((Func<Game_Server.User, int>) (u => u.kills)).Take<Game_Server.User>(2).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Game_Server.User current = enumerator.Current;
              this.addBlock((object) current.roomslot);
              this.addBlock((object) (current.rKills > r.timeattack.zombieForStage ? r.timeattack.zombieForStage : current.rKills));
            }
            break;
          }
        case 1:
          using (IEnumerator<Game_Server.User> enumerator = r.users.Values.OrderByDescending<Game_Server.User, int>((Func<Game_Server.User, int>) (u => u.hackPercentage)).Take<Game_Server.User>(2).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Game_Server.User current = enumerator.Current;
              this.addBlock((object) current.roomslot);
              this.addBlock((object) current.hackPercentage);
            }
            break;
          }
        case 2:
          using (IEnumerator<Game_Server.User> enumerator = r.users.Values.OrderByDescending<Game_Server.User, int>((Func<Game_Server.User, int>) (u => u.timeattackDamagedDoor)).Take<Game_Server.User>(2).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Game_Server.User current = enumerator.Current;
              this.addBlock((object) current.roomslot);
              this.addBlock((object) current.timeattackDamagedDoor);
            }
            break;
          }
        case 3:
          using (IEnumerator<Game_Server.User> enumerator = r.users.Values.OrderByDescending<Game_Server.User, int>((Func<Game_Server.User, int>) (u => (int)u.timeattackBossDamage)).Take<Game_Server.User>(2).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Game_Server.User current = enumerator.Current;
              this.addBlock((object) current.roomslot);
              this.addBlock((object) current.timeattackBossDamage);
            }
            break;
          }
        default:
          this.addBlock((object) 0);
          this.addBlock((object) 0);
          this.addBlock((object) 0);
          this.addBlock((object) 0);
          break;
      }
    }
  }
}
