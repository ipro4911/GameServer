// Decompiled with JetBrains decompiler
// Type: Game_Server.Web.WebServer
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Managers;
using System;
using System.Data;
using System.Net.Sockets;
using System.Text;

namespace Game_Server.Web
{
  internal class WebServer
  {
    private byte[] buffer = new byte[1024];
    private Socket socket;

    public WebServer(Socket s)
    {
      this.socket = s;
      s.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) null);
    }

    public virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void OnReceive(IAsyncResult iAr)
    {
      try
      {
        int length = this.socket.EndReceive(iAr);
        if (length <= 0)
          return;
        this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) null);
        byte[] bytes = new byte[length];
        Array.Copy((Array) this.buffer, 0, (Array) bytes, 0, length);
        string ip = this.socket.RemoteEndPoint.ToString().Split(':')[0];
        string str1 = Encoding.UTF8.GetString(bytes);
        if (str1.Length <= 0)
          return;
        if (GlobalServers.GetServer(ip) != null || Game_Server.Configs.Web.remote)
        {
          string[] strArray = str1.Split('|');
          string str2 = strArray[0];
          DataTable dataTable = DB.RunReader("SELECT * FROM users WHERE nickname='" + str2 + "'");
          if (dataTable.Rows.Count <= 0)
            return;
          int num = int.Parse(dataTable.Rows[0]["rank"].ToString());
          switch (strArray[1])
          {
            case "BROADCAST":
              string str3 = WordFilterManager.Replace(strArray[2]);
              Log.WriteLine(str2 + " broadcasted " + str3);
              string Message = str3.Replace(' ', '\x001D');
              UserManager.sendToServer((Packet) new SP_Chat(str2, SP_Chat.ChatType.Notice1, Message, num > 2 ? 999U : 0U, "NULL"));
              break;
            case "RELOAD_OUTBOX":
              UserManager.GetUser(str2)?.LoadOutboxItems();
              break;
          }
        }
        else
          Log.WriteError("Connection refused by IP: " + ip + " - Request: " + str1);
      }
      catch (Exception ex)
      {
        Log.WriteError("WebServer Socket error: " + ex.Message);
      }
    }
  }
}
