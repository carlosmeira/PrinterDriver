// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.UsbInfo
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

namespace SATOPrinterAPI
{
  internal class UsbInfo
  {
    private string uname = "";
    private string uportid = "";

    public UsbInfo(string name, string portid)
    {
      this.uname = name;
      this.uportid = portid;
    }

    public string Name => this.uname;

    public string PortID => this.uportid;

    public override string ToString() => this.uname;
  }
}
