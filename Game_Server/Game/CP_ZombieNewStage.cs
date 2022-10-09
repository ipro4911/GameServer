// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_ZombieNewStage
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_ZombieNewStage : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Room room = usr.room;
      Stopwatch stopwatch = new Stopwatch();
      int num1 = int.Parse(this.getBlock(0));
      int num2 = int.Parse(this.getBlock(1));
      if (num1 == 4 && num2 == 0)
      {
        room.timeattack.IsTimeOpenDoor = true;
        ++room.timeattack.PowPlayer;
      }
      if (num1 == 4 && num2 == 1)
      {
        room.timeattack.IsTimeOpenDoor = true;
        ++room.timeattack.PowPlayer;
      }
      if (num1 == 4 && num2 == 2 && room.zombiedifficulty == 1)
      {
        room.timeattack.IsTimeOpenDoor = true;
        ++room.timeattack.PowPlayer;
      }
      if (num1 != 6 || room.mode != 12 || usr.RandomSupplyBoxSelected)
        return;
      usr.RandomSupplyBoxSelected = true;
      int choose = int.Parse(this.getBlock(1));
      usr.timeattackBoxChoose = choose;
      int days = 30;
      string ItemCode;
      switch (new Random().Next(0, 4))
      {
        case 1:
          ItemCode = "DB29";
          break;
        case 2:
          ItemCode = "D908";
          break;
        case 3:
          ItemCode = "DM09";
          break;
        default:
          ItemCode = "DA46";
          break;
      }
      using (IEnumerator<Game_Server.User> enumerator = room.users.Values.OrderByDescending<Game_Server.User, int>((Func<Game_Server.User, int>) (u => u.kills)).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        Game_Server.User current = enumerator.Current;
        usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> You got '" + ItemCode + "' for " + (object) days + " days.", 999U, "NULL"));
        Inventory.AddItem(usr, ItemCode, days);
        usr.send((Packet) new SP_ZombieEndGameItem(choose, ItemCode, usr.roomslot, days));
        room.timeattack.waitBeforeSupplyBoxItemsOut = 20;
      }
    }
  }
}
