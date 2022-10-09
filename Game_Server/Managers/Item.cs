// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.Item
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;

namespace Game_Server.Managers
{
  internal class Item
  {
    public int Level = 1;
    private int[] Price = new int[6];
    private int[] Cash = new int[6];
    private int[] EA = new int[4]{ 1, 10, 30, 60 };
    private int[] useableBranch = new int[5];
    private int[] useableSlot = new int[8];
    public List<PackageItem> packageItems = new List<PackageItem>();
    public int ID;
    public string Code;
    public string Name;
    public int Damage;
    public bool Premium;
    public bool Buyable;
    public int BuyType;
    public string[] personal;
    public string[] surface;
    public PackageType packageType;
    public uint dinarReward;
    public bool accruable;
    public ushort maxAccrueCount;

    public int GetEACount(int Type)
    {
      return this.EA[Type];
    }

    public int GetPrice(int Type)
    {
      return this.Price[Type];
    }

    public int GetCashPrice(int Type)
    {
      return this.Cash[Type];
    }

    public int GetUseableSlot()
    {
      return Array.IndexOf<int>(this.useableSlot, 1);
    }

    public bool UseableBranch(int branch)
    {
      switch (branch)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
          return this.useableBranch[branch] == 1;
        default:
          return false;
      }
    }

    public bool UseableSlot(int slot)
    {
      switch (slot)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
          return this.useableSlot[slot] == 1;
        default:
          return false;
      }
    }

    public Item(
      int ID,
      string Code,
      string Name,
      string Price,
      string Cash,
      int BuyType,
      int Damage,
      uint dinarReward,
      byte packageType,
      string packageItems,
      string[] Surface,
      string[] Personal,
      string UseableBranch,
      string UseableSlot,
      bool accruable,
      ushort maxAccrueCount,
      int Level,
      bool Premium,
      bool Buyable)
    {
      try
      {
        this.ID = ID;
        for (int index = 0; index < 6; ++index)
        {
          this.Price[index] = -1;
          this.Cash[index] = -1;
        }
        this.Code = Code;
        this.Name = Name;
        this.BuyType = BuyType;
        string[] strArray1 = Price.Split(',');
        for (int index = 0; index < strArray1.Length; ++index)
          this.Price[index] = int.Parse(strArray1[index]);
        string[] strArray2 = Cash.Split(',');
        for (int index = 0; index < strArray2.Length; ++index)
          this.Cash[index] = int.Parse(strArray2[index]);
        if (UseableBranch != null)
        {
          string[] strArray3 = UseableBranch.Split(',');
          for (int index = 0; index < strArray3.Length; ++index)
          {
            int num = int.Parse(strArray3[index].ToString());
            switch (num)
            {
              case 0:
              case 1:
                this.useableBranch[index] = num;
                break;
            }
          }
        }
        this.packageType = (PackageType) packageType;
        if (packageItems != null && packageItems.Length >= 7)
        {
          string str1 = packageItems;
          string[] separator = new string[1]{ "," };
          foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            char[] chArray = new char[1]{ '/' };
            string[] strArray3 = str2.Split(chArray);
            string str3 = strArray3[0];
            short num = 3650;
            if (strArray3.Length == 2)
              num = short.Parse(strArray3[1]);
            this.packageItems.Add(new PackageItem()
            {
              item = str3,
              days = num
            });
          }
        }
        if (UseableSlot != null)
        {
          string[] strArray3 = UseableSlot.Split(',');
          for (int index = 0; index < strArray3.Length; ++index)
          {
            int num = int.Parse(strArray3[index].ToString());
            switch (num)
            {
              case 0:
              case 1:
                this.useableSlot[index] = num;
                break;
            }
          }
        }
        this.Damage = Damage;
        this.accruable = accruable;
        this.maxAccrueCount = maxAccrueCount;
        this.surface = Surface;
        this.personal = Personal;
        this.dinarReward = dinarReward;
        this.Level = Level;
        this.Premium = Premium;
        this.Buyable = Buyable;
      }
      catch (Exception ex)
      {
        Log.WriteError("Couldn't parse item code: " + this.Code);
      }
    }
  }
}
