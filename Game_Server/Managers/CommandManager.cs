// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.CommandManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections;
using System.Threading;

namespace Game_Server.Managers
{
    internal class CommandManager
    {
        private static Thread consoleThread;

        public static void Load()
        {
            CommandManager.consoleThread = new Thread(new ThreadStart(CommandManager.commandLoop));
            CommandManager.consoleThread.Priority = ThreadPriority.Normal;
            CommandManager.consoleThread.Start();
        }

        private static void commandLoop()
        {
            try
            {
                while (Program.running)
                {
                    string str = System.Console.ReadLine();
                    string[] strArray = str.Split(' ');
                    switch (strArray[0].ToLower())
                    {
                        case "notice":
                            IEnumerator enumerator = UserManager.getAllUsers().GetEnumerator();
                            try
                            {
                                while (enumerator.MoveNext())
                                    ((User)enumerator.Current).send((Packet)new SP_Chat("Notice", SP_Chat.ChatType.Notice1, str.Substring(7), 999, "Notice"));
                                continue;
                            }
                            finally
                            {
                                IDisposable disposable = enumerator as IDisposable;
                                if (disposable != null)
                                    disposable.Dispose();
                            }
                        case "roominfo":
                            int ID = int.Parse(strArray[1]);
                            Room room = Room.getRoom(int.Parse(strArray[2]), ID);
                            if (room != null)
                            {
                                Log.WriteLine("SYSTEM >> Here all information about room N° " + ID);
                                Log.WriteLine("SYSTEM >> Room Name: " + room.name);
                                Log.WriteLine("SYSTEM >> Room Status: " + (room.status == 2 ? "Play" : "Wait"));
                                Log.WriteLine(string.Concat(new object[4]
                                {
                  "SYSTEM >> Password: ",
                   room.password,
                  " / MapID: ",
                   room.mapid,
                                }));
                                Log.WriteLine(string.Concat(new object[4]
                                {
                  "SYSTEM >> Type: ",
                  room.RoomType,
                  " / Mode:",
                  room.mode,
                                }));
                                Log.WriteLine("SYSTEM >> Players: " + room.tempPlayers.Count + "/" + room.maxusers + ", Spectators " + room.spectators.Count + "/10");
                                continue;
                            }
                            else
                                continue;
                        case "kick":
                            foreach (User virtualUser in UserManager.getAllUsers())
                            {
                                if (virtualUser != null && (virtualUser.nickname.ToLower().Equals(strArray[1].ToLower()) || virtualUser.nickname.ToLower().Equals(strArray[1].ToLower())))
                                {
                                    virtualUser.disconnect();
                                    Log.WriteLine("User " + virtualUser.nickname + " is kicked from the server!");
                                    break;
                                }
                            }
                            Log.WriteError("User " + strArray[1] + " is not online or doesn't exist!");
                            continue;
                        case "ban":
                            foreach (User virtualUser in UserManager.getAllUsers())
                            {
                                if (virtualUser.nickname.ToLower().Equals(strArray[1].ToLower()) || virtualUser.username.ToLower().Equals(strArray[1].ToLower()))
                                {
                                    DB.RunQuery("UPDATE users SET rank='0' WHERE id=" + virtualUser.userId);
                                    virtualUser.disconnect();
                                    Log.WriteLine("You banned: " + virtualUser.nickname + ".");
                                }
                            }
                            Log.WriteError("User " + strArray[1] + " is not online or doesn't exist!");
                            continue;
                        case "event":

                            if (int.Parse(strArray[2]) != 0 && int.Parse(strArray[3]) == 0)
                            {
                                Program.isEvent = true;
                                Program.EventTime = int.Parse(strArray[1]);
                                Program.EXPEvent = int.Parse(strArray[2]);
                                Program.DinarEvent = -1;
                                foreach (User User in UserManager.getAllUsers())
                                {
                                    User.send((Packet)new SP_PingInformation(User));
                                    User.send((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, " EXP " + (int.Parse(strArray[2]) * 100) + "% started.", 999, "Server"));
                                }
                            }
                            if (int.Parse(strArray[3]) != 0 && int.Parse(strArray[2]) == 0)
                            {
                                Program.isEvent = true;
                                Program.EventTime = int.Parse(strArray[1]);
                                Program.DinarEvent = int.Parse(strArray[3]);
                                Program.EXPEvent = -1;
                                foreach (User User in UserManager.getAllUsers())
                                {
                                    User.send((Packet)new SP_PingInformation(User));
                                    User.send((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, " DINAR " + (int.Parse(strArray[3]) * 100) + "% started.", 999, "Server"));
                                }
                            }
                            if (int.Parse(strArray[2]) != 0 && int.Parse(strArray[3]) != 0)
                            {
                                Program.isEvent = true;
                                Program.EventTime = int.Parse(strArray[1]);
                                Program.EXPEvent = int.Parse(strArray[2]);
                                Program.DinarEvent = int.Parse(strArray[3]);
                                foreach (User User in UserManager.getAllUsers())
                                {
                                    User.send((Packet)new SP_PingInformation(User));
                                    User.send((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, " EXP " + (int.Parse(strArray[2]) * 100) + "% & DINAR " + (int.Parse(strArray[3]) * 100) + "% started.", 999, "Server"));
                                }
                            }
                            if (int.Parse(strArray[2]) == 0 && int.Parse(strArray[3]) == 0)
                            {
                                Program.isEvent = false;
                                Program.EventTime = -1;
                                Program.EXPEvent = -1;
                                Program.DinarEvent = -1;
                                foreach (User User in UserManager.getAllUsers())
                                {
                                    User.send((Packet)new SP_PingInformation(User));
                                    User.send((Packet)new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, " EXP " + (int.Parse(strArray[2]) * 100) + "% & DINAR " + (int.Parse(strArray[3]) * 100) + "% started.", 999, "Server"));
                                }
                            }
                            continue;
                        case "kickall":

                            foreach (User virtualUser in UserManager.getAllUsers())
                                virtualUser.disconnect();
                            continue;
                        case "reload":
                            Managers.ItemManager.DecryptBinFile(IO.workingDirectory + "//items.bin");
                            Managers.ItemManager.LoadItems();
                            Managers.ChannelManager.Setup();
                            Managers.EXPEventManager.Load();
                            Managers.MapDataManager.Load();
                            Managers.ZombieManager.Load();
                            Managers.VehicleManager.Load();
                            Managers.ClanManager.Load();
                            Managers.RoutineManager.Load();
                            Managers.CarePackage.Load();
                            Managers.NoticeManager.Load();
                            Managers.BanManager.Load();
                            Managers.GunSmithManager.Load();
                            Managers.WordFilterManager.Load();
                            Managers.UserManager.setup();
                            Managers.Packet_Manager.setup();
                            Managers.RoomPacketManager.setup();
                            continue;
                        case "stop":
                            Program.shutDown();
                            continue;
                        default:
                            continue;
                    }
                }
            }
            catch
            {
            }
        }
    }
}

   /* public static void ConsoleCommand()
    {
      while (true)
      {
        try
        {
          string str = System.Console.ReadLine();
          switch (str.Split(' ')[0])
          {
            case "notice":
              UserManager.sendToServer((Packet) new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, str.Substring(7), 999U, "NULL"));
              Log.WriteLine("Successfully notice: " + str.Substring(7));
              break;
            case "stop":
              Log.WriteLine("Server is going to be shutdown!");
              UserManager.sendToServer((Packet) new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "Server is going to be restarted, sorry!!!", 999U, "NULL"));
              UserManager.sendToServer((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Game_Server.Configs.Server.SystemName + " >> Server is going to be restarted, sorry!!!", 999U, "Server"));
              Thread.Sleep(500);
              Program.shutDown();
              break;
          }
        }
        catch
        {
        }
        Thread.Sleep(2000);
      }
    }
  }
}
   */