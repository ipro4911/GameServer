// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_ZombieSkillPointRequest
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_ZombieSkillPointRequest : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Room room = usr.room;
      if (room == null || !room.gameactive || (room.channel != 3 || room.mode != 11))
        return;
      int num = int.Parse(this.getBlock(1));
      bool flag = true;
      switch (num)
      {
        case 1:
          if (usr.skillPoints < 5)
          {
            usr.disconnect();
            flag = false;
            break;
          }
          break;
        case 2:
          if (usr.skillPoints < 10)
          {
            usr.disconnect();
            flag = false;
            break;
          }
          break;
        case 3:
          if (usr.skillPoints < 20)
          {
            usr.disconnect();
            flag = false;
            break;
          }
          break;
        default:
          flag = false;
          break;
      }
      if (!flag)
        return;
      usr.skillPoints = 0;
      room.send((Packet) new SP_Unknown((ushort) 31492, (object[]) this.getAllBlocks));
    }
  }
}
