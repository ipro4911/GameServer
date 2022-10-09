// Decompiled with JetBrains decompiler
// Type: DatabaseInfo
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

public class DatabaseInfo
{
  public static int COUNTRY_EDITION = 1;
  public static int REGION_EDITION_REV0 = 7;
  public static int REGION_EDITION_REV1 = 3;
  public static int CITY_EDITION_REV0 = 6;
  public static int CITY_EDITION_REV1 = 2;
  public static int ORG_EDITION = 5;
  public static int ISP_EDITION = 4;
  public static int PROXY_EDITION = 8;
  public static int ASNUM_EDITION = 9;
  public static int NETSPEED_EDITION = 10;
  public static int DOMAIN_EDITION = 11;
  public static int COUNTRY_EDITION_V6 = 12;
  public static int ASNUM_EDITION_V6 = 21;
  public static int ISP_EDITION_V6 = 22;
  public static int ORG_EDITION_V6 = 23;
  public static int DOMAIN_EDITION_V6 = 24;
  public static int CITY_EDITION_REV1_V6 = 30;
  public static int CITY_EDITION_REV0_V6 = 31;
  public static int NETSPEED_EDITION_REV1 = 32;
  public static int NETSPEED_EDITION_REV1_V6 = 33;
  private string info;

  public DatabaseInfo(string info)
  {
    this.info = info;
  }

  public int getType()
  {
    if (this.info == null | this.info == "")
      return DatabaseInfo.COUNTRY_EDITION;
    return int.Parse(this.info.Substring(4, 3)) - 105;
  }

  public bool isPremium()
  {
    return this.info.IndexOf("FREE") < 0;
  }

  public DateTime getDate()
  {
    for (int index = 0; index < this.info.Length - 9; ++index)
    {
      if (char.IsWhiteSpace(this.info[index]))
      {
        string s = this.info.Substring(index + 1, 8);
        try
        {
          return DateTime.ParseExact(s, "yyyyMMdd", (IFormatProvider) null);
        }
        catch (Exception ex)
        {
          Console.Write(ex.Message);
          break;
        }
      }
    }
    return DateTime.Now;
  }

  public string toString()
  {
    return this.info;
  }
}
