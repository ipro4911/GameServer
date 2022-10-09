// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.WordFilter
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Managers
{
  internal class WordFilter
  {
    public string normal;
    public string replace;

    public WordFilter(string normal, string replace)
    {
      this.normal = normal;
      this.replace = replace;
    }
  }
}
