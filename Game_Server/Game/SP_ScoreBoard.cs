// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_ScoreBoard
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;

namespace Game_Server.Game
{
  internal class SP_ScoreBoard : Packet
  {
    public SP_ScoreBoard(Room Room)
    {
      this.newPacket((ushort) 30032);
      this.addBlock((object) 1);
      this.addBlock((object) (Room.mode == 0 || Room.mode == 7 || Room.mode == 15 ? Room.DerbRounds : 0));
      this.addBlock((object) (Room.mode == 0 || Room.mode == 7 || Room.mode == 15 ? Room.NIURounds : 0));
      switch (Room.mode)
      {
        case 0:
        case 7:
          this.addBlock((object) Room.derbHeroKill);
          this.addBlock((object) Room.niuHeroKill);
          break;
        case 1:
          this.addBlock((object) Room.ffakillpoints);
          this.addBlock((object) Room.highestkills);
          break;
        case 2:
        case 3:
        case 4:
        case 5:
        case 8:
        case 16:
          this.addBlock((object) Room.KillsDerbaranLeft);
          this.addBlock((object) Room.KillsNIULeft);
          break;
        default:
          this.addBlock((object) 0);
          this.addBlock((object) 0);
          break;
      }
      this.addBlock((object) Room.users.Count);
      foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) Room.users.Values)
      {
        this.addBlock((object) user.roomslot);
        this.addBlock((object) user.rKills);
        this.addBlock((object) user.rDeaths);
        this.addBlock((object) user.rFlags);
        this.addBlock((object) user.rPoints);
        this.addBlock((object) 1);
      }
    }
  }
}
