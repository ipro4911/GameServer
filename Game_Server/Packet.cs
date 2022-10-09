// Decompiled with JetBrains decompiler
// Type: Game_Server.Packet
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Text;

namespace Game_Server
{
  internal class Packet : IDisposable
  {
    private StringBuilder packet = new StringBuilder();
    private ushort packetId;

    public byte[] GetBytes()
    {
      byte[] numArray = Cryption.encrypt(Encoding.GetEncoding("Windows-1250").GetBytes(this.packet.ToString().Remove(this.packet.Length - 1, 1).ToString() + (object) ' ' + (object) '\n'));
      this.Dispose();
      return numArray;
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

    protected void newPacket(ushort packetId)
    {
      if (this.packetId == (ushort) 0)
      {
        this.packetId = packetId;
        this.packet.Append(Environment.TickCount);
        this.packet.Append(" ");
        this.packet.Append(packetId);
        this.packet.Append(" ");
      }
      else
        Log.WriteError("Coudln't re-declare packetId!");
    }

    protected void addBlock(object block)
    {
      block = (object) block.ToString().Replace(' ', '\x001D');
      this.packet.Append(block.ToString());
      this.packet.Append(" ");
    }

    protected void Fill(object block, int length)
    {
      for (int index = 0; index < length; ++index)
        this.addBlock(block);
    }

    public void addRoomInfo(Room room)
    {
      this.addBlock((object) room.id);
      this.addBlock((object) room.status);
      this.addBlock((object) room.status);
      this.addBlock((object) room.master);
      this.addBlock((object) room.name);
      this.addBlock((object) room.enablepassword);
      this.addBlock((object) room.maxusers);
      this.addBlock((object) room.users.Count);
      this.addBlock((object) room.mapid);
      this.addBlock((object) (room.channel == 3 ? room.zombiedifficulty : (room.mode == 0 || room.mode == 7 || room.mode == 15 ? room.rounds : 0)));
      this.addBlock((object) room.rounds);
      this.addBlock((object) room.timelimit);
      this.addBlock((object) room.mode);
      this.addBlock((object) 4);
      this.addBlock((object) (room.isJoinable ? 1 : 0));
      this.addBlock((object) 0);
      this.addBlock((object) room.new_mode);
      this.addBlock((object) room.new_mode_sub);
      this.addBlock((object) (room.supermaster ? 1 : 0));
      this.addBlock((object) room.type);
      this.addBlock((object) room.levellimit);
      this.addBlock((object) room.premiumonly);
      this.addBlock((object) room.votekickOption);
      this.addBlock((object) (room.autostart ? 1 : 0));
      this.addBlock((object) 0);
      if (room.type == 1)
      {
        this.addBlock((object) 1);
        for (int Side = 0; Side < 2; ++Side)
        {
          Clan clan = room.GetClan(Side);
          if (clan == null)
          {
            this.Fill((object) -1, 2);
            this.addBlock((object) "?");
          }
          else
          {
            this.addBlock((object) clan.id);
            this.addBlock((object) clan.iconid);
            this.addBlock((object) clan.name);
          }
        }
      }
      else
        this.addBlock((object) -1);
    }
  }
}
