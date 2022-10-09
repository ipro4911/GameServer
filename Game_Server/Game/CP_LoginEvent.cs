// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_LoginEvent
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_LoginEvent : Handler
  {
    private string getWeapon(int Count)
    {
      switch (Count)
      {
        case 0:
          return "CM06";
        case 1:
          return "CI01";
        case 2:
          return "CD01";
        case 3:
          return "CF02";
        case 4:
          return "CC05";
        case 5:
          return "CA01";
        case 6:
          return "CR16";
        default:
          return "NULL";
      }
    }

    public override void Handle(Game_Server.User usr)
    {
      string weapon = this.getWeapon(usr.rewardEvent.progress);
      int days = 3;
      if (!usr.rewardEvent.doneToday)
      {
        usr.rewardEvent.doneToday = true;
        Item obj = ItemManager.GetItem(weapon);
        if (obj.dinarReward > 0U)
        {
          usr.dinar += (int) obj.dinarReward;
          DB.RunQuery("UPDATE users SET dinar = '" + (object) usr.dinar + "' WHERE id='" + (object) usr.userId + "'");
        }
        else if (weapon.StartsWith("B"))
          Inventory.AddCostume(usr, weapon, days);
        else
          Inventory.AddItem(usr, weapon, days);
        ++usr.rewardEvent.progress;
        usr.send((Packet) new SP_LoginEvent(usr, weapon, days));
        DB.RunQuery("UPDATE users SET loginEventProgress = '" + (object) usr.rewardEvent.progress + "', loginEventToday = '1' WHERE id='" + (object) usr.userId + "'");
      }
      else
        usr.send((Packet) new SP_LoginEvent(SP_LoginEvent.ErrorCodes.AlreadyChecked));
    }
  }
}
