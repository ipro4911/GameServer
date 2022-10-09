// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.ItemManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Game_Server.Managers
{
  internal class ItemManager
  {
    public static string Items = (string) null;
    public static Dictionary<string, Item> CollectedItems = new Dictionary<string, Item>();
    public static System.Collections.Generic.List<Item> List = new System.Collections.Generic.List<Item>();
    public static string MD5 = (string) null;

    private static string GetMD5HashFromFile(string fileName)
    {
      try
      {
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream) fileStream);
        fileStream.Close();
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < hash.Length; ++index)
          stringBuilder.Append(hash[index].ToString("x2"));
        return stringBuilder.ToString();
      }
      catch
      {
        return (string) null;
      }
    }

    public static void LoadItems()
    {
      if (ItemManager.Items == null)
        return;
      int num = -1;
      try
      {
        ItemManager.CollectedItems.Clear();
        ItemManager.MD5 = ItemManager.GetMD5HashFromFile("items.bin");
        string[] strArray = ItemManager.Items.Replace("\r", "").Split('\t', '\n');
        for (int index1 = 1; index1 < strArray.Length; ++index1)
        {
          try
          {
            if (strArray[index1] == "<!--")
            {
              string Name = "";
              string str1 = "";
              bool Buyable = false;
              string Price = "-1,-1,-1,-1,-1,-1";
              string Cash = "-1,-1,-1,-1,-1,-1";
              int BuyType = 3;
              int Level = 1;
              int Damage = -1;
              bool Premium = false;
              string UseableBranch = (string) null;
              string UseableSlot = (string) null;
              byte packageType = 0;
              string packageItems = (string) null;
              bool accruable = false;
              ushort maxAccrueCount = 0;
              uint dinarReward = 0;
              string[] Personal = new string[3]
              {
                "0",
                "0",
                "0"
              };
              string[] Surface = new string[3]
              {
                "0",
                "0",
                "0"
              };
              for (int index2 = 0; index2 < 300; ++index2)
              {
                string str2 = strArray[index1 + index2].Trim();
                string s = strArray[index1 + (index2 + 1)].Trim();
                if (str2.Contains("ENGLISH"))
                  Name = s;
                if (str2.Contains("CODE"))
                  str1 = s;
                if (str2.Contains("bBuy"))
                  Buyable = int.Parse(s) == 1;
                if (str2.Contains("DinarCost"))
                  Price = s;
                if (str2.Contains("CashCost"))
                  Cash = s;
                if (str2.Contains("ReqLevel"))
                  Level = int.Parse(s);
                if (str2.Contains("bPremiumOnly"))
                  Premium = int.Parse(s) == 1;
                if (str2.Contains("Power") && Damage == -1)
                  Damage = int.Parse(s);
                if (str2.Contains("BuyType"))
                  BuyType = int.Parse(s);
                if (str2.Contains("UseableBranch"))
                  UseableBranch = s;
                if (str2.Contains("UseableSlot"))
                  UseableSlot = s;
                if (str2.Contains("nPackageType"))
                  packageType = byte.Parse(s);
                if (str2.Contains("PackageComponent"))
                  packageItems = s;
                if (str2.Contains("RewardDinar"))
                  dinarReward = uint.Parse(s);
                if (str2.Contains("Low"))
                {
                  Personal[0] = s.Split(',')[0];
                  Surface[0] = s.Split(',')[1];
                }
                if (str2.Contains("Middle"))
                {
                  Personal[1] = s.Split(',')[0];
                  Surface[1] = s.Split(',')[1];
                }
                if (str2.Contains("High"))
                {
                  Personal[2] = s.Split(',')[0];
                  Surface[2] = s.Split(',')[1];
                }
                if (str2.Contains("bAccuruable"))
                  accruable = s.Contains("1");
                if (str2.Contains("AccrueCount"))
                  maxAccrueCount = ushort.Parse(s);
                if (str2.Contains("//-->"))
                {
                  index1 += index2;
                  break;
                }
              }
              bool flag = false;
              if (str1.StartsWith("D"))
              {
                flag = true;
                ++num;
              }
              Item obj = new Item(flag ? num : 0, str1, Name, Price, Cash,BuyType, Damage, dinarReward, packageType, packageItems, Surface, Personal, UseableBranch, UseableSlot, accruable, maxAccrueCount, Level, Premium, Buyable);
              ItemManager.CollectedItems.Add(str1, obj);
            }
          }
          catch (Exception ex)
          {
          }
        }
        Log.WriteLine("Successfully loaded [" + (object) ItemManager.CollectedItems.Count + "] Items");
      }
      catch
      {
        Log.WriteError("Error write loading items");
      }
    }

    public static Item GetItemByID(int id)
    {
      return ItemManager.CollectedItems.Values.Where<Item>((Func<Item, bool>) (i => i.ID == id)).FirstOrDefault<Item>();
    }

    public static string GetItemCodeByID(int id)
    {
      return ItemManager.CollectedItems.Values.Where<Item>((Func<Item, bool>) (i => i.ID == id)).First<Item>()?.Code;
    }

    public static int GetDamage(string Code, int Type = 2)
    {
      try
      {
        if (!ItemManager.CollectedItems.ContainsKey(Code))
          return 0;
        Item collectedItem = ItemManager.CollectedItems[Code];
        if (collectedItem.personal == null)
          return collectedItem.Damage;
        int num = int.Parse(collectedItem.personal[Type]);
        return int.Parse(Math.Truncate((double) (collectedItem.Damage * num / 100)).ToString());
      }
      catch
      {
        return 0;
      }
    }

    public static int GetVehicleDamage(string Code, int Type = 1)
    {
      try
      {
        if (!ItemManager.CollectedItems.ContainsKey(Code))
          return 0;
        Item collectedItem = ItemManager.CollectedItems[Code];
        if (collectedItem.surface == null)
          return collectedItem.Damage;
        int num = int.Parse(collectedItem.surface[Type]);
        return int.Parse(Math.Truncate((double) (collectedItem.Damage * num / 100)).ToString());
      }
      catch
      {
        return 0;
      }
    }

    public static Item GetItem(string Code)
    {
      if (Code.Contains("-"))
        return (Item) null;
      Code = Code.ToUpper();
      if (ItemManager.CollectedItems.ContainsKey(Code))
        return ItemManager.CollectedItems[Code];
      return (Item) null;
    }

    public static string DecryptBinRaw(byte[] Raw)
    {
      try
      {
        return ItemManager.DecryptBinRaw(Encoding.UTF8.GetString(Raw));
      }
      catch (Exception ex)
      {
        Log.WriteError(ex.Message);
        return (string) null;
      }
    }

    public static string DecryptBinRaw(string Raw)
    {
      try
      {
        string str = Encoding.Default.GetString(Encoding.UTF8.GetBytes(Raw));
        byte[] numArray = new byte[Raw.Length / 2];
        for (int index = 0; index < numArray.Length; ++index)
        {
          numArray[index] = Convert.ToByte(str.Substring(index * 2, 2), 16);
          numArray[index] = (byte) ((uint) numArray[index] ^ 42U);
        }
        return Encoding.UTF8.GetString(((IEnumerable<byte>) numArray).ToArray<byte>());
      }
      catch (Exception ex)
      {
        Log.WriteError(ex.Message);
        return (string) null;
      }
    }

    public static string DecryptBinFile(string filename)
    {
      try
      {
        StreamReader streamReader = new StreamReader((Stream) File.Open(filename, FileMode.Open, FileAccess.Read));
        string str = ItemManager.DecryptBinRaw(streamReader.ReadToEnd());
        streamReader.Close();
        return ItemManager.Items = str;
      }
      catch
      {
        Log.WriteError("Failed to decrypt " + filename);
        return (string) null;
      }
    }
  }
}
