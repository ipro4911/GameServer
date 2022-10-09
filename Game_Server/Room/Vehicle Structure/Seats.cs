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

namespace Game_Server
{
    class VehicleSeat
    {
        public int ID;
        public User seatOwner = null;
        public int MainCT = -1;
        public int MainCTMag = -1;
        public int SubCT = -1;
        public int SubCTMag = -1;
        public string MainCTCode;
        public string SubCTCode;

        public bool TakeSeat(User usr)
        {
            if (seatOwner == null)
            {
                seatOwner = usr;
                return true;
            }
            return false;
        }

        public void LeaveSeat(User usr)
        {
            if (seatOwner.userId == usr.userId)
                seatOwner = null;
        }
                
        public VehicleSeat(int _ID, int _MainCT, int _MainCTMag, int _SubCT, int _SubCTMag, string _MainCTCode, string _SubCTCode)
        {
            ID = _ID;
            MainCT = _MainCT;
            MainCTMag = _MainCTMag;
            SubCT = _SubCT;
            SubCTMag = _SubCTMag;
            MainCTCode = _MainCTCode;
            SubCTCode = _SubCTCode;
        }

        public bool IsSeatCode(string code)
        {
            return (code.ToUpper() == MainCTCode.ToUpper() || code.ToUpper() == SubCTCode.ToUpper());
        }
    }
}
