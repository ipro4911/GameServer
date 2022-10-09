// Decompiled with JetBrains decompiler
// Type: Game_Server.Program
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Anti_Cheat.Structure;
using Game_Server.Configs;
using Game_Server.Game;
using Game_Server.Managers;
using Game_Server.Networking;
using Game_Server.Web;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Game_Server
{
    internal class Program
    {
        public static bool isEvent = false;
        public static int EventTime = -1;
        public static int EXPEvent = -1;
        public static int DinarEvent = -1;
        private static Thread _ServerThread = (Thread)null;
        private static Thread _CommandThread = (Thread)null;
        public static bool running = false;
        public static Server server = (Server)null;
        public static LookupService ipLookup;
        internal static object timestamp;

        private static void ConsoleTitle()
        {
            while (true)
            {
                try
                {
                    TimeSpan timeSpan = DateTime.Now - Process.GetCurrentProcess().StartTime;
                    int count = Process.GetCurrentProcess().Threads.Count;
                    System.Console.Title = Game_Server.Configs.Console.title + " | " + (object)count + " T | [" + (object)UserManager.ServerUsers.Count + " online - " + (object)timeSpan.Days + " days, " + (object)timeSpan.Hours + " hours, " + (object)timeSpan.Minutes + " minutes]";
                }
                catch
                {
                }
                Thread.Sleep(1000);
            }
        }

        private static void Main(string[] args)
        {
            System.Console.Title = Game_Server.Configs.Console.title;
            System.Console.WriteLine("  _______ ______   _______ _____ _____ _  _____      _____ ____  _____  ______ ");
            System.Console.WriteLine(" |__   __/ __ \\ \\ / |_   _|_   _/ ____( )/ ____|    / ____/ __ \\|  __ \\|  ____|");
            System.Console.WriteLine("    | | | |  | \\ V /  | |   | || |    |/| (___     | |   | |  | | |__) | |__   ");
            System.Console.WriteLine("    | | | |  | |> <   | |   | || |       \\___ \\    | |   | |  | |  _  /|  __|  ");
            System.Console.WriteLine("    | | | |__| / . \\ _| |_ _| || |____   ____) |   | |___| |__| | | \\ \\| |____ ");
            System.Console.WriteLine("    |_|  \\____/_/ \\_|_____|_____\\_____| |_____/     \\_____\\____/|_|  \\_|______|");
            System.Console.WriteLine("|______________________________________________________________________________|");
            Log.WriteBlank(1);
            System.Console.WriteLine(" - Wrote by ToXiiC");
            System.Console.WriteLine(" - Thanks to CodeDragon, Kill1212");
            Log.WriteBlank(2);
            Log.setup("ToXiiC_Core_" + DateTime.Now.ToString("dd-MM-yyyy") + ".log");
            new Thread(new ThreadStart(Program.ConsoleTitle)).Start();
            if (System.Type.GetType("Mono.Runtime") != (System.Type)null)
                Log.WriteLine("This GameServer is running under Mono VM!");
            Program.running = Program.initializeStartup();
            if (Program.running)
                return;
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine("[" + new string('-', Game_Server.Configs.Console.width - 2) + "]");
            System.Console.ReadKey();
            System.Console.ReadKey();
        }
        private static void serverLoop()
        {
            try
            {
                while (!Program.running)
                    Thread.Sleep(100);
                while (Program.running)
                {
                    TimeSpan timeSpan = DateTime.Now - Process.GetCurrentProcess().StartTime;
                    //Console.Title = "[RUNNING]: BW VER4.0 By Lucio - Game Server |" + NetworkSocket.BannedIPs.Count + " IPs banned | Ram usage: " + (GC.GetTotalMemory(false) / 1024L) + " KB | " + UserManager.UserCount + " online players (Running since: " + timeSpan.Days + " days, " + timeSpan.Hours + " hours, " + timeSpan.Minutes + ", minutes!)";
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }
        }

        static bool initializeStartup()
        {
            string settingsFile = Application.StartupPath + "/gameserver.xml";

            IO.path = settingsFile;
            IO.workingDirectory = Application.StartupPath;

            if (!System.IO.File.Exists(settingsFile))
            {
                Log.WriteError("Error: Cannot find gameserver.xml");
                return false;
            }

            if (!System.IO.File.Exists(IO.workingDirectory + @"/items.bin"))
            {
                Log.WriteError("Error: Cannot find items.bin");
                return false;
            }

            string GeoIP = IO.workingDirectory + @"/GeoIP.dat";
            if (System.IO.File.Exists(GeoIP) == false)
            {
                Log.WriteError("Error: Cannot find GeoIP.dat");
                return false;
            }

            ipLookup = new LookupService(GeoIP, LookupService.GEOIP_MEMORY_CACHE);

            string host = IO.ReadValue("Database", "host");
            int port = int.Parse(IO.ReadValue("Database", "port"));
            string username = IO.ReadValue("Database", "user");
            string password = IO.ReadValue("Database", "password");
            string database = IO.ReadValue("Database", "database");
            int poolsize = int.Parse(IO.ReadValue("Database", "poolsize"));

            DB.openConnection(host, port, database, username, password, poolsize);

            Configs.Main.setup();

            DB.RunQuery("UPDATE users SET online='0' WHERE serverid='" + Configs.Server.serverId + "' OR serverid='-1'");
            Log.WriteLine("All accounts have been set offline");

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
            CommandManager.Load();
           /* Program._ServerThread = new Thread(new ThreadStart(Program.serverLoop));
            Program._ServerThread.Priority = ThreadPriority.BelowNormal;
            Program._ServerThread.Start();
            Program._CommandThread = new Thread(new ThreadStart(Program.commandLoop));
            Program._CommandThread.Priority = ThreadPriority.BelowNormal;
            Program._CommandThread.Start();
            new Thread(new ThreadStart(BanManager.unbanLoop))
            {
                Priority = ThreadPriority.BelowNormal
            }.Start();
           */


            if (!NetworkSocket.InitializeSocket(Game_Server.Configs.Server.ServerPort))
            {
                Log.WriteError("Error: Cannot Initialize a new socket on the port " + (object)Game_Server.Configs.Server.ServerPort);
                return false;
            }
            if (!TCP.Start(Game_Server.Configs.Server.GameplayPort))
            {
                Log.WriteError("Error: Cannot Initialize a new socket on the port " + (object)Game_Server.Configs.Server.GameplayPort);
                return false;
            }
            if (Game_Server.Configs.Server.AntiCheat.enabled)
            {
                AntiCheatServer antiCheatServer = new AntiCheatServer();
                PacketManager.Load();
                if (!antiCheatServer.Initialize(Game_Server.Configs.Server.AntiCheat.serverport))
                {
                    Log.WriteError("Error: Cannot Initialize a new socket on the port " + (object)Game_Server.Configs.Server.AntiCheat.serverport);
                    return false;
                }
            }
            if (Game_Server.Configs.Web.allow && !WebManager.openSocket(Game_Server.Configs.Web.port))
                Log.WriteError("Error: Cannot Initialize a new web socket on the port " + (object)Game_Server.Configs.Web.port);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.CurrentDomain_UnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(Program.Application_ThreadException);
            return true;
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.WriteError("Unhandled exception [2]: " + e.Exception.Message + " " + e.Exception.StackTrace);
        }

        private static void CurrentDomain_UnhandledException(
          object sender,
          UnhandledExceptionEventArgs e)
        {
            Log.WriteError("Unhandled exception [1]: " + (e.ExceptionObject as Exception).Message + " " + (e.ExceptionObject as Exception).StackTrace);
        }




       
        public static void shutDown()
        {
            if (!Program.running)
                return;
            Program.running = false;
            DB.RunQuery("UPDATE users SET online='0' WHERE serverid='" + (object)Game_Server.Configs.Server.serverId + "'");
            foreach (Room allRoom in ChannelManager.GetAllRooms())
            {
                allRoom.EndGame();
                allRoom.remove();
            }
            foreach (User allUser in UserManager.getAllUsers())
            {
                try
                {
                    allUser.disconnect();
                }
                catch
                {
                }
            }
            Environment.Exit(0);
        }
    }
}





