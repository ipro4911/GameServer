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
    class CaptureMode
    {
        ~CaptureMode()
        {
            GC.Collect();
        }
        Room room = null;
        public void Update()
        {
            if (room != null)
            {
            }
        }

        public CaptureMode(Room room)
        {
            this.room = room;
            this.NIUPoints = this.DerbaranPoints = 0;
        }

        public int NIUPoints = 0;

        public int DerbaranPoints = 0;
    }
}
