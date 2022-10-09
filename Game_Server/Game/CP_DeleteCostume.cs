// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_DeleteCostume
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_DeleteCostume : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.room != null)
        return;
      string block = this.getBlock(0);
      if (block.ToUpper() == "BA01" || block.ToUpper() == "BA02" || (block.ToUpper() == "BA03" || block.ToUpper() == "BA04") || block.ToUpper() == "BA05")
        usr.send((Packet) new SP_CostumeEquip(SP_CostumeEquip.ErrCode.CannotDeleteDefaultItem));
      else if (usr.HasCostume(block))
      {
        usr.deleteCostume(block);
        usr.CheckForCostume();
        usr.send((Packet) new SP_DeleteCostume(usr, block));
        usr.send((Packet) new SP_CashItemBuy(usr));
      }
      else
        Log.WriteError(usr.nickname + " tried to delete " + block + " but he haven't it!");
    }
  }
}
