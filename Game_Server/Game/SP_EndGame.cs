// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_EndGame
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_EndGame : Packet
  {
    public SP_EndGame(Game_Server.User usr)
    {
      Room room = usr.room;
      Log.WriteLine("[---- " + (object) room.mode + " ----]");
      this.newPacket((ushort) 30048);
      this.addBlock((object) 1);
      if (room.channel != 3)
      {
        this.addBlock((object) usr.ExpEarned);
        this.addBlock((object) usr.DinarEarned);
        this.Fill((object) 0, 2);
        if (room.mode != 5)
        {
          this.addBlock((object) (room.channel != 1 || room.mode != 0 && room.mode != 7 && room.mode != 15 ? room.KillsDerbaranLeft : room.DerbRounds));
          this.addBlock((object) (room.channel != 1 || room.mode != 0 && room.mode != 7 && room.mode != 15 ? room.KillsNIULeft : room.NIURounds));
        }
        else if (room.mapid == 42)
        {
          this.addBlock((object) (room.Mission3 != null ? 1 : 0));
          this.addBlock((object) (room.Mission3 != null ? 0 : 1));
        }
        else if (room.mapid == 60)
        {
          this.addBlock((object) (room.Mission2 != null ? 1 : 0));
          this.addBlock((object) (room.Mission2 != null ? 0 : 1));
        }
        else
          this.Fill((object) 0, 2);
        this.Fill((object) 0, 6);
      }
      else
      {
        if (room.zombie != null)
          this.addBlock((object) (room.zombie.Wave >= (room.zombiedifficulty > 0 ? 18 : 20) ? 1 : 0));
        else if (room.zombiedifficulty == 0)
          this.addBlock((object) (room.timeattack.Destructed ? 1 : 0));
        else
          this.addBlock((object) (room.timeattack.BossKilled ? 1 : 0));
        this.addBlock((object) room.timespent);
        this.addBlock((object) usr.ExpEarned);
        this.addBlock((object) usr.DinarEarned);
        this.Fill((object) 0, 10);
      }
      this.addBlock((object) room.master);
      this.addBlock((object) usr.exp);
      this.addBlock((object) usr.dinar);
    }
  }
}
