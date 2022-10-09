// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_DeleteItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_DeleteItem : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null)
        return;
      string block = this.getBlock(0);
      if (usr.HasItem(block))
      {
        int itemIndex = usr.GetItemIndex(block);
        if (itemIndex == -1)
          return;
        usr.deleteItem(block);
        string inventory = Inventory.calculateInventory(itemIndex);
        for (int index1 = 0; index1 < 5; ++index1)
        {
          for (int index2 = 0; index2 < 8; ++index2)
          {
            if (usr.equipment[index1, index2] == inventory || usr.equipment[index1, index2] == block)
              usr.equipment[index1, index2] = "^";
          }
        }
        usr.LoadRetails();
        usr.send((Packet) new SP_DeleteItem(usr, block));
      }
      else
        Log.WriteError(usr.nickname + " tried to delete " + block + " but he haven't it!");
    }
  }
}
