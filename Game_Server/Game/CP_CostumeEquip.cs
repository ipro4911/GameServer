// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CostumeEquip
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class CP_CostumeEquip : Handler
  {
    private string getDefaultClass(int Class)
    {
      if (Class >= 0 && Class <= 4)
        return "BA0" + (object) (Class + 1);
      return (string) null;
    }

    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null)
        return;
      bool flag = this.getBlock(0) == "0";
      int Class = int.Parse(this.getBlock(1));
      string block = this.getBlock(4);
      int index = int.Parse(this.getBlock(5));
      if (ItemManager.GetItem(block) == null || Class < 0 || Class > 4)
        return;
      if (usr.HasCostume(block))
      {
        if (block.StartsWith("BA"))
        {
          usr.costumes_char[Class] = (flag ? block : this.getDefaultClass(Class)) + ",^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
        }
        else
        {
          string[] strArray = usr.costumes_char[Class].Split(',');
          strArray[index] = flag ? block : "^";
          usr.costumes_char[Class] = string.Join(",", strArray);
        }
        string Code = usr.costumes_char[Class];
        usr.send((Packet) new SP_CostumeEquip(Class, Code));
        DB.RunQuery("UPDATE users_costumes SET class_" + (object) Class + "='" + Code + "' WHERE ownerid='" + (object) usr.userId + "'");
        usr.send((Packet) new SP_CashItemBuy(usr));
      }
      else
      {
        Log.WriteError(usr.nickname + " tried to equip " + block + " but he hasn't it!");
        usr.disconnect();
      }
    }
  }
}
