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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Game_Server
{
    class Vehicle
    {
        public int ID;
        public string Code;
        public string Name;
        public int Health;
        public int MaxHealth;
        public int RespawnTime;
        public int SpawnProtection = 5;
        public string ChangedCode = string.Empty;
        public string X = null;
        public string Y = null;
        public string Z = null;
        public string PosX = null;
        public string PosY = null;
        public string PosZ = null;
        public ConcurrentDictionary<int, VehicleSeat> Seats = new ConcurrentDictionary<int, VehicleSeat>();
        public string SeatString = null;
        public int RespawnTick = 0;
        public bool isJoinable = true;
        public int TimeWithoutOwner = 0;

        public void LoadSeats(string Seats)
        {
            this.Seats.Clear();

            int SeatID = 0;

            string[] seatSplit = Seats.Split(new char[] { ';' });
            foreach (string sSeat in seatSplit)
            {
                try
                {
                    //999,0:FB04 - 10,2:FJ03; 0,0:FA01-0,0:FA01
                    string[] theSeat = sSeat.Split(new char[] { '-' });

                    string[] MainSeatSplit = theSeat[0].Split(new char[] { ':' });
                    string[] SubSeatSplit = theSeat[1].Split(new char[] { ':' });

                    string splittedMain = MainSeatSplit[0];
                    string splittedSub = SubSeatSplit[0];

                    string[] MainCTSplit = splittedMain.Split(new char[] { ',' });
                    string[] SubCTSplit = splittedSub.Split(new char[] { ',' });

                    VehicleSeat VehSeat = new VehicleSeat(SeatID, int.Parse(MainCTSplit[0]), int.Parse(MainCTSplit[1]), int.Parse(SubCTSplit[0]), int.Parse(SubCTSplit[1]), MainSeatSplit[1], SubSeatSplit[1]);
                    this.Seats.TryAdd(SeatID, VehSeat);
                    SeatID++;
                }
                catch
                {
                    Log.WriteError("Error while loading seat: " + sSeat);
                }
            }
        }

        public Vehicle(int ID, string Code, string Name, int Health, int MaxHealth, int RespawnTime, string Seats, bool isJoinable)
        {
            this.ID = ID;
            this.Code = Code;
            this.Name = Name;
            this.Health = Health;
            this.MaxHealth = MaxHealth;
            this.RespawnTime = RespawnTime;
            this.isJoinable = isJoinable;
            SeatString = Seats;
            LoadSeats(Seats);
        }

        public VehicleSeat GetSeatByID(int ID)
        {
            if (Seats.ContainsKey(ID))
            {
                return (VehicleSeat)Seats[ID];
            }
            return null;
        }

        public List<User> Users
        {
            get
            {
                List<User> list = new List<User>();
                foreach (VehicleSeat s in Seats.Values)
                {
                    if (s.seatOwner != null)
                    {
                        list.Add(s.seatOwner);
                    }
                }
                return list;
            }
        }
                
        public int GetUserSeatID(User usr)
        {
            var v = Seats.Values.Where(r => r.seatOwner.userId == usr.userId).FirstOrDefault();
            if(v != null)
            {
                return v.ID;
            }
            return -1;
        }

        public bool IsRightVehicle(string code)
        {
            return this.Code == code;
        }

        public bool FreeSeat(int SeatID)
        {
            if(Seats.ContainsKey(SeatID))
            {
                VehicleSeat seat = (VehicleSeat)Seats[SeatID];
                if (seat.seatOwner == null)
                {
                    return true;
                }
            }
            return false;
        }

        public int Side
        {
            get
            {
                foreach (VehicleSeat VehicleSeat in Seats.Values)
                {
                    if (VehicleSeat.seatOwner != null)
                    {
                        User Owner = VehicleSeat.seatOwner;
                        Room Room = VehicleSeat.seatOwner.room;
                        return Room.GetSide(Owner);
                    }
                }
                return -1;
            }
        }

        public int getHealthPercentage(int Percentage)
        {
            return int.Parse(((Math.Truncate((double)(MaxHealth * Percentage))) / 100).ToString());
        }

        public List<User> Players
        {
            get
            {
                List<User> list = new List<User>();
                foreach (VehicleSeat seat in Seats.Values)
                {
                    list.Add(seat.seatOwner);
                }
                return list;
            }
        }

        public VehicleSeat GetSeatByUser(User usr)
        {
            try
            {
                return Seats.Values.Where(r => r.seatOwner.userId == usr.userId).First();
            }
            catch(Exception e) {Console.WriteLine(e); }

            return null;
        }

        public int GetSeat(User usr)
        {
            try
            {
                return Seats.Values.Where(r => r.seatOwner.userId == usr.userId).First().ID;
            }
            catch(Exception e) {Console.WriteLine(e); }

            return -1;
        }

        public void SwitchSeat(int ID, User usr)
        {
            VehicleSeat Seat = GetSeatByID(ID);
            if (Seat == null) return;
            if (Seat.ID == ID && Seat.seatOwner == null)
            {
               usr.currentSeat.LeaveSeat(usr);
               usr.currentSeat = Seat;
                Seat.seatOwner = usr;
            }
        }

        public bool TakeSeat(int ID, User usr)
        {
            Seats.Values.Where(r => r.seatOwner.userId == usr.userId).First().LeaveSeat(usr);

            foreach (VehicleSeat Seat in Seats.Values)
            {
                return Seat.TakeSeat(usr);
            }
            return false;
        }

        public bool Join(User usr)
        {
            foreach (VehicleSeat Seat in Seats.Values)
            {
                if (Seat.TakeSeat(usr))
                {
                   usr.currentVehicle = this;
                   usr.currentSeat = Seat;
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
                usr.currentSeat = null;
            }
           usr.currentVehicle = null;
        }
    }
}
