// Decompiled with JetBrains decompiler
// Type: SATOPrinterAPI.COMHelper
// Assembly: SATOPrinterAPI, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1CED7137-37FF-4597-9D4E-DAF488B01141
// Assembly location: D:\Downloads\Microsoft Edge\ApiPort.2.8.25.Offline\ApiPort.Offline\netcoreapp3.1\SATOPrinterAPI.dll

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace SATOPrinterAPI
{
  internal class COMHelper : InterfaceHelper
  {
    private int COMtimeout = 2500;
    private SerialPort comport;
    private Stream comStream;
    private const int MaxTXBytes = 1024;

    public COMHelper(bool PermanentConnect)
      : base(PermanentConnect)
    {
    }

    public override void CBReceive(IAsyncResult ar)
    {
      if (!(ar.AsyncState is Stream asyncState))
        return;
      try
      {
        this.comport.WriteTimeout = this.COMtimeout;
        this.comport.ReadTimeout = this.COMtimeout;
        int length = asyncState.EndRead(ar);
        if (length > 0)
        {
          byte[] numArray = new byte[length];
          Array.Copy((Array) this.buffer, (Array) numArray, length);
          if (this.fOnReceiveCallBack != null)
            this.fOnReceiveCallBack(numArray);
        }
        this.buffer.Initialize();
        this.Gar = asyncState.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(((InterfaceHelper) this).CBReceive), (object) asyncState);
      }
      catch
      {
      }
    }

    public override void Open(string name, object data, int timeout)
    {
      SerialCommSetting setting = (SerialCommSetting) data;
      try
      {
        this.comport = new SerialPort();
        this.ApplySetting(name, setting, ref this.comport);
        this.comport.Open();
        if (!this.comport.IsOpen)
          throw new Exception("Serial Port Connection Fail.");
        this.comport.DtrEnable = true;
        this.COMtimeout = timeout >= 2500 ? timeout : 2500;
        this.comport.WriteTimeout = this.COMtimeout;
        this.comport.ReadTimeout = this.COMtimeout;
        this.comStream = this.comport.BaseStream;
        if (this.comStream == null)
          throw new Exception("Serial Port Connection Fail.");
        if (!this.isPermanent)
          return;
        this.Gar = this.comStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(((InterfaceHelper) this).CBReceive), (object) this.comStream);
      }
      catch (Exception ex)
      {
        this.Close();
        throw new Exception(ex.Message);
      }
    }

    public void ApplySetting(string portName, SerialCommSetting setting, ref SerialPort com)
    {
      com.PortName = portName;
      com.BaudRate = setting.Baudrate;
      string[] strArray = setting.Parameters.Split('-');
      com.DataBits = strArray.Length == 3 ? int.Parse(strArray[0]) : throw new Exception("Invalid serial COM parameters");
      com.StopBits = (StopBits) Enum.Parse(typeof (StopBits), strArray[2]);
      switch (strArray[1].ToUpper())
      {
        case "N":
          com.Parity = Parity.None;
          break;
        case "E":
          com.Parity = Parity.Even;
          break;
        case "O":
          com.Parity = Parity.Odd;
          break;
        case "S":
          com.Parity = Parity.Space;
          break;
        case "M":
          com.Parity = Parity.Mark;
          break;
        default:
          com.Parity = Parity.None;
          break;
      }
      switch (setting.FlowControl.ToLower())
      {
        case "none":
          com.Handshake = Handshake.None;
          break;
        case "hardware":
          com.Handshake = Handshake.RequestToSend;
          break;
        case "xon/xoff":
          com.Handshake = Handshake.XOnXOff;
          break;
        default:
          com.Handshake = Handshake.None;
          break;
      }
    }

    public override byte[] Send(byte[] data, int ReplyCnt)
    {
      if (this.comStream != null)
      {
        if (this.comport.IsOpen)
        {
          try
          {
            Math.Min(data.Length, 1024);
            int offset = 0;
            int num = 0;
            do
            {
              int count = Math.Min(data.Length - offset, 1024);
              this.comport.Write(data, offset, count);
              num += count;
              offset += count;
            }
            while (offset != data.Length);
            if (!this.isPermanent)
            {
              if (ReplyCnt > 0)
              {
                for (int index = 0; index < ReplyCnt; ++index)
                {
                  try
                  {
                    Thread.Sleep(100);
                    this.buffer.Initialize();
                    this.comStream.ReadTimeout = this.COMtimeout;
                    int length = this.comStream.Read(this.buffer, 0, this.buffer.Length);
                    if (length > 0)
                    {
                      byte[] destinationArray = new byte[length];
                      Array.Copy((Array) this.buffer, (Array) destinationArray, length);
                      return destinationArray;
                    }
                    if (index == ReplyCnt - 1)
                      throw new Exception("No Reply received!");
                  }
                  catch (Exception ex)
                  {
                    if (index == 0)
                      throw ex;
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            this.Close();
            throw new Exception(ex.Message);
          }
          return (byte[]) null;
        }
      }
      throw new Exception("Couldn't establish connection");
    }

    public override void Close()
    {
      if (this.comStream != null)
      {
        this.comStream.Close();
        this.comStream = (Stream) null;
      }
      if (this.comport == null)
        return;
      if (this.comport.IsOpen)
        this.comport.Close();
      this.comport = (SerialPort) null;
    }
  }
}
