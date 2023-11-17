// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.SerialCommSetting
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

namespace SATOPrinterAPI
{
  public class SerialCommSetting
  {
    private int baudrate;
    private string parameters = "";
    private string flowControl = "";

    public SerialCommSetting()
    {
      this.baudrate = 9600;
      this.parameters = "8-N-1";
      this.flowControl = "None";
    }

    public int Baudrate
    {
      get => this.baudrate;
      set => this.baudrate = value;
    }

    public string Parameters
    {
      get => this.parameters;
      set => this.parameters = value;
    }

    public string FlowControl
    {
      get => this.flowControl;
      set => this.flowControl = value;
    }
  }
}
