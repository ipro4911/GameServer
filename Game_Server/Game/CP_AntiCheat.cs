// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_AntiCheat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server.Game
{
  internal class CP_AntiCheat : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      int num1 = int.Parse(this.getBlock(0));
      switch ((CP_AntiCheat.Subtype) num1)
      {
        case CP_AntiCheat.Subtype.LoginVerification:
          try
          {
            int num2 = int.Parse(this.getBlock(1));
            string block = this.getBlock(2);
            int num3 = int.Parse(this.getBlock(3));
            usr.hwid = block;
            if (!Game_Server.Configs.Server.AntiCheat.enabled)
              break;
            int num4 = (int) Math.Ceiling((Decimal) (num3 / 244));
            if (BanManager.isHWIDBanned(usr.hwid))
            {
              Log.WriteError(usr.nickname + " -> tried to login with banned hwid!");
              usr.disconnect();
              break;
            }
            if (num4 == usr.ticketId)
            {
              if (num2 != Game_Server.Configs.Server.ClientVersion && Game_Server.Configs.Server.ClientVersion != -1 && !Game_Server.Configs.Server.Debug)
              {
                Log.WriteError(usr.nickname + " tried to login with an older patch!");
                usr.send((Packet) new SP_WelcomePacket(SP_WelcomePacket.ErrorCodes.ClientVersionMissmatch));
                byte[] bytes = new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Notice1, "Your client version is different, please download patchs!", 999U, usr.nickname).GetBytes();
                for (int index = 0; index < 3; ++index)
                  usr.sendBuffer(bytes);
                usr.disconnect();
                break;
              }
              usr.AntiCheatCheck = true;
              break;
            }
            if (num4 == 0)
            {
              usr.send((Packet) new SP_CharacterInfo(SP_CharacterInfo.ErrCodes.NormalProcedure));
              Log.WriteError(usr.nickname + " invalid TicketID! Please check the DSETUP Client Version");
              usr.disconnect();
              break;
            }
            usr.send((Packet) new SP_CharacterInfo(SP_CharacterInfo.ErrCodes.NormalProcedure));
            Log.WriteError(usr.nickname + " invalid TicketID!");
            usr.disconnect();
            break;
          }
          catch
          {
            Log.WriteError(usr.nickname + " tried to login but system got error and he got kicked!");
            usr.disconnect();
            break;
          }
        case CP_AntiCheat.Subtype.Detection:
          int num5 = int.Parse(this.getBlock(1));
          string str1 = "Unknown (IP: " + usr.IP + ")";
          string str2;
                    switch (num5)
                    {
                        case 0:
                            str2 = "Present (maybe Bandicam / Fraps / GameBooster)";
                            break;
                        case 1:
                            str2 = "Draw Index Primitive";
                            break;
                        case 2:
                            str2 = "Cheat Engine";
                            break;
                        case 3:
                            str2 = "End Scene";
                            break;
                        case 4:
                            str2 = "Draw Index Primitive VMT";
                            break;
                        case 5:
                            str2 = "End Scene VMT";
                            break;
                            switch (num5)
                            {
                                default:
                                    break;
                            }
                       //case 6:
                            switch (int.Parse(this.getBlock(2)))
                            {
                                case 0:
                                    str2 = "Generic Injector";
                                    break;
                                case 1:
                                    str2 = "XTCheats Client";
                                    break;
                                case 2:
                                    str2 = "NetLimiter";
                                    break;
                                default:
                                    str2 = "Generic Illegal Third Party Tool";
                                    break;
                            
                    }            
            case 556:
              str2 = "Anticheat Detection Occured";
              break;
            case 9991:
              str2 = "Generic Hack Detection (v2)";
              break;
            default:
              str2 = "Generic Hack Detection";
              break;
                    }
                    DB.RunQuery("INSERT INTO anticheat_logs (userid, description, timestamp) VALUES ('" + (object) usr.userId + "', '" + usr.nickname + " - Detection Type: " + str2 + "', '" + (object) Generic.timestamp + "')");
          break;
        case CP_AntiCheat.Subtype.ShoxGuardDetection:
          ushort result;
          if (ushort.TryParse(this.getBlock(1), out result))
          {
            usr.lastShoxTick = Generic.timestamp;
            usr.shoxDetection = true;
            string str3 = this.getBlock(2).Replace('\x001D', ' ');
            string str4;
            switch (result)
            {
              case 1:
                str4 = "System API Hook has been detected";
                break;
              case 2:
                str4 = "Disallowed Program has been found";
                break;
              case 3:
                str4 = "User mode debugging has been detected";
                break;
              case 4:
                str4 = "Rendering API Hook has been detected";
                break;
              default:
                if (str3.Contains("reinterpret_cast"))
                {
                  string[] strArray = str3.Replace("reinterpret_cast<", "").Replace(">", "").Split(',');
                  int[] numArray = new int[3]
                  {
                    int.Parse(strArray[0]),
                    int.Parse(strArray[1]),
                    int.Parse(strArray[2])
                  };
                  if (numArray.Length == 3)
                  {
                    if ((numArray[0] ^ numArray[2]) != numArray[1])
                    {
                      Log.WriteError("[" + usr.nickname + "] Invalid ShoxGuard Algorithm");
                      usr.disconnect();
                    }
                  }
                  else
                  {
                    Log.WriteError("[" + usr.nickname + "] Invalid packet length");
                    usr.disconnect();
                  }
                }
                else
                  usr.disconnect();
                usr.shoxDetection = false;
                return;
            }
            if (usr.shoxDetection)
              break;
            DB.RunQuery("INSERT INTO anticheat_logs (userid, description, timestamp) VALUES ('" + (object) usr.userId + "', '" + usr.nickname + " - ShoxGuard Detection Type: (" + str4 + ") " + str3 + "', '" + (object) Generic.timestamp + "')");
            Log.WriteError("[SHOXGUARD] " + usr.nickname + " has been detected (ViolationType: " + str4 + ", data: " + str3);
            break;
          }
          usr.disconnect();
          break;
        case CP_AntiCheat.Subtype.KickRequest:
          if (Game_Server.Configs.Server.Debug)
            break;
          Log.WriteError("Received kick out request [DSETUP.dll] for the user " + usr.nickname);
          usr.disconnect();
          break;
        default:
          Log.WriteError("Unknown subtype of Anti Cheat received [" + (object) num1 + "]");
          usr.disconnect();
          break;
            }
        }

        private enum Subtype
    {
      LoginVerification = 5,
      Reauth = 6,
      Detection = 22, // 0x00000016
      ShoxGuardDetection = 43, // 0x0000002B
      KickRequest = 666, // 0x0000029A
    }

    private enum ShoxViolationType
    {
      NoViolation,
      SystemAPIInterception,
      DisallowedProgramFound,
      ProbeOfDebugging,
      RenderingAPIInterception,
    }
    }
}
