// Decompiled with JetBrains decompiler
// Type: Game_Server.Web.WebManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game_Server.Web
{
  internal class WebManager
  {
    private static byte[] buffer = new byte[1024];
    private static Socket socket;

    public static bool openSocket(int port)
    {
      try
      {
        WebManager.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        WebManager.socket.Bind((EndPoint) new IPEndPoint(IPAddress.Any, port));
        WebManager.socket.Listen(0);
        WebManager.socket.BeginAccept(new AsyncCallback(WebManager.acceptConnection), (object) WebManager.socket);
        Log.WriteLine("Listening web connections from " + (object) port);
        return true;
      }
      catch
      {
        Log.WriteError("Error while setting up asynchronous the socket server for connections on port " + (object) port + ".");
        Log.WriteError("Port " + (object) port + " could be invalid or in use already.");
        Thread.Sleep(2500);
        Environment.Exit(0);
      }
      return false;
    }

    private static void acceptConnection(IAsyncResult iAr)
    {
      if (!Program.running)
        return;
      WebManager.socket.BeginAccept(new AsyncCallback(WebManager.acceptConnection), (object) WebManager.socket);
      Socket s = ((Socket) iAr.AsyncState).EndAccept(iAr);
      Log.WriteLine("Web Server Connection from: " + s.RemoteEndPoint.ToString().Split(':')[0]);
      WebServer webServer = new WebServer(s);
    }
  }
}
