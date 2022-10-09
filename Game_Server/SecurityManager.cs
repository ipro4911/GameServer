using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Managers
{
    class SecurityManager
    {
        public static bool IsAllLettersOrDigits(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }
    }
}
