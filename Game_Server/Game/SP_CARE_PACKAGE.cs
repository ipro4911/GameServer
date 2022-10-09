// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_CARE_PACKAGE
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Game
{
  internal class SP_CARE_PACKAGE : Packet
  {
    internal class Open : Packet
    {
      public Open()
      {
        this.newPacket((ushort) 30272);
        this.addBlock((object) CarePackage.items.Count);
        foreach (CarePackageItem carePackageItem in CarePackage.items.Values)
        {
          this.addBlock((object) carePackageItem.Price);
          this.addBlock((object) carePackageItem.Method);
          this.addBlock((object) carePackageItem.Item);
          this.addBlock((object) -1);
          this.addBlock((object) carePackageItem.days);
          this.addBlock((object) carePackageItem.Item1);
          this.addBlock((object) -1);
          this.addBlock((object) carePackageItem.days1);
          this.addBlock((object) carePackageItem.Item2);
          this.addBlock((object) -1);
          this.addBlock((object) carePackageItem.days2);
          this.addBlock((object) carePackageItem.Item3);
          this.addBlock((object) -1);
          this.addBlock((object) carePackageItem.days3);
          this.addBlock((object) carePackageItem.Item4);
          this.addBlock((object) -1);
          this.addBlock((object) carePackageItem.days4);
        }
      }
    }

    internal class SendItem : Packet
    {
      public SendItem(Game_Server.User usr, string itemcode, int days, bool isdinar, bool win)
      {
        this.newPacket((ushort) 30273);
        this.addBlock((object) 1);
        this.addBlock((object) (isdinar ? 0 : 1));
        this.addBlock((object) (win ? 1 : 0));
        this.addBlock((object) itemcode);
        this.addBlock((object) 1);
        this.addBlock((object) Inventory.Itemlist(usr));
        this.addBlock((object) (isdinar ? usr.dinar : usr.cash));
        this.addBlock((object) usr.AvailableSlots);
      }
    }
  }
}
