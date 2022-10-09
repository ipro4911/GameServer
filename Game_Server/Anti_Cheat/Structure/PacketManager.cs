// Decompiled with JetBrains decompiler
// Type: Game_Server.Anti_Cheat.Structure.PacketManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Anti_Cheat.Data;
using System;
using System.Collections.Generic;

namespace Game_Server.Anti_Cheat.Structure
{
  internal class PacketManager
  {
    private static Dictionary<int, Handler> handlers = new Dictionary<int, Handler>();

        public static void Load()
        {
            PacketManager.AddPacket(10300, (Handler)new CP_Authentication());
        }

    public static Handler ParsePacket(string packetStr)
    {
      string[] strArray = packetStr.Split(' ');
      uint result1;
      uint.TryParse(strArray[0], out result1);
      int result2;
      int.TryParse(strArray[1], out result2);
      if (result1 > 0U && result2 > 0)
      {
        if (PacketManager.handlers.ContainsKey(result2))
        {
          string[] blocks = new string[strArray.Length - 2];
          Array.Copy((Array) strArray, 2, (Array) blocks, 0, strArray.Length - 2);
          Handler handler = PacketManager.handlers[result2];
          handler.FillData(result1, result2, blocks);
          return handler;
        }
        if (Game_Server.Configs.Server.Debug)
          Log.WriteError("Unhandled AC Packet ID " + (object) result2);
      }
      return (Handler) null;
    }

    private static void AddPacket(int packetId, Handler h)
    {
      if (!PacketManager.handlers.ContainsKey(packetId))
        PacketManager.handlers.Add(packetId, h);
      else
        Log.WriteError(packetId.ToString() + " key is already used in the dictionary (AC)");
    }
  }
}
