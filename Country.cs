// Decompiled with JetBrains decompiler
// Type: Country
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

public class Country
{
  private string code;
  private string name;

  public Country(string code, string name)
  {
    this.code = code;
    this.name = name;
  }

  public string getCode()
  {
    return this.code;
  }

  public string getName()
  {
    return this.name;
  }
}
