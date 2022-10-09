// Decompiled with JetBrains decompiler
// Type: Game_Server.TempItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server
{
  internal struct TempItem
  {
    public string name;
    public string code;
    public ushort days;

    public TempItem(string code, ushort days)
    {
      this.name = "Unknown";
      Item obj = ItemManager.GetItem(code);
      if (obj != null)
        this.name = obj.Name;
      this.code = code;
      this.days = days;
    }
  }
}
