// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_Heal
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_Heal : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      byte num1 = byte.Parse(this.getBlock(6));
      bool flag1 = this.getBlock(7) == "1";
      bool flag2 = this.getBlock(8) == "0" || this.getBlock(8) == "2";
      if (num1 >= (byte) 0 && (int) num1 <= room.maxusers)
      {
        User user = room.users[(int) num1];
        if (user == null)
          return;
        if (flag2)
        {
          string itemCodeById = ItemManager.GetItemCodeByID(usr.weapon);
          if (user.Health <= 0 || user.Health >= 1000 && itemCodeById != "DS01" || room.mode != 1 && room.GetSide(usr) != room.GetSide(user))
            return;
          if (!flag1)
          {
            if (usr.HasItem(itemCodeById) || usr.IsWhitelistedWeapon(itemCodeById))
            {
              if (user.roomslot != usr.roomslot && user.Health < 300)
                usr.rPoints += Game_Server.Configs.Server.Experience.OnFriendHeal;
              if (itemCodeById == "BS0E")
              {
                user.Health += 50;
              }
              else
              {
                switch (itemCodeById)
                {
                  case "DQ01":
                    user.Health += 300;
                    break;
                  case "DQ02":
                    user.Health += 400;
                    break;
                    case "DQ04":
                    user.Health += 300;
                     break; // DQ_MEDIC_KIT_2_V 
                    case "DQ03":
                    user.Health += 600;
                    break;
                  case "DS01":
                    int num2 = user.Health < 300 ? 300 : user.Health + 100;
                    user.Health = num2;
                    break;
                  case "DS10":
                    user.Health += 200;
                    break;
                }
                if (itemCodeById.StartsWith("DQ") && Generic.random(0, 500) < 20)
                  usr.RandomGunsmithResource();
              }
            }
          }
          else
            user.Health += 400;
          if (user.Health > 1000)
            user.Health = 1000;
        }
        else
        {
          user.Health -= 100;
          if (user.Health <= 0)
          {
            this.sendBlocks[3] = (object) 157;
            user.OnDie();
          }
        }
        this.sendBlocks[7] = (object) user.Health;
      }
      this.sendPacket = true;
    }
  }
}
