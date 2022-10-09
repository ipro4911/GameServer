// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.ChannelManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game_Server.Managers
{
  internal class ChannelManager
  {
    private static int chCount = 4;
    public static ConcurrentDictionary<int, Channel> channels = new ConcurrentDictionary<int, Channel>();
    public static Thread updateThread;

    public static void Setup()
    {
      for (int key = 0; key < ChannelManager.chCount; ++key)
        ChannelManager.channels.TryAdd(key, new Channel()
        {
          channelId = key
        });
      ChannelManager.updateThread = new Thread(new ThreadStart(ChannelManager.UpdateRooms));
      ChannelManager.updateThread.Start();
    }

    private static void UpdateRooms()
    {
      while (true)
      {
        foreach (Room allRoom in ChannelManager.GetAllRooms())
        {
          Room r = allRoom;
          Thread thread = new Thread((ThreadStart) (() =>
          {
            try
            {
              r.update();
            }
            catch
            {
            }
          }));
          try
          {
            if (r.users.Values.Where<User>((Func<User, bool>) (u =>
            {
              if (u != null)
                return u.IsConnectionAlive;
              return false;
            })).Count<User>() <= 0 || r.users.Count <= 0)
              r.remove();
            if (!Game_Server.Configs.Server.Debug)
            {
              foreach (User user in r.users.Values.Where<User>((Func<User, bool>) (u => u.tcpClient == null)))
              {
                Log.WriteDebug("[DEBUG] " + user.nickname + " has been kicked out from the server because hasn't a tcp connection");
                user.disconnect();
              }
            }
          }
          catch
          {
          }
        }
        Thread.Sleep(200);
      }
    }

    public static List<Room> GetAllRooms()
    {
      List<Room> roomList = new List<Room>();
      foreach (Channel channel in (IEnumerable<Channel>) ChannelManager.channels.Values)
        roomList.AddRange((IEnumerable<Room>) channel.rooms.Values);
      return roomList;
    }
  }
}
