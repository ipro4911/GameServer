// Decompiled with JetBrains decompiler
// Type: Game_Server.Generic
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Game_Server
{
  internal class Generic
  {
    private static Random r = new Random();

    public static int timestamp
    {
      get
      {
        return (int) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
      }
    }

    public static string currentDate
    {
      get
      {
        return DateTime.Now.ToString("HH:mm:ss - dd/MM/yy");
      }
    }

    public static int random(int r1, int r2)
    {
      return Generic.r.Next(r1, r2);
    }

    public static string runningSince
    {
      get
      {
        TimeSpan timeSpan = DateTime.Now - Process.GetCurrentProcess().StartTime;
        return "Running since " + (object) timeSpan.Days + " days, " + (object) timeSpan.Hours + " hours, " + (object) timeSpan.Minutes + " minutes!";
      }
    }

    public static string ReverseIP(string addr)
    {
      string[] strArray = addr.Split('.');
      Array.Reverse((Array) strArray);
      return string.Join(".", strArray);
    }

    public static string runningSinceWeb
    {
      get
      {
        TimeSpan timeSpan = DateTime.Now - Process.GetCurrentProcess().StartTime;
        return timeSpan.Days.ToString() + "d, " + (object) timeSpan.Hours + "h, " + (object) timeSpan.Minutes + "m";
      }
    }

    public static Color ConvertHexToRGB(string color)
    {
      byte num1 = 0;
      byte num2 = 0;
      byte num3 = 0;
      if (color.StartsWith("#"))
        color = color.Remove(0, 1);
      if (color.Length == 3)
      {
        num1 = Convert.ToByte(((int) color[0]).ToString() + (object) color[0], 16);
        num2 = Convert.ToByte(((int) color[1]).ToString() + (object) color[1], 16);
        num3 = Convert.ToByte(((int) color[2]).ToString() + (object) color[2], 16);
      }
      else if (color.Length == 6)
      {
        num1 = Convert.ToByte(((int) color[0]).ToString() + (object) color[1], 16);
        num2 = Convert.ToByte(((int) color[2]).ToString() + (object) color[3], 16);
        num3 = Convert.ToByte(((int) color[4]).ToString() + (object) color[5], 16);
      }
      return Color.FromArgb((int) num1, (int) num2, (int) num3);
    }

    public static float ByteToFloat(byte[] packet, int offset)
    {
      byte[] numArray = new byte[4];
      Array.Copy((Array) packet, offset, (Array) numArray, 0, 4);
      return BitConverter.ToSingle(numArray, 0);
    }

    public static int ByteToInteger(byte[] packet, int offset)
    {
      byte[] numArray = new byte[4];
      Array.Copy((Array) packet, offset, (Array) numArray, 0, 4);
      return BitConverter.ToInt32(numArray, 0);
    }

    public static ushort ByteToUShort(byte[] packet, int offset)
    {
      byte[] numArray = new byte[2];
      Array.Copy((Array) packet, offset, (Array) numArray, 0, 2);
      return BitConverter.ToUInt16(numArray, 0);
    }

    public static string convertToMD5(string Input)
    {
      try
      {
        byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(Input));
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < hash.Length; ++index)
          stringBuilder.Append(hash[index].ToString("x2"));
        return stringBuilder.ToString();
      }
      catch
      {
        return (string) null;
      }
    }

    public static int WarRockDateTime
    {
      get
      {
        return int.Parse(string.Format("{0:yyMMddHH}", (object) DateTime.Now));
      }
    }

    public static bool isMacAddress(string mac)
    {
      return !new Regex("^([:xdigit:]){12}$").IsMatch(mac);
    }

    public static bool IsAlphaNumeric(string input)
    {
      return !new Regex("[^a-zA-Z0-9]").IsMatch(input);
    }

    public static bool IsMD5Hash(string input)
    {
      return new Regex("[0-9a-f]{32}").IsMatch(input);
    }
  }
}
