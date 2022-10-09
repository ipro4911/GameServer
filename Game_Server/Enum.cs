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
    internal enum RoomMode : int
    {
        Explosive = 0,
        FFA = 1,
        FourVersusFour = 2,
        TDM = 3,
        Conquest = 4,
        BGExplosive = 5,
        TotalWar = 6,
        HeroMode = 7,
        CaptureMode = 9,
        Survival = 10,
        Defence = 11,
        TimeAttack = 12,
        Escape = 13,
        Annihilation = 15,
    }
}
