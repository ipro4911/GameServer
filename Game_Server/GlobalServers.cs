// Decompiled with JetBrains decompiler
// Type: Game_Server.GlobalServers
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;
using System.Data;

namespace Game_Server
{
  internal class GlobalServers
  {
    public static List<Server> servers = new List<Server>();

    public static void LoadServers()
    {
      DataTable dataTable = DB.RunReader("SELECT * FROM servers");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        int num = int.Parse(row["serverid"].ToString());
        Server server = new Server();
        server.id = num;
        server.name = row["name"].ToString();
        server.ip = row["ip"].ToString();
        server.flag = int.Parse(row["flag"].ToString());
        server.minrank = int.Parse(row["minrank"].ToString());
        server.slot = int.Parse(row["slot"].ToString());
        if (num == Game_Server.Configs.Server.serverId)
          Program.server = server;
        GlobalServers.servers.Add(server);
      }
      if (Program.server == null)
        Log.WriteError("ServerID has not been binded");
      Log.WriteLine("Successfully loaded " + (object) GlobalServers.servers.Count + " servers.");
    }

    public static Server GetServer(string ip)
    {
      foreach (Server server in GlobalServers.servers)
      {
        if (server.ip == ip)
          return server;
      }
      return (Server) null;
    }
  }
}
