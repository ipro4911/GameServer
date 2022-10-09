// Decompiled with JetBrains decompiler
// Type: Game_Server.OutboxItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server
{
  public struct OutboxItem
  {
    public int id;
    public string itemcode;
    public ushort days;
    public ushort count;
    public int timestamp;

    public OutboxItem(int id, string itemcode, ushort days, int timestamp, ushort count)
    {
      this.id = id;
      this.itemcode = itemcode;
      this.days = days;
      this.timestamp = timestamp;
      this.count = count;
    }
  }
}
