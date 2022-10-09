// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.Packet_Manager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server;
using System;
using System.Data;
using System.Diagnostics;
namespace Game_Server.Managers
{
    internal class CP_Emblem : Handler
    {
        public override void Handle(Game_Server.User usr) 
        {

            usr.send((Packet)new SP_EmblemSystem(usr)); // TODO:Add value when u change to database // 07.09.2022
        }
    }
}



class SP_EmblemSystem : Packet
{
    public int userId;
   // public int emblemid;



    string[] strArray = new string[1];
    public SP_EmblemSystem(Game_Server.User usr)
    {
        this.newPacket((ushort)30113);
        addBlock(1);
        DB.RunQuery("UPDATE users SET emblemid = '" + usr.emblemid + "'WHERE id='" + (object)usr.userId + "'");
    }
}


