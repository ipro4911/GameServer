// Decompiled with JetBrains decompiler
// Type: Game_Server.Vehicle
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server
{
  internal class Vehicle
  {
    public int SpawnProtection = 5;
    public string ChangedCode = string.Empty;
    public ConcurrentDictionary<int, VehicleSeat> Seats = new ConcurrentDictionary<int, VehicleSeat>();
    public bool isJoinable = true;
    public int ID;
    public string Code;
    public string Name;
    public int Health;
    public int MaxHealth;
    public int RespawnTime;
    public string X;
    public string Y;
    public string Z;
    public string PosX;
    public string PosY;
    public string PosZ;
    public string SeatString;
    public int RespawnTick;
    public int TimeWithoutOwner;

    public void LoadSeats(string Seats)
    {
      this.Seats.Clear();
      int num = 0;
      string str1 = Seats;
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        try
        {
          string[] strArray1 = str2.Split('-');
          string[] strArray2 = strArray1[0].Split(':');
          string[] strArray3 = strArray1[1].Split(':');
          string str3 = strArray2[0];
          string str4 = strArray3[0];
          string[] strArray4 = str3.Split(',');
          string[] strArray5 = str4.Split(',');
          VehicleSeat vehicleSeat = new VehicleSeat(num, int.Parse(strArray4[0]), int.Parse(strArray4[1]), int.Parse(strArray5[0]), int.Parse(strArray5[1]), strArray2[1], strArray3[1]);
          this.Seats.TryAdd(num, vehicleSeat);
          ++num;
        }
        catch
        {
          Log.WriteError("Error while loading seat: " + str2);
        }
      }
    }

    public Vehicle(
      int ID,
      string Code,
      string Name,
      int Health,
      int MaxHealth,
      int RespawnTime,
      string Seats,
      bool isJoinable)
    {
      this.ID = ID;
      this.Code = Code;
      this.Name = Name;
      this.Health = Health;
      this.MaxHealth = MaxHealth;
      this.RespawnTime = RespawnTime;
      this.isJoinable = isJoinable;
      this.SeatString = Seats;
      this.LoadSeats(Seats);
    }

    public VehicleSeat GetSeatByID(int ID)
    {
      if (this.Seats.ContainsKey(ID))
        return this.Seats[ID];
      return (VehicleSeat) null;
    }

    public List<User> Users
    {
      get
      {
        List<User> userList = new List<User>();
        foreach (VehicleSeat vehicleSeat in (IEnumerable<VehicleSeat>) this.Seats.Values)
        {
          if (vehicleSeat.seatOwner != null)
            userList.Add(vehicleSeat.seatOwner);
        }
        return userList;
      }
    }

    public int GetUserSeatID(User usr)
    {
      VehicleSeat vehicleSeat = this.Seats.Values.Where<VehicleSeat>((Func<VehicleSeat, bool>) (r => r.seatOwner.userId == usr.userId)).FirstOrDefault<VehicleSeat>();
      if (vehicleSeat != null)
        return vehicleSeat.ID;
      return -1;
    }

    public bool IsRightVehicle(string code)
    {
      return this.Code == code;
    }

    public bool FreeSeat(int SeatID)
    {
      return this.Seats.ContainsKey(SeatID) && this.Seats[SeatID].seatOwner == null;
    }

    public int Side
    {
      get
      {
        foreach (VehicleSeat vehicleSeat in (IEnumerable<VehicleSeat>) this.Seats.Values)
        {
          if (vehicleSeat.seatOwner != null)
          {
            User seatOwner = vehicleSeat.seatOwner;
            return vehicleSeat.seatOwner.room.GetSide(seatOwner);
          }
        }
        return -1;
      }
    }

    public int getHealthPercentage(int Percentage)
    {
      return int.Parse((Math.Truncate((double) (this.MaxHealth * Percentage)) / 100.0).ToString());
    }

    public List<User> Players
    {
      get
      {
        List<User> userList = new List<User>();
        foreach (VehicleSeat vehicleSeat in (IEnumerable<VehicleSeat>) this.Seats.Values)
          userList.Add(vehicleSeat.seatOwner);
        return userList;
      }
    }

    public VehicleSeat GetSeatByUser(User usr)
    {
      try
      {
        return this.Seats.Values.Where<VehicleSeat>((Func<VehicleSeat, bool>) (r => r.seatOwner.userId == usr.userId)).First<VehicleSeat>();
      }
      catch
      {
      }
      return (VehicleSeat) null;
    }

    public int GetSeat(User usr)
    {
      try
      {
        return this.Seats.Values.Where<VehicleSeat>((Func<VehicleSeat, bool>) (r => r.seatOwner.userId == usr.userId)).First<VehicleSeat>().ID;
      }
      catch
      {
      }
      return -1;
    }

    public void SwitchSeat(int ID, User usr)
    {
      VehicleSeat seatById = this.GetSeatByID(ID);
      if (seatById == null || seatById.ID != ID || seatById.seatOwner != null)
        return;
      usr.currentSeat.LeaveSeat(usr);
      usr.currentSeat = seatById;
      seatById.seatOwner = usr;
    }

    public bool TakeSeat(int ID, User usr)
    {
      this.Seats.Values.Where<VehicleSeat>((Func<VehicleSeat, bool>) (r => r.seatOwner.userId == usr.userId)).First<VehicleSeat>().LeaveSeat(usr);
      using (IEnumerator<VehicleSeat> enumerator = this.Seats.Values.GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current.TakeSeat(usr);
      }
      return false;
    }

    public bool Join(User usr)
    {
      foreach (VehicleSeat vehicleSeat in (IEnumerable<VehicleSeat>) this.Seats.Values)
      {
        if (vehicleSeat.TakeSeat(usr))
        {
          usr.currentVehicle = this;
          usr.currentSeat = vehicleSeat;
          return true;
        }
      }
      return false;
    }

    public void Leave(User usr)
    {
      if (usr.currentSeat != null)
      {
        usr.currentSeat.LeaveSeat(usr);
        usr.currentSeat = (VehicleSeat) null;
      }
      usr.currentVehicle = (Vehicle) null;
    }
  }
}
