// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_WelcomePacket
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Globalization;

namespace Game_Server.Game
{
  internal class SP_WelcomePacket : Packet
  {
    public int WeekCalculation(DateTime dt)
    {
      return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    public SP_WelcomePacket(SP_WelcomePacket.ErrorCodes ErrorCode)
    {
      this.newPacket((ushort) 24832);
      this.addBlock((object) (int) ErrorCode);
    }

    public SP_WelcomePacket(Game_Server.User usr)
    {
      string str = DateTime.Now.Second.ToString() + "/" + (object) DateTime.Now.Minute + "/" + (object) (DateTime.Now.Hour + 18) + "/" + (object) (DateTime.Now.Day - 1) + "/" + (object) (DateTime.Now.Month - 1) + "/" + (object) (DateTime.Now.Year - 1900) + "/" + (object) this.WeekCalculation(DateTime.Now) + "/" + (object) DateTime.Now.DayOfYear + "/0";
      this.newPacket((ushort) 24832);
      this.addBlock((object) 1);
      this.addBlock((object) str);
      this.addBlock((object) usr.connectionId);
    }

    internal enum ErrorCodes
    {
      Success = 1,
      ClientVersionMissmatch = 90020, // 0x00015FA4
    }
  }
}
