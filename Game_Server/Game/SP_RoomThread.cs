// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_RoomThread
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_RoomThread : Packet
  {
    public SP_RoomThread(Room room, int type = 0)
    {
      this.newPacket((ushort) 30016);
      if (room.channel == 3)
      {
        this.addBlock((object) (room.timeattack != null ? room.timeleft : 0));
        this.addBlock((object) room.timespent);
        this.addBlock((object) room.ZombiePoints);
        this.Fill(0, 2);
      }
      else if (room.mode == 8)
      {
        this.addBlock((object) room.timespent);
        this.Fill((object) 0, 4);
        this.addBlock((object) room.kills);
        this.addBlock((object) 0);
        this.addBlock((object) room.timeleft);
        this.Fill((object) 0, 2);
        this.addBlock((object) room.TotalWarDerb);
        this.addBlock((object) room.TotalWarNIU);
      }
      else
      {
        this.addBlock((object) room.timespent);
        this.addBlock((object) room.timeleft);
        if (room.mode == 2 || room.mode == 3 || (room.mode == 4 || room.mode == 5) || room.mode == 16 && (room.mapid == 90 || room.mapid == 91))
        {
          this.addBlock((object) room.AliveDerb);
          this.addBlock((object) room.AliveNIU);
          this.addBlock((object) room.KillsDerbaranLeft);
          this.addBlock((object) room.KillsNIULeft);
        }
        else if (room.mode == 0 || room.mode == 7 || room.mode == 15)
        {
          if (room.mode == 15)
            this.addBlock((object) 0);
          this.addBlock((object) room.DerbRounds);
          this.addBlock((object) room.NIURounds);
          this.addBlock((object) (room.mode == 7 ? room.derbHeroKill : room.AliveDerb));
          this.addBlock((object) (room.mode == 7 ? room.niuHeroKill : room.AliveNIU));
          this.addBlock((object) (room.mode == 7 ? room.derbHeroUsr : room.DerbRounds));
          this.addBlock((object) (room.mode == 7 ? room.niuHeroUsr : room.NIURounds));
        }
        else
        {
          this.addBlock((object) room.DerbRounds);
          this.addBlock((object) room.NIURounds);
          this.addBlock((object) room.ffakillpoints);
          this.addBlock((object) room.highestkills);
        }
      }
      this.addBlock((object) 2);
      this.addBlock((object) type);
      this.addBlock((object) room.ConquestCountdown);
      if (room.mode != 5)
        return;
      switch (room.mapid)
      {
        case 42:
          this.addBlock((object) (room.Mission1 == null ? 0 : 1));
          this.addBlock(room.Mission1 == null ? (object) "NONE" : (object) room.Mission1);
          this.addBlock((object) (room.Mission2 == null ? 0 : 1));
          this.addBlock(room.Mission2 == null ? (object) "NONE" : (object) room.Mission2);
          this.addBlock((object) (room.Mission3 == null ? 0 : 1));
          this.addBlock(room.Mission3 == null ? (object) "NONE" : (object) room.Mission3);
          this.addBlock((object) room.GetActualMission);
          break;
        case 60:
          this.addBlock((object) (room.Mission1 == null ? 0 : 1));
          this.addBlock(room.Mission1 == null ? (object) "NONE" : (object) room.Mission1);
          this.addBlock((object) (room.Mission2 == null ? 0 : 1));
          this.addBlock(room.Mission2 == null ? (object) "NONE" : (object) room.Mission2);
          this.addBlock((object) (room.Mission3 == null ? 0 : 1));
          this.addBlock(room.Mission3 == null ? (object) "NONE" : (object) room.Mission3);
          this.addBlock((object) room.GetActualMission);
          if (room.Mission1 == null)
          {
            this.addBlock((object) -2);
            this.Fill((object) 0, 2);
            this.addBlock((object) 1);
            this.addBlock((object) ((room.HackPercentage.BaseA + room.HackPercentage.BaseB).ToString() + ".0000"));
            break;
          }
          this.addBlock((object) 0);
          break;
      }
    }
  }
}
