//// Decompiled with JetBrains decompiler
//// Type: Game_Server.Game.CP_RankingList
//// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
//// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

//namespace Game_Server.Game
//{
//  internal class CP_RankingList : Handler
//  {
//    public override void Handle(Game_Server.User usr)
//    {
//      usr.send((Packet) new SP_MyRank(usr));
//      ushort tab = ushort.Parse(this.getBlock(0));
//      ushort page = ushort.Parse(this.getBlock(1));
//      ushort sortType = ushort.Parse(this.getBlock(2));
//      usr.send((Packet) new SP_RankingList(tab, page, sortType));
//    }
//  }
//}
