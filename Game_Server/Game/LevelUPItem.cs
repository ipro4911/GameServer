// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.LevelUPItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class LevelUPItem
  {
    public string Code;
    public int Days;

    public LevelUPItem(string _Code, int _Days)
    {
      this.Code = _Code;
      this.Days = _Days;
    }
  }
}
