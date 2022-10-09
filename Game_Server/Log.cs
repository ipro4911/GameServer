// Decompiled with JetBrains decompiler
// Type: Game_Server.Log
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Diagnostics;
using System.IO;

namespace Game_Server
{
  internal class Log
  {
    private static object writeObj = new object();
    private static StreamWriter LogFile;

    public static bool setup(string logFile)
    {
      try
      {
        Log.LogFile = new StreamWriter(logFile, true);
        Log.LogFile.WriteLine("/* Start up: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " */");
        Log.LogFile.WriteLine("");
        Log.LogFile.Flush();
      }
      catch
      {
      }
      return false;
    }

    public static void WriteLine(string str)
    {
      Log.writeline(str, ConsoleColor.DarkGreen);
    }

    public static void WriteError(string str)
    {
      Log.writeline(str, ConsoleColor.DarkRed);
      if (Log.LogFile == null)
        return;
      DateTime now = DateTime.Now;
      StackFrame frame = new StackTrace().GetFrame(2);
      Log.LogFile.WriteLine("[" + now.ToShortDateString() + " " + now.ToLongTimeString() + "] [" + frame.GetMethod().ReflectedType.Name + "." + frame.GetMethod().Name + "] » " + str);
      Log.LogFile.Flush();
    }

    public static void WriteDebug(string str)
    {
      Log.writeline(str, ConsoleColor.DarkMagenta);
      if (Log.LogFile == null)
        return;
      DateTime now = DateTime.Now;
      StackFrame frame = new StackTrace().GetFrame(2);
      Log.LogFile.WriteLine("[" + now.ToShortDateString() + " " + now.ToLongTimeString() + "] [" + frame.GetMethod().ReflectedType.Name + "." + frame.GetMethod().Name + "] » " + str);
      Log.LogFile.Flush();
    }

    public static void WriteBlank(int count = 1)
    {
      for (int index = 0; index < count; ++index)
        Console.WriteLine("");
    }

    private static void writeline(string str, ConsoleColor c)
    {
      lock (Log.writeObj)
      {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("hh:mm:ss:fff - dd/MM/yyyy") + "] > ");
        Console.ForegroundColor = c;
        Console.Write(str);
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Gray;
      }
    }

    internal static void WriteFile(string outlog)
    {
      if (Log.LogFile == null)
        return;
      DateTime now = DateTime.Now;
      StackFrame frame = new StackTrace().GetFrame(2);
      Log.LogFile.WriteLine("[" + now.ToShortDateString() + " " + now.ToLongTimeString() + "] [" + frame.GetMethod().ReflectedType.Name + "." + frame.GetMethod().Name + "] » " + outlog);
      Log.LogFile.Flush();
    }
  }
}
