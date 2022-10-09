// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.GunSmith
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class GunSmith
  {
    public int gameid;
    public int cost;
    public string item;
    public string rare;
    public string[] required_materials;
    public string[] required_items;
    public string[] lose_items;

    public GunSmith(
      int gameid,
      int cost,
      string item,
      string rare,
      string[] required_materials,
      string[] required_items,
      string[] lose_items)
    {
      this.gameid = gameid;
      this.cost = cost;
      this.item = item;
      this.rare = rare;
      this.required_materials = required_materials;
      this.required_items = required_items;
      this.lose_items = lose_items;
    }
  }
}
