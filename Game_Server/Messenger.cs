// Decompiled with JetBrains decompiler
// Type: Game_Server.Messenger
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;

namespace Game_Server
{
  internal class Messenger : IDisposable
  {
    public int id;
    public string nickname;
    public int status;
    public int requesterId;
    public bool isOnline;

    public Messenger(int id, string nickname, int status, int requesterId)
    {
      this.id = id;
      this.nickname = nickname;
      this.status = status;
      this.requesterId = requesterId;
      this.isOnline = false;
      if (UserManager.GetUser(id) == null)
        return;
      this.isOnline = true;
    }

    protected virtual void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
