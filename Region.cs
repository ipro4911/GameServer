// Decompiled with JetBrains decompiler
// Type: Region
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

public class Region
{
  public string countryCode;
  public string countryName;
  public string region;

  public Region()
  {
  }

  public Region(string countryCode, string countryName, string region)
  {
    this.countryCode = countryCode;
    this.countryName = countryName;
    this.region = region;
  }

  public string getcountryCode()
  {
    return this.countryCode;
  }

  public string getcountryName()
  {
    return this.countryName;
  }

  public string getregion()
  {
    return this.region;
  }
}
