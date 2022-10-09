// Decompiled with JetBrains decompiler
// Type: Location
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

public class Location
{
  private static double EARTH_DIAMETER = 12756.4;
  private static double PI = 3.14159265;
  private static double RAD_CONVERT = Location.PI / 180.0;
  public string countryCode;
  public string countryName;
  public string region;
  public string city;
  public string postalCode;
  public double latitude;
  public double longitude;
  public int dma_code;
  public int area_code;
  public string regionName;
  public int metro_code;

  public double distance(Location loc)
  {
    double latitude1 = this.latitude;
    double longitude1 = this.longitude;
    double latitude2 = loc.latitude;
    double longitude2 = loc.longitude;
    double d1 = latitude1 * Location.RAD_CONVERT;
    double d2 = latitude2 * Location.RAD_CONVERT;
    double num1 = d2 - d1;
    double num2 = (longitude2 - longitude1) * Location.RAD_CONVERT;
    double d3 = Math.Pow(Math.Sin(num1 / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
    return Location.EARTH_DIAMETER * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3));
  }
}
