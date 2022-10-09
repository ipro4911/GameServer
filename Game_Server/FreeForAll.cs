/*
 _____   ___ __  __  _____ _____   ___  _  __              ___   ___   __    __ 
/__   \ /___\\ \/ /  \_   \\_   \ / __\( )/ _\            / __\ /___\ /__\  /__\
  / /\///  // \  /    / /\/ / /\// /   |/ \ \            / /   //  /// \// /_\  
 / /  / \_//  /  \ /\/ /_/\/ /_ / /___    _\ \          / /___/ \_/// _  \//__  
 \/   \___/  /_/\_\\____/\____/ \____/    \__/          \____/\___/ \/ \_/\__/  
__________________________________________________________________________________

Created by: ToXiiC
Thanks to: CodeDragon, Kill1212, CodeDragon

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.GameModes
{
    class FreeForAll
    {
        ~FreeForAll()
        {
            GC.Collect();
        }
        Room room = null;
        public void Update()
        {
            if (room != null)
            {
                if (room.SpawnLocation < 0 || room.SpawnLocation >= 15) room.SpawnLocation = 0;

                foreach (User usr in room.users.Values)
                {
                    if (usr.rKills > room.highestkills)
                    {
                        room.highestkills = usr.rKills;
                    }
                }
                
                if (room.timeleft <= 0 || room.highestkills >= room.ffakillpoints)
                {
                    room.EndGame();
                    return;
                }
            }
        }

       #region Gun Game

        //public bool isGunGame = true;

        ///*Dictionary<int, string[]> weapons = new Dictionary<int,string[]>()
        //{ 
        //    { 0, new string[] { "DA04", "DB01", "DB05", "DB10", "DF01", "DF01", "DF01", "DF01", "DF01" } }, // Engineer
        //    { 1, new string[] { "DA04", "DB01", "DB05", "DB10", "DF01", "DF01", "DF01", "DF01", "DF01" } }, // Medic
        //    { 2, new string[] { "DA04", "DB01", "DB05", "DB10", "DG05", "DF01", "DF01", "DF01", "DF01" } }, // Sniper
        //    { 3, new string[] { "DA04", "DB01", "DB05", "DB10", "DC02", "DF01", "DF01", "DF01", "DF01" } }, // Assault
        //    { 4, new string[] { "DA04", "DB01", "DB05", "DB10", "DJ01", "DF01", "DF01", "DF01", "DF01" } }, // Heavy Trooper
        //};*/

        //List<string> weapons = new List<string>() { "DF01", "DF03", "DF05", "DC03", "DC01", "DG03", "DG07", "DF09", "DC12", "DF20", "DG09" };

        //List<string> gunGameInventory = new List<string>();

        //public Dictionary<int, int> gunGameScores = new Dictionary<int, int>();
        //Dictionary<int, string> gunGameUsrInv = new Dictionary<int, string>();

        //public void InitializeGunGame()
        //{
        //    // Initializing user score
        //    gunGameScores.Clear();
        //    gunGameUsrInv.Clear();
        //    for (int i = 0; i < this.room.maxusers; i++)
        //    {
        //        gunGameScores.Add(i, 0);
        //        gunGameUsrInv.Add(i, "");
        //    }
            
        //    isGunGame = true;

        //    // avoiding to reload every time

        //    if (gunGameInventory.Count == 0)
        //    {
        //        for (int i = 0; i < weapons.Count; i++)
        //        {
        //            string item = weapons[i] + "-1-1-20072010-0-0-0-0-0";
        //            if (!gunGameInventory.Contains(item))
        //            {
        //                gunGameInventory.Add(item);
        //            }
        //        }
        //    }
        //}

        //public void GunGameJoin(User usr)
        //{
        //    List<string> items = gunGameInventory;
        //    for (int i = items.Count; i < usr.inventory.Length; i++)
        //    {
        //        items.Add("^");
        //    }

        //    gunGameUsrInv[usr.roomslot] = string.Join(",", items);

        //    GunGameUpdate(usr);
        //}

        //public void GunGameUpdate(User usr)
        //{
        //    string defaultEquipment = weapons[(int)gunGameScores[usr.roomslot]] + ",^,^,^,^,^,^,^";
            
        //    usr.send(new Game.SP_Unknown(30976, 1, "F,F,F,F", defaultEquipment, defaultEquipment, defaultEquipment, defaultEquipment, defaultEquipment, gunGameUsrInv[usr.roomslot], 0));
        //}

        //public void GunGameLeave(User usr)
        //{
        //    usr.send(new Game.SP_UpdateInventory(usr, null));
        //}

        //public void UpdateGunGameScore(int roomSlot)
        //{
        //    if(gunGameScores.ContainsKey(roomSlot))
        //    {
        //        int r = (int)gunGameScores[roomSlot] + 1;

        //        if (r < 0) r = 0;
        //        else if (r > 30) r = 30; // Impossible, but let's add a check

        //        gunGameScores[roomSlot] = r;
        //    }
        //}

        //public string GetGunGameWeapon(User usr)
        //{
        //    if (gunGameScores.ContainsKey(usr.roomslot))
        //    {
        //        return weapons[(int)gunGameScores[usr.roomslot]];
        //    }
        //    return null;
        //}
        
        #endregion 

        public FreeForAll(Room room)
        {
            this.room = room;
        }
    }
}
