// Decompiled with JetBrains decompiler
// Type: Game_Server.AntiCheatServer
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Anti_Cheat;
using System;
using System.Net;
using System.Net.Sockets;

namespace Game_Server
{
  internal class AntiCheatServer
  {
    private Socket socket;
    private int port;
    private int sessionId;

    public bool Initialize(int port)
    {
      this.port = port;
      try
      {
        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this.socket.Bind((EndPoint) new IPEndPoint(IPAddress.Any, port));
        this.socket.Listen(0);
        this.socket.BeginAccept(new AsyncCallback(this.OnNewClient), (object) this.socket);
        Log.WriteLine("Listening AntiCheat Client(s) connections on port " + (object) port);
        return true;
      }
      catch
      {
      }
      return false;
    }

    public void OnNewClient(IAsyncResult iAR)
    {
      try
      {
        Socket socket = ((Socket) iAR.AsyncState).EndAccept(iAR);
        Client client = new Client(socket, this.sessionId);
        ++this.sessionId;
        Log.WriteLine("New AntiCheat Connection from " + socket.RemoteEndPoint.ToString());
        this.socket.BeginAccept(new AsyncCallback(this.OnNewClient), (object) this.socket);
      }
      catch
      {
      }
    }
  }
}
