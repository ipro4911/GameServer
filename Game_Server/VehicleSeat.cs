// Decompiled with JetBrains decompiler
// Type: Game_Server.VehicleSeat
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server
{
  internal class VehicleSeat
  {
    public int MainCT = -1;
    public int MainCTMag = -1;
    public int SubCT = -1;
    public int SubCTMag = -1;
    public int ID;
    public User seatOwner;
    public string MainCTCode;
    public string SubCTCode;

    public bool TakeSeat(User usr)
    {
      if (this.seatOwner != null)
        return false;
      this.seatOwner = usr;
      return true;
    }

    public void LeaveSeat(User usr)
    {
      if (this.seatOwner.userId != usr.userId)
        return;
      this.seatOwner = (User) null;
    }

    public VehicleSeat(
      int _ID,
      int _MainCT,
      int _MainCTMag,
      int _SubCT,
      int _SubCTMag,
      string _MainCTCode,
      string _SubCTCode)
    {
      this.ID = _ID;
      this.MainCT = _MainCT;
      this.MainCTMag = _MainCTMag;
      this.SubCT = _SubCT;
      this.SubCTMag = _SubCTMag;
      this.MainCTCode = _MainCTCode;
      this.SubCTCode = _SubCTCode;
    }

    public bool IsSeatCode(string code)
    {
      if (!(code.ToUpper() == this.MainCTCode.ToUpper()))
        return code.ToUpper() == this.SubCTCode.ToUpper();
      return true;
    }
  }
}
