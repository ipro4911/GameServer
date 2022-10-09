// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.DeathMatch
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Generic;

namespace Game_Server.GameModes
{
  internal class DeathMatch
  {
    public bool isGunGame = true;
    private List<string> weapons = new List<string>()
    {
      "DF01",
      "DF03",
      "DF05",
      "DC03",
      "DC01",
      "DG03",
      "DG07",
      "DF09",
      "DC12",
      "DF20",
      "DG09"
    };
    private List<string> gunGameInventory = new List<string>();
    public Dictionary<int, int> gunGameScores = new Dictionary<int, int>();
    private Dictionary<int, string> gunGameUsrInv = new Dictionary<int, string>();
    private Room room;

    ~DeathMatch()
    {
      GC.Collect();
    }

    public void Update()
    {
      if (this.room == null || this.room.timeleft > 0 && this.room.KillsNIULeft > 0 && this.room.KillsDerbaranLeft > 0)
        return;
      this.room.EndGame();
    }

    public void InitializeGunGame()
    {
      this.gunGameScores.Clear();
      this.gunGameUsrInv.Clear();
      for (int key = 0; key < this.room.maxusers; ++key)
      {
        this.gunGameScores.Add(key, 0);
        this.gunGameUsrInv.Add(key, "");
      }
      this.isGunGame = true;
      if (this.gunGameInventory.Count != 0)
        return;
      for (int index = 0; index < this.weapons.Count; ++index)
      {
        string str = this.weapons[index] + "-1-1-20072010-0-0-0-0-0";
        if (!this.gunGameInventory.Contains(str))
          this.gunGameInventory.Add(str);
      }
    }

    public void GunGameJoin(Game_Server.User usr)
    {
      List<string> gunGameInventory = this.gunGameInventory;
      for (int count = gunGameInventory.Count; count < usr.inventory.Length; ++count)
        gunGameInventory.Add("^");
      this.gunGameUsrInv[usr.roomslot] = string.Join(",", (IEnumerable<string>) gunGameInventory);
      this.GunGameUpdate(usr);
    }

    public void GunGameUpdate(Game_Server.User usr)
    {
      string str = "^,^,DJ27,^,^,^,^,^";
      usr.send((Packet) new SP_Unknown((ushort) 30976, new object[9]
      {
        (object) 1,
        (object) "F,F,F,F",
        (object) str,
        (object) str,
        (object) str,
        (object) str,
        (object) str,
        (object) this.gunGameUsrInv[usr.roomslot],
        (object) 0
      }));
    }

    public void GunGameLeave(Game_Server.User usr)
    {
      usr.send((Packet) new SP_UpdateInventory(usr, (List<string>) null));
    }

    public void UpdateGunGameScore(int roomSlot)
    {
      if (!this.gunGameScores.ContainsKey(roomSlot))
        return;
      int num = this.gunGameScores[roomSlot] + 1;
      if (num < 0)
        num = 0;
      else if (num > 30)
        num = 30;
      this.gunGameScores[roomSlot] = num;
    }

    public string GetGunGameWeapon(Game_Server.User usr)
    {
      if (this.gunGameScores.ContainsKey(usr.roomslot))
        return this.weapons[this.gunGameScores[usr.roomslot]];
      return (string) null;
    }

    public DeathMatch(Room room)
    {
      this.room = room;
    }
  }
}
