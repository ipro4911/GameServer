// Decompiled with JetBrains decompiler
// Type: Game_Server.Zombie
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server
{
  internal class Zombie
  {
    public int Points = 1;
    public int Damage = 150;
    public int FollowUser = -1;
    public int ID;
    public string Name;
    public int Health;
    public bool SkillPoint;
    public int Type;
    public int timestamp;
    public int respawn;

    public Zombie(int ID, int FollowUser, int timestamp, int Type)
    {
      this.ID = ID;
      this.FollowUser = FollowUser;
      this.timestamp = timestamp;
      this.Type = Type;
      this.respawn = 0;
      this.Health = 0;
    }

    public void Reset()
    {
      ZombieManager.GetZombieData(this);
    }
  }
}
