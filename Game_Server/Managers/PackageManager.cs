// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.PackageManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Managers
{
  internal class PackageManager
  {
    public static bool AddItem(User usr, string itemCode)
    {
      if (itemCode == "CC36" || itemCode == "CC37" || (itemCode == "CC56" || itemCode == "CC57"))
        return false;
      string[] array = new string[1]{ "CZ99" };
      Item obj = ItemManager.GetItem(itemCode);
      if (obj != null)
      {
        switch (itemCode)
        {
                    case "CC41": usr.AddPremium(3, 30); break;
                    case "CC44": usr.AddPremium(2, 30); break;
                    case "CU83": Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); break;
                    case "CZ23": Inventory.AddItem(usr, "CR99", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); usr.AddPremium(3, 30); break;
                    case "CZ13": Inventory.AddItem(usr, "CZ66", 5000); Inventory.AddItem(usr, "CZ66", 5000); break;
                    case "CU81": Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); break;
                    case "CU82": Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); break;
                    case "CU87": Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); break;
                    case "CU84": Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); break;
                    case "CU90": Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); break;
                    case "CV06": Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); break;
                    case "CV05": Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); break;
                    case "CV04": Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); break;
                    case "CV03": Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); break;
                    case "CU88": Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); break;
                    case "CU85": Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); break;
                    case "CU91": Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); break;
                    case "CU89": Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); break;
                    case "CU86": Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); break;
                    case "CU92": Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); break;
                    case "CC42": usr.AddPremium(3, 90); break;
                    case "CC43": usr.AddPremium(3, 180); break;
                    case "CC74": usr.AddPremium(3, 7); break;
                    case "CC75": usr.AddPremium(3, 15); break;
                    case "CC73": usr.AddPremium(3, 3); break;
                    case "CU08": usr.AddPremium(3, 30); break;
                    case "CS14": usr.AddPremium(3, 30); break;
                    case "CS11": usr.AddPremium(3, 30); break;
                    case "CS09": usr.AddPremium(3, 30); break;
                    case "CS10": usr.AddPremium(3, 30); break;
                    case "CS08": usr.AddPremium(3, 30); break;
                    case "CS93": usr.AddPremium(3, 30); break;
                    case "CC45": usr.AddPremium(2, 90); break;
                    case "CC46": usr.AddPremium(2, 180); break;
                    case "CC60": usr.AddPremium(4, 30); break;
                    case "CC61": usr.AddPremium(4, 90); break;
                    case "CC62": usr.AddPremium(4, 180); break;
                    case "CV65": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CY90", 5000); break;
                    case "CV09": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CY28", 5000); break;
                    case "CV67": usr.AddPremium(3, 30); break;
                    case "CV68": usr.AddPremium(3, 30); break;
                    case "CV69": usr.AddPremium(3, 30); break;
                    case "CV64": usr.AddPremium(3, 30); break;
                    case "CV63": usr.AddPremium(3, 30); break;
                    case "CV66": usr.AddPremium(3, 30); break;
                    case "CP60": usr.AddPremium(3, 30); break;
                    case "CP61": usr.AddPremium(3, 30); break;
                    case "CP62": usr.AddPremium(3, 30); break;
                    case "CP63": usr.AddPremium(3, 30); break;
                    case "CP64": usr.AddPremium(3, 30); break;
                    case "CV74": usr.AddPremium(3, 30); break;
                    case "CV45": usr.AddPremium(3, 30); break;
                    case "CV44": usr.AddPremium(3, 30); break;
                    case "CV42": usr.AddPremium(3, 30); break;
                    case "CV71": usr.AddPremium(3, 30); break;
                    case "CU66": usr.AddPremium(3, 30); break;
                    case "CV12": usr.AddPremium(3, 30); break;
                    case "CV13": usr.AddPremium(3, 30); break;
                    case "CV10": usr.AddPremium(3, 30); break;
                    case "CV61": usr.AddPremium(3, 30); break;
                    case "CV60": usr.AddPremium(3, 30); break;
                    case "CV51": usr.AddPremium(3, 30); break;
                    case "CV52": usr.AddPremium(3, 30); break;
                    case "CV48": usr.AddPremium(3, 30); break;
                    case "CV47": usr.AddPremium(3, 30); break;
                    case "CU40": usr.AddPremium(3, 30); break;


                }

                if (obj.dinarReward > 0U)
        {
          usr.dinar += (int) obj.dinarReward;
          DB.RunQuery("UPDATE users SET dinar = '" + (object) usr.dinar + "' WHERE id='" + (object) usr.userId + "'");
        }
        if (obj.packageItems.Count > 0 && Array.IndexOf<string>(array, obj.Code) == -1 && (!itemCode.StartsWith("B") || Inventory.GetFreeCostumeSlotCount(usr) >= obj.packageItems.Count) && (itemCode.StartsWith("B") || Inventory.GetFreeItemSlotCount(usr) >= obj.packageItems.Count))
        {
          foreach (PackageItem packageItem in obj.packageItems)
          {
            if (Inventory.GetFreeItemSlotCount(usr) > 0)
            {
              if (packageItem.item.StartsWith("B"))
                Inventory.AddCostume(usr, packageItem.item, (int) packageItem.days);
              else
                Inventory.AddItem(usr, packageItem.item, (int) packageItem.days);
            }
            else
              DB.RunQuery("INSERT INTO inbox (ownerid, itemcode, days) VALUES ('" + (object) usr.userId + "', '" + packageItem.item + "', '" + (object) packageItem.days + "')");
          }
          return true;
        }
      }
      return false;
    }
  }
}
