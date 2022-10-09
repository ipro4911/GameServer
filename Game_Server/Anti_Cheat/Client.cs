// Decompiled with JetBrains decompiler
// Type: Game_Server.Anti_Cheat.Client
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Anti_Cheat.Structure;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Game_Server.Anti_Cheat
{
  internal class Client
  {
    private byte[] buffer = new byte[1024];
    private Socket socket;
    public int sessionId;

    public Client(Socket socket, int sessionId)
    {
      this.socket = socket;
      this.sessionId = sessionId;
      socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) null);
    }

    private void disconnect()
    {
      try
      {
        this.socket.Close();
      }
      catch
      {
      }
      if (this.socket == null)
        return;
      this.socket = (Socket) null;
    }

    public void send(Game_Server.Anti_Cheat.Structure.Packet p)
    {
      try
      {
        byte[] bytes = p.GetBytes();
        if (bytes == null || bytes.Length <= 0)
          return;
        this.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), (object) null);
      }
      catch
      {
        this.disconnect();
      }
    }

    private void sendCallBack(IAsyncResult iAr)
    {
      if (this.socket == null)
        return;
      try
      {
        this.socket.EndSend(iAr);
      }
      catch
      {
      }
    }

    private void OnReceive(IAsyncResult iAr)
    {
      try
      {
        int length = this.socket.EndReceive(iAr);
        if (length > 0)
        {
          this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) null);
          byte[] bytes = new byte[length];
          Array.Copy((Array) this.buffer, 0, (Array) bytes, 0, length);
          try
          {
            Game_Server.Anti_Cheat.Structure.Handler handler = PacketManager.ParsePacket(Encoding.GetEncoding("Windows-1250").GetString(bytes));
            if (handler == null)
              return;
            new Thread((ThreadStart) (() => handler.Handle(this))).Start();
          }
          catch
          {
          }
        }
        else
          this.disconnect();
      }
      catch
      {
        this.disconnect();
      }
    }
  }
}
