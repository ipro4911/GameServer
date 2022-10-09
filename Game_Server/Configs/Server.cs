// Decompiled with JetBrains decompiler
// Type: Game_Server.Configs.Server
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Configs
{
  internal class Server
  {
    public static int MaxSessions = 1000;
    public static int PingRequestTick = 5000;
    public static int serverId = 1;
    public static int ServerPort = 10375;
    public static int GameplayPort = 10376;
    public static string SystemName = "SYSTEM";
    public static bool Debug = true;
    public static int ClientVersion = 10;
    public static int MaxSpectators = 5;
    public static int MaxPing = 600;

    public static void Load()
    {
      try
      {
        Server.serverId = int.Parse(IO.ReadValue(nameof (Server), "ServerID"));
        Server.Debug = IO.ReadValue(nameof (Server), "DebugMode") == "true";
        Server.ClientVersion = int.Parse(IO.ReadValue(nameof (Server), "ClientVersion"));
        Server.ServerPort = int.Parse(IO.ReadValue(nameof (Server), "ServerPort"));
        Server.GameplayPort = int.Parse(IO.ReadValue(nameof (Server), "GameplayPort"));
        Server.MaxSessions = int.Parse(IO.ReadValue(nameof (Server), "MaxSessions"));
        Server.PingRequestTick = int.Parse(IO.ReadValue(nameof (Server), "PingRequestTick"));
        Server.SystemName = IO.ReadValue(nameof (Server), "SystemName");
        Server.MaxSpectators = int.Parse(IO.ReadValue(nameof (Server), "MaxSpectators"));
        Server.MaxPing = int.Parse(IO.ReadValue(nameof (Server), "MaxPing"));
        Server.LoadSub();
      }
      catch (Exception ex)
      {
        Log.WriteError("Couldn't Load server info " + ex.Message);
      }
    }

    public static void LoadSub()
    {
      Server.Channels.Load();
      Server.Clan.Load();
      Server.AntiCheat.Load();
      Server.Player.Load();
      Server.LoginEvent.Load();
      Server.Experience.Load();
      Server.ChatEvent.Load();
      Server.Christmas.Load();
      Server.RandomBoxEvent.Load();
      Server.ChristmasBoxEvent.Load();
      Server.SupplyBoxEvent.Load();
    }

    internal class Player
    {
      public static int MaxInventorySlot = 100;
      public static int MaxCostumeSlot = 100;
      public static int LevelupDinar = 2500;
      public static int LevelupCash = 25000;
      public static bool CouponEvent = true;
      public static bool CarePackage = true;

      public static void Load()
      {
        Server.Player.MaxInventorySlot = int.Parse(IO.ReadValue(nameof (Player), "MaxInventorySlots"));
        Server.Player.MaxCostumeSlot = int.Parse(IO.ReadValue(nameof (Player), "MaxCostumesSlots"));
        Server.Player.LevelupDinar = int.Parse(IO.ReadValue(nameof (Player), "LevelupDinar"));
        Server.Player.LevelupCash = int.Parse(IO.ReadValue(nameof (Player), "LevelupCash"));
        Server.Player.CouponEvent = bool.Parse(IO.ReadValue(nameof (Player), "CouponEvent"));
        Server.Player.CarePackage = bool.Parse(IO.ReadValue(nameof (Player), "CarePackage"));
      }
    }

    internal class AntiCheat
    {
      public static string name = "System";
      public static bool enabled = false;
      public static int routinetick = 15;
      public static int serverport = 5040;

      public static void Load()
      {
        Server.AntiCheat.name = IO.ReadValue(nameof (AntiCheat), "AntiCheatName");
        Server.AntiCheat.enabled = bool.Parse(IO.ReadValue(nameof (AntiCheat), "AntiEnabled"));
        Server.AntiCheat.routinetick = int.Parse(IO.ReadValue(nameof (AntiCheat), "AntiRoutineTick"));
        Server.AntiCheat.serverport = int.Parse(IO.ReadValue(nameof (AntiCheat), "ServerPort"));
      }
    }

    internal class Clan
    {
      public static int MaxClanSlot = 100;
      public static int ClanDefaultSlot = 20;
      public static int CreationCost = 10000;
      public static string DefaultAnnouncment = "";
      public static string DefaultDescription = "";

      public static void Load()
      {
        Server.Clan.CreationCost = int.Parse(IO.ReadValue(nameof (Clan), "CreationCost"));
        Server.Clan.MaxClanSlot = int.Parse(IO.ReadValue(nameof (Clan), "MaxSlots"));
        Server.Clan.ClanDefaultSlot = int.Parse(IO.ReadValue(nameof (Clan), "DefaultSlot"));
        Server.Clan.DefaultAnnouncment = IO.ReadValue(nameof (Clan), "DefaultAnnouncment");
        Server.Clan.DefaultDescription = IO.ReadValue(nameof (Clan), "DefaultDescription");
      }
    }

    internal class Channels
    {
      public static bool Infantry = true;
      public static bool Vehicular = true;
      public static bool Zombie = true;

      public static void Load()
      {
        Server.Channels.Infantry = bool.Parse(IO.ReadAttribute(nameof (Server), nameof (Channels), "Infantry"));
        Server.Channels.Vehicular = bool.Parse(IO.ReadAttribute(nameof (Server), nameof (Channels), "Vehicular"));
        Server.Channels.Zombie = bool.Parse(IO.ReadAttribute(nameof (Server), nameof (Channels), "Zombie"));
      }
    }

    internal class Experience
    {
      public static double ExpRate = 2.0;
      public static double DinarRate = 1.0;
      public static int OnKillPoints = 5;
      public static int OnHSKillPoints = 3;
      public static int OnDeathPoints = 1;
      public static int OnTakeFlag = 3;
      public static int OnVehicleKillAdditional = 5;
      public static int OnVehicleKill = 10;
      public static int OnFriendHeal = 5;
      public static int OnBombPlant = 10;
      public static int OnBombDefuse = 20;
      public static int OnNormalPlaceUse = 5;
      public static int OnLandPlaceUse = 5;
      public static int OnMissionHack = 15;
      public static int MaxFlags = 30;
      public static int MaxExperience = 6069;
      public static int MaxDinars = 6069;

      public static void Load()
      {
        Server.Experience.ExpRate = double.Parse(IO.ReadValue(nameof (Experience), "EXPRate").Replace(".", ","));
        Server.Experience.DinarRate = double.Parse(IO.ReadValue(nameof (Experience), "DinarRate").Replace(".", ","));
        Server.Experience.OnKillPoints = int.Parse(IO.ReadValue(nameof (Experience), "OnKillPoints"));
        Server.Experience.OnHSKillPoints = int.Parse(IO.ReadValue(nameof (Experience), "OnHSKillPoints"));
        Server.Experience.OnDeathPoints = int.Parse(IO.ReadValue(nameof (Experience), "OnDeathPoints"));
        Server.Experience.OnTakeFlag = int.Parse(IO.ReadValue(nameof (Experience), "OnTakeFlag"));
        Server.Experience.OnVehicleKillAdditional = int.Parse(IO.ReadValue(nameof (Experience), "OnVehicleKillAdditional"));
        Server.Experience.OnVehicleKill = int.Parse(IO.ReadValue(nameof (Experience), "OnVehicleKill"));
        Server.Experience.OnFriendHeal = int.Parse(IO.ReadValue(nameof (Experience), "OnFriendHeal"));
        Server.Experience.OnBombPlant = int.Parse(IO.ReadValue(nameof (Experience), "OnBombPlant"));
        Server.Experience.OnBombDefuse = int.Parse(IO.ReadValue(nameof (Experience), "OnBombDefuse"));
        Server.Experience.OnNormalPlaceUse = int.Parse(IO.ReadValue(nameof (Experience), "OnNormalPlaceUse"));
        Server.Experience.OnLandPlaceUse = int.Parse(IO.ReadValue(nameof (Experience), "OnLandPlaceUse"));
        Server.Experience.OnMissionHack = int.Parse(IO.ReadValue(nameof (Experience), "OnMissionHack"));
        Server.Experience.MaxFlags = int.Parse(IO.ReadValue(nameof (Experience), "MaxFlags"));
        Server.Experience.MaxExperience = int.Parse(IO.ReadValue(nameof (Experience), "MaxExperience"));
        Server.Experience.MaxDinars = int.Parse(IO.ReadValue(nameof (Experience), "MaxDinars"));
      }
    }

    internal class LoginEvent
    {
      public static bool enabled = false;
      public static int MinDays = 1;
      public static int MaxDays = 3;
      public static string[] items;

      public static void Load()
      {
        Server.LoginEvent.enabled = bool.Parse(IO.ReadValue(nameof (LoginEvent), "Enabled"));
        Server.LoginEvent.items = IO.ReadValue(nameof (LoginEvent), "Items").Split(',');
        string[] strArray = IO.ReadValue(nameof (LoginEvent), "DaysRange").Split('-');
        int.TryParse(strArray[0], out Server.LoginEvent.MinDays);
        int.TryParse(strArray[1], out Server.LoginEvent.MaxDays);
      }
    }

    internal class ChatEvent
    {
      public static bool enabled = false;
      public static int eventId = -1;
      public static bool daily = false;
      public static string message = "This_is_message_test";
      public static string popupMessage = "This_is_message_test";
      public static int MinDays = 1;
      public static int MaxDays = 3;
      public static string[] items;

      public static void Load()
      {
        Server.ChatEvent.enabled = bool.Parse(IO.ReadValue(nameof (ChatEvent), "Enabled"));
        Server.ChatEvent.daily = bool.Parse(IO.ReadValue(nameof (ChatEvent), "Daily"));
        Server.ChatEvent.items = IO.ReadValue(nameof (ChatEvent), "Items").Split(',');
        Server.ChatEvent.message = IO.ReadValue(nameof (ChatEvent), "Message");
        Server.ChatEvent.popupMessage = IO.ReadValue(nameof (ChatEvent), "PopupMessage");
        string[] strArray = IO.ReadValue(nameof (ChatEvent), "DaysRange").Split('-');
        int.TryParse(strArray[0], out Server.ChatEvent.MinDays);
        int.TryParse(strArray[1], out Server.ChatEvent.MaxDays);
        int.TryParse(IO.ReadValue(nameof (ChatEvent), "EventID"), out Server.ChatEvent.eventId);
      }
    }

    internal class Christmas
    {
      public static bool enabled = false;
      public static double ExpRate = 1.0;
      public static double DinarRate = 0.25;

      public static bool IsChristmas
      {
        get
        {
          if (DateTime.Today.Day == 25)
            return DateTime.Today.Month == 12;
          return false;
        }
      }

      public static void Load()
      {
        Server.Christmas.enabled = bool.Parse(IO.ReadValue(nameof (Christmas), "Enabled"));
        double.TryParse(IO.ReadValue(nameof (Christmas), "ExpRate"), out Server.Christmas.ExpRate);
        double.TryParse(IO.ReadValue(nameof (Christmas), "DinarRate"), out Server.Christmas.DinarRate);
      }
    }

    internal class RandomBoxEvent
    {
      public static bool Enabled = false;
      public static int hour = 14;
      public static int MinDays = 1;
      public static int MaxDays = 3;
      public static string BoxCode = "CZ99";
      public static string[] items;

      public static void Load()
      {
        Server.RandomBoxEvent.Enabled = bool.Parse(IO.ReadValue(nameof (RandomBoxEvent), "Enabled"));
        Server.RandomBoxEvent.hour = int.Parse(IO.ReadValue(nameof (RandomBoxEvent), "Hour"));
        Server.RandomBoxEvent.items = IO.ReadValue(nameof (RandomBoxEvent), "Items").Split(',');
        Server.RandomBoxEvent.BoxCode = IO.ReadValue(nameof (RandomBoxEvent), "BoxCode");
        string[] strArray = IO.ReadValue(nameof (RandomBoxEvent), "DaysRange").Split('-');
        int.TryParse(strArray[0], out Server.RandomBoxEvent.MinDays);
        int.TryParse(strArray[1], out Server.RandomBoxEvent.MaxDays);
      }
    }

    internal class ChristmasBoxEvent
    {
      public static bool Enabled = false;
      public static int hour = 14;
      public static int MinDays = 1;
      public static int MaxDays = 3;
      public static string BoxCode = "CZ99";
      public static string[] items;

      public static void Load()
      {
        Server.ChristmasBoxEvent.Enabled = bool.Parse(IO.ReadValue(nameof (ChristmasBoxEvent), "Enabled"));
        Server.ChristmasBoxEvent.hour = int.Parse(IO.ReadValue(nameof (ChristmasBoxEvent), "Hour"));
        Server.ChristmasBoxEvent.items = IO.ReadValue(nameof (ChristmasBoxEvent), "Items").Split(',');
        Server.ChristmasBoxEvent.BoxCode = IO.ReadValue(nameof (ChristmasBoxEvent), "BoxCode");
        string[] strArray = IO.ReadValue(nameof (ChristmasBoxEvent), "DaysRange").Split('-');
        int.TryParse(strArray[0], out Server.ChristmasBoxEvent.MinDays);
        int.TryParse(strArray[1], out Server.ChristmasBoxEvent.MaxDays);
      }
    }

    internal class SupplyBoxEvent
    {
      public static bool Enabled;

      public static void Load()
      {
        Server.SupplyBoxEvent.Enabled = bool.Parse(IO.ReadValue(nameof (SupplyBoxEvent), "Enabled"));
      }
    }

    internal class ItemShop
    {
      public static string[] hiddenItems = new string[6]
      {
        "CB02",
        "CB53",
        "CB54",
        "CZ83",
        "CZ84",
        "CZ85"
      };
      public static string[] attendanceBox = new string[4]
      {
        "DF50",
        "DJ22",
        "DG48",
        "DC85"
      };
    }
  }
}
