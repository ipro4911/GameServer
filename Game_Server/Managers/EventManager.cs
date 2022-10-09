// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.EventManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Threading;

namespace Game_Server.Managers
{
  internal class EventManager
  {
    public Thread OneMinuteThread;

    public void Load()
    {
    }

    public void OneMinuteTick()
    {
      while (true)
        Thread.Sleep(60000);
    }
  }
}
