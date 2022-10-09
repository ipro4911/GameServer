using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Game
{
    /// <summary>
    /// This class is used for crypt/uncrypt packets or MD5
    /// </summary>
    class Cryption
    {
        //  public const byte clientXor = 62;
        // public const byte serverXor = 96;

        public const byte clientXor = 0x5B;

        public const byte serverXor = 0x38;

        public static byte[] encrypt(byte[] inputByte)
        {
            for (int i = 0; i < inputByte.Length; i++)
            {
                inputByte[i] ^= serverXor;
            }
            return inputByte;
        }

        public static byte[] decrypt(byte[] inputByte)
        {
            for (int i = 0; i < inputByte.Length; i++)
            {
                inputByte[i] ^= clientXor;
            }
            return inputByte;
        }
    }
}
