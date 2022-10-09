// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.MapData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Managers
{
  internal class MapData
  {
    public int mapId;
    public string name;
    public int flags;
    public int derb;
    public int niu;
    public string vehicleString;

    public MapData(int mapId, string name, int flags, int derb, int niu, string vehicleString)
    {
      this.mapId = mapId;
      this.name = name;
      this.flags = flags;
      this.derb = derb;
      this.niu = niu;
      this.vehicleString = vehicleString;
    }
  }
}
